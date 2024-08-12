using Microsoft.AspNetCore.Mvc;
using Treasure.Models;
using Treasure.Service;

namespace Treasure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProblemController(ILogger<ProblemController> logger, ITreasureService treasureService) : ControllerBase
    {
        private readonly ILogger<ProblemController> _logger = logger;
        private readonly ITreasureService _treasureService = treasureService;

        [HttpGet(Name = "GetProblem")]
        public ProblemPagingResponseDTO Get([FromQuery] ProblemQueryDTO parms)
        {
            return _treasureService.GetPaging(parms);
        }
        [HttpPost(Name = "AddProblem")]
        public Task<Boolean> Post([FromBody] ProblemDTO problemModel, [FromQuery] Boolean isResolveNow)
        {
            return Task.FromResult(true);
        }
    }
}
