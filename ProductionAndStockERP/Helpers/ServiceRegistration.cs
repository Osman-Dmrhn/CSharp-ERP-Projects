using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Services;

namespace ProductionAndStockERP.Helpers
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<IActivityLogsService,ActivityLogsService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductionOrderService,ProductionOrderService>();
            services.AddScoped<IStockTransactionService, StockTransactionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IReportService, ReportService>();
            return services;
        }
    }
}
