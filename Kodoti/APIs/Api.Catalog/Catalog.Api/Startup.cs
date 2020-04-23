using Catalog.Api.Config;
using Catalog.Api.Validations;
using Domain.Dto.Layer;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence.Layer;
using Services.Layer;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Catalog.Api
{
    public class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IStoreService, StoreService>();
            services.AddTransient<IValidator<ProductCreateDto>, ProductCreateDtoValidation>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAccessToAll", builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin());
            });

            services.AddMvc().AddFluentValidation();

            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("SecretKey"));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddApiVersioning(
                options =>
                {
                    options.ReportApiVersions = true;
                }
            );

            // configure swagger
            services.AddSwaggerGen(options =>
            {
                // specify two versions
                options.SwaggerDoc("v1", new Info()
                {
                    Version = "v1",
                    Title = "Api.Catalog v1",
                    Description = "v1 API Description",
                    TermsOfService = "Terms of usage v1"
                });

                options.SwaggerDoc("v2", new Info()
                {
                    Version = "v2",
                    Title = "Api.Catalog v2",
                    Description = "v2 API Description",
                    TermsOfService = "Terms of usage v2"
                });

                options.DocumentFilter<ReplaceVersionWithExactValueInPath>();

                options.DocInclusionPredicate((version, desc) =>
                {
                    var versions = desc.ControllerAttributes()
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v.ToString()}" == version);
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            AutoMapperConfig.Config();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api.Catalog v1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "Api.Catalog v2");
            });

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Default}/{action=Index}/{id?}"
                    );
            });
        }
    }
}