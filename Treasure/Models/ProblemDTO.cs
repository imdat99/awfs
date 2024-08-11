using Treasure.Data.Entities;

namespace Treasure.Models
{
    public class ProblemDTO : ProblemData
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public decimal? Result { get; set; } = null;
        public bool? IsSuccess { get; set; }
        public required List<List<int>> Matrix { get; set; }
    }
    public class ProblemPagingDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal? Result { get; set; } = null;
    }
    public class ProblemQueryDTO
    {
        public bool? IsSolved { get; set; }
        public string? Title { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    public class ProblemPagingResponseDTO
    {
        public IEnumerable<ProblemPagingDTO> Items { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
    }
}
