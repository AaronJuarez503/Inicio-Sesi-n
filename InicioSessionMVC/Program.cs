var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

// builder.Services.AddAuthentication()
//     .AddFacebook(facebookOptions =>
//     {
//         facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
//         facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
//         facebookOptions.CallbackPath = "/signin-facebook"; // Asegúrate de que coincida con la URL de redireccionamiento de Facebook
//     });
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
