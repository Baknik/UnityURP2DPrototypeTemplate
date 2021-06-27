using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ComponentCollection : MonoBehaviour
{
    [HideInInspector]
    public List<Component> Components;
}
