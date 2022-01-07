using MVCEngine.Data;
using System.Collections.Generic;
using UnityEngine;

namespace MVCEngine
{
    public class GraphicsQualityController : MonoBehaviour, IController
    {
        private int _fpsFrameCount;
        private float _summedFrameTime;

        private Performance _performance;

        public void Initialize(Performance performance)
        {
            _performance = performance;

            ResetFPSCounter();

            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 300;
        }

        private void Update()
        {
            if (_performance.CountFPS)
            {
                _fpsFrameCount++;
                _summedFrameTime += Time.deltaTime;

                if (_summedFrameTime >= 1f)
                {
                    _performance.SetFPS((float)_fpsFrameCount);

                    ResetFPSCounter();
                }
            }
        }

        private void ResetFPSCounter()
        {
            _fpsFrameCount = 0;
            _summedFrameTime = 0f;
        }

        public Performance GetPerformanceModel()
        {
            return _performance;
        }
    }
}