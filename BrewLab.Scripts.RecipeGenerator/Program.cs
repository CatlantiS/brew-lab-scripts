using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BrewLab.Scripts.RecipeGenerator
{
    class Program
    {
        //Because I'm all about that Baas.  
        private static readonly string _baseAddress = "http://blabdatadev01.cloudapp.net:3000/api/v1/";
        //Default user for now...
        private static readonly int _userID = 1;

        private static Random _random = new Random();

        static void Main(string[] args)
        {
            foreach(Recipe recipe in _generateRecipes(quantity: 1000))
            {
                var id = _postRecipe(recipe).Result;

                string log = string.Format("Recipe {0} created.", id);

                Console.WriteLine(log);
            }

            Console.ReadKey();
        }

        private static IEnumerable<Recipe> _generateRecipes(int quantity = 100)
        {
            Func<bool, Recipe> generateRecipe = null;

            generateRecipe = new Func<bool, Recipe>(isVersion =>
            {
                string name = _generateText(10);

                double volume = ((double)_random.Next(20) / 20) * 5;

                string units = string.Empty;

                if (volume > 2.5)
                    units = "Gallons";
                else
                    units = "Liters";

                string yeastType = _generateText(25);

                var recipe = new Recipe
                {
                    Name = name,
                    UserID = _userID,
                    Volume = volume,
                    Units = units,
                    YeastType = yeastType,
                    Versions = !isVersion ? Enumerable.Range(0, 10)
                        .Select(_ => generateRecipe(true)).ToList() : null
                };

                return recipe;
            });

            for (int i = 0; i < quantity; i++)
                yield return generateRecipe(false);
        }

        private static async Task<string> _postRecipe(Recipe recipe)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(_baseAddress)})
            {
                HttpResponseMessage response = await client.PostAsJsonAsync<Recipe>("recipes/", recipe);

                var result = await response.Content.ReadAsStringAsync();

                return result;
            }
        }

        private static string _generateText(int textLength)
        {
            //http://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings-in-c
            //Making it more likely to have spaces like some natural text strings.
            var pool = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-    ";

            var text = new string(Enumerable.Repeat(pool, textLength)
                .Select(x => x[_random.Next(pool.Length)])
                .ToArray());

            return text;
        }
    }
}
