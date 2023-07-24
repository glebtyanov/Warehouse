using DAL.Context;
using DAL.Interfaces;
using DAL.Repositories;

namespace DAL.UnitsOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICustomerRepository CustomerRepository { get; private set; }

        public IDepartmentRepository DepartmentRepository { get; private set; }

        public IDepartmentWorkerRepository DepartmentWorkerRepository { get; private set; }

        public IOrderProductRepository OrderProductRepository { get; private set; }

        public IOrderRepository OrderRepository { get; private set; }

        public IPositionRepository PositionRepository { get; private set; }

        public IProductRepository ProductRepository { get; private set; }

        public IStatusRepository StatusRepository { get; private set; }

        public ITransactionRepository TransactionRepository { get; private set; }

        public IWorkerRepository WorkerRepository { get; private set; }

        public UnitOfWork(WarehouseContext warehouseContext)
        {
            CustomerRepository = new CustomerRepository(warehouseContext);
            DepartmentRepository = new DepartmentRepository(warehouseContext);
            DepartmentWorkerRepository = new DepartmentWorkerRepository(warehouseContext);
            OrderProductRepository = new OrderProductRepository(warehouseContext);
            OrderRepository = new OrderRepository(warehouseContext);
            PositionRepository = new PositionRepository(warehouseContext);
            ProductRepository = new ProductRepository(warehouseContext);
            StatusRepository = new StatusRepository(warehouseContext);
            TransactionRepository = new TransactionRepository(warehouseContext);
            WorkerRepository = new WorkerRepository(warehouseContext);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
