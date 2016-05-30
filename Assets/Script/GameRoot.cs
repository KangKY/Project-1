using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameRoot : MonoBehaviour {
    public float step_timer = 0.0f;

	// Update is called once per frame
	void Update () {
        this.step_timer += Time.deltaTime;
    }
    
    public float getPlayTime()
    {
        return this.step_timer;
    }
}
