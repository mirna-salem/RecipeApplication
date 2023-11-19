namespace RecipeApplication.Models
{
	public class Nutrient
	{
		public string name { get; set; }
		public decimal amount { get; set; }
		public string unit { get; set; }
		public int percentOfDailyNeeds { get; set; }
	}
}
