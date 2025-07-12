using Microsoft.OpenApi.Models;

namespace ProductionAndStockERP.Helpers
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerGenConfiguration(this IServiceCollection services)
        {
            // AddSwaggerGen servisini burada çağırıyoruz
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Production And Stock ERP API", Version = "v1" });

                // ------------ BU BÖLÜM AUTHORIZE BUTONUNU OLUŞTURUR -----------
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "JWT Authorization header'ını Bearer şeması ile kullanın. \n\n 'Bearer ' (boşluk) SONRASINDA token'ınızı girin.\n\nÖrnek: 'Bearer 12345abcdef'",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
                // ---------------------------------------------------------------
            });

            return services;
        }
    }
}
