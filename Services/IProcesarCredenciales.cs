using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;


namespace FirmarDocumentos.Services
{
    public interface IProcesarCredenciales
    {
        Task GuardarCertificadosAsync(IFormFile certificado);
        Task GuardarclavePrivadaAsync(IFormFile clavePrimaria);
    }
}
