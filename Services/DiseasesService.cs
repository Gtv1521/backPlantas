using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlantasBackend.Dto.Diseases;
using PlantasBackend.Interfaces;
using PlantasBackend.Models;
using PlantasBackend.Utils;

namespace PlantasBackend.Services
{
    public class DiseasesService
    {
        private readonly IDataCrud<DiseasesModel> _diseases;
        private readonly upImage _utilityImage;
        public DiseasesService(IDataCrud<DiseasesModel> diseases, upImage utilityImage)
        {
            _diseases = diseases;
            _utilityImage = utilityImage;
        }

        public async Task<bool> DeleteOneAsync(string id)
        {
            try
            {
                var diseases = await GetOneAsync(id);
                await _utilityImage.DeleteCloudinary(diseases.IdImage);
                await _diseases.DeleteById(id);
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
                throw new ApplicationException($"could not delete disease '{id}' - {ex.Message}");
            }
        }

        public async Task<List<DiseasesModel>> GetAllAsync()
        {
            try
            {
                return await _diseases.GetAllData();
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Could not get data for diseases - {ex.Message}");
            }
        }

        public async Task<DiseasesModel> GetOneAsync(string id)
        {
            try
            {
                return await _diseases.GetById(id);
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Cound not found disease with {id} - {ex.Message}");
            }
        }

        public async Task<bool> InsertOneAsync(DiseasesDto model)
        {
            try
            {
                (string Image, string IdImage) = await _utilityImage.UpCloudinary(model.Image, "Diseases");
                var modelo = new DiseasesModel{
                    DiseasesName = model.Name,
                    DiseasesDescription = model.Description,
                    Imagen = Image,
                    IdImage = IdImage,
                }; 
                await _diseases.InsertData(modelo);
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
                throw new ApplicationException($"Could not insert data for disease - {ex.Message}");
            }
        }

        public async Task<bool> UpdateOneAsync(DiseasesModel model)
        {
            try
            {
                await _diseases.UpdateData(model);
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
                throw new ApplicationException($"Could not update disease '{model.DiseasesName}' - {ex.Message}");
            }
        }
    }
}