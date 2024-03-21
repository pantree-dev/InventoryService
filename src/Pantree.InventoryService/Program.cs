using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Pantree.InventoryService.Application.UserInventory.Queries;
using Pantree.InventoryService.Domain.Repositories;
using Pantree.InventoryService.Helpers;
using Pantree.InventoryService.Infrastructure.Database;
using Pantree.InventoryService.Infrastructure.Database.Repositories;
using Pantree.InventoryService.Json;

var builder = WebApplication.CreateBuilder(args);
{
    // TODO: setup logging
    // TODO: configure CORS and security/jwt verification
    // TODO: setup configuration for message bus with whatever we plan to use
    
    builder.Services.AddDbContext<AppDbContext>(cfg => {
        // TODO: production db connection string when running in prod
        cfg.UseSqlite("Data Source=LocalDatabase.db");
    });
    
    // add our MediatR cqrs pipeline
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
        typeof(Program).Assembly,
        typeof(FetchPagedUserInventoryQuery).Assembly
    ));
    
    // setup our repositories
    builder.Services.AddScoped<IEventOutboxRepository, EventOutboxRepository>();
    builder.Services.AddScoped<IUserInventoryRepository, UserInventoryRepository>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    
    // handle our guid convertors a little better
    // configure newtonsoft to work better with fast-endpoints by configuring the settings better!
    JsonConvert.DefaultSettings = () => new JsonSerializerSettings {
        Formatting = Formatting.Indented,
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        Converters = new List<JsonConverter>() {
            new GuidJsonConverter(),
            new NullableGuidJsonConverter()
        }
    };
    
    // setup our fast-endpoints
    // TODO: we will also need to configure the AWS Lambda once I have learned how it goes
    builder.Services
        .AddFastEndpoints()
        .SwaggerDocument();
}

var app = builder.Build();
{
    app.UseHttpsRedirection();
    app.UseRouting();
    app
        .UseDefaultExceptionHandler()
        .UseFastEndpoints(cfg => {
            cfg.Versioning.Prefix = "v";
            cfg.Endpoints.Configurator = definition => {
                // remove the security from fast-endpoints due to the gateway handling the token/auth process
                definition.AllowAnonymous();
            };
            cfg.Serializer.ResponseSerializer = (rsp, dto, cType, jCtx, ct) => {
                rsp.ContentType = cType;
                return rsp.WriteAsync(JsonConvert.SerializeObject(dto), ct);
            };
            cfg.Serializer.RequestDeserializer = async (req, tDto, jCtx, ct) => {
                using var reader = new StreamReader(req.Body);
                return JsonConvert.DeserializeObject(await reader.ReadToEndAsync(), tDto);
            };
        })
        .UseSwaggerGen();
}

// start the web-application/microservice
app.PreStartup().Run();