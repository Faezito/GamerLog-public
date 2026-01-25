using System.Net.Http;
using System.Text.Json;
using Z1.Model.APIs;
using Z3.DataAccess.Externo;


public interface ISteamServicos
{
    Task<Player?> GetPlayerAsync(string steamId);
    Task<List<Game>> GetGamesAsync(string steamId);
}
public class SteamServicos : ISteamServicos
{
    private readonly HttpClient _httpClient;
    private readonly IAPIsDataAccess _api;

    public SteamServicos(IAPIsDataAccess api)
    {
        _httpClient = new HttpClient();
        _api = api;
    }

    public async Task<Player?> GetPlayerAsync(string steamId)
    {
        string apiKey = await ObterChave(30);
        var url = $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key={apiKey}&steamids={steamId}";

        var json = await _httpClient.GetStringAsync(url);
        var result = JsonSerializer.Deserialize<SteamPlayerResponse>(json);

        return result?.response?.players?.FirstOrDefault();
    }

    public async Task<List<Game>> GetGamesAsync(string steamId)
    {
        string apiKey = await ObterChave(30);

        var url = $"https://api.steampowered.com/IPlayerService/GetOwnedGames/v1/?key={apiKey}&steamid={steamId}&include_appinfo=true&include_played_free_games=true";

        var json = await _httpClient.GetStringAsync(url);
        var result = JsonSerializer.Deserialize<SteamGamesResponse>(json);

        return result?.response?.games ?? new List<Game>();
    }


    private async Task<string> ObterChave(int id)
    {
        APIModel api = await _api.Obter(id);
        return api.Token;
    }
}
