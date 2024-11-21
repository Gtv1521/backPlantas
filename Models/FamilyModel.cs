using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PlantasBackend.Models
{
    public class FamilyModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string? Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
    }
}