using System.Collections.Generic;

public enum GameTag
{
    Cuestick,
    Cueball,
    Ball,
    Table,
}


public static class GameTags
{
    public static readonly Dictionary<GameTag, string> TagToName = new()
    {
        { GameTag.Cuestick, "Cuestick" },
        { GameTag.Cueball, "Cueball" },
        { GameTag.Ball, "Ball" },
        { GameTag.Table, "Table" },
    };
    
    public static string GetTagName(GameTag tag)
    {
        return TagToName[tag];
    }
}
