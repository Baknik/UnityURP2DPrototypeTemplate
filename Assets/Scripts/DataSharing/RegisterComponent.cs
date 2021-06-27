using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterComponent : MonoBehaviour
{
    public Component Component;
    public ComponentCollection Collection;

    private void OnEnable()
    {
        Collection.Components.Add(Component);
    }

    private void OnDestroy()
    {
        Collection.Components.Remove(Component);
    }
}
