using DAL.Interfaces;

namespace DAL.UnitsOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository CustomerRepository { get; }

        IDepartmentRepository DepartmentRepository { get; }

        IDepartmentWorkerRepository DepartmentWorkerRepository { get; }

        IOrderProductRepository OrderProductRepository { get; }

        IOrderRepository OrderRepository { get; }

        IPositionRepository PositionRepository { get; }

        IProductRepository ProductRepository { get; }

        IStatusRepository StatusRepository { get; }

        ITransactionRepository TransactionRepository { get; }
        IWorkerRepository WorkerRepository { get; }
    }
}
