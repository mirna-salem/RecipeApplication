function handleUnauthenticatedUser() {
    var buttons = document.querySelectorAll('.add-to-meal-plan-button');

    buttons.forEach(function (button) {
        button.addEventListener('click', function (event) {
            // Get the value of the 'SearchPhrase' parameter directly from the URL
            var searchPhrase = new URLSearchParams(window.location.search).get('SearchPhrase');

            // Construct the login URL with the dynamic searchPhrase parameter and perform the redirection
            window.location.href = `/Identity/Account/Login?returnUrl=${encodeURIComponent(`/Recipe/ShowSearchResults?SearchPhrase=${searchPhrase}`)}`;
        });
    });
}

// Call the function when the page loads
handleUnauthenticatedUser(); 
