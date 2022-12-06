using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;


namespace FirmarDocumentos.Services
{
    public interface IProcesarCredencialesService
    {
        Task GuardarCertificadosAsync(IFormFile certificado);
        Task GuardarclavePrivadaAsync(IFormFile clavePrimaria);
        string[] ObtenerInformacionCertificado(string rutaCertificado);
        byte[] EmpacarClaves(string rutaCertificado, string rutaClavePrivada);

    }
}
