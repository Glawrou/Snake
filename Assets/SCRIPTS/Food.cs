using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public MeshRenderer[] MyMesh;

    Road.MaterialControl MC;

    public void SetMaterial(Road.MaterialControl _MC, int material)
    {
        MC = _MC;

        foreach (var item in MyMesh)
        {
            item.material = MC.ColorMaterial[material];
        }
    }

    public bool GetMaterial(GameObject gameObject)
    {
        if (gameObject.GetComponent<MeshRenderer>().material.name != MyMesh[0].material.name)
            return true;
        else
            return false;
    }

}
