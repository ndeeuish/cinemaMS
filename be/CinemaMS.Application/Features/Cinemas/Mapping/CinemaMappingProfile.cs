using AutoMapper;
using CinemaMS.Application.Features.Cinemas.Commands;
using CinemaMS.Application.Features.Cinemas.DTOs;
using CinemaMS.Domain.Entities.Cinemas;

namespace CinemaMS.Application.Features.Cinemas.Mapping;

public class CinemaMappingProfile : Profile
{
    public CinemaMappingProfile()
    {
        CreateMap<Cinema, CinemaDto>();
        CreateMap<CreateCinemaCommand, Cinema>();
    }
}
