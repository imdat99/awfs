using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Treasure.Data.Entities;

public partial class Problem
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    [JsonIgnore]
    public virtual ICollection<ProblemData> ProblemData { get; set; } = new List<ProblemData>();
    [JsonIgnore]
    public virtual ICollection<ProblemResult> ProblemResults { get; set; } = new List<ProblemResult>();
}
