using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace RecipeApplication.Areas.Identity.Data;

// Add profile data for application users by adding properties to the RecipeApplicationUser class
[DataContract]
public class RecipeApplicationUser : IdentityUser
{
    [DataMember(Name = "username")]
    public string SpoonacularUsername { get; set; }
    [DataMember(Name = "spoonacularPassword")]
    public string SpoonacularPassword { get; set; }
    [DataMember(Name = "hash")]
    public string SpoonacularHash { get; set; }
}

