using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformCollection : ComponentCollection
{
    public List<Transform> Get()
    {
        return Components.ConvertAll(x => (Transform)x);
    }
}
