using Aplicatie_Licenta.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.Service.Interface
{
    public interface IColorService
    {
        // get all color used by app
        Task<IEnumerable<ColorCode>> GetAllColors();
    }
}
