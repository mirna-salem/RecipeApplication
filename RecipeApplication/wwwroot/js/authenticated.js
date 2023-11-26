$(document).ready(function () {
    const shiftedDiv = document.getElementById('search-results');
    const sidebarEl = document.getElementById("sidebar");
    const breakfastList = document.getElementById("breakfast-list");
    const lunchList = document.getElementById("lunch-list");
    const dinnerList = document.getElementById("dinner-list");
    const nutrientSum = document.getElementById("nutrient-summary");


    let isSidebarOpen = false;
    let selectedTitle;
    let selectedId;
    let selectedUrl;

    $(".add-to-meal-plan-button").on("click", function (event) {
        event.preventDefault();

        // Hide modal
        $('#modal-body, #modal-overlay').hide();

        // Get user selections
        var selectedSlot = $("input[name='slot']:checked").val();
        var selectedServings = $("#servings").val();

        // Add meal plan item
        addMealPlanItem(selectedSlot, selectedServings);
    });

    $('#clear-meal-plan-day').click(function (event) {
        // Clear meal plan for the day
        event.preventDefault();
        clearMealPlanDay();
    });

    // Open modal on click
    $(".add-to-meal-plan-button-before-selection").on("click", function (event) {
        // Set user selections
        selectedTitle = $(this).data("title");
        selectedId = $(this).data("id");
        selectedUrl = $(this).data("url");

        var modal = document.getElementById('modal-body');
        var modalOverlay = document.getElementById('modal-overlay');

        modal.style.display = 'block';
        modalOverlay.style.display = 'block';
    });

    // When the user clicks anywhere outside of the modal, close it
    window.onclick = function (event) {
        var modal = document.getElementById('modal-body');
        var modalOverlay = document.getElementById('modal-overlay');

        if (event.target === modalOverlay) {
            modal.style.display = 'none';
            modalOverlay.style.display = 'none';
        }
    }

    // When user clicks anywhere outside of sidebar, close it
    document.addEventListener("click", (event) => {
        if (isSidebarOpen) {

            // Check if the click target is not the sidebar or a descendant of the sidebar
            if (!sidebarEl.contains(event.target) && event.target !== sidebarEl &&
                !event.target.classList.contains("sidebarText") && !event.target.classList.contains("recipe-img")) {

                // Close the sidebar
                sidebarEl.style.transform = "translate(-260px)";
                shiftedDiv.style.marginLeft = 0 + 'px';
                isSidebarOpen = false;
            }
        }
    });

    function addMealPlanItem(selectedSlot, selectedServings) {

        var selectedSlotValid = selectedSlot !== undefined;
        var selectedServingsValid = selectedServings !== '';


        if (selectedSlotValid && selectedServingsValid) {
            $.ajax({
                type: "POST",
                url: "/Recipe/AddMealPlanItem",
                data: { itemId: selectedId, itemTitle: selectedTitle, itemSourceUrl: selectedUrl, itemSlot: selectedSlot, itemServing: selectedServings },
                dataType: "json",
                success: function (response) {

                    sidebarEl.style.transform = "translate(0px)";
                    sidebarEl.scrollTop = 0;
                    shiftedDiv.style.marginLeft = 200 + 'px';

                    // Clear 
                    clearList(breakfastList);
                    clearList(lunchList);
                    clearList(dinnerList);
                    clearList(nutrientSum);


                    // Breakfast
                    if (response.items.some(item => item.slot === 1)) {
                        response.items.forEach(function (item) {
                            createListItem(item, breakfastList, 1);
                        });
                    } else {
                        breakfastList.innerHTML = "<div>No breakfast added yet.</div>";
                    }

                    // Lunch
                    if (response.items.some(item => item.slot === 2)) {
                        response.items.forEach(function (item) {
                            createListItem(item, lunchList, 2);
                        });
                    } else {
                        lunchList.innerHTML = "<div>No lunch added yet.</div>";
                    }

                    // Dinner
                    if (response.items.some(item => item.slot === 3)) {
                        response.items.forEach(function (item) {
                            createListItem(item, dinnerList, 3);
                        });
                    } else {
                        dinnerList.innerHTML = "<div>No dinner added yet.</div>";
                    }


                    // Nutrient Summary
                    response.nutritionSummary.nutrients.forEach(function (nutrient) {
                        if (nutrient.name == "Calories" || nutrient.name == "Fat" || nutrient.name == "Protein" || nutrient.name == "Carbohydrates") {
                            var summary = document.createElement("p");
                            summary.innerHTML = nutrient.name + ": " + nutrient.amount + "" + nutrient.unit;
                            nutrientSum.appendChild(summary);
                        }
                    });


                    // Set side bar to open
                    isSidebarOpen = true;

                },
                error: function (error) {
                    console.error("Error:", error);
                }
            });
        }
        else {
            if (!selectedSlotValid && !selectedServingsValid) {
                alert('Please select meal type and enter the number of servings before proceeding.');
            } else if (!selectedServingsValid) {
                alert('Please enter the number of servings before proceeding.');
            } else {
                alert('Please select meal type before proceeding.');
            }
        }


        
    }

    function createListItem(item, list, slotNr) {
        if (item.slot == slotNr) {
            var listItem = document.createElement("li");
            listItem.classList.add("meal-plan-item");
            listItem.dataset.itemId = item.id;

            var link = document.createElement("a");
            link.href = item.value.sourceUrl;
            link.target = "_blank";
            link.textContent = item.value.title;

            var button = document.createElement("button");
            button.classList.add("delete-button");
            var icon = document.createElement("i");
            icon.classList.add("fa", "fa-trash");
            icon.setAttribute("aria-hidden", "true");
            button.setAttribute("title", "Delete");

            button.appendChild(icon);
            button.addEventListener("click", function () {
                DeleteMealPlanItem(item.id, item.slot);
            });

            listItem.appendChild(button);
            listItem.appendChild(link);

            list.appendChild(listItem);
        }     
    }

    function clearList(list) {
        list.innerHTML = "";
    }

    function clearMealPlanDay() {
        $.ajax({
            url: '/Recipe/ClearMealPlanDay',
            type: 'DELETE',
            contentType: 'application/json',
            success: function (response) {
                // Clear Breakfast List
                var breakfastList = document.getElementById("breakfast-list");
                breakfastList.innerHTML = "No breakfast added yet.";

                // Clear Lunch List
                var lunchList = document.getElementById("lunch-list");
                lunchList.innerHTML = "No lunch added yet.";

                // Clear Dinner List
                var dinnerList = document.getElementById("dinner-list");
                dinnerList.innerHTML = "No dinner added yet.";

                // Clear Nutrient Summary
                var nutrientSum = document.getElementById("nutrient-summary");
                nutrientSum.innerHTML = "";

                console.log('Delete request was successful');
            },
            error: function (error) {
                console.error('Error deleting meal plan day', error);
            }
        });

    }


    function DeleteMealPlanItem(itemId, itemSlot) {
        $.ajax({
            type: "DELETE",
            url: "/Recipe/DeleteMealPlanItem",
            data: { itemId: itemId },
            dataType: "json",
            success: function (response) {
                // Remove the deleted item from the list
                var items = document.querySelectorAll('.meal-plan-item');
                items.forEach(function (item) {
                    if (parseInt(item.dataset.itemId) === itemId) {
                        item.remove();
                    }
                });


                // Check if there are any remaining items
                var remainingItems = document.querySelectorAll('.meal-plan-item');
                if (remainingItems.length === 0) {
                    if (itemSlot == 1) {
                        var breakfastList = document.getElementById("breakfast-list");
                        breakfastList.innerHTML = "No breakfast added yet.";
                    }
                }
            },
            error: function (error) {
                console.error("Error:", error);
            }
        });
    }


    function showNotReadyMessage() {
        alert("This feature isn't ready yet. Check back later.");
    }
});
