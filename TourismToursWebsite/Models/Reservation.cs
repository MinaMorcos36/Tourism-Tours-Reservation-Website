using System;
using System.Collections.Generic;

namespace TourismToursWebsite.Models;

public partial class Reservation
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? TourId { get; set; }

    public DateOnly ReservationDate { get; set; }

    public string UserName { get; set; } = null!;

    public string UserEmail { get; set; } = null!;

    public string UserPhone { get; set; } = null!;

    public DateOnly? TourDate { get; set; }

    public virtual Tour? Tour { get; set; }

    public virtual User? User { get; set; }
}
