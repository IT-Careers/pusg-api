using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PUSG.Web.Hubs;
using PUSG.Web.Static;

namespace PUSG.Web
{
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
            services.AddSingleton<ISessionService, SessionService>();
            services.AddSingleton<IGameService, GameService>();
            services.AddSingleton<GameLoopService>();
            services.AddHostedService<GameLoopService>(provider => provider.GetService<GameLoopService>());

            services.AddControllers();
            services.AddSignalR();

            services.AddCors(options =>
            {
                options.AddPolicy("CustomPolicy", builder =>
                {
                    builder
                        .WithOrigins("https://it-careers.github.io", "http://localhost:8001")
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors("CustomPolicy");

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<HomeHub>("/home");
                endpoints.MapHub<GameHub>("/game");
                endpoints.MapControllers();
            });
        }
    }
}
