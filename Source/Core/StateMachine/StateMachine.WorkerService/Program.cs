using MassTransit;
using MessageBroker.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using StateMachine.WorkerService;
using StateMachine.WorkerService.Data;
using StateMachine.WorkerService.Data.Enities;
using StateMachine.WorkerService.Models;
using System.Reflection;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddMassTransit(cfgMassTransit =>
        {
            cfgMassTransit.AddSagaStateMachine<OrderStateMachine, OrderStateInstanceEntity>().EntityFrameworkRepository(cfgEntityFrameworkRepository =>
            {
                cfgEntityFrameworkRepository.AddDbContext<DbContext, OrderStateDbContext>((provider, builder) =>
                {
                    builder.UseNpgsql(hostContext.Configuration.GetConnectionString("PostgreSQL"), m =>
                    {
                        m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                    });
                });
            });

            cfgMassTransit.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(builder =>
            {
                builder.Host(hostContext.Configuration.GetConnectionString("RabbitMq"));
                builder.ReceiveEndpoint(RabbitMqConstant.OrderSagaQueueName, e =>
                {
                    e.ConfigureSaga<OrderStateInstanceEntity>(provider);
                });
            }));
        });

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();