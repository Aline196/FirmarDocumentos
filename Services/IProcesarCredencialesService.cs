using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;


namespace FirmarDocumentos.Services
{
    public interface IProcesarCredencialesService
    {
        Task GuardarCertificadosAsync(IFormFile certificado);
        Task GuardarclavePrivadaAsync(IFormFile clavePrimaria);
        string[] ObtenerInformacionCertificado(string rutaCertificado);
    }
}
