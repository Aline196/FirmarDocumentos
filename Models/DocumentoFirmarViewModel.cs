using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace FirmarDocumentos.Models
{
    public class DocumentoFirmarViewModel
    {
        public IFormFile MyProperty { get; set; }
    }
}
