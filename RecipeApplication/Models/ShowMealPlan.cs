namespace RecipeApplication.Models
{
    public class ShowMealPlan
    {
        public NutritionSummary nutritionSummary { get; set; }
        public NutritionSummaryBreakfast nutritionSummaryBreakfast { get; set; }
        public NutritionSummaryLunch nutritionSummaryLunch { get; set; }
        public NutritionSummaryDinner nutritionSummaryDinner { get; set; }
        public int date { get; set; }
        public string day { get; set; }
        public Item[] items { get; set; }
    }
}
