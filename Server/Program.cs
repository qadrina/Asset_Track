using Asset_Track.Shared.Models;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Asset_Track.Server.Hubs;						

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();							  
builder.Services.AddResponseCompression(options =>											   
{
	options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(															  
		new[] {"application/octet-stream"});									  
});   

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AssetTrackContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();										   
var app = builder.Build();

// Configure the HTTP request pipeline.
// [Asset Tracking]	   
app.UseResponseCompression();							 
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
	app.UseSwaggerUI();				   
	app.UseSwagger();				 
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
// [Asset Tracking]	   
app.MapHub<AssetHub>("/assethub");							  
app.MapFallbackToFile("index.html");

app.Run();
