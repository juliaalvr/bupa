using BlazorApp1.Components;
using BupaMOTApp.Services;   


//setting up the web application
var builder = WebApplication.CreateBuilder(args);

// Register the MOTService for dependency injection
builder.Services.AddScoped<MOTService>(); 


// Register HttpClient with a base address 
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://beta.check-mot.service.gov.uk/") }); // Set the API base URL

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

//app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
