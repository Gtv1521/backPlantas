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
    public class PlagueService
    {
        private readonly IDataCrud<PlagueModel> _collection;
        private readonly upImage _image;

        public PlagueService(IDataCrud<PlagueModel> collection, upImage image)
        {
            _collection = collection;
            _image = image;
        }

        public async Task<bool> DeleteOneAsync(string id)
        {
            try
            {
                await _collection.DeleteById(id);
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
                // throw new ApplicationException($"Failed to delete  plague {id} -- {ex.Message} ");
            }
        }

        public Task<List<PlagueModel>> GetAllAsync()
        {
            try
            {
                return _collection.GetAllData();
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Failed get data for plague -- {ex.Message}");
            }
        }

        public async Task<PlagueModel> GetOneAsync(string id)
        {
            try
            {
                return await _collection.GetById(id);
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Failed get data of the plague - {ex.Message}");
            }
        }

        public async Task<bool> InsertOneAsync(DiseasesDto model)
        {
            try
            {
                (string Url, string PublicId) = await _image.UpCloudinary(model.Image, "Plague");
                var modelo = new PlagueModel
                {
                    Name = model.Name,
                    Description = model.Description,
                    Image = Url,
                    IdImage = PublicId
                };

                await _collection.InsertData(modelo);
                return true;
            }
            catch (System.Exception)
            {
                return false;
                // throw new ApplicationException($"Failed at inserting plague {ex.Message}");
            }
        }

        public async Task<bool> UpdateOneAsync(DataDiseaseDto model)
        {
            try
            {
                (string Url, string PublicId) = await _image.UpCloudinary(model.Image, "Plague");
                var modelo = new PlagueModel
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Image = Url,
                    IdImage = PublicId
                };
                await _collection.UpdateData(modelo);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}