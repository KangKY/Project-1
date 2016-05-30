using UnityEngine;
using System.Collections;

public class BGM : MonoBehaviour {
    public AudioClip game_clip;
    public AudioClip home_clip;
    void Start()
    {
        //if(!this.GetComponent<AudioSource>().isPlaying)
        this.GetComponent<AudioSource>().volume = ButtonControl.BGM_Sound_Volume;
        DontDestroyOnLoad(this);
        PlayHomeBGM();
        //this.gameObject.SetActive(false);


        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }
    public void PlayGameBGM()
    {
        this.GetComponent<AudioSource>().clip = game_clip;
        this.GetComponent<AudioSource>().volume = ButtonControl.BGM_Sound_Volume;
        this.GetComponent<AudioSource>().Play();
    }

    public void PlayHomeBGM()
    {
        this.GetComponent<AudioSource>().clip = home_clip;
        this.GetComponent<AudioSource>().volume = ButtonControl.BGM_Sound_Volume;
        this.GetComponent<AudioSource>().Play();
    }
}
