# Yummy Recipe Web Application

Welcome to Yummy, a C# MVC website application that makes meal planning a breeze! With this application, users can search for recipes using the Spoonacular API, generate personalized meal plans, and create custom meal plans tailored to their preferences. The platform goes beyond by allowing users to create accounts, enabling them to save their daily meal plans and track their nutritional information, including macronutrients.

## 🚀 Getting Started (Local Development)

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQL Server LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)
- A free [Spoonacular API key](https://spoonacular.com/food-api)

### Setup Steps
1. **Clone the repository:**
   ```sh
   git clone https://github.com/mirna-salem/RecipeApplication.git
   cd RecipeApplication
   ```
2. **Restore dependencies:**
   ```sh
   dotnet restore
   ```
3. **Configure your API key:**
   - Copy your Spoonacular API key into `RecipeApplication/appsettings.Development.json`:
     ```json
     "SpoonacularApiKey": "YOUR_SPOONACULAR_API_KEY_HERE"
     ```
4. **Set up the database:**
   ```sh
   cd RecipeApplication
   dotnet ef database update
   cd ..
   ```
5. **Run the application:**
   ```sh
   cd RecipeApplication
   dotnet run
   ```
   The app will be available at the URLs shown in the terminal (e.g., https://localhost:7092).

## 🏗️ Project Architecture

- **ASP.NET Core MVC**: Main web framework for controllers, views, and routing.
- **Entity Framework Core**: Handles data access and migrations, using SQL Server LocalDB for development.
- **Identity**: User authentication and management via ASP.NET Core Identity.
- **Spoonacular API**: External API for recipe and meal plan data.

**Solution Structure:**
```
RecipeApplication/
  ├── Controllers/         # MVC controllers (main logic)
  ├── Models/              # Data models and view models
  ├── Areas/Identity/      # User authentication and identity management
  ├── Views/               # Razor views (UI)
  ├── wwwroot/             # Static files (CSS, JS, images)
  ├── Migrations/          # Entity Framework migrations
  ├── appsettings.*.json   # Configuration files
  └── Program.cs           # App startup and configuration
```

## 🌐 Deployed Application
You can access the deployed application at: https://recipeapplication.azurewebsites.net/

## 🤝 Contributing
Pull requests are welcome! For major changes, please open an issue first to discuss what you would like to change.

## 📫 Contact
For questions or support, please open an issue on GitHub or contact the maintainer.


