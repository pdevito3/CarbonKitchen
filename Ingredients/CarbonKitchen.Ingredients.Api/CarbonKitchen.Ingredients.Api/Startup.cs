namespace CarbonKitchen.Ingredients.Api
{
    using AutoBogus;
    using Autofac;
    using AutoMapper;
    using FluentValidation.AspNetCore;
    using CarbonKitchen.Ingredients.Api.Data;
    using CarbonKitchen.Ingredients.Api.Data.Entities;
    using CarbonKitchen.Ingredients.Api.Services;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Sieve.Services;
    using System;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<SieveProcessor>();
            
            services.AddScoped<IIngredientRepository, IngredientRepository>();

            services.AddMvc()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddDbContext<IngredientDbContext>(opt => 
                opt.UseInMemoryDatabase("IngredientDb"));

            services.AddControllers()
                .AddNewtonsoftJson();
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var context = app.ApplicationServices.GetService<IngredientDbContext>())
            {
                context.Database.EnsureCreated();

                // auto generate some fake data. added rules to accomodate placeholder validation rules
                context.Ingredients.Add(new AutoFaker<Ingredient>()
                    .RuleFor(fake => fake.IngredientDateField1, fake => fake.Date.Past())
                    .RuleFor(fake => fake.RecipeId, fake => fake.Random.Number()));
                context.Ingredients.Add(new AutoFaker<Ingredient>()
                    .RuleFor(fake => fake.IngredientDateField1, fake => fake.Date.Past())
                    .RuleFor(fake => fake.RecipeId, fake => fake.Random.Number()));
                context.Ingredients.Add(new AutoFaker<Ingredient>()
                    .RuleFor(fake => fake.IngredientDateField1, fake => fake.Date.Past())
                    .RuleFor(fake => fake.RecipeId, fake => fake.Random.Number()));

                context.SaveChanges();
            }
        }
    }
}
