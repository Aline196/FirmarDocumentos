using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace FirmarDocumentos.Services
{
    //Servicio:Logica de negocio
    public class ProcesarPDFService : IProcesarPDFService
    {
        private readonly IHostingEnvironment environment;

        public ProcesarPDFService(IHostingEnvironment environment)
        {
            this.environment = environment;
        }

        //Metodo para guardar el pdf
        public async Task GuardarArchivoAsync(IFormFile archivo)
        {
            string path = Path.Combine(environment.WebRootPath, "Archivos"); 

            if (!Directory.Exists(path)) 
            {
                Directory.CreateDirectory(path);
            }


            string fileName = Path.GetFileName(archivo.FileName); 
            using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                await archivo.CopyToAsync(stream);

            }


        }
    }
}
