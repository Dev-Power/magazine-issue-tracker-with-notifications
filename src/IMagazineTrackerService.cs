namespace MagazineTracker;

public interface IMagazineTrackerService
{
    Task<int> GetLatestIssueNumber();
    Task<string> GetLatestIssueCoverUrl();
    Task<string> GetIssuePdfUrl(int issueNumber);
}