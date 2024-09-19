using Asp.Versioning;
using Asp.Versioning.Builder;
using Bookify.Application.Bookings.GetBooking;
using Bookify.Application.Bookings.ReserveBooking;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Bookigns;

public static class BookingsEndpoints
{
    public static IEndpointRouteBuilder MapBookingEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("bookings/{id}", GetBooking)
            .RequireAuthorization()
            .WithName(nameof(GetBooking));

        builder.MapPost("bookings", ReserveBooking)
            .RequireAuthorization()
            .WithName(nameof(ReserveBooking));

        return builder;
    }

    public static async Task<IResult> GetBooking(
        ISender sender,
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetBookingQuery(id);

        var result = await sender.Send(query, cancellationToken);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return Results.NotFound();
    }

    public static async Task<IResult> ReserveBooking(
        ISender sender,
        ReserveBookingRequest request,
        CancellationToken cancellationToken)
    {
        var query = new ReserveBookingCommand(
            request.ApartmentId,
            request.UserId,
            request.StartDate,
            request.EndDate);

        var result = await sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return Results.BadRequest(result.Error);
        }

        return Results.CreatedAtRoute(nameof(GetBooking), new { id = result.Value }, result.Value);
    }
}
