namespace RecipeApplication.Models
{
	public class ValueModel
	{
		public int id { get; set; }
		public int servings { get; set; }
		public string title { get; set; }
		public string imageType { get; set; }
		public string? image { get; set; }
		public string SourceUrl { get; set; }
	}
}
