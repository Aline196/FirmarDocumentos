using Microsoft.AspNetCore.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Org.BouncyCastle.Bcpg;
using FirmarDocumentos.Models;

namespace FirmarDocumentos.Controllers
{
    public class ProcesarPdf : Controller
    {
        string pathOldFile = @""; //Si se ingresa el archivo no debo de almacenar la ruta, lo que debo de pasar es el archivo directo
        string pathNewFile = @"";// ruta que indica donde se guardara el nuevo archivo que ya contiene la modificacion
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ModificarPdf(DocumentoFirmarViewModel docFirmar)
        {
            PdfReader read = new PdfReader(pathOldFile);
            Rectangle size = read.GetPageSizeWithRotation(1);
            Document document= new Document(size);

            FileStream archivo = new FileStream(pathNewFile, FileMode.Create, FileAccess.Write);
            PdfWriter write = PdfWriter.GetInstance(document, archivo);
            document.Open();

            PdfContentByte modificado = write.DirectContent;
            modificado.BeginText();
            //Agregar los datos que se optienen de el certificado en una variable text

            modificado.ShowTextAligned(1, text, 30, size.Height, 0);
            modificado.EndText();

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
