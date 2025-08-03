using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    public Canvas menu;
    public Canvas options;
    public Animation anim;

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
    public void HowBack()
    {
        anim.Play("3");
        options.enabled = false;
        menu.enabled = true;

    }
    public void HowTo()
    {
        options.enabled = false;
        menu.enabled = false;
        anim.Play("2");
    }


    IEnumerator CamAnimStart()
    {
        anim.Play();
        while (true)
        {
            if (!anim.isPlaying)
            {
                SceneManager.LoadSceneAsync("SampleScene");
            }
            yield return null;
        }
    }
}

