
const string defaultCorsPolicy = "defaultCorsPolicy";
var builder = WebApplication.CreateBuilder(args);

var proxyBuilder = builder.Services.AddReverseProxy();
builder.Services.AddDaprClient();

proxyBuilder.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddCors(opts =>
{
    opts.AddPolicy(defaultCorsPolicy, builder =>
    {
        builder.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins("http://localhost:4200", "https://pollstar.hexmaster.nl");
    });
});

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
