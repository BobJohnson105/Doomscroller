using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    public Canvas menu;
    public Canvas options;
    public Animation startanim;
    public Animation howtoanim;
    // Start is called before the first frame update
    void Start()
    {
        options.enabled = false;
    }

    public void LoadGame()
    {
        //menu.enabled = false;
        StartCoroutine(CamAnimStart());
    }
    public void LoadOptions()
    {
        menu.enabled = false;
        options.enabled = true;
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Back()
    {
        options.enabled = false;
        menu.enabled = true; 
    }
    public void HowTo()
    {
        options.enabled = false;
        menu.enabled = false;
        StartCoroutine(CamAnimHowTo());
    }


    IEnumerator CamAnimStart()
    {
        startanim.Play();
        while (true)
        {
            if (!startanim.isPlaying)
            {
                SceneManager.LoadSceneAsync("SampleScene");
            }
            yield return null;
        }
    }
    IEnumerator CamAnimHowTo()
    {
        howtoanim.Play();
        while (true)
        {
            if (!howtoanim.isPlaying)
            
            yield return null;
        }
    }
}
