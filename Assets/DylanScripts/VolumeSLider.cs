using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSLider : MonoBehaviour
{
    [SerializeField] Slider volume;

    // Start is called before the first frame update
    void Start()
    {
        volume.value = 0.5f;
    }

    // Update is called once per frame
    public void ChangeVolume()
    {
        AudioListener.volume = volume.value;
    }
}
