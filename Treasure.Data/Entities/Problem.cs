using System;
using System.Collections.Generic;

namespace Treasure.Data.Entities;

public partial class Problem
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ProblemData? ProblemData { get; set; }

    public virtual ProblemResult? ProblemResult { get; set; }
}
