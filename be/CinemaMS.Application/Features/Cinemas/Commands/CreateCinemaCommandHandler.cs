using AutoMapper;
using CinemaMS.Application.Features.Cinemas.DTOs;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Entities.Cinemas;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Cinemas.Commands;

public class CreateCinemaCommandHandler : CommandHandlerBase<CreateCinemaCommand, CinemaDto>
{
    private readonly ICinemaRepository _cinemaRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCinemaCommandHandler(
        ICinemaRepository cinemaRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _cinemaRepository = cinemaRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public override async Task<CinemaDto> Handle(CreateCinemaCommand request, CancellationToken cancellationToken)
    {
        var cinema = _mapper.Map<Cinema>(request);
        
        _cinemaRepository.Add(cinema);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CinemaDto>(cinema);
    }
}
