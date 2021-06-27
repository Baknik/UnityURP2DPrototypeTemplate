using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSpinner : MonoBehaviour
{
    public OrbCollection AllOrbs;

    private void Update()
    {
        List<Orb> orbs = AllOrbs.Get();
        foreach (Orb o in orbs)
        {
            o.transform.Rotate(Vector3.forward * o.RotationSpeed * Time.deltaTime);
        }
    }
}
