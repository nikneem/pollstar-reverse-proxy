using PollStar.ReverseProxy.App;
using PollStar.ReverseProxy.App.Proxy;
using Yarp.ReverseProxy.Configuration;


var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddSingleton<IProxyConfigProvider>(new PollStarProxyConfigProvider())
    .AddReverseProxy();
builder.Services.AddDaprClient();

//var proxyBuilder = builder.Services.AddReverseProxy();
//proxyBuilder.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddCors(opts =>
{
    opts.AddPolicy(Constants.DefaultCorsPolicy, builder =>
    {
        builder.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins("http://localhost:4200", "https://pollstar.hexmaster.nl");
    });
});

builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors();
app.MapReverseProxy();

app.Run();
