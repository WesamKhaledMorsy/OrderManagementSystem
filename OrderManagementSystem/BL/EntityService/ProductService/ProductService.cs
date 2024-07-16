using AutoMapper;
using OrderManagementSystem.BL.Repository;
using OrderManagementSystem.BL.UnitOfWork;
using OrderManagementSystem.DL.Entities;
using OrderManagementSystem.DL;
using OrderManagementSystem.Models;


namespace OrderManagementSystem.BL.EntityService.ProductService
{
    public class ProductService: IProductService
    {
        private readonly IGenericRepository<Product> _IGenericRepository;
        private readonly IUnitOfWork<Product> _IUnitOfWork;
        private readonly AppDBContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public ProductService(IGenericRepository<Product> iGenericRepository, IUnitOfWork<Product> iUnitOfWork,
            AppDBContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _IGenericRepository = iGenericRepository;
            _IUnitOfWork = iUnitOfWork;
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;

        }
        public Product GetProductById(int productid)
        {
            return _IGenericRepository.GetById(productid);
        }
        public Product CreateNewProduct(ProductModel product)
        {
            var productmap = _mapper.Map<Product>(product);
            var ReqResult = _IGenericRepository.Insert(productmap);
            _IUnitOfWork.Save();
            return ReqResult;
        }
        public Product UpdateProduct(ProductModel product)
        {
            var productmap = GetProductById((int)product.ProductId);
            if (productmap is null)
            {
                throw new ArgumentException("Id not found");
            }
            product.ProductId = productmap.ProductId;
            productmap = _mapper.Map<Product>(product);
            var ReqResult = _IGenericRepository.Update(productmap);
            _IUnitOfWork.Save();
            return ReqResult;
        }

        public IQueryable<Product> GetAllProducts()
        {
            var productsList = _IGenericRepository.GetAll();
            return productsList;
        }

       
    }
}
