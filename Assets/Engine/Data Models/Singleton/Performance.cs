using UnityEngine;

namespace MVCEngine.Data
{
    public class Performance : IDataModel, ISingletonData
    {
        public bool CountFPS{ get; private set; }
        public float FPS { get; private set; }

        public Performance()
        {
            CountFPS = true;
            FPS = 0f;
        }

        public void SetFPS(float fps)
        {
            FPS = fps;
        }

        public void StartCountingFPS()
        {
            CountFPS = true;
        }

        public void StopCountingFPS()
        {
            CountFPS = false;
        }
    }
}