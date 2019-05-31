public class BattleLogGenerator
{
    readonly string waitText = "∇";
    private BattleText format;
    private Actor player;
    private Actor enemy;
        
    public BattleLogGenerator(BattleText format, Actor player, Actor enemy)
    {
        this.format = format;
        this.player = player;
        this.enemy = enemy;
    }

    public string Generate(string displayMessage, string message, int value, bool isWait)
    {
        message = join(displayMessage, message);
        message = addWaitText(message, isWait); 
        return Translate(message, value);
    }
        
    private string join(string displayMassage, string message)
    {
        if (displayMassage.Contains(waitText))
        {
            return displayMassage.Replace(waitText, string.Empty) + "\n" + message;
        }
        return message;
    }
        
    private string addWaitText(string message, bool wait)
    {
        if (wait)
        {
            return message + waitText;
        }
        return message;
    }
        
    private string Translate(string message, int value)
    {
        return message.Replace("<PlayerName>", player.Name)
            .Replace("<EnemyName>", enemy.Name)
            .Replace("<Points>", value.ToString());
    }
}