namespace CinemaMS.Application.Features.Cinemas.DTOs;

public class CinemaDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Hotline { get; set; } = default!;
}
