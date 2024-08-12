using System;
using Treasure.Models;

namespace Treasure.Service;

public interface ITreasureService
{
    ProblemPagingResponseDTO GetPaging(ProblemQueryDTO queryDTO);
    ProblemDTO GetProblemById(int id);
}
