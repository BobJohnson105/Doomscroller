using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    public Canvas menu;
    public Animation startanim;
    // Start is called before the first frame update
    public void LoadGame()
    {
        //menu.enabled = false;
        StartCoroutine(CamAnim());
    }
    public void LoadOptions()
    {
        SceneManager.LoadSceneAsync("options");
    }
    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator CamAnim()
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
}
