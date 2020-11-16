using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DevionGames.QuestSystem
{
	[System.Serializable]
	public class QuestDatabase : ScriptableObject
	{
		public List<Quest> items = new List<Quest>();
		public List<Configuration.Settings> settings = new List<Configuration.Settings>();

	}
}