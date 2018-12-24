using System;
using System.Data.SqlClient;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FWTL.Core.CQRS;
using FWTL.Core.Extensions;
using FWTL.Core.Services.Dapper;
using FWTL.Core.Services.Redis;
using FWTL.Core.Services.Unique;
using FWTL.Database;
using FWTL.Infrastructure.CQRS;
using FWTL.Infrastructure.Dapper;
using FWTL.Infrastructure.EventHub;
using FWTL.Infrastructure.Identity;
using FWTL.Infrastructure.Unique;
using FWTL.Infrastructure.User;
using FWTL.Infrastructure.Validation;
using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.EventHubs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using Serilog;
using StackExchange.Redis;

namespace FWTL.Api
{
    public class IocConfig
    {
        public static void OverrideWithLocalCredentials(ContainerBuilder builder)
        {
            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                var credentials = new JobDatabaseCredentials();
                credentials.BuildLocalConnectionString(
                        configuration["Api:Sql:Url"],
                        configuration["Api:Sql:Catalog"]);

                return credentials;
            }).SingleInstance();
        }

        public static void RegisterCredentials(ContainerBuilder builder)
        {
            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                var credentails = new IdentityModelCredentials()
                {
                    ClientId = configuration["Auth:Client:Id"],
                    ClientSecret = configuration["Auth:Client:Secret"]
                };

                return credentails;
            }).SingleInstance();

            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                var credentails = new RedisCredentials();
                credentails.BuildConnectionString(
                    configuration["Redis:Name"],
                    configuration["Redis:Password"],
                    configuration["Redis:Port"].To<int>(),
                    isSsl: true,
                    allowAdmin: true);

                return ConnectionMultiplexer.Connect(credentails.ConnectionString);
            }).SingleInstance();

            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                var credentials = new JobDatabaseCredentials();
                credentials.BuildConnectionString(
                    configuration["Api:Sql:Url"],
                    configuration["Api:Sql:Port"].To<int>(),
                    configuration["Api:Sql:Catalog"],
                    configuration["Api:Sql:User"],
                    configuration["Api:Sql:Password"]);

                return credentials;
            }).SingleInstance();

            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                var connectionStringBuilder = new EventHubsConnectionStringBuilder(configuration["EventHub:ConnectionString"])
                {
                    EntityPath = configuration["EventHub:EntityPath"]
                };

                return connectionStringBuilder;
            }).SingleInstance();
        }

        public static IContainer RegisterDependencies(IServiceCollection services, IHostingEnvironment env, IConfiguration rootConfiguration)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var builder = new ContainerBuilder();
            builder.Populate(services);
            RegisterCredentials(builder);

            if (env.IsDevelopment())
            {
                OverrideWithLocalCredentials(builder);
            }

            builder.Register<IDiscoveryCache>(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                var cache = new DiscoveryCache(configuration["Auth:Client:Url"]);
                return cache;
            }).SingleInstance();

            builder.Register(b =>
            {
                return rootConfiguration;
            }).SingleInstance();

            builder.Register(b =>
            {
                var redis = b.Resolve<ConnectionMultiplexer>();
                return redis.GetDatabase();
            }).InstancePerLifetimeScope();

            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                var redis = b.Resolve<ConnectionMultiplexer>();
                return redis.GetServer(configuration["Redis:Url"]);
            }).InstancePerLifetimeScope();

            builder.RegisterType<IEventDispatcher>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(IEventHandler<>)).InstancePerDependency();

            builder.RegisterType<CommandDispatcher>().As<ICommandDispatcher>().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(ICommandHandler<>)).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(ICommandHandler<,>)).InstancePerLifetimeScope();

            builder.RegisterType<QueryDispatcher>().As<IQueryDispatcher>().InstancePerRequest().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(IQueryHandler<,>)).InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(AppAbstractValidation<>)).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(IReadCacheHandler<,>)).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(IWriteCacheHandler<,>)).InstancePerLifetimeScope();

            builder.Register<ILogger>(b =>
            {
                return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error)
                .WriteTo.File("Logs/queries.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .CreateLogger();
            });

            builder.Register<IClock>(b =>
            {
                return SystemClock.Instance;
            }).SingleInstance();

            builder.Register<IMemoryCache>(b =>
            {
                return new MemoryCache(new MemoryCacheOptions());
            }).SingleInstance();

            builder.Register<IDatabaseConnector<JobDatabaseCredentials>>(b =>
            {
                var databaseCredentials = b.Resolve<JobDatabaseCredentials>();
                var logger = b.Resolve<ILogger>();
                ProfileDbConnection databaseConnection = new ProfileDbConnection(new SqlConnection(databaseCredentials.ConnectionString), logger);
                return new DapperConnector<JobDatabaseCredentials>(databaseConnection);
            }).InstancePerLifetimeScope();

            builder.RegisterType<GuidService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<CurrentUserProvider>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<RandomService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<EventHubService>().AsImplementedInterfaces().SingleInstance();

            builder.Register<IClock>(b =>
            {
                return SystemClock.Instance;
            }).SingleInstance();

            builder.Register(b =>
            {
                var connectionStringBuilder = b.Resolve<EventHubsConnectionStringBuilder>();
                return EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
            }).InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}