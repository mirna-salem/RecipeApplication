namespace RecipeApplication.Models
{
	public class RecipeModel
	{
		public List<Recipe> Results { get; set; }
		public int Offset { get; set; }
		public int Number { get; set; }
		public int TotalResults { get; set; }
	}
}
