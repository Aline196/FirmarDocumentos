using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Crypto.Tls;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FirmarDocumentos.Services
{
    public class ProcesarCredencialesService : IProcesarCredenciales
    {

        private readonly IHostingEnvironment environment;

        public ProcesarCredencialesService(IHostingEnvironment environment)
        {
            this.environment = environment;
        }

        //Metodo para recibir el Certificado
        public async Task GuardarCertificadosAsync(IFormFile certificado)
        {
            string path = Path.Combine(environment.WebRootPath, "Credenciales");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            string fileName = Path.GetFileName(certificado.FileName);
            using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                await certificado.CopyToAsync(stream);

            }
        }


        //Metodo para obtener los datos del certificado Ingresado
        public string[] ObtenerInformacionCertificado(ref byte[] certificado)
        {
            X509Certificate2 certEmisor = new X509Certificate2(); // Genera un objeto del tipo de certificado          

            certEmisor.Import(certificado); // Importa los datos del certificado que acabas de leer

            string[] datosCertificado =
            {
                certEmisor.GetExpirationDateString(), //fecha
                certEmisor.GetSerialNumberString(), // serial
                certEmisor.GetPublicKeyString(), // clave publica
                certEmisor.GetName(), // nombre del propietario de certificado concatenado con rfc, correo, serie etc
                certEmisor.GetIssuerName(),  // nombre quien emitio el certificado    
                certEmisor.Subject, // obtine el nombre destino del propietarios de certificado    
                "" // aqui ira el rfc

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





        //Metodo para guardar la CLAVE PRIVADA
        public async Task GuardarclavePrivadaAsync(IFormFile clavePrivada)
        {
            string path = Path.Combine(environment.WebRootPath, "Credenciales");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            string fileName = Path.GetFileName(clavePrivada.FileName);
            using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                await clavePrivada.CopyToAsync(stream);

            }
        }





    }
}
