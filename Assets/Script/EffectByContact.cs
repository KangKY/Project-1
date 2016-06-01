using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EffectByContact : MonoBehaviour {

    public GameObject score_effect;
    public GameObject score_text_effect;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BackGround"))
        {
            return;
        }
        if (PlayerMain.ten_point_jump)
            score_text_effect.GetComponent<Text>().text = "+10";
   
        else
            score_text_effect.GetComponent<Text>().text = "+2";
      
        Effect_delete.timerForDelete = 0.0f;
        Effect_delete.is_show = true;
        score_text_effect.SetActive(true);
        score_effect.SetActive(true);
    }
}
