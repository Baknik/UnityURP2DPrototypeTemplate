using MVCEngine.Data;
using System;
using UnityEngine;

namespace MVCEngine
{
    public class TimeController : MonoBehaviour, IController
    {
        private CustomTime _time;

        public void Initialize(CustomTime time)
        {
            _time = time;
        }
    }
}