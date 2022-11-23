using iTextSharp.text.io;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace FirmarDocumentos.Models
{
    public class DocumentoFirmarViewModel
    {
        [Required]
        public HttpPostedFileBase DocumentoPdf { get; set; }
    }
}
