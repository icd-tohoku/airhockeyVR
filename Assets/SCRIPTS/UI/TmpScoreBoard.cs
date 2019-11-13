using System;
using AirHockey.Data;
using TMPro;
using UnityEngine;

namespace AirHockey.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TmpScoreBoard : ScoreBoardBase
    {
        [SerializeField] private TextMeshProUGUI scoreBoard = null;

        protected override void OnScoreChanged(PlayerType type, int score)
        {
            if (type == playerType)
            {
                scoreBoard.text = score.ToString();
            }
        }
    }
}