using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public Transform Target;

    private void LateUpdate()
    {
        this.transform.position = Target.position;
    }
}
