namespace PixelService.Application.Services;

public interface IProducerService<T>
{
    Task ProduceAsync(T entity);
}