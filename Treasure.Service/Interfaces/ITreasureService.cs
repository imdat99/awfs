using System;
using Treasure.Models;

namespace Treasure.Service;

public interface ITreasureService
{
    Task<ProblemPagingResponseDTO> GetPaging(ProblemQueryDTO queryDTO);
    ProblemDTO GetProblemById(int id);
    Task<Boolean> AddProblem(ProblemDTO problemModel, Boolean isResolveNow);
    Task<double> ResolveProblem(int id);
    Task<bool> RemoveProblem(int[] id);
    Task<bool> UpdateProblem(int id, ProblemDTO problemModel);
}
