using System;
using System.Collections.Generic;

namespace Treasure.Data.Entities;

public partial class ProblemResult
{
    public int Id { get; set; }

    public int ProblemId { get; set; }

    public bool IsResolved { get; set; }

    public double? Result { get; set; }

    public virtual Problem Problem { get; set; } = null!;
}
