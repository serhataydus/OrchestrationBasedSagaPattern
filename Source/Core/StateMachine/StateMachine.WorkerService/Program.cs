using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using MessageBroker.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using StateMachine.WorkerService;
using StateMachine.WorkerService.Data;
using StateMachine.WorkerService.Data.Enities;
using StateMachine.WorkerService.Services;

Microsoft.Extensions.Hosting.IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<IOrderStateDbContext, OrderStateDbContext>(options =>
         options.UseNpgsql(hostContext.Configuration.GetConnectionString("PostgreSQL")),
         ServiceLifetime.Transient);

        services.AddMassTransit(cfgMassTransit =>
        {
            cfgMassTransit.AddSagaStateMachine<OrderStateMachineService, OrderStateInstanceEntity>().EntityFrameworkRepository(opt =>
            {
                opt.ExistingDbContext<OrderStateDbContext>();
                opt.LockStatementProvider = new PostgresLockStatementProvider();
                opt.ConcurrencyMode = ConcurrencyMode.Optimistic;
            });

            cfgMassTransit.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(builder =>
            {
                builder.Host(hostContext.Configuration.GetConnectionString("RabbitMq"));
                builder.ReceiveEndpoint(RabbitMqConstants.OrderSagaQueueName, e =>
                {
                    e.ConfigureSaga<OrderStateInstanceEntity>(provider);
                });
            }));
        });
        services.AddMassTransitHostedService();
        services.AddHostedService<Worker>();
    })
    .Build();

using (IServiceScope? scope = host.Services.CreateScope())
{
    OrderStateDbContext? dataContext = scope.ServiceProvider.GetRequiredService<OrderStateDbContext>();
    dataContext.Database.Migrate();
}

await host.RunAsync();