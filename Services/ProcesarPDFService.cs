using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;


namespace FirmarDocumentos.Services
{
    //Servicio:Logica de negocio
    public class ProcesarPDFService : IProcesarPDFService
    {
        private readonly IHostingEnvironment environment;

        private string path = @"C:\Users\megonzalez\source\repos\FirmarDocumentos\wwwroot\Archivos\Cuarto Acuerdo.pdf";

        public ProcesarPDFService(IHostingEnvironment environment)
        {
            this.environment = environment;
        }

        //Metodo para guardar el pdf
        public async Task GuardarArchivoAsync(IFormFile archivo)
        {
            path = Path.Combine(environment.WebRootPath, "Archivos"); 

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

       


        public async Task ModificarPdf()
        {
            //string pathOldFile = Path; //Si se ingresa el archivo no debo de almacenar la ruta, lo que debo de pasar es el archivo directo
            string pathNewFile = @"C:\Users\megonzalez\source\repos\FirmarDocumentos\wwwroot\Archivos\Cuarto AcuerdoModificado.pdf";
            string pathOldFile = path;

            PdfReader read = new PdfReader(pathOldFile); //Lee el pdf original
            Rectangle size = read.GetPageSizeWithRotation(1); //obtiene el tamaño de nuestro pdf
            Document document = new Document(size);//documento de itextsharp para realizar el trabajo asignandole el tamaño del original

            // Creamos el objeto en el cual haremos la inserción
            FileStream archivo = new FileStream(pathNewFile, FileMode.Create, FileAccess.ReadWrite);
            PdfWriter write = PdfWriter.GetInstance(document, archivo);

            document.Open();

            

            //El contenido del pdf, aqui se hace la escritura del contenido
            PdfContentByte modificado = write.DirectContent;

            //crea una nueva pagina y agrega el pdf original
            PdfImportedPage nuevaPagina = write.GetImportedPage(read, 1);
            modificado.AddTemplate(nuevaPagina, 0, 0);

            //Propiedades de nuestra fuente a insertar
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            modificado.SetColorFill(BaseColor.RED);
            modificado.SetFontAndSize(bf, 15);

            //Se abre el flujo para escribir el texto
            modificado.BeginText();
            //Se asigna el texto
            string text = "HOLA SOY TU MODIFICACION";
            //Agregar los datos que se optienen de el certificado en una variable text

            // Le damos posición y rotación al texto
            // la posición de Y es al revés de como estamos acostumbrados
            modificado.ShowTextAligned(1, text, 30, size.Height-30, 0);
            modificado.EndText();

            

            document.Close();
            archivo.Close();
            write.Close();
            read.Close();

        }




    }
}
