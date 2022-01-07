using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVCEngine.Data
{
    [Serializable]
    public class SaveGame : IDataModel
    {
        public List<string> CollectedCollectibles;
        public bool RangeUpgradeUnlocked;

        public SaveGame()
        {
            CollectedCollectibles = new List<string>();
            RangeUpgradeUnlocked = false;
        }

        public void SaveToJSON(string saveGameName)
        {
            try
            {
                string json = JsonUtility.ToJson(this);
                System.IO.File.WriteAllText($"{Application.persistentDataPath}/{saveGameName}.sav", json);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        public void LoadFromJSON(string saveGameName)
        {
            try
            {
                string json = System.IO.File.ReadAllText($"{Application.persistentDataPath}/{saveGameName}.sav");
                JsonUtility.FromJsonOverwrite(json, this);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }
    }
}