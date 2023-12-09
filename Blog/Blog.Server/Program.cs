using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Blog.DataAccess.Data;
using Blog.Common.Interface.IService;
using Blog.Server.Service;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.Modal;
using Blazored.Typeahead;
using MudBlazor.Services;
using Blog.Server.SignalR;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddSignInManager<SignInManager<IdentityUser>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();
builder.Services.AddBlazoredModal();

var baseAddress = builder.Configuration["BaseAddress"];

builder.Services.AddHttpClient<IBlogService, BlogService>(client => client.BaseAddress = new Uri(baseAddress))
    .SetHandlerLifetime(TimeSpan.FromHours(2));
builder.Services.AddHttpClient<IPostService, PostService>(client => client.BaseAddress = new Uri(baseAddress))
    .SetHandlerLifetime(TimeSpan.FromHours(2));
builder.Services.AddHttpClient<ITagService, TagService>(client => client.BaseAddress = new Uri(baseAddress))
    .SetHandlerLifetime(TimeSpan.FromHours(2));
builder.Services.AddHttpClient<IPostTagService, PostTagService>(client => client.BaseAddress = new Uri(baseAddress))
    .SetHandlerLifetime(TimeSpan.FromHours(2));
builder.Services.AddHttpClient<IUserSubscribedBlogService, UserSubscribedBlogService>(client => client.BaseAddress = new Uri(baseAddress))
    .SetHandlerLifetime(TimeSpan.FromHours(2));
builder.Services.AddHttpClient<ICommentService, CommentService>(client => client.BaseAddress = new Uri(baseAddress))
    .SetHandlerLifetime(TimeSpan.FromHours(2));
builder.Services.AddHttpClient<IAccountService, AccountService>(client => client.BaseAddress = new Uri(baseAddress))
    .SetHandlerLifetime(TimeSpan.FromHours(2));

//MudBlazor
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSignalR();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

builder.Services.AddScoped<AuthenticationStateProvider, AuthProvider>();
builder.Services.AddScoped<AuthProvider>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseResponseCompression();

app.MapBlazorHub();
app.MapHub<UpdateHub>("/updateHub");
app.MapHub<TagUpdateHub>("/tagUpdateHub");
app.MapFallbackToPage("/_Host");

app.UseAuthentication();
app.UseAuthorization();

app.Run();
