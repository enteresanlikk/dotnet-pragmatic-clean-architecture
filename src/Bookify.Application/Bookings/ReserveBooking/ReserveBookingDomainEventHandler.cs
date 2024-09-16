using Bookify.Application.Abstractions.Email;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Bookify.Domain.Bookings.Events;
using Bookify.Domain.Users;
using MediatR;

namespace Bookify.Application.Bookings.ReserveBooking;

internal sealed class ReserveBookingDomainEventHandler : INotificationHandler<BookingReservedDomainEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IEmailService _emailService;

    public ReserveBookingDomainEventHandler(
        IUserRepository userRepository,
        IBookingRepository bookingRepository,
        IEmailService emailService)
    {
        _userRepository = userRepository;
        _bookingRepository = bookingRepository;
        _emailService = emailService;
    }

    public async Task Handle(BookingReservedDomainEvent notification, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(notification.BookingId, cancellationToken);

        if (booking is null)
        {
            return;
        }

        var user = await _userRepository.GetByIdAsync(notification.UserId, cancellationToken);

        if (user is null)
        {
            return;
        }

        await _emailService.SendAsync(
            user.Email,
            "Booking reserved!",
            "You have 10 minutes to confirm this booking");
    }
}
