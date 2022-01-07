using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Layer Mask", menuName = "Data Objects/New Layer Mask")]
public class LayerMaskData : ScriptableObject
{
    public LayerMask Value;
}
