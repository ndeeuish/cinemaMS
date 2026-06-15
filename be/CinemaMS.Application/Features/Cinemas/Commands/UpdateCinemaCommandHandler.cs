using AutoMapper;
using CinemaMS.Application.Features.Cinemas.DTOs;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Repositories;

using CinemaMS.Domain.Exceptions;

namespace CinemaMS.Application.Features.Cinemas.Commands;

public class UpdateCinemaCommandHandler : CommandHandlerBase<UpdateCinemaCommand, CinemaDto>
{
    private readonly ICinemaRepository _cinemaRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCinemaCommandHandler(ICinemaRepository cinemaRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _cinemaRepository = cinemaRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public override async Task<CinemaDto> Handle(UpdateCinemaCommand request, CancellationToken cancellationToken)
    {
        var cinema = await _cinemaRepository.GetByIdAsync(request.Id, cancellationToken);
        if (cinema == null)
            throw new NotFoundException("Cinema", request.Id);

        cinema.Name = request.Name;
        cinema.Address = request.Address;
        cinema.Hotline = request.Hotline;

        _cinemaRepository.Update(cinema);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CinemaDto>(cinema);
    }
}
