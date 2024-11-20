using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using PlantasBackend.Models.settings;

namespace PlantasBackend.Utils
{
    public class upImage
    {
        private readonly Cloudinary _cloudinary;
        private string _imagePath;
        public upImage(IOptions<CloudinaryModel> settings)
        {
            _cloudinary = new(settings.Value.Url);
        }

        public async Task CreateFolder(string folderName)
        {
            string projectRoot = AppDomain.CurrentDomain.BaseDirectory;

            string folderPath = Path.Combine(projectRoot, folderName);

            // Verificar si la carpeta ya existe
            if (!Directory.Exists(folderPath))
            {
                await Task.Run(() => Directory.CreateDirectory(folderPath));
                System.Console.WriteLine("Folder created");
            }
        }


        public async Task<string> ImageUpload(IFormFile img)
        {
            var Name = Guid.NewGuid().ToString() + ".jpg";//crea un id que sera el nombre dela imagen
            string Route = $"Upload/{Name}"; //reprensenta la carpeta donde se almacena la imagen
            // esta using crea un archivo
            using (var stream = new FileStream(Route, FileMode.Create))
            {
                // esto copia y pega los datos de img y crea un nuevo archivo en la ruta especificada
                await img.CopyToAsync(stream);
            }
            // retorna la ruta del archivo 
            return Route;
        }

        public async Task<(string?, string?)> UpCloudinary(IFormFile image, string path)
        {
            await CreateFolder("Upload"); //crea la carpeta de uploads si no existe
            if (image == null) return (null, null);
            var route = await ImageUpload(image); //se crea la carpeta y se almacena imagen en 
            _cloudinary.Api.Secure = true;
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(@"" + route),
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true,
                Folder = path
            };
            _imagePath = route.ToString();
            var uploadResult = _cloudinary.Upload(uploadParams);
            //guardar Url de la imagen para la base de datos
            string urlImage = Convert.ToString(uploadResult.SecureUrl);
            string IdImage = Convert.ToString(uploadResult.PublicId);

            return (urlImage, IdImage);
        }

        // delete a file from the server in dir upload.
        public void DeleteDir()
        {
            if (_imagePath.Length > 0)
            {
                File.Delete(_imagePath);
            }
        }

        // delete a file from cloudinary
        public async Task DeleteCloudinary(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var resultado = await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}