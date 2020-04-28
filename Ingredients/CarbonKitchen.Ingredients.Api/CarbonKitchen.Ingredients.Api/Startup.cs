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
                context.Ingredients.Add(new Ingredient {
                    IngredientId = 1
                    , RecipeId = 1
                    , Amount = 1
                    , Unit = "package"
                    , Name = "(20 oz) boneless skinless chicken thighs"
                  });
                context.Ingredients.Add(new Ingredient { 
                    IngredientId = 2
                    , RecipeId = 1
                    , Amount = 1
                    , Unit = "teaspoon"
                    , Name = "salt"
                  });
                context.Ingredients.Add(new Ingredient {
                    IngredientId = 3
                    , RecipeId = 1
                    , Amount = .5
                    , Unit = "teaspoon"
                    , Name = "pepper"
                  });
                context.Ingredients.Add(new Ingredient {
                    IngredientId = 4
                    , RecipeId = 1
                    , Amount = 2
                    , Unit = "tablespoons"
                    , Name = "butter"
                  });
                context.Ingredients.Add(new Ingredient {
                    IngredientId = 5
                    , RecipeId = 1
                    , Amount = 1
                    , Unit = "package"
                    , Name = "(20 oz) boneless skinless chicken thighs"
                  });
                context.Ingredients.Add(new Ingredient {
                    IngredientId = 6
                    , RecipeId = 1
                    , Amount = 1
                    , Unit = "teaspoon"
                    , Name = "salt"
                  });
                context.Ingredients.Add(new Ingredient {
                    IngredientId = 7
                    , RecipeId = 1
                    , Amount = .5
                    , Unit = "teaspoon"
                    , Name = "pepper"
                  });
                context.Ingredients.Add(new Ingredient {
                    IngredientId = 8
                    , RecipeId = 1
                    , Amount = 2
                    , Unit = "tablespoons"
                    , Name = "butter"
                  });

                context.Ingredients.Add(new Ingredient {
                    IngredientId = 9
                    , RecipeId = 1
                    , Amount = 1
                    , Unit = "package"
                    , Name = "(20 oz) boneless skinless chicken thighs"
                  });
                context.Ingredients.Add(new Ingredient {
                    IngredientId = 10
                    , RecipeId = 1
                    , Amount = 1
                    , Unit = "teaspoon"
                    , Name = "salt"
                  });
                context.Ingredients.Add(new Ingredient {
                    IngredientId = 11
                    , RecipeId = 1
                    , Amount = .5
                    , Unit = "teaspoon"
                    , Name = "pepper"
                  });
                context.Ingredients.Add(new Ingredient {
                    IngredientId = 12
                    , RecipeId = 1
                    , Amount = 2
                    , Unit = "tablespoons"
                    , Name = "butter"
                  });
                // auto generate some fake data. added rules to accomodate placeholder validation rules
                context.Ingredients.Add(new AutoFaker<Ingredient>()
                    .RuleFor(fake => fake.Amount, fake => fake.Random.Number())
                    .RuleFor(fake => fake.RecipeId, 2));
                context.Ingredients.Add(new AutoFaker<Ingredient>()
                    .RuleFor(fake => fake.Amount, fake => fake.Random.Number())
                    .RuleFor(fake => fake.RecipeId, 2));
                context.Ingredients.Add(new AutoFaker<Ingredient>()
                    .RuleFor(fake => fake.Amount, fake => fake.Random.Number())
                    .RuleFor(fake => fake.RecipeId, 2));

                context.SaveChanges();
            }
        }
    }
}
