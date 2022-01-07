using UnityEngine;

namespace MVCEngine.Data
{
    public class PostProcessingVolume : IDataModel
    {
        public int ID { get; private set; }
        public float Weight { get; private set; }

        public PostProcessingVolume(int id, float weight)
        {
            ID = id;
            Weight = weight;
        }

        public void SetWeight(float weight)
        {
            Weight = weight;
        }
    }
}