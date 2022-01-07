using UnityEngine;

namespace MVCEngine.Data
{
    public class CustomTime : IDataModel, ISingletonData
    {
        public float TimeScale { get; private set; }

        public CustomTime()
        {
            TimeScale = 1f;
        }

        public void SetTimeScale(float timeScale)
        {
            TimeScale = timeScale;
        }
    }
}