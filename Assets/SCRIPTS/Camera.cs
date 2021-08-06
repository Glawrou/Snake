using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Camera : MonoBehaviour
{
    public Transform target;

    HeadSnake CS;

    public ControlUI CUI;

    private void Start()
    {
        CS = target.gameObject.GetComponent<HeadSnake>();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(4, transform.position.y, target.position.z - 10), CS.ControlHead.SpeedSnake * Time.deltaTime);
    }

    public void GameOver()
    {
        CUI.GameOver();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [System.Serializable()]
    public class ControlUI
    {
        public Image PanelGameOver;
        public Text TextGameOver;
        public Button ButtonGameOver;
        public Image ImageButten;

        public void GameOver()
        {
            PanelGameOver.enabled = true;
            TextGameOver.enabled = true;
            ButtonGameOver.enabled = true;
            ImageButten.enabled = true;
        }       
    }
}
