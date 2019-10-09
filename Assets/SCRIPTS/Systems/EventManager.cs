using System;
using AirHockey.Data;

namespace AirHockey.Systems
{
    public static class EventManager
    {
        public static event Action<PlayerType, int> ScoringEvent;
        public static event Action<PlayerType> GameFinishEvent;

        public static void InvokeScoringEvent(PlayerType type, int score) => ScoringEvent?.Invoke(type, score);
        public static void InvokeGameFinishEvent(PlayerType winner) => GameFinishEvent?.Invoke(winner);
    }
}
