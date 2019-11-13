using System.Collections;
using System.Collections.Generic;
using AirHockey.Data;
using AirHockey.Systems;
using UnityEngine;

namespace AirHockey.UI
{
    public class OperationCanvas : MonoBehaviour
    {
        public void ResetGame()
        {
            GameData.ChangeScore(PlayerType.Desktop, 0);
            GameData.ChangeScore(PlayerType.VR, 0);
            
            EventManager.InvokeInitPuckEvent(false);
        }

        public void ResetPuckPosition()
        {
            EventManager.InvokeInitPuckEvent(false);
        }
    }
}