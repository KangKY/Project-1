using UnityEngine;
using System.Collections;

public class Set_Anim : MonoBehaviour {
    public Animator anim;
    public string default_state;
    // Use this for initialization
    void Awake () {
        if(HomeControl.home_start)
            this.anim.Play(default_state);    
    }
}
