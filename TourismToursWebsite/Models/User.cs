using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace TourismToursWebsite.Models;

public partial class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? PasswordHash { get; set; }

    public string Role { get; set; } = null!;

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
