using UnityEngine;
using System.Collections;

public class Effect_delete : MonoBehaviour {
    public static float timerForDelete;
    public float DeleteLimit;
    public static bool is_show = false;
    // Update is called once per frame
    void Update()
    {
        timerForDelete += Time.deltaTime;
        if (timerForDelete > DeleteLimit && is_show)
        {
            //Destroy(gameObject); 
            gameObject.SetActive(false);
        }
    }
}
