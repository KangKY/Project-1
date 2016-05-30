using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour {



    void DeleteEffect()
    {
       
        PlayerMain.pool.RemoveItem(this.gameObject);
    }
}
