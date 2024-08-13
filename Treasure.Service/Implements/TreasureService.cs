using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Text;
using Treasure.Data.Entities;
using Treasure.Models;
using Treasure.Service.Implements;
using Treasure.Service.Utils;

namespace Treasure.Service;
public class TreasureService(ILogger<TreasureService> logger, TreasureContext context) : ITreasureService
{
    private readonly ILogger<TreasureService> _logger = logger;
    private readonly TreasureContext _context = context;
    public async Task<ProblemPagingResponseDTO> GetPaging(ProblemQueryDTO request)
    {
        Expression<Func<Problem, bool>> condition = x => true;

        if (!string.IsNullOrEmpty(request.Title))
        {
            condition = condition.AndAlso(x => x.Title.Contains(request.Title));
        }
        if (request.IsSolved != null)
        {
            condition = condition.AndAlso(x => (bool)request.IsSolved ? x.ProblemResult.Result != null : x.ProblemResult.Result == null);
        }
        var result = _context.Problems
            .Where(condition)
            .Select(p => new ProblemPagingDTO
            {
                Id = p.Id,
                Title = p.Title,
                Result = p.ProblemResult.Result
            })
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize);
        var totalCount = _context.Problems.Count(condition);
        return new ProblemPagingResponseDTO
        {
            Items = result,
            TotalCount = totalCount,
            CurrentPage = request.Page,
            TotalPage = (int)Math.Ceiling((double)totalCount / request.PageSize)
        };
    }

    public ProblemDTO GetProblemById(int id)
    {
        var result = _context.Problems.Select(p => new ProblemDTO
        {
            Id = p.Id,
            Title = p.Title,
            Matrix = JsonConvert.DeserializeObject<List<List<int>>>(Encoding.UTF8.GetString(p.ProblemData.Matrix)),
            Row = p.ProblemData.Row,
            Col = p.ProblemData.Col,
            Result = p.ProblemResult.Result,
            ChestTypes = p.ProblemData.ChestTypes,
        }).FirstOrDefault(p => p.Id == id);

        return result ?? throw new Exception("Notfound");
    }
    public async Task<Boolean> AddProblem(ProblemDTO problemModel, Boolean isResolveNow)
    {
        using var dbTransaction = _context.Database.BeginTransaction();
        try
        {
            var problem = new Problem { Title = problemModel.Title };
            _context.Problems.Add(problem);
            await _context.SaveChangesAsync();
            var matrixData = JsonConvert.SerializeObject(problemModel.Matrix);
            var problemData = new ProblemData
            {
                ProblemId = problem.Id,
                Row = problemModel.Row,
                Col = problemModel.Col,
                ChestTypes = problemModel.ChestTypes,
                Matrix = Encoding.UTF8.GetBytes(matrixData)
            };
            _context.ProblemData.Add(problemData);
            await _context.SaveChangesAsync();
            if (isResolveNow)
            {
                var result = TreasureResolve.Solve(problemModel.Row, problemModel.Col, (int)problemModel.ChestTypes, problemModel.Matrix);
                _context.ProblemResults.Add(new ProblemResult { ProblemId = problem.Id, Result = (decimal)result });
                await _context.SaveChangesAsync();
            }
            dbTransaction.Commit();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding problem");
            dbTransaction.Rollback();
            return false;
        }
    }
    public async Task<double> ResolveProblem(int id)
    {
        var problemData = _context.ProblemData.FirstOrDefault(p => p.ProblemId == id);
        if (problemData == null)
        {
            throw new Exception("Notfound");
        }
        var matrixData = JsonConvert.DeserializeObject<List<List<int>>>(Encoding.UTF8.GetString(problemData.Matrix));
        var result = TreasureResolve.Solve(problemData.Row, problemData.Col, (int)problemData.ChestTypes, matrixData);
        _context.ProblemResults.Add(new ProblemResult { ProblemId = id, Result = (decimal)result });
        await _context.SaveChangesAsync();
        return result;
    }

    public async Task<bool> RemoveProblem(int[] ids)
    {
        try
        {
            var problems = _context.Problems.Where(p => ids.Contains(p.Id));
            _context.Problems.RemoveRange(problems);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing problem");
            return false;
        }
    }

    public async Task<bool> UpdateProblem(int id, ProblemDTO problemModel)
    {
        using var dbTransaction = _context.Database.BeginTransaction();
        try
        {
            var problem = _context.Problems.FirstOrDefault(p => p.Id == id);
            var problemData = _context.ProblemData.FirstOrDefault(p => p.ProblemId == id);
            if (problem == null || problemData == null)
            {
                throw new Exception("Notfound");
            }
            problem.Title = problemModel.Title;
            _context.Problems.Update(problem);

            var matrixData = JsonConvert.SerializeObject(problemModel.Matrix);
            problemData.Row = problemModel.Row;
            problemData.Col = problemModel.Col;
            problemData.ChestTypes = problemModel.ChestTypes;
            problemData.Matrix = Encoding.UTF8.GetBytes(matrixData);
            _context.ProblemData.Update(problemData);
            if (problem.ProblemResult != null) { 
                _context.ProblemResults.Remove(problem.ProblemResult); 
            }
            await _context.SaveChangesAsync();
            dbTransaction.Commit();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating problem");
            dbTransaction.Rollback();
            return false;
        }
    }
}
