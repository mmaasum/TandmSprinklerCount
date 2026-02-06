using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TandmSprinklerCount.Models.Responses;
using TandmSprinklerCount.Services;

namespace TandmSprinklerCount.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FireDesignController : ControllerBase
    {
        private readonly ISprinklerService _sprinklerService;
        private readonly ILogger<FireDesignController> _logger;

        public FireDesignController(
            ISprinklerService sprinklerService,
            ILogger<FireDesignController> logger)
        {
            _sprinklerService = sprinklerService;
            _logger = logger;
        }

        /// <summary>
        /// Calculates sprinkler layout and pipe connections.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(FireDesignResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<FireDesignResponse>> Get()
        {
            _logger.LogInformation("Calculating sprinkler layout.");

            try
            {
                var sprinklerList = (await _sprinklerService.GenerateLayoutAsync())?.ToList();

                if (sprinklerList == null || sprinklerList.Count == 0)
                {
                    _logger.LogWarning("No sprinklers could be placed.");
                    return NotFound(new FireDesignResponse
                    {
                        NumberOfSprinklers = 0,
                        Sprinklers = new List<SprinklerLayout>()
                    });
                }

                var response = new FireDesignResponse
                {
                    NumberOfSprinklers = sprinklerList.Count,
                    Sprinklers = sprinklerList
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while calculating sprinkler layout.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An internal error occurred." });
            }
        }

    }
}
