using Microsoft.Extensions.Options;
using Twilio.Rest.Api.V2010.Account;

namespace MagazineTracker;

public class SmsService : INotificationService
{
    private readonly SmsSettings _smsSettings;

    public SmsService(IOptions<SmsSettings> smsSettings)
    {
        _smsSettings = smsSettings.Value;
    }
    
    public async Task SendNewIssueNotification(int issueNumber, string coverUrl, string mediaUrl)
    {
        MessageResource.Create(
            body: $"Here's the latest issue (#{issueNumber}) of The MagPi Magazine: {mediaUrl}",
            from: new Twilio.Types.PhoneNumber(_smsSettings.FromPhoneNumber),
            to: new Twilio.Types.PhoneNumber(_smsSettings.ToPhoneNumber),
            mediaUrl: string.IsNullOrEmpty(coverUrl) ? null : new []
            {
                new Uri(coverUrl)
            }.ToList()
        );
    }
}