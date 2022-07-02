using Aplicatie_Licenta.Models;
using Aplicatie_Licenta.Service.Schemas.Color;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.Service
{
    public static class ColorService
    {
        public static async Task<IEnumerable<ColorCode>> GetAllColors()
        {
            using HttpClient client = new();
            var request = await client.GetAsync("http://localhost:8000/api/colors");
            var response = await request.Content.ReadAsStringAsync();

            var colors = JsonConvert.DeserializeObject<IEnumerable<ColorOut>>(response);

            return colors.Select(color => new ColorCode
            {
                Code = color.code,
                Red = color.red,
                Green = color.green,
                Blue = color.blue
            });
        }
    }
}
