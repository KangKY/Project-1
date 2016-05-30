using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour
{
    GameControl gameControl;
    void Start()
    {
        this.gameControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("BackGround"))
        {
            return;
        }

        for(int num = 0; num < 10; num++)
            this.gameControl.pool[num].RemoveItem(other.gameObject);
       // this.gameControl.pool[this.gameControl.rand_down].RemoveItem(other.gameObject);
        //Destroy(other.gameObject);
    }
}
