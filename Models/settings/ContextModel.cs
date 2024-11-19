using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlantasBackend.Models.settings
{
    public class ContextModel
    {
        [Required]
        public string? ConnectionStrings { get; set; }
        [Required]
        public string? DatabaseName { get; set; }
    }
}