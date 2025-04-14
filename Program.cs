
using Microsoft.Graph;
using YourNamespace.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ Register services BEFORE building the app
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IHtmlFileService, HtmlFileService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy
            .WithOrigins("http://localhost:3000") // Update if you're using Vite (5173)
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddSingleton<GraphServiceClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return GraphClientFactory.Create(config);
});

builder.Services.AddScoped<ICodeCoverageReportService, SharePointCoverageReportService>();



var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();             // Enable middleware
    app.UseSwaggerUI();          // Enable Swagger UI
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// ✅ Enable CORS middleware BEFORE UseAuthorization
app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
