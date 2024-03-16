using BitMiracle.LibTiff.Classic;
using DocumentFormat.OpenXml.Bibliography;
using System.Diagnostics;

namespace ValidarImagen
{
    class Program
    {
        static void Main(string[] args)
        {
            string ruta = @"I:\Kodak\Bancolombi";
            string rutainforme = @"C:\informe.txt";

            

            Console.WriteLine("Iniciando búsqueda de archivos TIFF");

            try
            {
                GenerarInformeValidacion(ruta, rutainforme);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error durante el proceso: {ex.Message}");
            }

            Console.WriteLine("Presiona cualquier tecla para salir.");
            Console.ReadKey();
        }

        static void GenerarInformeValidacion(string ruta, string rutainforme)
        {
            List<string[]> informacionImagenes = new List<string[]>();
            int archivosProcesados = 0;

            var directoriosEscanear = Directory.GetDirectories(ruta);
            foreach (var dir in directoriosEscanear)
            {
                var directoriosDentroFechas = Directory.GetDirectories(dir);
                foreach (var sobres in directoriosDentroFechas)
                {
                    var archivosTiffEncontrados = Directory.GetFiles(sobres, "*.tif");
                    try
                    {
                        foreach(var archivo in archivosTiffEncontrados)
                        {
                            using (Tiff imagenTiff = Tiff.Open(archivo, "r"))
                            {
                                if (imagenTiff == null)
                                {
                                    //DirectoryInfo directoryInfo = new DirectoryInfo(archivo);

                                    Generarinformetxt(rutainforme, archivo);
                                    Console.WriteLine($"Error al procesar la imagen {imagenTiff}");
                                }
                                Console.WriteLine($"Imagen buena OK {imagenTiff}");
                                Generarinformetxt(rutainforme, archivo);
                                
                            }
                        }            
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al procesar la imagen {archivosTiffEncontrados}: {ex.Message}");
                    }
                }
            }


        }

        static void Generarinformetxt(string rutainf, string archivo)
        {
            FileInfo fileInfo = new FileInfo(archivo);
            bool imagenAbierta = true; 
            DateTime fechaCreacion = fileInfo.CreationTime;
            long tamañoKB = fileInfo.Length / 1024;

            

            using (StreamWriter writer = new StreamWriter(rutainf, true)) 
            {
                
                writer.WriteLine($"{archivo} |  {(imagenAbierta ? "OK" : "ERROR")} |  {fechaCreacion} |   {tamañoKB}");
            }
            Console.WriteLine($"Informe generado en: {rutainf}");
        }







    }


}
