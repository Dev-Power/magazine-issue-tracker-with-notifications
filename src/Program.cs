using MagazineTracker;
using MagazineTracker.Data;
using MagazineTracker.MagazineTrackerServices;
using MagazineTracker.NotificationServices;
using Twilio;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostBuilderContext, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddTransient<IMagazineIssueRepository, JsonMagazineIssueRepository>();
        services.AddTransient<IMagazineTrackerService, MagPiTrackerService>();
        services.AddTransient<INotificationService, SmsService>();
        services.Configure<DatabaseSettings>(hostBuilderContext.Configuration.GetSection("DatabaseSettings"));
        services.Configure<SmsSettings>(hostBuilderContext.Configuration.GetSection("SmsSettings"));
        
        var accountSid = hostBuilderContext.Configuration["Twilio:AccountSid"];
        var authToken = hostBuilderContext.Configuration["Twilio:Authtoken"];
        TwilioClient.Init(accountSid, authToken);
    })
    .Build();

await host.RunAsync();
