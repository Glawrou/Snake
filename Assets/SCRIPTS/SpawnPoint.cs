using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject Food;
    public GameObject Let;
    public GameObject Rubi;

    public Transform Pointes;

    public Road MyRoad;

    Road.MaterialControl MC;

    private void Start()
    {
        MC = MyRoad.MaterialController;

        int Rand = Random.Range(1,100);

        if(Rand < 50)
        {
            Instantiate(Food, transform.position, Quaternion.identity, Pointes).GetComponent<Food>().SetMaterial(MC, Random.Range(0, 5));
        }
        else if(Rand < 70)
        {
            Instantiate(Let, transform.position, Quaternion.identity, Pointes);
        }
        else if(Rand < 90)
        {
            Instantiate(Rubi, transform.position, Quaternion.identity, Pointes);
        }
        else
        {

        }
    }
}
