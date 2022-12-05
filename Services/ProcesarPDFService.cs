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
using iText.Signatures;

namespace FirmarDocumentos.Services
{
    //Servicio:Logica de negocio
    public class ProcesarPDFService : IProcesarPDFService
    {
        private readonly IHostingEnvironment _environment;

        private string path = @"C:\Users\megonzalez\source\repos\FirmarDocumentos\wwwroot\Archivos\Primer acuerdo.pdf";
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
           
            //string file = @"C:\Users\Aline\source\repos\FirmarDocumentos\wwwroot\Archivos\Tercer acuerdo.pdf";
            //string fileModified = @"C:\Users\Aline\source\repos\FirmarDocumentos\wwwroot\Archivos\Tercer acuerdoModificado.pdf";


            string file = @"C:\Users\megonzalez\source\repos\FirmarDocumentos\wwwroot\Archivos\Tercer acuerdo.pdf";
            string fileModified = @"C:\Users\megonzalez\source\repos\FirmarDocumentos\wwwroot\Archivos\Tercer Acuerdo-Modificado.pdf";



            // Read pdf 
            PdfDocument pdfDocument = new PdfDocument(
                new PdfReader(file),
                new PdfWriter(fileModified));
           

            // Add text to new page
            Document document = new Document(pdfDocument, pdfDocument.GetDefaultPageSize());
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

            //document.Add(new Paragraph($"e.Firma: e.Firma"));

            //document.Add(new Paragraph($"Fecha de expiración: {informacionCertificado[0]}"));
            //document.Add(new Paragraph($"Número de serie: {informacionCertificado[1]}"));
            //document.Add(new Paragraph($"Clave pública: {informacionCertificado[2]}"));
            //document.Add(new Paragraph($"Nombre del propietario: {informacionCertificado[3]}"));
            //document.Add(new Paragraph($"Emisor: {informacionCertificado[4]}"));
            //document.Add(new Paragraph($"Firmante: {informacionCertificado[5]}"));
            //document.Add(new Paragraph($"RFC: {informacionCertificado[6]}"));

            document.Add(new Paragraph($"e.Firma: e.Firma"));
            document.Add(new Paragraph($"Emisor: {informacionCertificado[0]}"));
            document.Add(new Paragraph($"Clave pública: {informacionCertificado[1]}"));
            document.Add(new Paragraph($"Firmante: {informacionCertificado[2]}"));
            document.Add(new Paragraph($"RFC: {informacionCertificado[3]}"));

            //Proceso de firmado en el pdf
            //PdfSigner firma = new PdfSigner(pdfDocument);

            //PdfSignatureAppearance appearance = firma.GetSignatureAppearance();
            //appearance.SetReason("My reason to sign...")
            //    .SetLocation("Lahore")
            //    .SetPageRect(new Rectangle(36, 648, 200, 100))
            //    .SetPageNumber(1);
            //firma.SetFieldName("MyFieldName");




            document.Close();
            pdfDocument.Close();

        }




    }
}
