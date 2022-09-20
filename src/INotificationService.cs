namespace MagazineTracker;

public interface INotificationService
{
    Task SendNewIssueNotification(int issueNumber, string coverUrl, string mediaUrl);
}