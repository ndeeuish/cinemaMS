using AutoMapper;
using CinemaMS.Application.Features.Cinemas.DTOs;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Repositories;

using CinemaMS.Domain.Exceptions;

namespace CinemaMS.Application.Features.Cinemas.Queries;

public class GetCinemaByIdQueryHandler : QueryHandlerBase<GetCinemaByIdQuery, CinemaDto>
{
    private readonly ICinemaRepository _cinemaRepository;
    private readonly IMapper _mapper;

    public GetCinemaByIdQueryHandler(ICinemaRepository cinemaRepository, IMapper mapper)
    {
        _cinemaRepository = cinemaRepository;
        _mapper = mapper;
    }

    public override async Task<CinemaDto> Handle(GetCinemaByIdQuery request, CancellationToken cancellationToken)
    {
        var cinema = await _cinemaRepository.GetByIdAsync(request.Id, cancellationToken);
        if (cinema == null)
            throw new NotFoundException("Cinema", request.Id);

        return _mapper.Map<CinemaDto>(cinema);
    }
}
