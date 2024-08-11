using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Treasure.Data.Entities;

public partial class ProblemResult
{
    public int Id { get; set; }
    [JsonIgnore]
    public int ProblemId { get; set; }

    public decimal? Result { get; set; }
    [JsonIgnore]
    public virtual Problem Problem { get; set; } = null!;
}
