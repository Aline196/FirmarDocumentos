
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FirmarDocumentos.Controllers
{
    public class FirmaElectonicaController : Controller
    {
        private readonly IHostingEnvironment environment;

        public FirmaElectonicaController(IHostingEnvironment environment)
        {
            this.environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> GuardarArchivo(IFormFile archivo)
        {

            string wwwPath = environment.WebRootPath; //ruta de la carpeta wwroot
            string contentPath = environment.ContentRootPath;

            string path = Path.Combine(environment.WebRootPath, "Archivos"); //mezcla de la ruta de la carpeta y el nombre del archivo

            if (!Directory.Exists(path)) //Creacion del directorio
            {
                Directory.CreateDirectory(path);
            }


            string fileName = Path.GetFileName(archivo.FileName); //se crea el nombre del archivo
            using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create)) 
            {
                await archivo.CopyToAsync(stream);

                ViewBag.Message += string.Format("<b>{0}</b> Archivo subido.<br />", fileName);
            }
           
            return View("Views/Home/Index.cshtml");
        }

        public string[] ObtenerInformacionCertificado(byte[] certificadoEnByte)
        {
            X509Certificate2 certEmisor = new X509Certificate2(); // Genera un objeto del tipo de certificado          

            certEmisor.Import(certificadoEnByte); // Importa los datos del certificado qeu acabas de leer

            string[] datosCertificado =
            {
                certEmisor.GetExpirationDateString(), //fecha
                certEmisor.GetSerialNumberString(), // serial
                certEmisor.GetPublicKeyString(), // clave publica
                certEmisor.GetName(), // nombre del propietario de certificado concatenado con rfc, correo, serie etc
                certEmisor.GetIssuerName(),  // nombre quien emitio el certificado    
                certEmisor.Subject, // obtine el nombre destino del propietarios de certificado    
                "" // aqui hira el rfc

            };

            var numeroSerie = datosCertificado[1];
            var numeroSerieFormateado = "";

            for (int i = 0; i < numeroSerie.Length; i++)
            {
                if (i % 2 != 0)
                {
                    numeroSerieFormateado = numeroSerieFormateado + numeroSerie.Substring(i, 1);

                }

            }
            datosCertificado[1] = numeroSerieFormateado;

            //string patrones = @"(SERIALNUMBER=)|(OID.2.5.4.45=)|(E=)|(C=)|(,)|(O=)|(OID.2.5.4.41=)|(CN=)";
            string patrones = @"(SERIALNUMBER=)|(OID.2.5.4.45=)|(E=)|(C=)|(,)|(/)|(O=)|(OID.2.5.4.41=)|(CN=)";

            bool rfcEncontrado = false, nombreEncontrado = false;

            foreach (string resultado in Regex.Split(datosCertificado[5], patrones)) // certificado[5] son los datos del nombre
            {
                if (rfcEncontrado)
                    datosCertificado[6] = resultado;

                rfcEncontrado = false;

                if (nombreEncontrado)
                    datosCertificado[5] = resultado;

                nombreEncontrado = false;

                if (resultado == "OID.2.5.4.45=")
                    rfcEncontrado = true;

                if (resultado == "OID.2.5.4.41=")
                    nombreEncontrado = true;
            }


            return datosCertificado;


        }
    }
}
