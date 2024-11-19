using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlantasBackend.Interfaces;
using PlantasBackend.Models;

namespace PlantasBackend.Services
{
    public class DiseasesService
    {
        private readonly IDataCrud<DiseasesModel> _diseases;
        public DiseasesService(IDataCrud<DiseasesModel> diseases)
        {
            _diseases = diseases;
        }

        public async Task<bool> DeleteOneAsync(string id)
        {
            try
            {
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

        public async Task<bool> InsertOneAsync(DiseasesModel model)
        {
            try
            {
                await _diseases.InsertData(model);
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