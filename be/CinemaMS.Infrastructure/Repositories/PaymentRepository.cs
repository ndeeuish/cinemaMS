using CinemaMS.Domain.Entities.Bookings;
using CinemaMS.Domain.Repositories;
using CinemaMS.Infrastructure.Data;

namespace CinemaMS.Infrastructure.Repositories;

public class PaymentRepository : RepositoryBase<Payment, int>, IPaymentRepository
{
    public PaymentRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
