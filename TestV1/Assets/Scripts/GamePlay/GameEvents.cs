using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameBasicEvent
{
    CueballHitBall,
}

public static class GameEvents
{
    public static Dictionary<GameBasicEvent, Action> BasicEvents = new();
    
    
    public static void RegisterBasicEvent(GameBasicEvent @event, Action action)
    {
        BasicEvents[@event] += action;
    }

    public static void UnregisterBasicEvent(GameBasicEvent @event, Action action)
    {
        BasicEvents[@event] -= action;
    }

    public static void InvokeBasicEvent(GameBasicEvent @event)
    {
        if (BasicEvents.TryGetValue(@event, out var action))
        {
            action?.Invoke();
        }
    }
}
