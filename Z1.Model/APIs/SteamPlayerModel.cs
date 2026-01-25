public class SteamPlayerResponse
{
    public PlayerResponse response { get; set; }
}

public class PlayerResponse
{
    public List<Player> players { get; set; }
}

public class Player
{
    public string steamid { get; set; }
    public string personaname { get; set; }
    public string avatarfull { get; set; }
}
