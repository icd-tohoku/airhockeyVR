using System;
using AirHockey.Data;
using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.UI
{
    [RequireComponent(typeof(Text))]
    public class DefaultScoreBoard : ScoreBoardBase
    {
        [SerializeField] private Text scoreBoard = null;

        protected override void OnScoreChanged(PlayerType type, int score)
        {
            if (type == playerType)
            {
                scoreBoard.text = score.ToString();
            }
        }
    }
}