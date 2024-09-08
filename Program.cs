using EmailService;
using EmailService.Services;

var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddSingleton<IUserRepo, UserRepo>();
builder.Services.AddTransient<IUserRepo,UserRepo>(provider =>
{
    return new UserRepo(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
