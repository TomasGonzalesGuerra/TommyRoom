using Blazored.LocalStorage;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TommyRoom.Web;
using TommyRoom.Web.Auth;
using TommyRoom.Web.Repositories;
using TommyRoom.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["BackEndApiUrl"]!) });

// ── Auth ───────────────────────────────────────────────────────────────────
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<RoomWebProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, RoomWebProvider>(x => x.GetRequiredService<RoomWebProvider>());
builder.Services.AddScoped<ILoginService, RoomWebProvider>(x => x.GetRequiredService<RoomWebProvider>());
// ── Servicios ──────────────────────────────────────────────────────────────
builder.Services.AddSweetAlert2();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<SesionService>();
builder.Services.AddSingleton<HubClient>();

var host = builder.Build();
var authProvider = host.Services.GetRequiredService<RoomWebProvider>();

//await authProvider.InitializeAsync();
await host.RunAsync();
