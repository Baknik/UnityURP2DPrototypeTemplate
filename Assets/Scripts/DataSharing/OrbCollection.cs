using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbCollection : ComponentCollection
{
    public List<Orb> Get()
    {
        return Components.ConvertAll(x => (Orb)x);
    }
}
