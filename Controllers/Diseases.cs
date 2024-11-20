using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PlantasBackend.Dto.Diseases;
using PlantasBackend.Models;
using PlantasBackend.Models.Responses;
using PlantasBackend.Services;

namespace PlantasBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Diseases : ControllerBase
    {
        private readonly ILogger<DiseasesModel> _logger;
        private readonly DiseasesService _service;

        public Diseases(ILogger<DiseasesModel> logger, DiseasesService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// Get something data from the diseases registered.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="404">Not fount.</response>
        /// <response code="500">Error internal server.</response>
        [HttpGet]
        [Route("view_all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DiseasesModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllDiseases()
        {
            try
            {
                _logger.LogInformation("Getting all diseases information");
                var diseases = await _service.GetAllAsync();
                if (diseases.Count == 0) return NotFound(new ResultData { StatusCode = 404, Message = "Not found deseases added." });
                return Ok(diseases);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting diseases: {ex.Message}");
                return Problem("An error occurred.", "/api/diseases", 500, "Server Error");
            }
        }

        /// <summary>
        /// Get information of disease for id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks> 
        /// Sample request: 
        /// 
        ///     GET /api/Diseases/view_disease_id/{id}
        ///     {
        ///         "id": "673d0d7ab2310184458dbfde"
        ///     }
        /// </remarks>
        /// <response code="200">Disease.</response>
        /// <response code="404">Not found.</response>
        /// <response code="500">Error server.</response>
        [HttpGet]
        [Route("view_disease/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DiseasesModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByDiseases(string id)
        {
            try
            {
                _logger.LogInformation($"Getting disease information for id: {id}");
                var disease = await _service.GetOneAsync(id);
                if (disease == null) return NotFound(new ResultData { StatusCode = 404, Message = $"Disease with id {id} not found." });
                return Ok(disease);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting disease: {ex.Message}");
                return Problem("An error occurred.", "/api/diseases", 500, "Server Error");
            }
        }

        /// <summary>
        /// Insert one disease.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks> 
        /// Sample request: 
        /// 
        ///     POST /api/Diseases/insert
        ///     {
        ///         "name": "",
        ///         "description": "",
        ///         "image": ""
        ///     }
        /// </remarks>
        /// <response code="200">Disease.</response>
        /// <response code="400">Failed of get data.</response>
        /// <response code="404">Not found.</response>
        /// <response code="500">Error server.</response>
        [HttpPost]
        [Route("insert")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InsertDisease([FromForm] DiseasesDto model)
        {
            try
            {
                if (await _service.InsertOneAsync(model))
                {
                    _logger.LogInformation("Added a new disease.");
                    return Ok(new ResultData
                    {
                        StatusCode = 200,
                        Message = "Disease add successfully."
                    });
                }
                return BadRequest(new ResultData
                {
                    StatusCode = 400,
                    Message = "Error failed insert data."
                });

            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Error getting disease: {ex.Message}");
                return Problem("An error occurred.", "/api/diseases", 500, "Server Error");
            }

        }

        /// <summary>
        /// Update data from disease.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request: 
        /// 
        ///     POST /api/diseases/update
        ///     {
        ///         "id": "",
        ///         "name": "",
        ///         "description": "",
        ///         "image": ""
        ///     }
        /// </remarks>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("update")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateDisease([FromBody] DiseasesModel model)
        {
            try
            {
                if (await _service.UpdateOneAsync(model))
                {
                    _logger.LogInformation("Updated disease.");
                    return Ok(new ResultData
                    {
                        StatusCode = 200,
                        Message = "Disease updated successfully."
                    });
                }
                return BadRequest(new ResultData
                {
                    StatusCode = 400,
                    Message = "Error failed update disease."
                });

            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Error getting disease: {ex.Message}");
                return Problem("An error occurred.", "/api/diseases", 500, "Server Error");
            }
        }

        /// <summary>
        /// Delete a disease for id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks> 
        /// Sample request: 
        /// 
        ///     DELETE /api/Diseases/delete/{id}
        ///     {
        ///         "id": "673d0d7ab2310184458dbfde"
        ///     }
        /// </remarks>
        /// <response code="200">Disease.</response>
        /// <response code="404">Not found.</response>
        /// <response code="500">Error server.</response>
        [HttpDelete]
        [Route("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDisease(string id)
        {
            try
            {
                if (await _service.DeleteOneAsync(id))
                {
                    _logger.LogInformation("Deleted disease.");
                    return Ok(new ResultData
                    {
                        StatusCode = 200,
                        Message = "Disease deleted successfully."
                    });
                }
                return NotFound(new ResultData
                {
                    StatusCode = 404,
                    Message = $"Disease with id {id} not found."
                });

            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Error getting disease: {ex.Message}");
                return Problem("An error occurred.", "/api/diseases", 500, "Server Error");
            }
        }
    }
}