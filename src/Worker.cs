using MagazineTracker.Data;

namespace MagazineTracker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IMagazineIssueRepository _magazineIssueRepository;
    private readonly IMagazineTrackerService _magazineTrackerService;
    private readonly INotificationService _notificationService;

    public Worker(ILogger<Worker> logger, IMagazineIssueRepository magazineIssueRepository, IMagazineTrackerService magazineTrackerService, INotificationService notificationService)
    {
        _logger = logger;
        _magazineIssueRepository = magazineIssueRepository;
        _magazineTrackerService = magazineTrackerService;
        _notificationService = notificationService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            
            var latestProcessedIssue = await _magazineIssueRepository.GetLatestIssue();
            var latestIssueNumber = await _magazineTrackerService.GetLatestIssueNumber();
            if (latestIssueNumber > latestProcessedIssue.IssueNumber)
            {
                _logger.LogInformation("New issue detected: {latestIssueNumber}", latestIssueNumber);
                var coverUrl = await _magazineTrackerService.GetLatestIssueCoverUrl();
                var pdfUrl = await _magazineTrackerService.GetIssuePdfUrl(latestIssueNumber);
                await _notificationService.SendNewIssueNotification(latestIssueNumber, coverUrl, pdfUrl);
                await _magazineIssueRepository.SaveLatestIssue(latestIssueNumber);
            }
            else
            {
                _logger.LogInformation("No new issue is detected.");
            }
            
            await Task.Delay(1000 * 60 * 60, stoppingToken);
        }
    }
}
