using MVCEngine.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace MVCEngine
{
    public class PostProcessingController : MonoBehaviour, IController
    {
        private Dictionary<int, PostProcessingVolume> _volumes;

        [SerializeField]
        private IntData _generalPostProcessingVolumeID;

        public void Initialize()
        {
            _volumes = new Dictionary<int, PostProcessingVolume>();
        }

        public PostProcessingVolume Register(int id, float weight)
        {
            PostProcessingVolume model = null;

            if (!_volumes.ContainsKey(id))
            {
                model = new PostProcessingVolume(id, weight);
                _volumes.Add(id, model);
            }

            return model;
        }

        public void Unregister(int id)
        {
            _volumes.Remove(id);
        }
    }
}