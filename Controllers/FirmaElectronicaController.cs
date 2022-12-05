
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
        private readonly IHostingEnvironment _environment;
        private readonly IProcesarPDFService _procesarPDFService;
        private readonly IProcesarCredencialesService _procesarCredenciales;

        public FirmaElectronicaController(
            IHostingEnvironment environment, 
            IProcesarPDFService procesarPDFService,
            IProcesarCredencialesService procesarCredenciales)
        {
            this._environment = environment;                                  
            this._procesarPDFService = procesarPDFService;
            this._procesarCredenciales = procesarCredenciales;
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
           await _procesarPDFService.GuardarArchivoAsync(archivo);
        }

        
        public async Task GuardarCertificados(IFormFile certificado)
        {
            await _procesarCredenciales.GuardarCertificadosAsync(certificado);
        }

        

        public async Task GuardarclavePrivada(IFormFile clavePrivada)
        {
            await _procesarCredenciales.GuardarclavePrivadaAsync(clavePrivada);
        }

        public async Task ModificarPdf()
        {

            // Se haría una llamada a otro servicio para obtener el certificado
            // y el documento asociado a un usuario...


            //var datosUsuario = await ObtenerDatosUsuario(idUsuario);

            //var rutaCertificado = datosUsuario.RutaCertificado;
            //var rutaPdf = datosUsuario.RutaPdf;


            //var rutaCertificado = @"E:\Documents\Documentos personales\FIEL_SAGA990406LI5_20221107122530\saga990406li5.cer";
            var rutaCertificado = @"C:\Users\megonzalez\Documents\Desarollo SAT\2 FIEL_GUPH751126M88_20220301120726\2 FIEL_GUPH751126M88_20220301120726\guph751126m88.cer";
            var informacionCertificado = _procesarCredenciales.ObtenerInformacionCertificado(rutaCertificado);
            await _procesarPDFService.ModificarPdf(informacionCertificado);
        }
        

       
    }
}
