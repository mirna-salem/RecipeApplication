using RecipeApplication.Models;
using System;
using System.Collections.Generic;

namespace RecipeApplication.Services
{
    public class DemoDataService
    {
        public RecipeModel GetDemoRecipes(string searchPhrase)
        {
            var recipes = new List<Recipe>
            {
                new Recipe
                {
                    Id = 1,
                    Title = "Chicken Pasta",
                    ReadyInMinutes = 30,
                    Servings = 4,
                    Image = "https://spoonacular.com/recipeImages/1-556x370.jpg",
                    SourceUrl = "https://example.com/recipe1",
                    Summary = "A delicious chicken pasta dish perfect for dinner.",
                    Cuisines = new List<string> { "Italian" },
                    DishTypes = new List<string> { "main course" },
                    Diets = new List<string> { "gluten free" },
                    Vegetarian = false,
                    Vegan = false,
                    GlutenFree = true,
                    DairyFree = true,
                    VeryHealthy = true,
                    Cheap = false,
                    VeryPopular = true,
                    AggregateLikes = 150,
                    HealthScore = 85.5
                },
                new Recipe
                {
                    Id = 2,
                    Title = "Vegetarian Salad",
                    ReadyInMinutes = 15,
                    Servings = 2,
                    Image = "https://spoonacular.com/recipeImages/2-556x370.jpg",
                    SourceUrl = "https://example.com/recipe2",
                    Summary = "Fresh and healthy vegetarian salad.",
                    Cuisines = new List<string> { "Mediterranean" },
                    DishTypes = new List<string> { "salad" },
                    Diets = new List<string> { "vegetarian" },
                    Vegetarian = true,
                    Vegan = true,
                    GlutenFree = true,
                    DairyFree = true,
                    VeryHealthy = true,
                    Cheap = true,
                    VeryPopular = false,
                    AggregateLikes = 75,
                    HealthScore = 95
                }
                // Add more demo recipes as needed
            };

            if (!string.IsNullOrEmpty(searchPhrase))
            {
                recipes = recipes.FindAll(r =>
                    r.Title.ToLower().Contains(searchPhrase.ToLower()) ||
                    (r.Cuisines != null && r.Cuisines.Exists(c => c.ToLower().Contains(searchPhrase.ToLower()))) ||
                    (r.Diets != null && r.Diets.Exists(d => d.ToLower().Contains(searchPhrase.ToLower())))
                );
            }

            return new RecipeModel
            {
                Results = recipes,
                Offset = 0,
                Number = recipes.Count,
                TotalResults = recipes.Count
            };
        }

        public ShowMealPlan GetDemoMealPlan()
        {
            return new ShowMealPlan
            {
                date = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                day = DateTime.Now.DayOfWeek.ToString(),
                nutritionSummary = new NutritionSummary
                {
                    nutrients = new Nutrient[]
                    {
                        new Nutrient { name = "Calories", amount = 1850, unit = "kcal", percentOfDailyNeeds = 92 },
                        new Nutrient { name = "Protein", amount = 85, unit = "g", percentOfDailyNeeds = 170 },
                        new Nutrient { name = "Fat", amount = 65, unit = "g", percentOfDailyNeeds = 100 },
                        new Nutrient { name = "Carbohydrates", amount = 220, unit = "g", percentOfDailyNeeds = 73 }
                    }
                },
                nutritionSummaryBreakfast = new NutritionSummaryBreakfast
                {
                    nutrients = new Nutrient[]
                    {
                        new Nutrient { name = "Calories", amount = 450, unit = "kcal", percentOfDailyNeeds = 22 },
                        new Nutrient { name = "Protein", amount = 25, unit = "g", percentOfDailyNeeds = 50 },
                        new Nutrient { name = "Fat", amount = 15, unit = "g", percentOfDailyNeeds = 23 },
                        new Nutrient { name = "Carbohydrates", amount = 55, unit = "g", percentOfDailyNeeds = 18 }
                    }
                },
                nutritionSummaryLunch = new NutritionSummaryLunch
                {
                    nutrients = new Nutrient[]
                    {
                        new Nutrient { name = "Calories", amount = 650, unit = "kcal", percentOfDailyNeeds = 32 },
                        new Nutrient { name = "Protein", amount = 35, unit = "g", percentOfDailyNeeds = 70 },
                        new Nutrient { name = "Fat", amount = 25, unit = "g", percentOfDailyNeeds = 38 },
                        new Nutrient { name = "Carbohydrates", amount = 75, unit = "g", percentOfDailyNeeds = 25 }
                    }
                },
                nutritionSummaryDinner = new NutritionSummaryDinner
                {
                    nutrients = new Nutrient[]
                    {
                        new Nutrient { name = "Calories", amount = 750, unit = "kcal", percentOfDailyNeeds = 38 },
                        new Nutrient { name = "Protein", amount = 25, unit = "g", percentOfDailyNeeds = 50 },
                        new Nutrient { name = "Fat", amount = 25, unit = "g", percentOfDailyNeeds = 38 },
                        new Nutrient { name = "Carbohydrates", amount = 90, unit = "g", percentOfDailyNeeds = 30 }
                    }
                },
                items = new Item[]
                {
                    new Item
                    {
                        id = 1,
                        slot = 1,
                        position = 0,
                        type = "RECIPE",
                        value = new ValueModel
                        {
                            id = 1,
                            title = "Chicken Pasta",
                            servings = 2,
                            imageType = "jpg"
                        }
                    },
                    new Item
                    {
                        id = 2,
                        slot = 2,
                        position = 0,
                        type = "RECIPE",
                        value = new ValueModel
                        {
                            id = 2,
                            title = "Vegetarian Salad",
                            servings = 1,
                            imageType = "jpg"
                        }
                    }
                }
            };
        }
    }
} 