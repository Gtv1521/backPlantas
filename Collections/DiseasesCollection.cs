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
    public class DiseasesCollection : IDataCrud<DiseasesModel>, IOneData<DiseasesModel>
    {
        private readonly IMongoCollection<DiseasesModel> _collection;
        public DiseasesCollection(Context context)
        {
            _collection = context.GetCollection<DiseasesModel>("Diseases");
        }

        public async Task DeleteById(string id)
        {
            try
            {
                var filter = Builders<DiseasesModel>.Filter.Eq("_id", new ObjectId(id));
                await _collection.DeleteOneAsync(filter);
            }
            catch (System.Exception ex)
            {

                throw new ApplicationException($"Could not delete {id} - {ex.Message}");
            }
        }

        public async Task<List<DiseasesModel>> GetAllData()
        {
            try
            {
                return await _collection.FindAsync(new BsonDocument()).Result.ToListAsync();
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Could not get all diseases - {ex.Message}");
            }
        }

        public async Task<DiseasesModel> GetById(string id)
        {
            try
            {
                return await _collection.FindAsync(new BsonDocument { { "_id", new ObjectId(id) } }).Result.FirstOrDefaultAsync();
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Could not get data for {id} - {ex.Message}");
            }
        }

        public Task<DiseasesModel> GetOneData(string name)
        {
            throw new NotImplementedException();
        }

        public async Task InsertData(DiseasesModel model)
        {
            try
            {
                await _collection.InsertOneAsync(model);
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Could not insert data for disease - {ex.Message}");
            }
        }

        public Task UpdateData(DiseasesModel model)
        {
            try
            {
                var filter = Builders<DiseasesModel>.Filter.Eq(s => s.Id, model.Id);
                return _collection.ReplaceOneAsync(filter, model);
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Could not update data for disease - {ex.Message}");
            }
        }
    }
}