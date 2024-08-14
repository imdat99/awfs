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
        public ProblemPagingResponseDTO Get([FromQuery] ProblemQueryDTO parms) => _treasureService.GetPaging(parms);
        [HttpPost(Name = "AddProblem")]
        public Task<Boolean> Post([FromBody] ProblemDTO problemModel, [FromQuery] Boolean isResolveNow) => _treasureService.AddProblem(problemModel, isResolveNow);
        [HttpGet("{id}", Name = "GetProblemById")]
        public ProblemDTO Get(int id) => _treasureService.GetProblemById(id);
        [Route("resolve/{id}")]
        [HttpGet]
        public Task<double> Resovle(int id) => _treasureService.ResolveProblem(id);
        [Route("remove")]
        [HttpPost]
        public Task<bool> Remove([FromBody] int[] id) => _treasureService.RemoveProblem(id);
        [HttpDelete("{id}", Name = "RemoveProblem")]
        public Task<bool> Delete(int id) => _treasureService.RemoveProblem([id]);
        [HttpPut("{id}", Name = "UpdateProblem")]
        public Task<bool> Put(int id, [FromBody] ProblemDTO problemModel) => _treasureService.UpdateProblem(id, problemModel);
    }
}
