using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Path = System.IO.Path;

namespace FirmarDocumentos.Services
{
    //Servicio:Logica de negocio
    public class ProcesarPDFService : IProcesarPDFService
    {
        private readonly IHostingEnvironment _environment;

        private string path = @"C:\Users\Aline\source\repos\FirmarDocumentos\wwwroot\Archivos\Primer acuerdo.pdf";
        //private string path = @"C:\Users\megonzalez\source\repos\FirmarDocumentos\wwwroot\Archivos\Primer acuerdo.pdf";
        //private string path;
        public ProcesarPDFService(IHostingEnvironment environment)
        {
            this._environment = environment;
        }

        //Metodo para guardar el pdf
        public async Task GuardarArchivoAsync(IFormFile archivo)
        {
            path = Path.Combine(_environment.WebRootPath, "Archivos"); 

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

       


        public async Task ModificarPdf(string [] informacionCertificado)
        {
            //string pathOldFile = Path; //Si se ingresa el archivo no debo de almacenar la ruta, lo que debo de pasar es el archivo directo
            //string pathNewFile = @"C:\Users\megonzalez\source\repos\FirmarDocumentos\wwwroot\Archivos\Cuarto AcuerdoModificado.pdf";
           
            

            string file = @"C:\Users\Aline\source\repos\FirmarDocumentos\wwwroot\Archivos\Tercer acuerdo.pdf";
            string fileModified = @"C:\Users\Aline\source\repos\FirmarDocumentos\wwwroot\Archivos\Tercer acuerdoModificado.pdf";




            // Read pdf and add new page
            PdfDocument pdfDocument = new PdfDocument(
                new PdfReader(file),
                new PdfWriter(fileModified));
           

            // Add text to new page
            Document document = new Document(pdfDocument, pdfDocument.GetDefaultPageSize());

            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

            document.Add(new Paragraph($"Fecha de expiración: {informacionCertificado[0]}"));


            document.Add(new Paragraph($"Número de serie: {informacionCertificado[1]}"));
            document.Add(new Paragraph($"Clave pública: {informacionCertificado[2]}"));
            document.Add(new Paragraph($"Nombre del propietario: {informacionCertificado[3]}"));
            document.Add(new Paragraph($"Emisor: {informacionCertificado[4]}"));
            document.Add(new Paragraph($"Sujeto: {informacionCertificado[5]}"));
            document.Add(new Paragraph($"RFC: {informacionCertificado[6]}"));
            document.Close();
            pdfDocument.Close();

        }




    }
}
