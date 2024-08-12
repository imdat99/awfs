using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Treasure.Data.Entities;

public partial class ProblemData
{
    public int Id { get; set; }
    public int ProblemId { get; set; }

    public int Row { get; set; }

    public int Col { get; set; }

    public int? ChestTypes { get; set; }
    public byte[] Matrix { get; set; } = null!;
    public virtual Problem Problem { get; set; } = null!;
}
