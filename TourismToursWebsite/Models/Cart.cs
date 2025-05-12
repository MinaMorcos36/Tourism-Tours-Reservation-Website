using System;
using System.Collections.Generic;

namespace TourismToursWebsite.Models;

public partial class Cart
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? TourId { get; set; }

    public int? Quantity { get; set; }

    public DateTime? AddedDate { get; set; }

    public virtual Tour? Tour { get; set; }

    public virtual User? User { get; set; }
}
