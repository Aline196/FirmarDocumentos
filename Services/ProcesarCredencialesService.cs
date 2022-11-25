using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Crypto.Tls;
using System.IO;
using System.Threading.Tasks;

namespace FirmarDocumentos.Services
{
    public class ProcesarCredencialesService : IProcesarCredenciales
    {

        private readonly IHostingEnvironment environment;

        public ProcesarCredencialesService(IHostingEnvironment environment)
        {
            this.environment = environment;
        }

        //Metodo para recibir el Certificado
        public async Task GuardarCertificadosAsync(IFormFile certificado)
        {
            string path = Path.Combine(environment.WebRootPath, "Credenciales");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            string fileName = Path.GetFileName(certificado.FileName);
            using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                await certificado.CopyToAsync(stream);

            }
        }

        //Metodo para guardar la CLAVE PRIVADA
        public async Task GuardarclavePrivadaAsync(IFormFile clavePrivada)
        {
            string path = Path.Combine(environment.WebRootPath, "Credenciales");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            string fileName = Path.GetFileName(clavePrivada.FileName);
            using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                await clavePrivada.CopyToAsync(stream);

            }
        }

        //Metodo para guardar los datos d
        //public async Task GuardarCredencialesAsync(IFormFile file)
        //{

        //}

    }
}
