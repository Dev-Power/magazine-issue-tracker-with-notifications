using AngleSharp;

namespace MagazineTracker;

public class MagPiTrackerService : IMagazineTrackerService
{
    private const string MagpiRootUrl = "https://magpi.raspberrypi.com";
    
    public async Task<int> GetLatestIssueNumber()
    {
        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync($"{MagpiRootUrl}/issues/");
        var latestCoverLinkSelector = ".c-latest-issue > .c-latest-issue__cover > a";
        var latestCoverLink = document.QuerySelector(latestCoverLinkSelector);
        var rawLink = latestCoverLink.Attributes.GetNamedItem("href").Value;
        return int.Parse(rawLink.Substring(rawLink.LastIndexOf('/') + 1));
    }

    public async Task<string> GetLatestIssueCoverUrl()
    {
        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync($"{MagpiRootUrl}/issues/");
        var latestCoverImageSelector = ".c-latest-issue > .c-latest-issue__cover > a > img";
        var latestCoverImage = document.QuerySelector(latestCoverImageSelector);
        var latestCoverImageUrl = latestCoverImage.Attributes.GetNamedItem("src").Value;
        return latestCoverImageUrl;
    }

    public async Task<string> GetIssuePdfUrl(int issueNumber)
    {
        var issueUrl = $"{MagpiRootUrl}/issues/{issueNumber}/pdf/download";
        
        var config = AngleSharp.Configuration.Default.WithDefaultLoader();
        var address = issueUrl;
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync(address);
        var cellSelector = "iframe";
        var cell = document.QuerySelector(cellSelector);
        var iframeSrc = cell.Attributes.GetNamedItem("src").Value;

        return $"{MagpiRootUrl}/{iframeSrc.TrimStart('/')}";
    }
}