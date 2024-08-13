using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using Treasure.Data.Entities;
using Treasure.Service;
using Treasure.Test.Helper;
using Xunit;
using Treasure.Models;
using System.Threading.Tasks;

namespace Treasure.Test
{
    public class TreasureServiceTest
    {
        private readonly TreasureContext _mockDbContext;
        private readonly string ContextDatabase = Guid.NewGuid().ToString();
        public TreasureServiceTest()
        {
            var context = MockDbContext.CreateInMemoryDbContext(ContextDatabase);
            var problem = new Problem {Id = 112233, Title = "Problem 1"};
            context.Problems.Add(problem);
            var problemData = new ProblemData { ProblemId = 112233, Matrix = Encoding.UTF8.GetBytes("[[2,1,1,1],[1,1,1,1],[2,1,1,3]]"), Row = 3, Col = 4, ChestTypes = 3 };
            context.ProblemData.Add(problemData);
            //var problemResult = new ProblemResult { ProblemId = 112233, Result = 1 };
            //context.ProblemResults.Add(problemResult);
            context.SaveChanges();
            _mockDbContext = MockDbContext.CreateInMemoryDbContext(ContextDatabase);
        }
        private ITreasureService GetService()
        {
            ILogger<TreasureService> logger = new NullLogger<TreasureService>();
            return new TreasureService(logger, _mockDbContext);
        }
        public class PagingTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { "", null, 1, 1};
                yield return new object[] { "ahihi",true, 0, 0};
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        [Theory]
        [ClassData(typeof(PagingTestData))]
        public async Task GetPaging_ShouldReturnCorrectResult(string title, bool isSolve, int expectedTotalCount, int expectedTotalPage)
        {
            var service = GetService();
            var result = await service.GetPaging(new ProblemQueryDTO{ Title = title, IsSolved = isSolve });
            Assert.Equal(expectedTotalCount, result.TotalCount);
            Assert.Equal(expectedTotalPage, result.TotalPage);
        }
        [Fact]
        public void GetById_Ok()
        {
            var service = GetService();
            var result = service.GetProblemById(112233);
            Assert.Equal(112233, result.Id);
        }
        [Fact]
        public void GetById_Fail()
        {
            var service = GetService();
            var exception = Assert.Throws<Exception>(() => service.GetProblemById(1122334));
            Assert.Equal("Notfound", exception.Message);
        }
        [Fact]
        public void ResolveProblem_ok()
        {
            var service = GetService();
            var result = Task.Run(() => service.ResolveProblem(112233));
            Assert.Equal(5, result.Result);
        }
        [Fact]
        public void ResolveProblem_Fail()
        {
            var service = GetService();
            var exception = Assert.ThrowsAsync<Exception>(() => service.ResolveProblem(1122334));
            Assert.Equal("Notfound", exception.Result.Message);
        }
        [Fact]
        public void RemoveProblem_Ok()
        {
            var service = GetService();
            var result = Task.Run(() => service.RemoveProblem(new int[] { 112233 }));
            Assert.True(result.Result);
        }
        [Fact]
        public void AddProblem_Ok()
        {
            var service = GetService();
            var result = Task.Run(() => service.AddProblem(new ProblemDTO{ Id = 112233, Title = "ahihi", Matrix = new List<List<int>>{ new List<int>{1,2,3}, new List<int>{1,2,3} }, Row = 2, Col = 3, ChestTypes = 2 }, true));
            Assert.True(result.Result);
        }[Fact]
        public void AddProblem_Fail()
        {
            var service = GetService();
            var result = Task.Run(() => service.AddProblem(new ProblemDTO{ Id = 112233, Title = "ahihi", Matrix = new List<List<int>>{ new List<int>{1,2,3}, new List<int>{1,2,3} }, Row = 2, Col = 1000, ChestTypes = 2 }, true));
            Assert.True(result.Result == false);
        }
        [Fact]
        public void UpdateProblem_Ok() 
        {
            var service = GetService();
            var result = Task.Run(() => service.UpdateProblem(112233, new ProblemDTO{ Id = 112233, Title = "ahihi", Matrix = new List<List<int>>{ new List<int>{1,2,3}, new List<int>{1,2,3} }, Row = 2, Col = 3, ChestTypes = 2 }));
            Assert.True(result.Result);
        }
        [Fact]
        public void UpdateProblem_Fail()
        {
            var service = GetService();
            var result = Task.Run(() => service.UpdateProblem(1122343, new ProblemDTO { Id = 112233, Title = "ahihi", Matrix = new List<List<int>> { new List<int> { 1, 2, 3 }, new List<int> { 1, 2, 3 } }, Row = 1000, Col = 1000, ChestTypes = 2 }));
            Assert.True(result.Result == false);
        }

    }
}