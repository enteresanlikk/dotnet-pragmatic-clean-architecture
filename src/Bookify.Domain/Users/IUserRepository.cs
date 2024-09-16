namespace Bookify.Domain.Users;

public interface IApartmentRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    void Add(User user);
}
