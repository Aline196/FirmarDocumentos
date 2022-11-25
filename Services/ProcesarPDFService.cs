using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace FirmarDocumentos.Services
{
    public class ProcesarPDFService : IProcesarPDFService
    {
        private readonly IHostingEnvironment environment;

        public ProcesarPDFService(IHostingEnvironment environment)
        {
            this.environment = environment;
        }

        public async Task GuardarArchivoAsync(IFormFile archivo)
        {
            string path = Path.Combine(environment.WebRootPath, "Archivos"); //mezcla de la ruta de la carpeta y el nombre del archivo

            if (!Directory.Exists(path)) //Creacion del directorio
            {
                Directory.CreateDirectory(path);
            }


            string fileName = Path.GetFileName(archivo.FileName); //se crea el nombre del archivo
            using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                await archivo.CopyToAsync(stream);

            }


        }
    }
}
