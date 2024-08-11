using Microsoft.AspNetCore.Mvc;
using Treasure.Data.Entities;
using Treasure.Models;

namespace Treasure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProblemController(ILogger<ProblemController> logger) : ControllerBase
    {
        private readonly ILogger<ProblemController> _logger = logger;

        [HttpGet(Name = "GetProblem")]
        public Task<ProblemPagingResponseDTO> Get([FromQuery] ProblemQueryDTO parms)
        {
            return Task.FromResult(new ProblemPagingResponseDTO());
        }
        [HttpPost(Name = "AddProblem")]
        public Task<Boolean> Post([FromBody] ProblemDTO problemModel, [FromQuery] Boolean isResolveNow)
        {
            return Task.FromResult(true);
        }
    }
}
