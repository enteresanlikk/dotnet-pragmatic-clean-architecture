using Bookify.Domain.Abstractions;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Apartments;

public sealed class Apartment : Entity
{
    public Apartment(
        Guid id,
        Name name,
        Description description,
        Adress adress,
        Money price,
        Money cleaningFee,
        List<Amenity> amenitities)
        : base(id)
    {
        Name = name;
        Description = description;
        Adress = adress;
        Price = price;
        CleaningFee = cleaningFee;
        Amenitities = amenitities;
    }

    public Name Name { get; private set; }

    public Description Description { get; private set; }

    public Adress Adress { get; private set; }

    public Money Price { get; private set; }

    public Money CleaningFee { get; private set; }

    public DateTime? LastBookedOnUtc { get; internal set; }

    public List<Amenity> Amenitities { get; private set; } = new();
}
