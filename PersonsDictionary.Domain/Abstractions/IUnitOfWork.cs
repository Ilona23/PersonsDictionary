namespace Domain.Abstractions;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken);
}
