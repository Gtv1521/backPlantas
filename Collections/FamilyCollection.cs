using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using PlantasBackend.Interfaces;
using PlantasBackend.Models;
using PlantasBackend.Repositories;

namespace PlantasBackend.Collections
{
    public class FamilyCollection : IDataCrud<FamilyModel>
    {
        private readonly IMongoCollection<FamilyModel> _collection;
        public FamilyCollection(Context context)
        {
            _collection = context.GetCollection<FamilyModel>("Family");
        }

        public async Task DeleteById(string id)
        {
            try
            {
                await _collection.DeleteOneAsync(id);
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Error deleting family - {ex.Message}");
            }
        }

        public async Task<List<FamilyModel>> GetAllData()
        {
            try
            {
                return await _collection.FindAsync<FamilyModel>(new BsonDocument()).Result.ToListAsync();
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Could not find familys - {ex.Message}");
            }
        }

        public async Task<FamilyModel> GetById(string id)
        {
            try
            {
                return await _collection.FindAsync(new BsonDocument { { "_id", new ObjectId(id) } }).Result.FirstOrDefaultAsync();
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Could not find data of {id} - {ex.Message}");
            }
        }

        public async Task InsertData(FamilyModel model)
        {
            try
            {
                await _collection.InsertOneAsync(model);
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Could not insert data famamily - {ex.Message}");
            }
        }

        public async Task UpdateData(FamilyModel model)
        {
            try
            {
                var filter = Builders<FamilyModel>.Filter.Eq(s => s.Id, model.Id);
                await _collection.ReplaceOneAsync(filter, model);
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Could not update data of family - {model.Id} - {ex.Message}");
            }
        }
    }
}