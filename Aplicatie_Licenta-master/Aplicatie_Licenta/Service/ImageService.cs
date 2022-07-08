using Aplicatie_Licenta.Models;
using ColorThiefDotNet;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Image = Aplicatie_Licenta.Models.Image;

namespace Aplicatie_Licenta.Service
{
    public static class ImageService
    {
        // -------------------- upload a image to the server and get collors ------------- //
        public static async Task<Image?> UploadImage(string filePath)
        {
            using var client = new HttpClient();

            var fileName = Path.GetFileName(filePath);
            using var requestContent = new MultipartFormDataContent();
            
            using var fileStream = File.OpenRead(filePath);

            requestContent.Add(new StreamContent(fileStream), "image", fileName);
            var response = await client.PostAsync("http://localhost:8000/api/images", requestContent);

            var strResult =  await response.Content.ReadAsStringAsync();
            var image = Newtonsoft.Json.JsonConvert.DeserializeObject<Image>(strResult);

            if (image != null)
            {
                var colors = GetColors(filePath);
                image.color1 = colors[0];
                image.color2 = colors[1];
                image.color3 = colors[2];
            }
            return image;
        }

        // -------------------- get the colors from the image ------------- //
public static List<string> GetColors(string filePath)
{
    var colorThief = new ColorThief();
    var image = new Bitmap(filePath);
    var colors = colorThief.GetPalette(image, 3);
    List<string> colorsList = new List<string>();
    for (int i = 0; i < colors.Count; i++)
    {
        colorsList.Add(colors[i].Color.ToHexString());
    }
    return colorsList;
}
    }
}
