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
			{PlayerType.Desktop, 0},
			{PlayerType.VR, 0}
		};

		public static void AddScore(PlayerType type)
		{
			Instance.Scores[type] += 1;

			EventManager.InvokeScoringEvent(type, Instance.Scores[type]);

			if (Instance.Scores[type] >= 10)
			{
				EventManager.InvokeGameFinishEvent(type);
			}
		}
	}
}
