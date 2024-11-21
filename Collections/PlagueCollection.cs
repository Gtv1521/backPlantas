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
    public class PlagueCollection : IDataCrud<PlagueModel>
    {
        private readonly IMongoCollection<PlagueModel> _db;

        public PlagueCollection(Context context)
        {
            _db = context.GetCollection<PlagueModel>("Plague");
        }

        public async Task DeleteById(string id)
        {
            try
            {
                await _db.DeleteOneAsync(id);
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"failed to delete plague - {ex.Message}");
            }
        }

        public async Task<List<PlagueModel>> GetAllData()
        {
            try
            {
                return await _db.FindAsync(new BsonDocument()).Result.ToListAsync();
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Failed getting data - {ex.Message}");
            }
        }

        public async Task<PlagueModel> GetById(string id)
        {
            try
            {
                return await _db.FindAsync(new BsonDocument { { "_id", new ObjectId(id) } }).Result.FirstOrDefaultAsync();
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Failed getting data - {ex.Message}");
            }
        }

        public async Task InsertData(PlagueModel model)
        {
            try
            {
                await _db.InsertOneAsync(model);
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Failed at inset data to plague {ex.Message}");
            }
        }

        public async Task UpdateData(PlagueModel model)
        {
            try
            {
                var filter = Builders<PlagueModel>.Filter.Eq(s => s.Id, model.Id);
                await _db.ReplaceOneAsync(filter, model);
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Failed updating data - {ex.Message}");
            }
        }
    }
}