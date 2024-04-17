namespace MvcApp.Services
{
    public interface IUnitOfWork
    {
        Task CommitAsync(CancellationToken token);
        CategoryRepository CategoryRepository { get; }
        BookRepository BookRepository { get; }
        OrderRepository OrderRepository { get; }
        OrderDetailsRepository OrderDetailsRepository { get; }
        ShoppingCartRepository ShoppingCartRepository { get; }
    }
}
