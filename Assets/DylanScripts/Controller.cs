using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadGame()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }
    public void LoadOptions()
    {
        SceneManager.LoadSceneAsync("options");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
