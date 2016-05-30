using UnityEngine;
using System.Collections;

public class Effect_Sound : MonoBehaviour {

    public AudioClip button_sound;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }
    public void Button_Sound()
    {
        this.GetComponent<AudioSource>().clip = button_sound;
        this.GetComponent<AudioSource>().volume = ButtonControl.FX_Sound_Volume;
        this.GetComponent<AudioSource>().Play();
    }

}
