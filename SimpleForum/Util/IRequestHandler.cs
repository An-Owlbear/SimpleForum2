namespace SimpleForum.Interfaces;

public interface IRequestHandler<in T, R>
{
    public Task<R> Handle(T param, CancellationToken cancellationToken = default);
}