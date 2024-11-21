using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlantasBackend.Collections;
using PlantasBackend.Dto;
using PlantasBackend.Interfaces;
using PlantasBackend.Models;
using PlantasBackend.Repositories;
using PlantasBackend.Utils;

namespace PlantasBackend.Services
{
    public class PlantsService
    {
        private readonly IDataCrud<PlantsModel> _interface;
        private readonly IOneData<PlantsModel> _oneData;
        private readonly upImage _upImage;

        public PlantsService(IDataCrud<PlantsModel> dataCrud, upImage image, IOneData<PlantsModel> oneData)
        {
            _interface = dataCrud ?? throw new ArgumentNullException(nameof(dataCrud));
            _upImage = image;
            _oneData = oneData ?? throw new ArgumentNullException(nameof(_oneData));
        }


        // method for into one plants.
        public async Task<bool> InsertOneAsync(AllPlantDto model)
        {
            try
            {
                (string Imagen, string IdImagen) = await _upImage.UpCloudinary(model.Image, "Plants");

                var modelo = new PlantsModel
                {
                    Name = model.Name,
                    Description = model.Description,
                    Imagen = Imagen,
                    IdImagen = IdImagen,
                    PlantFamilyId = model.PlantFamilyId,
                    DiseaseIds = model.DiseaseIds,
                };
                _upImage.DeleteDir();
                await _interface.InsertData(modelo);
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
                throw new ApplicationException($"Failed to insert the plant - {ex.Message}");
            }
        }

        // method for update one plants.
        public async Task<bool> UpdateOneAsync(PlantsModel model)
        {
            try
            {
                await _interface.UpdateData(model);
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
                throw new ApplicationException($"Failed to update the plant - {ex.Message}");
            }
        }

        // method for delete one plants.
        public async Task<bool> DeleteOneAsync(string id)
        {
            try
            {
                var resul = await GetOneAsync(id);
                await _upImage.DeleteCloudinary(resul.IdImagen);
                await _interface.DeleteById(id);
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
                throw new ApplicationException($"Could not delete {id} - {ex.Message}");
            }
        }

        // method for get data of one disease
        public Task<PlantsModel> GetOneAsync(string id)
        {
            try
            {
                return _interface.GetById(id);
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Not found data _ {ex.Message}");
            }
        }

        // method for getting something data from of diseases
        public Task<List<PlantsModel>> GetAllAsync()
        {
            try
            {
                return _interface.GetAllData();
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Failed to get all data - {ex.Message}");
            }
        }

        //method for getting one plant for name 
        public async Task<PlantsModel> GetByNameAsync(string name)
        {
            try
            {
                return await _oneData.GetOneData(name);
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"could not request data for {name} - {ex.Message}");
            }
        }

    }
}