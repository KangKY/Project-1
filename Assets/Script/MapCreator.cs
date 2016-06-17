using UnityEngine;
using System.Collections;


public class MapCreator : MonoBehaviour
{
    public float moveSpeed;
    private GameObject[] current;
    private Renderer render;
    private GameControl gameControl;

    void Start()
    {
        render = this.GetComponent<Renderer>();
        this.gameControl =
           GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
    }

    void Update()
    {
        //current = this.gameControl.cur_BG;
        if (moveSpeed != 0 && !this.gameControl.is_collider)
        {
            // GC Alloc 현저히 줄게 하려면 if문 사용 X     
            render.sharedMaterial.mainTextureOffset
                    = new Vector2(Time.time * moveSpeed, 0f);
            
        }
    }
}
