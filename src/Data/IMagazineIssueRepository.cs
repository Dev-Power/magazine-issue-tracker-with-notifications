namespace MagazineTracker.Data;

public interface IMagazineIssueRepository
{
    Task<LatestMagazineIssue> GetLatestIssue();
    Task SaveLatestIssue(int latestIssueNumber);
}