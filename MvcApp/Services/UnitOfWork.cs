
using MvcApp.EFCore.Data;

namespace MvcApp.Services
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private readonly AppDbContext _context;
        public CategoryRepository CategoryRepository { get; private set; }
        public BookRepository BookRepository { get; private set; }
        public OrderRepository OrderRepository { get; private set; }
        public OrderDetailsRepository OrderDetailsRepository { get; private set; }
        public ShoppingCartRepository ShoppingCartRepository { get; private set; }

        public UnitOfWork(AppDbContext context, 
            CategoryRepository CategoryRepository, 
            BookRepository bookRepository, 
            OrderRepository orderRepository, 
            OrderDetailsRepository orderDetailsRepository, 
            ShoppingCartRepository shoppingCartRepository)
        {
            _context = context;
            this.CategoryRepository = CategoryRepository;
            BookRepository = bookRepository;
            OrderRepository = orderRepository;
            OrderDetailsRepository = orderDetailsRepository;
            ShoppingCartRepository = shoppingCartRepository;
        }
        public async Task CommitAsync(CancellationToken token)
        {
            await _context.SaveChangesAsync(token);
        }

        public void Dispose()
        {
           _context.Dispose();
        }
    }
}
