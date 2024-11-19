using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using PlantasBackend.Interfaces;
using PlantasBackend.Models;
using PlantasBackend.Repositories;

namespace PlantasBackend.Collections
{
    public class PlantsCollection : IDataCrud<PlantsModel>, IOneData<PlantsModel>
    {
        private readonly IMongoCollection<PlantsModel> _collection;
        public PlantsCollection(Context context)
        {
            _collection = context.GetCollection<PlantsModel>("Plant");
        }

        // delete an collection 
        public async Task DeleteById(string id)
        {
            try
            {
                var filter = Builders<PlantsModel>.Filter.Eq("_id", new ObjectId(id));
                await _collection.DeleteOneAsync(filter);
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Could not delete {id} - {ex.Message}");
            }
        }

        // method return data of somtehing Plants
        public async Task<List<PlantsModel>> GetAllData()
        {
            try
            {
                return await _collection.FindAsync(new BsonDocument()).Result.ToListAsync();
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public async Task<PlantsModel> GetById(string id)
        {
            try
            {
                return await _collection.FindAsync(new BsonDocument { { "_id", new ObjectId(id) } }).Result.FirstOrDefaultAsync();
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Not found data - {ex.Message}");
            }
        }

        public async Task<PlantsModel> GetOneData(string name)
        {
            try
            {
                var filter = Builders<PlantsModel>.Filter.Eq("Name", name);
                var result = await _collection.Find(filter).FirstOrDefaultAsync();
                return result;

                 
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        // Insert data for new plant in database. 
        public async Task InsertData(PlantsModel model)
        {
            try
            {
                await _collection.InsertOneAsync(model);
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Could not insert date - {ex.Message}");
            }
        }

        // update data of plant.
        public async Task UpdateData(PlantsModel model)
        {
            try
            {
                var filter = Builders<PlantsModel>.Filter.Eq(s => s.Id, model.Id);
                await _collection.ReplaceOneAsync(filter, model);
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Failed updated plant - {ex.Message}");
            }
        }


    }
}