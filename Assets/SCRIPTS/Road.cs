using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public GameObject Food;
    public GameObject Let;

    public GameObjectStart MyStart;

    public MaterialControl MaterialController;


    private void Awake()
    {
        MaterialController.SetRandomColor();
    }

    [System.Serializable]
    public class MaterialControl
    {
        public Material[] ColorMaterial = new Material[6];
        public Texture2D[] ColorTexture = new Texture2D[6];

        public ColorRoad ThisColor;

        public enum ColorRoad
        {
            Green,
            Blue,
            Turquoise,
            Orange,
            Yellow,
            Crimson
        }

        public void SetRandomColor()
        {
            ThisColor = (ColorRoad)(int)Random.Range(0, 5);
        }

    }

}
