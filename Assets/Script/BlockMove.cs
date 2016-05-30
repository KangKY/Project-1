using UnityEngine;
using System.Collections;

public class BlockMove : MonoBehaviour {

    // Objects
    private PlayerMain player = null;
    private PlayerMain player2 = null;
    
    private GameControl gameControl = null;


    public GameObject explode = null;
    
    public int speed;

    Vector2 velocity;
    // Use this for initialization
    void Awake () {
      
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMain>();
        this.player2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerMain>();
        
        this.gameControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
     

    }

    void FixedUpdate()
    {
        velocity = this.GetComponent<Rigidbody2D>().velocity;
        velocity.x = -(this.speed + 50 * GameControl.game_turn_count) * Time.deltaTime;
        this.GetComponent<Rigidbody2D>().velocity = velocity;
        
    }

 

    /// <summary>
    /// 플레이어와 적이 충돌했을 때 이펙트 효과 생성
    /// </summary>
    void OnExplode()
    {
        Quaternion spawnRotation = new Quaternion();
        //Debug.LogError("OnExplode() Call!");

        Instantiate(explode, transform.position + new Vector3(0, 0.5f, 0), spawnRotation);
    }

    /// <summary>
    /// 적이 플레이어와 충돌했을 때 자동으로 호출되는 함수
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BackGround"))
        {
            return;
        }
        if(this.gameControl.Collider_OnOff)
        {
           
            if (other.CompareTag("Player") || other.CompareTag("Player2"))
            {

                this.gameControl.GetComponent<AudioSource>().clip = gameControl.dead_sound;
                this.gameControl.GetComponent<AudioSource>().Play();
               
                this.gameControl.ClearObstacle();

                // 게임 컨트롤 코루틴 제거 = 적 생성 스탑
                this.gameControl.StopAllCoroutines();

                // 충돌 boolean 변수 설정
                this.gameControl.is_collider = true;
                // 해당하는 애니메이션 실행
                if (other.CompareTag("Player"))
                {
                    this.player.anim.SetFloat("Dead", 0.2f);
                    this.player2.anim.Stop();
                }
                else
                {
                    this.player.anim.Stop();
                    this.player2.anim.SetFloat("Dead", 0.2f);
                }



                // 충돌 이펙트!
                OnExplode();


                // 첫 번째 스테이지에서 죽었을 때 업적 언락
                if(this.gameControl.level_control.level == 0)
                {
                    this.gameControl.UnLockAchievement(1);
                }
            }
        }
        
            
    }
}
