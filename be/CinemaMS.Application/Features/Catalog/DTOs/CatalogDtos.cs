namespace CinemaMS.Application.Features.Catalog.DTOs;

public class GenreDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
}

public class AgeRestrictionDto
{
    public int Id { get; set; }
    public string Code { get; set; } = default!;
    public string Description { get; set; } = default!;
}

public class RoomTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Surcharge { get; set; }
}

public class SeatTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Surcharge { get; set; }
}
