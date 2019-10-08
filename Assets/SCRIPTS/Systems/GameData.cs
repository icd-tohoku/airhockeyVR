using System.Collections;
using System.Collections.Generic;
using AirHockey.Data;
using UnityEngine;

namespace AirHockey.Systems
{
	/// <summary>
	/// Declare as singleton class because this class have only data used in one scene
	/// </summary>
	public class GameData : SingletonMonoBehaviour<GameData>
	{
		public Dictionary<PlayerType, int> Scores { get; private set; } = new Dictionary<PlayerType, int>()
		{
			{PlayerType.Player1, 0},
			{PlayerType.Player2, 0}
		};

		public void AddScore(PlayerType type)
		{
			Instance.Scores[type] += 1;

			EventManager.InvokeScoringEvent(type, Scores[type]);
		}
	}
}
