using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using roadcap.recipes.entities.Contexts;

namespace roadcap.recipes.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string localHostOrigin = "AllowLocalhost";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Create a policy to allow any localhost app to access this service
            services.AddCors(options =>
            {
                options.AddPolicy(localHostOrigin, builder =>
                {
                    builder.WithOrigins("https://localhost:44308");
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });

            // Typically would store in user secrets or environment var.  But... we're using localdb and integrated security, so no worries
            services.AddDbContext<RoadcapRecipesContext>(options => options.UseSqlServer("Data Source=(localdb)\\ProjectsV13;Initial Catalog=roadcap.recipes;Integrated Security=True;MultipleActiveResultSets=true;"));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoadcapRecipesContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            context.Database.EnsureCreated();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(localHostOrigin);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
