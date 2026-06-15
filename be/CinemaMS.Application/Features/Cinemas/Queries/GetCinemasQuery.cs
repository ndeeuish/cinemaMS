using CinemaMS.Application.Features.Cinemas.DTOs;
using CinemaMS.Application.Features.Common.Pagination;
using MediatR;

namespace CinemaMS.Application.Features.Cinemas.Queries;

public class GetCinemasQuery : FilterBase, IRequest<PagedResult<CinemaDto>>
{
}
