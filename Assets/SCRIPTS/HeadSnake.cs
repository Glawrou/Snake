using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class HeadSnake : MonoBehaviour
{
    public ControlSnake ControlHead;

    public Tail ControlTail;

    bool GameOver = false;

    public UnityEvent OnGameOver;

    public Text CountFood, CountRubi;

    private int RubiCount = 0;
    private bool Flawer = false;

    private void Update()
    {
        //
        if (GameOver == false)
            ControlHead.ControlMovment(Flawer);
        //

        //
        if (GameOver == false)
        {
            StartCoroutine(ControlTail.FerstPosition(transform, (int)ControlHead.SpeedSnake));
            ControlTail.FolowingTail();
        }
        //

        if(RubiCount == 3)
        {
            RubiCount = 0;
            StartCoroutine(StartFlawer());
            CountFood.text = "0";
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(Flawer == false)
        {
            if (other.tag == "Food")
            {
                if (other.GetComponent<Food>().GetMaterial(gameObject))
                    EGameOver();
                else
                {
                    RubiCount = 0;
                    CountFood.text = (System.Convert.ToInt32(CountFood.text) + 1).ToString();
                    ControlTail.SpawnTail(gameObject);
                    StartCoroutine(ControlTail.ThereIs());
                }
            }

            if (other.tag == "Rubi")
            {
                CountRubi.text = (System.Convert.ToInt32(CountRubi.text) + 1).ToString();
                Destroy(other.gameObject);
                RubiCount++;
            }

            if (other.tag == "Let")
            {
                EGameOver();
            }

            if (other.tag == "Respawn")
            {
                StartCoroutine(ControlTail.RestartColor(gameObject, other));
            }

            if (other.tag == "Finish")
            {
                EGameOver();
            }
        }
        else
        {
            if(other.tag == "Food" ||  other.tag == "Rubi" || other.tag == "Let")
            {
                Destroy(other.gameObject);
            }
            else if(other.tag == "Finish")
            {
                EGameOver();
            }
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Food")
        {
            other.gameObject.transform.position = Vector3.Lerp(other.gameObject.transform.position, gameObject.transform.position, 2f * Time.deltaTime);
            if(Vector3.Distance(other.gameObject.transform.position, gameObject.transform.position) < 0.7f)
            {
                Destroy(other.gameObject);
            }
        }
    }

    IEnumerator StartFlawer()
    {
        Flawer = true;
        int StartSpeed = (int)ControlHead.SpeedSnake;
        ControlHead.SpeedSnake = StartSpeed * 3;
        yield return new WaitForSeconds(5);        
        ControlHead.SpeedSnake = StartSpeed;
        yield return new WaitForSeconds(1);
        Flawer = false;
    }

    public void EGameOver()
    {
        GameOver = true;
        OnGameOver.Invoke();
    }

    [System.Serializable]
    public class ControlSnake
    {
        public CharacterController CCSnake;

        [Range(1, 20)]
        public float SpeedSnake = 10;

        public Transform MyTransform;

        Vector3 positionMouse;
        Vector3 startPositionMouse;

        public ControlSnake(Transform _transform)
        {
            MyTransform = _transform;
        }

        public void ControlMovment(bool Flawer)
        {
            if(Flawer == false)
            {
                CCSnake.Move(new Vector3(0, 0, SpeedSnake * Time.deltaTime));

                positionMouse = Input.mousePosition;

                if (Input.GetMouseButtonDown(0))
                {
                    startPositionMouse = Input.mousePosition;
                }
                else if (Input.GetMouseButton(0))
                {
                    float movement = (-startPositionMouse.x + positionMouse.x) / 60;

                    CCSnake.Move(new Vector3(movement, 0));

                    ControlRotate(movement);

                    startPositionMouse = Input.mousePosition;
                }
            }
            else
            {
                CCSnake.Move(new Vector3(0, 0, SpeedSnake * Time.deltaTime));

                if(MyTransform.position.x < 0)
                {
                    CCSnake.Move(new Vector3(5 * Time.deltaTime, 0, 0));
                }
                else if(MyTransform.position.x > 0)
                {
                    CCSnake.Move(new Vector3(-5 * Time.deltaTime, 0, 0));
                }
            }
            
        }

        public void ControlRotate(float movement)
        {
            float Rotarion = 0;

            Rotarion += movement * 100;

            MyTransform.rotation = Quaternion.Euler(0, Rotarion, 0);

        }


    }

    [System.Serializable]
    public class Tail
    {
        public Transform Head;
        public List<Transform> TailTransform = new List<Transform>();
        public GameObject TailGameobject;

        Vector3[] MyTransformSomeTimeAgo = new Vector3[20];

        public void SpawnTail(GameObject gameObject)
        {
            GameObject GO = Instantiate(TailGameobject, new Vector3(0, 0, 0), Quaternion.identity);
            GO.GetComponent<MeshRenderer>().material = gameObject.GetComponent<MeshRenderer>().material;
            TailTransform.Add(GO.GetComponent<Transform>());
        }

        public IEnumerator FerstPosition(Transform MyTransform, int Speed)
        {
            int i = 0;

            foreach (var item in MyTransformSomeTimeAgo)
            {
                Vector3 s = MyTransform.position;
                yield return new WaitForSeconds((1.0f / Speed) * (i + 1));
                MyTransformSomeTimeAgo[i] = s;
                i++;
            }
        }

        public void FolowingTail()
        {

            int i = 0;

            foreach (var item in TailTransform)
            {
                if(i == 0)
                {
                    item.LookAt(Head.position);
                }
                else
                {
                    if(TailTransform[i - 1] != null)
                    item.LookAt(TailTransform[i-1].position);
                }

                item.position = Vector3.Lerp(item.position, MyTransformSomeTimeAgo[i], 10f);

                i++;
            }    
        }

        public IEnumerator RestartColor(GameObject gameObject, Collider other)
        {
            Road.MaterialControl MC = other.GetComponent<GameObjectStart>().GetMaterial();

            gameObject.GetComponent<MeshRenderer>().material = MC.ColorMaterial[(int)MC.ThisColor];

            int i = 0;

            foreach (var item in TailTransform)
            {
                yield return new WaitForSeconds(0.05f * (i + 1));
                item.gameObject.GetComponent<MeshRenderer>().material = MC.ColorMaterial[(int)MC.ThisColor];
                i++;
            }

        }

        public IEnumerator ThereIs()
        {
            int i = 0;        

            foreach (var item in TailTransform)
            {
                yield return new WaitForSeconds(0.05f * (i + 1));
                item.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                yield return new WaitForSeconds(0.05f * (i + 1));
                item.localScale = new Vector3(1, 1, 1);
                i++;

            }
        }

    }



}
