using System.Collections.Generic;

public enum GameTag
{
    Cuestick,
    Cueball,
    Ball,
    Table,
    Rack,
    AimLine,
    TrajectoryLine,
    GhostBall,
}


public static class GameTags
{
    public static readonly Dictionary<GameTag, string> TagToName = new()
    {
        { GameTag.Cuestick, "Cuestick" },
        { GameTag.Cueball, "Cueball" },
        { GameTag.Ball, "Ball" },
        { GameTag.Table, "Table" },
        { GameTag.Rack, "Rack" },
        { GameTag.AimLine, "AimLine" },
        { GameTag.TrajectoryLine, "TrajectoryLine" },
        { GameTag.GhostBall, "GhostBall" },
    };
    
    public static string GetTagName(GameTag tag)
    {
        return TagToName[tag];
    }
}
