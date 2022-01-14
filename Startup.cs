using CinemaApi.Hosting.Filters;
using CinemaApi.Hosting.Scopes;
using CinemaApi.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace CinemaApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            });

            services.AddDbContext<MovieNightDbContext>(s =>
            {
                s.UseSqlServer(Configuration.GetConnectionString("MovieNight"));
            });

            services.AddScoped<MoviesRepository>();
            services.AddScoped<SessionsRepository>();
            services.AddScoped<TicketsRepository>();

            services.AddCors(options =>
            {
                options.AddPolicy(
                    name: "Development",
                    builder =>
                    {
                        builder
                            .WithOrigins("http://localhost:141149")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = Configuration["Authentication:Authority"];
                options.Audience = Configuration["Authentication:Audience"];
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin", policy => policy.Requirements.Add(new HasScopeRequirement("admin", Configuration["Authentication:Authority"])));
            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } else
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseCors("Development");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
