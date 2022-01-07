using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayAtZeroPosition : MonoBehaviour
{
    private void FixedUpdate()
    {
        if (this.transform.parent.gameObject.name == "Grab Point")
        {
            this.transform.localPosition = Vector3.zero;
            this.transform.forward = this.transform.parent.forward;
        }
    }
}
