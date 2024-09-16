using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Bookings;

public class PricingService
{
    public PricingDetails CalculatePrice(Apartment apartment, DateRange period)
    {
        Currency currency = apartment.Price.Currency;

        Money pricedForPeriod = new(
            apartment.Price.Amount * period.LengthInDays,
            currency);

        decimal percentageUpCharge = 0;
        foreach (var amenity in apartment.Amenitities)
        {
            percentageUpCharge += amenity switch
            {
                Amenity.GardenView or Amenity.MountainView => 0.5m,
                Amenity.AirConditioning => 0.01m,
                Amenity.Parking => 0.01m,
                _ => 0
            };
        }

        Money amenitiesUpCharge = Money.Zero();
        if (percentageUpCharge > 0) {
            amenitiesUpCharge = new Money(
                pricedForPeriod.Amount * percentageUpCharge,
                currency);
        }

        Money totalPrice = Money.Zero();

        totalPrice += pricedForPeriod;

        if (!apartment.CleaningFee.IsZero())
        {
            totalPrice += apartment.CleaningFee;
        }

        totalPrice += amenitiesUpCharge;

        return new PricingDetails(pricedForPeriod, apartment.CleaningFee, amenitiesUpCharge, totalPrice);
    }
}
