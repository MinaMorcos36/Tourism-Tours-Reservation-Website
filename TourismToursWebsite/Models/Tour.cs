using System;
using System.Collections.Generic;

namespace TourismToursWebsite.Models;

public partial class Tour
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public string? Location { get; set; }

    public DateOnly? Date { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
