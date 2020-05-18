namespace CarbonKitchen.ShoppingListItems.Api
{
    using AutoBogus;
    using Autofac;
    using AutoMapper;
    using FluentValidation.AspNetCore;
    using CarbonKitchen.ShoppingListItems.Api.Data;
    using CarbonKitchen.ShoppingListItems.Api.Data.Entities;
    using CarbonKitchen.ShoppingListItems.Api.Services;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json;
    using Sieve.Services;
    using System;

    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "MyCorsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression();

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<SieveProcessor>();
            
            services.AddScoped<IShoppingListItemRepository, ShoppingListItemRepository>();

            services.AddMvc()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>())
                .AddNewtonsoftJson(
                op =>
                {
                    op.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    op.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
                });

            services.AddDbContext<ShoppingListItemDbContext>(opt => opt.UseInMemoryDatabase("ShoppingListItemDb"));

            services.AddMediatR(typeof(Startup));

            services.AddControllers();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

        // https://autofaccn.readthedocs.io/en/latest/integration/aspnetcore.html
        public void ConfigureContainer(ContainerBuilder builder)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(MyAllowSpecificOrigins);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var context = app.ApplicationServices.GetService<ShoppingListItemDbContext>())
            {
                context.Database.EnsureCreated();

                // auto generate some fake data. added rules to accomodate placeholder validation rules
                context.ShoppingListItems.Add(new AutoFaker<ShoppingListItem>()
                    .RuleFor(fake => fake.ShoppingListId, 10)
                    .RuleFor(fake => fake.Category, "Produce")
                    .RuleFor(fake => fake.Amount, fake => fake.Random.Number()));
                context.ShoppingListItems.Add(new AutoFaker<ShoppingListItem>()
                    .RuleFor(fake => fake.ShoppingListId, 10)
                    .RuleFor(fake => fake.Category, "Produce")
                    .RuleFor(fake => fake.Amount, fake => fake.Random.Number()));
                context.ShoppingListItems.Add(new AutoFaker<ShoppingListItem>()
                    .RuleFor(fake => fake.ShoppingListId, 2)
                    .RuleFor(fake => fake.Category, "Meat")
                    .RuleFor(fake => fake.Amount, fake => fake.Random.Number()));

                context.SaveChanges();
            }

            app.UseResponseCompression();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
        }
    }
}
