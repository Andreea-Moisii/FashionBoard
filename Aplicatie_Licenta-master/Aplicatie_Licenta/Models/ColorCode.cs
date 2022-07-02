namespace Aplicatie_Licenta.Models
{
    public class ColorCode
    {
        public string Code { get; set; }
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }


        public ColorCode()
        {
            Code = "#000000";
            Red = 0;
            Green = 0;
            Blue = 0;
        }

        public ColorCode(string code, int red, int green, int blue)
        {
            Code = code;
            Red = red;
            Green = green;
            Blue = blue;
        }

    }
}
