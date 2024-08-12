using System;
using Treasure.Models;

namespace Treasure.Service;
public class TreasureService : ITreasureService
{
    public ProblemPagingResponseDTO GetPaging(ProblemQueryDTO queryDTO)
    {
        return new ProblemPagingResponseDTO();
    }

    public ProblemDTO GetProblemById(int id)
    {
        return new ProblemDTO { Title = "", Matrix = new List<List<int>>() { } };
    }
}
