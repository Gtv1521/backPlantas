using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlantasBackend.Dto;
using PlantasBackend.Dto.Familys;
using PlantasBackend.Interfaces;
using PlantasBackend.Models;

namespace PlantasBackend.Services
{
    public class FamilyService
    {
        private readonly IDataCrud<FamilyModel> _collection;
        private readonly IOneData<FamilyNameDto> _collectionOne;

        public FamilyService(IDataCrud<FamilyModel> collection, IOneData<FamilyNameDto> collectionOne)
        {
            _collection = collection;
            _collectionOne = collectionOne;
        }

        // Method for deleted one family
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
                throw new ApplicationException($"Failed to delete family - {ex.Message}");
            }
        }

        // Get data of domething familys 
        public async Task<List<FamilyModel>> GetAllAsync()
        {
            try
            {
                return await _collection.GetAllData();
            }
            catch (System.Exception ex)
            {

                throw new ApplicationException($"Failed of get data familys - {ex.Message} ");
            }
        }
        
        // method get data for family for the name
        public async Task<List<FamilyNameDto>> GetFamilyPlant(string name)
        {
            try
            {
                return await _collectionOne.GetOneData(name);
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Failed of get data family- {ex.Message}");
            }
        }

        // Get data of one family
        public async Task<FamilyModel> GetOneAsync(string id)
        {
            try
            {
                return await _collection.GetById(id);
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Failed of get data - {ex.Message}");
            }
        }

        // This insert one family a tha database
        public async Task<bool> InsertOneAsync(PlantsDto model)
        {
            try
            {
                var modelo = new FamilyModel
                {
                    Name = model.Name,
                    Description = model.Description,

                };
                await _collection.InsertData(modelo);
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
                throw new ApplicationException($"Failed insert of data family - {ex.Message}");
            }
        }

        // This update data of one family for id
        public async Task<bool> UpdateOneAsync(FamilyModel model)
        {
            try
            {
                await _collection.UpdateData(model);
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
                throw new ApplicationException($"Failed update of data family - {ex.Message}");
            }
        }
    }
}