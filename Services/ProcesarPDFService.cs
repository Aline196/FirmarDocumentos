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
        string path;

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

       


        public IActionResult ModificarPdf(ref string path)
        {
            //string pathOldFile = Path; //Si se ingresa el archivo no debo de almacenar la ruta, lo que debo de pasar es el archivo directo
            string pathNewFile = @"C:\Users\megonzalez\Documents\Desarollo SAT";
            string pathOldFile = path;

            PdfReader read = new PdfReader(pathOldFile); //Lee el pdf original
            Rectangle size = read.GetPageSizeWithRotation(1); //obtiene el tamaño de nuestro pdf
            Document document = new Document(size);//documento de itextsharp para realizar el trabajo asignandole el tamaño del original

            // Creamos el objeto en el cual haremos la inserción
            FileStream archivo = new FileStream(pathNewFile, FileMode.Create, FileAccess.Write);
            PdfWriter write = PdfWriter.GetInstance(document, archivo);
            document.Open();

            //El contenido del pdf, aqui se hace la escritura del contenido
            PdfContentByte modificado = write.DirectContent;

            //Propiedades de nuestra fuente a insertar
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            modificado.SetColorFill(BaseColor.RED);
            modificado.SetFontAndSize(bf, 8);

            //Se abre el flujo para escribir el texto
            modificado.BeginText();
            //Se asigna el texto
            string text = "HOLA SOY TU MODIFICACION";
            //Agregar los datos que se optienen de el certificado en una variable text

            // Le damos posición y rotación al texto
            // la posición de Y es al revés de como estamos acostumbrados
            modificado.ShowTextAligned(1, text, 30, size.Height, 0);
            modificado.EndText();

            //crea una nueva pagina y agrega el pdf original
            PdfImportedPage pagina = write.GetImportedPage(read, 1);
            modificado.AddTemplate(pagina, 0, 0);

            document.Close();
            archivo.Close();
            write.Close();
            read.Close();



            return null;
        }




    }
}
