using System;
using System.Collections;
using System.Collections.Generic;
using AirHockey.Data;
using AirHockey.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.UI
{
    public class ScoreBoard : MonoBehaviour
    {
        [SerializeField] private PlayerType playerType = PlayerType.None;
        [SerializeField] private Text scoreBoard = null;

        private void Awake()
        {
            EventManager.ScoringEvent += OnScoreChanged;
        }

        private void OnScoreChanged(PlayerType type, int score)
        {
            if (type == playerType)
            {
                scoreBoard.text = $"{playerType}: {score}";
            }
        }

        private void OnDestroy()
        {
            EventManager.ScoringEvent -= OnScoreChanged;
        }
    }
}