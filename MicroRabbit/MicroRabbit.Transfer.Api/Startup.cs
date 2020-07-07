using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Infra.IOC;
using MicroRabbit.Transfer.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MicroRabbit.Transfer.Data;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Banking.Domain.Events;
using MicroRabbit.Transfer.Domain.EventHandlers;

namespace MicroRabbit.Transfer.Api
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
            services.AddDbContext<TransferDBContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("TransferDBConnection"));
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Transfer Microservices", Version = "v1" });
            });
            services.AddMediatR(typeof(Startup));
            RegisterServices(services);
        }
        private void RegisterServices(IServiceCollection services)
        {
            DependencyContainer.RegisterServices(services);
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Transfer Microservices v1");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            ConfigureEvents(app);
        }

        private static void ConfigureEvents(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subsribe<Transfer.Domain.Events.TransferCreatedEvent, Transfer.Domain.EventHandlers.TransferEventHandler>();
        }
    }
}
