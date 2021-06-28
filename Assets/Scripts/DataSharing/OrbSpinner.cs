using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSpinner : MonoBehaviour
{
    public OrbCollection AllOrbs;
    public TransformCollection AllLightTransforms;

    private void Update()
    {
        List<Orb> orbs = AllOrbs.Get();
        List<Transform> lightTransforms = AllLightTransforms.Get();

        foreach (Orb o in orbs)
        {
            float closestLightDistance = float.MaxValue;
            foreach (Transform lt in lightTransforms)
            {
                float distance = Vector3.Distance(lt.position, o.transform.position);
                if (distance < closestLightDistance)
                {
                    closestLightDistance = distance;
                }
            }
            o.transform.Rotate(Vector3.forward * o.RotationSpeed * Time.deltaTime * closestLightDistance);
        }
    }
}
