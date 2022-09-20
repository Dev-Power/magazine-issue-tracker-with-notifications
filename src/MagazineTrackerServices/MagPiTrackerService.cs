using AngleSharp;

namespace MagazineTracker.MagazineTrackerServices;

public class MagPiTrackerService : IMagazineTrackerService
{
    private const string MAGPI_ROOT_URL = "https://magpi.raspberrypi.com";
    
    public async Task<int> GetLatestIssueNumber()
    {
        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync($"{MAGPI_ROOT_URL}/issues/");
        var latestCoverLinkSelector = ".c-latest-issue > .c-latest-issue__cover > a";
        var latestCoverLink = document.QuerySelector(latestCoverLinkSelector);
        var rawLink = latestCoverLink.Attributes.GetNamedItem("href").Value;
        return int.Parse(rawLink.Substring(rawLink.LastIndexOf('/') + 1));
    }

    public async Task<string> GetLatestIssueCoverUrl()
    {
        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync($"{MAGPI_ROOT_URL}/issues/");
        var latestCoverImageSelector = ".c-latest-issue > .c-latest-issue__cover > a > img";
        var latestCoverImage = document.QuerySelector(latestCoverImageSelector);
        var latestCoverImageUrl = latestCoverImage.Attributes.GetNamedItem("src").Value;
        return latestCoverImageUrl;
    }

    public async Task<string> GetIssuePdfUrl(int issueNumber)
    {
        var issueUrl = $"{MAGPI_ROOT_URL}/issues/{issueNumber}/pdf/download";
        
        var config = AngleSharp.Configuration.Default.WithDefaultLoader();
        var address = issueUrl;
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync(address);
        var cellSelector = "iframe";
        var cell = document.QuerySelector(cellSelector);
        var iframeSrc = cell.Attributes.GetNamedItem("src").Value;

        return $"{MAGPI_ROOT_URL}/{iframeSrc.TrimStart('/')}";
    }
}