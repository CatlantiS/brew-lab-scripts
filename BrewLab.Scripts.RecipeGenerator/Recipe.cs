using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrewLab.Scripts.RecipeGenerator
{
    internal class Recipe
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("userId")]
        public string UserID { get; set; }

        [JsonProperty("volume")]
        public double Volume { get; set; }

        [JsonProperty("units")]
        public string Units { get; set; }

        [JsonProperty("yeastType")]
        public string YeastType { get; set; }
    }
}
