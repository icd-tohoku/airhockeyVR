using System;
using System.Collections;
using System.Collections.Generic;
using AirHockey.Data;
using AirHockey.Systems;
using UnityEngine;

namespace AirHockey.UI
{
    public abstract class ScoreBoardBase : MonoBehaviour
    {
        [SerializeField] protected PlayerType playerType;

        private void Awake()
        {
            EventManager.ScoringEvent += OnScoreChanged;
        }

        private void OnDestroy()
        {
            EventManager.ScoringEvent -= OnScoreChanged;
        }

        protected abstract void OnScoreChanged(PlayerType type, int score);
    }
}