using iText.Kernel.Geom;
using iText.Signatures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Crypto.Tls;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Path = System.IO.Path;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace FirmarDocumentos.Services
{
    public class ProcesarCredencialesService : IProcesarCredencialesService
    {

        private readonly IHostingEnvironment _environment;

        public ProcesarCredencialesService(IHostingEnvironment environment)
        {
            this._environment = environment;
        }

        //Metodo para recibir el Certificado
        public async Task GuardarCertificadosAsync(IFormFile certificado)
        {
            string path = Path.Combine(_environment.WebRootPath, "Credenciales");

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


        //Método para guardar la CLAVE PRIVADA
        public async Task GuardarclavePrivadaAsync(IFormFile clavePrivada)
        {
            string path = Path.Combine(_environment.WebRootPath, "Credenciales");

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


        //Metodo para obtener los datos del certificado Ingresado
        public string[] ObtenerInformacionCertificado(string rutaCertificado)
        {
            X509Certificate2 certEmisor = new X509Certificate2(rutaCertificado); // Genera un objeto del tipo de certificado          

            string[] datosCertificado =
            {
                //certEmisor.GetExpirationDateString(), //fecha
                //certEmisor.GetSerialNumberString(), // serial
                //certEmisor.GetPublicKeyString(), // clave publica
                //certEmisor.GetName(), // nombre del propietario de certificado concatenado con rfc, correo, serie etc
                //certEmisor.GetIssuerName(),  // nombre quien emitio el certificado    
                //certEmisor.Subject, // obtine el nombre destino del propietarios de certificado    
                //"" // aqui ira el rfc

                certEmisor.GetIssuerName(),
                certEmisor.GetSerialNumberString(), // serial
                certEmisor.GetName(), // nombre del propietario de certificado concatenado con rfc, correo, serie etc   
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

            foreach (string resultado in Regex.Split(datosCertificado[2], patrones)) // certificado[5] son los datos del nombre
            {
                if (rfcEncontrado)
                    datosCertificado[3] = resultado;

                rfcEncontrado = false;

                if (nombreEncontrado)
                    datosCertificado[2] = resultado;

                nombreEncontrado = false;

                if (resultado == "OID.2.5.4.45=")
                    rfcEncontrado = true;

                if (resultado == "OID.2.5.4.41=")
                    nombreEncontrado = true;
            }


            return datosCertificado;


        }


        //Método para generar el archivo PFX
        public byte[] EmpacarClaves(string rutaCertificado, string rutaClavePrivada)
        {
            using(X509Certificate2 cert = new X509Certificate2(rutaCertificado))
            {
                using(RSA Key = RSA.Create())
                {
                    Key.ImportPkcs8PrivateKey(File.ReadAllBytes(rutaClavePrivada), out _);

                    using (X509Certificate2 certificadoConKEY = cert.CopyWithPrivateKey(Key))
                    {
                        byte[] pkcs12 = certificadoConKEY.Export(X509ContentType.Pfx, "12345678");
                    }
                }
            }
            return null;
         
        }
    }
}
