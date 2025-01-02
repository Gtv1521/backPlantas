using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlantasBackend.Dto;
using PlantasBackend.Models;
using PlantasBackend.Models.Responses;
using PlantasBackend.Services;

namespace PlantasBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Family : ControllerBase
    {
        private readonly ILogger<FamilyModel> _logger;
        private readonly FamilyService _service;

        public Family(ILogger<FamilyModel> logger, FamilyService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// Get all familys registered 
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Data familys.</response>
        /// <response code="404">No data of family</response>
        /// <response code="500">Error server</response>
        [HttpGet]
        [Route("view_all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FamilyModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllFamily()
        {
            try
            {
                var family = await _service.GetAllAsync();
                if (family.Count == 0) return NotFound(new ResultData { StatusCode = 404, Message = "Not found familys" });
                return Ok(family);
            }
            catch (System.Exception ex)
            {
                _logger.LogWarning($"{DateTime.UtcNow} -- {ex.Message}");
                return Problem(ex.Message, $"/api/Family/view_all", 500, "Server error");
            }
        }

        /// <summary>
        /// Get one family registered
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/Family/view_family/{id}
        ///     {
        ///         "id": "673424ba711637b1f4ee2418"
        ///     }
        /// </remarks>
        /// <response code="200">Data Family</response>
        /// <response code="404">No data family</response>
        /// <response code="500">Error server</response>
        [HttpGet]
        [Route("view_family/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FamilyModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> GetFamilyById(string id)
        {
            try
            {
                _logger.LogInformation($"Getted family {id} - {DateTime.UtcNow}");
                var family = await _service.GetOneAsync(id);
                if (family == null) return NotFound(new ResultData { StatusCode = 404, Message = "Not found data for family" });
                return Ok(family);
            }
            catch (System.Exception ex)
            {
                _logger.LogWarning($"Failed at get data of family which id: {id} -- {DateTime.UtcNow} -- {ex.Message}");
                return Problem(ex.Message, $"/api/Family/view_family/{id}", 500, "Server error");
            }
        }

        /// <summary>
        /// Get data of family for the name 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="404">Not found </response>
        /// <response code="500">server error</response>
        [HttpGet]
        [Route("view_family_name/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FamilyModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> GetFamilyByName(string name)
        {
            try
            {
                _logger.LogInformation("Get interface family - {Time}", DateTime.UtcNow);
                var result = await _service.GetFamilyPlant(name);
                if (result == null) return NotFound(new ResultData { StatusCode = 404, Message = "Not found data of family" });
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError("Failed get data for family {Time}", DateTime.UtcNow);
                return Problem(ex.Message, "/Api/Family/view_family_name/{name}", 500, "Server error");
            }
        }

        /// <summary>
        /// Insert data into family.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/Family/insert
        ///     {
        ///         "Name": "",
        ///         "Description": "Family"
        ///     }
        /// </remarks>
        /// <response code="200">OK</response>
        /// <response code="400">Error insert data family</response>
        /// <response code="500">Error server</response>
        [HttpPost]
        [Route("insert")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> InsertFamily([FromForm] PlantsDto family)
        {
            try
            {
                _logger.LogInformation($"Insert new family - {DateTime.UtcNow}");
                var result = await _service.InsertOneAsync(family);
                if (result != true) return BadRequest(new ResultData { StatusCode = 400, Message = "Error inserting family" });
                return Ok(new ResultData { StatusCode = 201, Message = "Family created successfully" });
            }
            catch (System.Exception ex)
            {
                _logger.LogWarning($"{DateTime.UtcNow} -- {ex.Message}");
                return Problem(ex.Message, $"/api/Family/insert", 500, "Server error");
            }
        }

        /// <summary>
        /// Update data into family.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/Family/update
        ///     {
        ///         "Id": "",
        ///         "Name": "family new",
        ///         "Description": "new Description"
        ///     }
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="400">Error update data family</response>
        /// <response code="500">Error server</response>
        [HttpPost]
        [Route("update")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateDataFamily([FromForm] FamilyModel model)
        {
            try
            {
                _logger.LogInformation($"Update data family wich id: {model.Id} -- {DateTime.UtcNow}");
                var result = await _service.UpdateOneAsync(model);
                if (result != true) return BadRequest(new ResultData { StatusCode = 400, Message = "Error updating family" });
                return Ok(new ResultData { StatusCode = 200, Message = "Family updated successfully" });
            }
            catch (System.Exception ex)
            {
                _logger.LogWarning($" Failed at update data for family -- {DateTime.UtcNow} -- {ex.Message}");
                return Problem(ex.Message, $"/api/Family/update", 500, "Server error");
            }
        }

        /// <summary>
        /// Delete a family for id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE /api/Family/delete/{id}
        ///     {
        ///         "id": "668d4a0d986faf64730e222b"
        ///     }
        /// </remarks>
        /// <response code="200">OK</response>
        /// <response code="400">Error delete data family</response>
        /// <response code="500">Error server</response>
        [HttpDelete]
        [Route("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteDataFamily(string id)
        {
            try
            {
                _logger.LogInformation($"Deleted family wich id: {id} - {DateTime.UtcNow}");
                var result = await _service.DeleteOneAsync(id);
                if (result != true) return BadRequest(new ResultData { StatusCode = 400, Message = "Failed deleting family" });
                return Ok(new ResultData { StatusCode = 200, Message = "Delete family successfully" });
            }
            catch (System.Exception ex)
            {
                _logger.LogWarning($" Failed at delete family which id. {id} -- {DateTime.UtcNow} -- {ex.Message}");
                return Problem(ex.Message, $"/api/Family/delete/{id}", 500, "Server error");
            }
        }
    }
}