using System.Collections;
using System.Collections.Generic;
using AirHockey.Data;
using AirHockey.Systems;
using UnityEngine;

namespace AirHockey.Fields
{
    public class GoalEntity : MonoBehaviour
    {
        [SerializeField] private PlayerType goalOwner;
        public PlayerType GoalOwner => goalOwner;
    }
}