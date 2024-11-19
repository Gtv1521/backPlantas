using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PlantasBackend.Models
{
    public class PlantsModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Imagen { get; set; }
        public string? IdImagen { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? PlantFamilyId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]  
        public List<string>? DiseaseIds { get; set; }

    }
}