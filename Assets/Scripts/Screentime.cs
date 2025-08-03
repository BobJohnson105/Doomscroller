using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Screentime : MonoBehaviour
{
    public Canvas goodend;
    public Canvas mainUI;
    public Animation cameraAnim;
    public GameObject phone;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Shutoff()
    {
        goodend.enabled = true;
        mainUI.enabled = false;
        phone.SetActive(false);
        cameraAnim.Play();
    }

    public void Quit()
    {
        SceneManager.LoadSceneAsync("ParallelScene");
    }
}
