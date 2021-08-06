using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectStart : MonoBehaviour
{
    public Road MyRoad;
    Road.MaterialControl MaterialController;

    public MeshRenderer MyMesh;
    public ParticleSystem MyParticle;

    private void Start()
    {
        MaterialController = MyRoad.MaterialController;
        MyMesh.material = MaterialController.ColorMaterial[(int)MaterialController.ThisColor];
        var shape = MyParticle.shape;
        shape.texture = MaterialController.ColorTexture[(int)MaterialController.ThisColor];
    }

    public Road.MaterialControl GetMaterial()
    {
        return MaterialController;
    }



}
