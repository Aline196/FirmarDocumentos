
using FirmarDocumentos.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FirmarDocumentos.Controllers //Controlador que manejara la entrada de los archivos
{
    public class FirmaElectronicaController : Controller
    {
        private readonly IHostingEnvironment environment;
        private readonly IProcesarPDFService procesarPDFService;
        private readonly IProcesarCredenciales procesarCredenciales;

        public FirmaElectronicaController(IHostingEnvironment environment, IProcesarPDFService procesarPDFService, IProcesarCredenciales procesarCredenciales)
        {
            this.environment = environment;                                  
            this.procesarPDFService = procesarPDFService;
            this.procesarCredenciales = procesarCredenciales;
        }

        public IActionResult Index()
        {
            return View();
        }


        //Seccion donde estaran los metodos para cargar los archivos
        //Llama al metodo GuardarArchivo para guardar el pedf ingresado
        [HttpPost]
        public async Task GuardarArchivo(IFormFile archivo)
        {
           await procesarPDFService.GuardarArchivoAsync(archivo);
        }

        
        public async Task GuardarCertificados(IFormFile certificado)
        {
            await procesarCredenciales.GuardarCertificadosAsync(certificado);
        }

        

        public async Task GuardarclavePrivada(IFormFile clavePrivada)
        {
            await procesarCredenciales.GuardarclavePrivadaAsync(clavePrivada);
        }




       
    }
}
