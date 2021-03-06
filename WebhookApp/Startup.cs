using System;
using Hangfire;
using Hangfire.LiteDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebhookApp.Jobs;
using WebhookApp.Rules;
using WebhookApp.Services;

namespace WebhookApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddSingleton<ConfigService>();
            services.AddSingleton<BotService>();
            services.Scan(scan => {
                scan.FromAssemblyOf<IUpdateRule>()
                    .AddClasses(classes => classes.AssignableTo<IUpdateRule>())
                    .AsSelfWithInterfaces()
                    .WithScopedLifetime();
            });
            services.AddScoped<UpdateService>();
            services.AddHangfire(configuration => {
                configuration
                    .UseLiteDbStorage();
            });
            services.AddHangfireServer(options =>
                options.SchedulePollingInterval = TimeSpan.FromSeconds(5));
            services
                .AddControllers()
                .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BotService botService) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors();
            
            botService.Run();

            app.UseHangfireDashboard();
            
            JobManager jobManager = new JobManager();
            jobManager.AddBattleNotification(9,30);
            jobManager.AddBattleNotificationPing(9,31);
            jobManager.AddBattleNotificationPing(9,55);
            jobManager.AddFinishBattleOperation(10,00);
            
            jobManager.AddBattleNotification(12,30);
            jobManager.AddBattleNotificationPing(12,31);
            jobManager.AddBattleNotificationPing(12,55);
            jobManager.AddFinishBattleOperation(13,00);
            
            jobManager.AddBattleNotification(15,30);
            jobManager.AddBattleNotificationPing(15,31);
            jobManager.AddBattleNotificationPing(15,55);
            jobManager.AddFinishBattleOperation(16,00);
            
            jobManager.AddBattleNotification(18,30);
            jobManager.AddBattleNotificationPing(18,31);
            jobManager.AddDaily<FactoryEatNotificationJob>(18,32);
            jobManager.AddBattleNotificationPing(18,55);
            jobManager.AddFinishBattleOperation(19,00);
            
            jobManager.AddBattleNotification(21,30);
            jobManager.AddBattleNotificationPing(21,31);
            jobManager.AddBattleNotificationPing(21,55);
            jobManager.AddFinishBattleOperation(22,00);

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
