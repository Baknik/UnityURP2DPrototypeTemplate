using MVCEngine;
using MVCEngine.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

[RequireComponent(typeof(Volume))]
public class PostProcessingVolumeView : MonoBehaviour
{
    private Volume _volume;

    [SerializeField]
    private IntData _volumeID;

    public PostProcessingVolume PostProcessingVolume { get; private set; }

    private void Awake()
    {
        _volume = GetComponent<Volume>();
    }

    private void OnEnable()
    {
        PostProcessingVolume = Engine.Instance.PostProcessingController.Register(_volumeID.Value, _volume.weight);

        if (PostProcessingVolume == null)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    private void OnDisable()
    {
        Engine.Instance.PostProcessingController.Unregister(_volumeID.Value);

        PostProcessingVolume = null;
    }

    private void Update()
    {
        _volume.weight = PostProcessingVolume.Weight;
    }
}
