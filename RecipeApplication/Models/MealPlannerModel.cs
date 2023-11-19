namespace RecipeApplication.Models
{
    public class MealPlannerModel
    {
        public int date { get; set; }
        public int slot { get; set; }
        public int position { get; set; }
        public string type { get; set; }
        public ValueModel value { get; set; }
    }
}
