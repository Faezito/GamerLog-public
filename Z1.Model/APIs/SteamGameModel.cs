public class SteamGamesResponse
{
    public GamesResponse response { get; set; }
}

public class GamesResponse
{
    public int game_count { get; set; }
    public List<Game> games { get; set; }
}

public class Game
{
    public int appid { get; set; }
    public string name { get; set; }
    public int playtime_forever { get; set; } // minutos
}
