using Andyyy.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Andyyy.Controllers
{
    [Route("api/worksheets")]
    [ApiController]
    public class DapperController : ControllerBase
    {
        private readonly WorkSheetReposity _worksheetReposity;
        private readonly ILogger<DapperController> _logger;
        private IDatabase _cacheDb;

        public DapperController(WorkSheetReposity worksheetReposity, ILogger<DapperController> logger)
        {
            _worksheetReposity = worksheetReposity;
            _logger = logger;
        }

        // GET: api/<DapperController>
        [HttpGet]
        public async Task<IActionResult> GetWorkSheets()
        {
            try
            {
                var worksheets = await _worksheetReposity.GetWorkSheets();
                return Ok(worksheets);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<DapperController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkSheet(int id)
        {

            try
            {
                var result = await _worksheetReposity.GetWorkSheet(id);
                if (result == null)
                    return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<DapperController>
        [HttpPost]
        public async Task<ActionResult<WorkSheet>> CreateWorkSheet([FromBody]WorkSheet worksheet)
        {
            try
            {
                var createdWorksheet = await _worksheetReposity.CreateWorkSheet(worksheet);
                return CreatedAtRoute(new { id = createdWorksheet.Id }, createdWorksheet);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<DapperController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkSheet(int id, WorkSheet worksheet)
        {
            try
            {
                var dbWorkSheet = await _worksheetReposity.GetWorkSheet(id);
                if (dbWorkSheet == null)
                    return NotFound();
                await _worksheetReposity.UpdateWorkSheet(id, worksheet);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<DapperController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkSheet(int id)
        {
            try
            {
                var dbWorkSheet = await _worksheetReposity.GetWorkSheet(id);
                if (dbWorkSheet == null)
                    return NotFound();
                await _worksheetReposity.DeleteWorkSheet(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
    }
}
