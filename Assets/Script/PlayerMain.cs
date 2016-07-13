using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum STEP
{
    NONE = -1,//상태정보 없음
    RUN = 0,// 달리는 상태
    JUMP,//점프
    NUM,//상태 갯수 = 3
};

public class PlayerMain : MonoBehaviour
{

    public static bool is_perfect_jump_Up = false;
    public static bool is_perfect_jump_Down = false;
    public static bool ten_point_jump = false;
    public static float JUMP_HEIGHT_MAX = 2.0f;//점프 최대치
    public float JUMP_HEIGHT;//점프 높이

    public float JUMP_KEY_RELEASE_REDUCE;//점프 후 감속도
    public static float JUMP_SPEED_REDUCE;
    public enum UI_Button
    {
        NONE = -1,
        DOWN,
        UP,
        CENTER_DOWN,
        NUM,
    };

    /// <summary>
    /// 점프 관련 변수들
    /// </summary>
   
    public bool is_landed = false;//착지했는가
    public bool is_double_jmp = false;
    public bool on_enemy = false;
    public bool is_touch = false;
    public bool jump = false;
    public static MemoryPool pool = new MemoryPool();
    public GameObject effect;
    public int ran;
    public uint cmp_score;
    public Animator anim;
    public AudioClip[] Perfect_Sound;
    public AudioClip[] Nomarl_Sound;

    private STEP step = STEP.NONE;
    private STEP next_step = STEP.NONE;
    private float step_timer = 0.0f;//경과 시간
    private Vector2 origin_position;// 원래 포지션
    private float click_timer = -1.0f;// 버튼이 눌린 후의 시간.
    private float CLICK_GRACE_TIME = 0.5f;// 점프하고 싶은 의사를 받아들일 시간
    private string BG;
    private string Enemy;
    private GameControl gameControl;
    // 착지 조금 전에 점프를 눌렀을 경우 점프가 되게 하는 멤버 변수들

    protected bool is_pass = false;
    protected Vector3 player_now_position;
    protected Vector3 player_under_position;
    protected uint score = 100;


    void Awake()
    {
        is_perfect_jump_Up = false;
        is_perfect_jump_Down = false;
        if (this.name == "Player1")
        {
            this.BG = "Bg";
            this.Enemy = "Enemy";
        }
        else if (this.name == "Player2")
        {
            this.BG = "Bg2";
            this.Enemy = "Enemy_2";
        } 
    }


    // Use this for initialization
    void Start()
    {    
        this.gameControl =
           GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        this.anim = GetComponent<Animator>();

        this.next_step = STEP.RUN;

        pool.Create(effect, 7);

        // 애니메이션 초기화
        this.anim.SetFloat("Speed", 0.0f);

        // 시작 위치에서의 Y값 저장( 점프 높이 
        this.origin_position = this.transform.localPosition;
        JUMP_SPEED_REDUCE = JUMP_KEY_RELEASE_REDUCE;
    }

    /*
    // Description : 플레이어가 땅에 착지했는지 판단(is_landed)
    // Type : abstract type
    // Parameter : null
    // return : void
    */
    public void check_landed()
    {
        this.is_landed = false;
       
        player_now_position = this.transform.position;
        player_under_position = player_now_position + Vector3.down * 1.0f;
            

        if (!Physics2D.Linecast(player_now_position, player_under_position,
            1 << LayerMask.NameToLayer(this.BG)))//두 개 사이에 아무것도 없을 때
        {
            return;//아무것도 하지 않고 do while문을 빠져나감
        }

        //두개 사이에 뭔가 있을 때 아래의 처리가 실행
        if (this.step == STEP.JUMP)// 점프 상태라면,
        {
            if (this.step_timer < Time.deltaTime * 1.0f)// 경과 시간이 3.0f미만이라면
            {
                return;
            }
        }
        // 두 위치 사이에 뭔가 있고 JUMP 직후가 아닐 때만 아래가 실행
        switch(this.name)
        {
            case "Player1":
                is_perfect_jump_Up = false;
             
                break;
            case "Player2":
                is_perfect_jump_Down = false;
                break;
        }

        this.anim.SetFloat("Speed", 0.0f);
        this.is_landed = true;
            
        this.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        this.on_enemy = false;
        ten_point_jump = false;
    }

    /*
    // Description : 적을 통과할 때 점수를 증가시켜주는 메서드
    // Type : abstract type
    // Parameter : null
    // return : void
    */
    public void PassEnemy()
    {
        Vector2 player_now_position = this.transform.position;
        Vector2 player_under_position = player_now_position + new Vector2(-2.0f, -5.0f);

        
        if (Physics2D.Linecast(player_now_position, player_under_position,
            1 << LayerMask.NameToLayer(this.Enemy)) && !is_pass)
        {
            if (this.is_double_jmp)
            {
                this.gameControl.DoubleJmp_cnt();
            }

            on_enemy = true;
            is_pass = true;
            
            if (is_perfect_jump_Up && is_perfect_jump_Down)
            {
                if (this.is_double_jmp)
                {
                    ten_point_jump = true;
                    GameControl.score_result += 10;
                }
                    
                else
                {
                    GameControl.score_result += 2;
                }
                
            }
            else if (is_perfect_jump_Up || is_perfect_jump_Down)
            {

                GameControl.score_result += 2;
            }
            else
            {
                GameControl.score_result += 1;
            }
          
            this.is_double_jmp = false;
            this.gameControl.Score_txt.GetComponent<Text>().text =
                GameControl.score_result.ToString();
            
        }
        else if (!Physics2D.Linecast(player_now_position, player_under_position,
            1 << LayerMask.NameToLayer(this.Enemy)) && is_pass)
        {   
            on_enemy = false;
            is_pass = false;
            is_perfect_jump_Up = false;
            is_perfect_jump_Down = false;
        }
       
    }

    void PerfectJump_Effect()
    {
        GameObject gameObject;
        gameObject = pool.NewItem();
        
        gameObject.transform.position = this.transform.position + new Vector3(0, 0f, -1);
 
       
        if (this.name == "Player1")
        {
            gameObject.GetComponent<Animator>().Play("Perfect_Jump_Up");
        }
       

    }



    public void DoubleJmp()
    {
        this.is_double_jmp = true;
    }

    // Update is called once per frame
    void Update()
    {
        this.check_landed();//착지 상태인지 체크

        if (!this.is_landed)
        {     
            this.step = STEP.JUMP;
            this.PassEnemy();
        }

        // Test Source for Recording

        if (this.name == "Player1" && Input.GetKeyDown(KeyCode.Joystick2Button6)) 
            UpDownKey(0);
        else if (this.name == "Player1" && Input.GetKeyUp(KeyCode.Joystick2Button6))
            UpDownKey(1);
        if (this.name == "Player2" && Input.GetKeyDown(KeyCode.Joystick2Button5))
            UpDownKey(0);
        else if (this.name == "Player2" && Input.GetKeyUp(KeyCode.Joystick2Button5))
            UpDownKey(1);
        if(Input.GetKeyDown(KeyCode.Joystick2Button10))
        {
            UpDownKey(2);
        }
        else if(Input.GetKeyUp(KeyCode.Joystick2Button10))
        {
            UpDownKey(1);
        }

        this.step_timer += Time.deltaTime;//경과 시간 진행

        if (this.is_touch && !this.gameControl.is_collider)// 마우스 버튼을 눌렀으면
        {
            
            if (this.is_landed)
            {

                Perfect_Jump();
                switch (this.name)
                {
                    case "Player1":
                        if (is_perfect_jump_Up)
                        {
                            this.GetComponent<AudioSource>().clip = Perfect_Sound[Random.Range(0, 2)];
                            this.PerfectJump_Effect();
                        }
                        else
                            this.GetComponent<AudioSource>().clip = Nomarl_Sound[Random.Range(0, 2)];
                        break;
                    case "Player2":
                        if (is_perfect_jump_Down)
                        {
                            this.GetComponent<AudioSource>().clip = Perfect_Sound[Random.Range(0, 2)];
                            this.PerfectJump_Effect();
   
                          
                        }
                        else
                            this.GetComponent<AudioSource>().clip = Nomarl_Sound[Random.Range(0, 2)];
                        break;
                }

                this.GetComponent<AudioSource>().Play();
                

                this.jump = true;
                this.click_timer = 0.0f;// 타이머를 리셋
                this.anim.SetFloat("Speed", Random.Range(0.1f, 0.3f));
            }
        }

    }

    void FixedUpdate()
    {
        Vector2 velocity = this.GetComponent<Rigidbody2D>().velocity;
        if (jump && !this.gameControl.is_collider)
        {
            //this.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 100, ForceMode2D.Force);

            velocity.y = Mathf.Lerp(this.transform.localPosition.y, this.JUMP_KEY_RELEASE_REDUCE + this.JUMP_HEIGHT, 1.0f);
        //    this.step = STEP.JUMP;
        }
        else
        {
            if (this.click_timer >= 0.0f)
            {
                this.click_timer += Time.deltaTime;
            }
        }

        if (this.next_step == STEP.NONE)
        {
            switch (this.step)
            {
                case STEP.RUN:

                    if (0.0f <= this.click_timer && this.click_timer <= CLICK_GRACE_TIME)
                    {
                        if (this.is_landed)
                        {
                            this.click_timer = -1.0f;
                            this.next_step = STEP.JUMP;
                        }
                    }
                    break;
                case STEP.JUMP:
                    if (this.is_landed)
                    {
                        this.next_step = STEP.RUN;
                    }
                    break;
            }
        }
        while (this.next_step != STEP.NONE)
        {
           
            this.step = this.next_step;
            this.next_step = STEP.NONE;

           
            switch (this.step)
            {
                case STEP.JUMP:

                    this.jump = true;
                    //this.GetComponent<Rigidbody2D>().gravityScale = 2.0f;
                    break;
            }
            this.step_timer = 0.0f;//상태가 변경했으니 경과 시간을 리셋
        }


        switch (this.step)
        {
            case STEP.RUN:
                Time.timeScale = 1;
                this.anim.SetFloat("Speed", 0.0f);
                this.is_double_jmp = false;
                
                //Debug.Log(this.name + "Run");
                break;
            case STEP.JUMP:
                do
                {
                    if (this.transform.localPosition.y > this.origin_position.y + 1f)
                    {
                        this.GetComponent<Rigidbody2D>().gravityScale = JUMP_KEY_RELEASE_REDUCE;
                    }
                    if (this.is_touch)
                    // 마우스 버튼이 떨어진 순간이 아니면 = 버튼을 누르고 있다면
                    {
                        /*if( on_second_jump)
                        {
                            this.GetComponent<Rigidbody2D>().gravityScale = 1;
                            break;
                        }*/
                        
                        if (this.transform.localPosition.y > this.origin_position.y + 1.0f)
                        {
                            this.GetComponent<Rigidbody2D>().gravityScale = JUMP_KEY_RELEASE_REDUCE;
                        }
                        else
                            break;//아무것도 하지않고 루프 탈출

                    }

                    //상하방향 속도가 0이하면(하강 중이라면)
                    if (velocity.y < 0.0f)
                    {
                        this.is_touch = false;
                        this.GetComponent<Rigidbody2D>().gravityScale = JUMP_KEY_RELEASE_REDUCE;

                        break;
                    }
                    
                } while (false);

                break;

         
        }
        this.GetComponent<Rigidbody2D>().velocity = velocity;
        this.jump = false;
    }

    /*
    // Description : UI의 점프 버튼을 터치했을 때와 땟을 때
    // Parameter : int nKey
    // return : void
    */
    public void UpDownKey(int button)
    {

        if (ButtonControl.gameState == Game_state.GAME && !this.gameControl.is_collider)
        {
            if (UI_Button.DOWN == (UI_Button)button)
            {
                //this.GetComponent<AudioSource>().Play();
                this.is_touch = true;
            }
            else if (UI_Button.UP == (UI_Button)button)
            {
                this.is_touch = false;
            }
            else if (UI_Button.CENTER_DOWN == (UI_Button)button)
            {
                if (!this.gameControl.is_collider)
                {
                    //this.GetComponent<AudioSource>().Play();

                    this.DoubleJmp();
                    this.is_touch = true;
                  
                }
            }
        }
    }

    public void Perfect_Jump()
    {
        Vector2 player_position = this.transform.position;
        Vector2 perfect_jump = player_position + new Vector2(1.2f, 0.0f);

        Debug.DrawLine(player_position, perfect_jump);
            if (this.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (Physics2D.Linecast(player_position, perfect_jump,
                1 << LayerMask.NameToLayer("Enemy")))
                {
                    //  OnExplode();

                    is_perfect_jump_Up = true;
                }

            }
            if (this.gameObject.layer == LayerMask.NameToLayer("Player_2"))
            {
                if (Physics2D.Linecast(player_position, perfect_jump,
                1 << LayerMask.NameToLayer("Enemy_2")))
                {
                    // OnExplode();

                    is_perfect_jump_Down = true;
                }
            }
    }


    /// <summary>
    /// 점수 카운팅 효과 코루틴
    /// </summary>
    /// <returns> null => 1 프레임 대기 </returns>
    /*public IEnumerator Score_Counting()
    {
            cmp_score = 0;
   
            while (cmp_score < this.gameControl.nPer_score)
            {
                this.gameControl.score_result += this.score;

                this.cmp_score += this.score;

                this.gameControl.Score_txt.GetComponent<Text>().text =
                this.gameControl.score_result.ToString();

                yield return null;
            }    
    }*/
}
