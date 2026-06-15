using AutoMapper;
using CinemaMS.Application.Features.Cinemas.DTOs;
using CinemaMS.Application.Features.Common.Pagination;
using CinemaMS.Domain.Repositories;
using MediatR;

namespace CinemaMS.Application.Features.Cinemas.Queries;

public class GetCinemasQueryHandler : IRequestHandler<GetCinemasQuery, PagedResult<CinemaDto>>
{
    private readonly ICinemaRepository _cinemaRepository;
    private readonly IMapper _mapper;

    public GetCinemasQueryHandler(ICinemaRepository cinemaRepository, IMapper mapper)
    {
        _cinemaRepository = cinemaRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<CinemaDto>> Handle(GetCinemasQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _cinemaRepository.GetPagedAsync(
            request.PageIndex, 
            request.PageSize, 
            request.Keyword, 
            cancellationToken);

        return new PagedResult<CinemaDto>
        {
            Items = _mapper.Map<IEnumerable<CinemaDto>>(items),
            TotalItems = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }
}
