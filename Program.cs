using EmailService;
using EmailService.Models;
using EmailService.Services;
using System.Runtime;

var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddSingleton<IUserRepo, UserRepo>();
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

builder.Services.AddTransient<IUserRepo,UserRepo>(provider =>
{
    return new UserRepo(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddHostedService<Worker>();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

var host = builder.Build();
host.Run();
