using System;
using Hangfire;
using Hangfire.LiteDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using WebhookApp.Data;
using WebhookApp.Jobs;
using WebhookApp.Rules;
using WebhookApp.Services;
using WebhookApp.Services.Battle;
using WebhookApp.Services.Laboratory;
using WebhookApp.Services.Lottery;
using WebhookApp.Services.Ping;
using WebhookApp.Services.Smoothie;

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
            services.AddOptions<BotConfig>()
                .BindConfiguration(nameof(BotConfig))
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<BotConfig>>().Value);
            services.AddDbContext<ApplicationDbContext>();
            services.AddActivatedSingleton<BotService>();
            services.AddSingleton<ISmoothieService, SmoothieService>();
            services.AddScoped<IBattleService, BattleService>();
            services.AddScoped<ILotteryService, LotteryService>();
            services.AddScoped<IPingService, PingService>();
            services.Scan(scan => {
                scan.FromAssemblyOf<IUpdateRule>()
                    .AddClasses(classes => classes.AssignableTo<IUpdateRule>())
                    .AsSelfWithInterfaces()
                    .WithScopedLifetime();
            });
            services.AddScoped<UpdateService>();
            services.AddTransient<ILaboratoryService, LaboratoryService>();
            services.AddHangfire(configuration => {
                configuration
                    .UseLiteDbStorage();
            });
            services.AddHangfireServer(options =>
                options.SchedulePollingInterval = TimeSpan.FromSeconds(5));
            services
                .AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BotService botService) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors();

            JobManager jobManager = new JobManager();
            jobManager.AddDaily<SmoothieResetJob>(3,0);

            jobManager.AddDaily<BattleNotificationJob>(9, 30);
            jobManager.AddDaily<BattleNotificationPingJob>(9, 31);
            jobManager.AddDaily<BattleNotificationPingJob>(9, 55);
            jobManager.AddDaily<FinishBattleOperationJob>(10, 00);
            
            jobManager.AddDaily<BattleNotificationJob>(12, 30);
            jobManager.AddDaily<BattleNotificationPingJob>(12, 31);
            jobManager.AddDaily<BattleNotificationPingJob>(12, 55);
            jobManager.AddDaily<FinishBattleOperationJob>(13, 00);
            
            jobManager.AddDaily<BattleNotificationJob>(15, 30);
            jobManager.AddDaily<BattleNotificationPingJob>(15, 31);
            jobManager.AddDaily<BattleNotificationPingJob>(15, 55);
            jobManager.AddDaily<FinishBattleOperationJob>(16, 00);
            
            jobManager.AddDaily<BattleNotificationJob>(18, 30);
            jobManager.AddDaily<BattleNotificationPingJob>(18, 31);
            jobManager.AddDaily<FactoryEatNotificationJob>(18,32);
            jobManager.AddDaily<BattleNotificationPingJob>(18, 55);
            jobManager.AddDaily<FinishBattleOperationJob>(19, 00);

            jobManager.AddDaily<LotteryPingJob>(19,17);
            
            jobManager.AddDaily<BattleNotificationJob>(21, 30);
            jobManager.AddDaily<BattleNotificationPingJob>(21, 31);
            jobManager.AddDaily<BattleNotificationPingJob>(21, 55);
            jobManager.AddDaily<FinishBattleOperationJob>(22, 00);
            jobManager.AddDaily<SleepNotificationJob>(22,05);
            jobManager.AddDaily<SleepNotificationJob>(23,00);

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
