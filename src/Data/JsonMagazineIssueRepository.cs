using System.Text.Json;
using Microsoft.Extensions.Options;

namespace MagazineTracker.Data;

public class JsonMagazineIssueRepository : IMagazineIssueRepository
{
    private readonly DatabaseSettings _databaseSettings;

    public JsonMagazineIssueRepository(IOptions<DatabaseSettings> databaseSettings)
    {
        _databaseSettings = databaseSettings.Value;
    }
    
    public async Task<LatestMagazineIssue> GetLatestIssue()
    {
        var dbAsJson = await File.ReadAllTextAsync(_databaseSettings.JsonFilePath);
        var latestIssue = JsonSerializer.Deserialize<LatestMagazineIssue>(dbAsJson);
        return latestIssue;
    }

    public async Task SaveLatestIssue(int latestIssueNumber)
    {
        var dbAsJson = await File.ReadAllTextAsync(_databaseSettings.JsonFilePath);
        var latestIssue = JsonSerializer.Deserialize<LatestMagazineIssue>(dbAsJson);
        latestIssue.IssueNumber = latestIssueNumber;
     
        dbAsJson = JsonSerializer.Serialize(latestIssue);
        await File.WriteAllTextAsync(_databaseSettings.JsonFilePath, dbAsJson);
    }
}