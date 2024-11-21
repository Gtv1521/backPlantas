using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlantasBackend.Dto.Diseases;
using PlantasBackend.Models;
using PlantasBackend.Models.Responses;
using PlantasBackend.Services;

namespace PlantasBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Plague : ControllerBase
    {
        private readonly ILogger<PlagueModel> _logger;
        private readonly PlagueService _service;
        public Plague(ILogger<PlagueModel> logger, PlagueService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// Get something list of data for plagues
        /// </summary>
        /// <returns></returns>
        /// <response code="200">List plagues</response>
        /// <response code="404">Not found plagues</response>
        /// <response code="500">Server error</response>
        [HttpGet]
        [Route("view_all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PlagueModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllPlague()
        {
            try
            {
                _logger.LogInformation("Request data for all plagues -- {Time}", DateTime.UtcNow);
                var result = await _service.GetAllAsync();
                if (result.Count > 0) return NotFound(new ResultData { StatusCode = 404, Message = "Not found data fot plague" });
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError("Failed at get data for all plagues -- {Time}", DateTime.UtcNow);
                return Problem(ex.Message, "/api/Plague/view_all", 500, $"Server error");
            }
        }

        /// <summary>
        /// Get plague for id of requested 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/Plague/view_plague/{id}
        ///     {
        ///         "id": "668eab036a05f5dfcb196f32"
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Data plague</response>
        /// <response code="404">Not found data plague</response>
        /// <response code="500">Server error</response>
        [HttpGet]
        [Route("view_plague/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlagueModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> GetPlagueById(string id)
        {
            try
            {
                _logger.LogInformation("Request data for all plagues  -- {@id} -- {Time} ", id, DateTime.UtcNow);
                var result = await _service.GetOneAsync(id);
                if (result == null) return NotFound(new ResultData { StatusCode = 404, Message = "Not found data of plague which id: {id}" });
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError("Failed at get data of plague which id: {@id} - {Time}", id, DateTime.UtcNow);
                return Problem(ex.Message, "/api/Plague/view_plague/{id}", 500, "Server error");
            }
        }

        /// <summary>
        /// Insert data for new plagues
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/Plague/insert
        ///     {
        ///         "Name": "",
        ///         "Description": "Plague",
        ///         "Image": ""
        ///     }
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="200">Plague added</response>
        /// <response code="400">Failed at insert plague</response>
        /// <response code="500">Server error</response>
        [HttpPost]
        [Route("insert")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> InsertPlague([FromForm] DiseasesDto model)
        {
            try
            {
                _logger.LogInformation("Inseting data for new plagues... {Time}", DateTime.UtcNow);
                var result = await _service.InsertOneAsync(model);
                if (result) return Ok(new ResultData { StatusCode = 200, Message = "Plague added successfully" });
                return BadRequest(new ResultData { StatusCode = 400, Message = "Failed at insert data od the plague" });
            }
            catch (System.Exception ex)
            {
                _logger.LogError("Failed at insert data of plague -- {Time}", DateTime.UtcNow);
                return Problem(ex.Message, "/api/Plague/insert", 500, "Server error");
            }
        }

        /// <summary>
        /// Update date of one plague
        /// </summary>
        /// <remarks> 
        /// Sample request:
        /// 
        ///     POST /api/Plague/update
        ///     {
        ///         "Id": "",
        ///         "Name": "",
        ///         "Description": "Plague",
        ///         "Image": ""
        ///     }
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="200">Data update successfully</response>
        /// <response code="400">Failed at update data</response>
        /// <response code="500">Server error</response>
        [HttpPost]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> UpdatePlague([FromForm] DataDiseaseDto model)
        {
            try
            {
                _logger.LogInformation("Update data of plague which id {@id} -- {Time}", model.Id, DateTime.UtcNow);
                var result = await _service.UpdateOneAsync(model);
                if (!result) return BadRequest(new ResultData { StatusCode = 400, Message = "Failed at update data on plague" });
                return Ok(new ResultData { StatusCode = 200, Message = "Update plague successfully" });
            }
            catch (System.Exception ex)
            {
                _logger.LogError("Failed at update data on plague which id: {@id} -- {Time}", model.Id, DateTime.UtcNow);
                return Problem(ex.Message, "/api/Plague/update", 500, "Server error");
            }
        }

        /// <summary>
        /// Delete one plague for id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks> 
        /// Sample request:
        /// 
        ///     DELETE /api/Plague/delete/{id}
        ///     {
        ///         "id": "668d4a0d986faf64730e222b"
        ///     }
        /// </remarks>
        /// <response code="200">Delete plague successfully </response>
        /// <response code="400">Failed delete plague</response>
        /// <response code="500">Server error </response>
        [HttpDelete]
        [Route("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> DeletePlague(string id)
        {
            try
            {
                _logger.LogInformation("Deleting plague which id: {@id} -- {Time}", id, DateTime.UtcNow);
                var result = await _service.DeleteOneAsync(id);
                if (!result) return BadRequest(new ResultData { StatusCode = 400, Message = $"Failed at delete plague which id: {id}" });
                return Ok(new ResultData { StatusCode = 200, Message = "Plage successfully deleted" });
            }
            catch (System.Exception ex)
            {
                _logger.LogError("Failed plague deleted which id : {@id} -- {Time} ", id, DateTime.UtcNow);
                return Problem(ex.Message, "/api/Plague/delete", 500, "Server error");
            }
        }
    }
}