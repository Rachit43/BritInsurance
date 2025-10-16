using System;
using System.Collections.Generic;

namespace BritInsurance.Domain.Entities;

public partial class Item
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Product Product { get; set; } = null!;
}
