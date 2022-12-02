using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FirmarDocumentos.Services
{
    public interface IProcesarPDFService
    {

        Task GuardarArchivoAsync(IFormFile file);
        Task ModificarPdf();


    }
}
