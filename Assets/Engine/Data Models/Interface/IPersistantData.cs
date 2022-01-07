using UnityEngine;

namespace MVCEngine.Data
{
    public interface IPersistantData
    {
        public void Load(SaveGame saveGame);
        public void Save(SaveGame saveGame);
    }
}