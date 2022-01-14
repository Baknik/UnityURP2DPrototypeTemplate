using MVCEngine.Data;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MVCEngine
{
	public class Engine
	{
		private static Engine _instance;

		public static Engine Instance
		{
			get
			{
				if (Engine._instance == null)
				{
					Engine._instance = new Engine();
				}

				return Engine._instance;
			}
		}

		private List<ISingletonData> _singletonDataModels;
		private SaveGame _saveGame;

		private Engine()
		{
			// Initialize singleton data models
			_singletonDataModels = new List<ISingletonData>();

			_saveGame = new SaveGame();
			_saveGame.LoadFromJSON("save_0");

			// Initialize controllers
			var engineContainer = new GameObject { name = "[Core Engine]" };
			Transform engineParentTransform = engineContainer.transform;
			Object.DontDestroyOnLoad(engineContainer);

			// Inject runtime data model dependencies
		}

		public void Save()
		{
			_saveGame.SaveToJSON("save_0");
		}
	}
}

