using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;




public static class SetStartClass
{
    public static bool is_first = true;
    
}


[System.Serializable]
public struct StageClass
{
    public GameObject[] Stages;
}

public class GameControl : MonoBehaviour
{
    // 플레이 횟수
    private static int Play_count = 0;
    // 플레이 가능한지?
    public static bool Can_play = true;
    // 게임 바퀴 수( 5스테이지까지 간 횟수 )
    public static int game_turn_count = 0;

    // 도움말 창
    public GameObject helper_popup;
    public Text help_popup_text;
    private const string helper_01 = "If you donate, you can remove the ads.";
    private const string helper_02 = "If you jump near the enemies, you will receive extra points.";
    private const string helper_03 = "Challenge a variety of achievements.";
    private string[] helper_text;

    // Unity Ads valuable
    ShowOptions _ShowOpt = new ShowOptions();
    public bool UnityAds_On = false;
    public static int Ads_Count;

    // UI 변수
    public ButtonControl Btn_Control;
    public Button Btn_Left;
    public Button Btn_Center;
    public Button Btn_Right;

    public Image board;

    // 플레이어
    private PlayerMain player;
    private PlayerMain player2;

    //레벨 디자인 변수
    public LevelControl level_control = null;
    private GameRoot gameRoot = null;
    private CreationInfo enemy;
    public TextAsset level_data_text = null;
    private int cur_level = 0; // 현재 레벨


    // 적 및 장애물 생성
    public MemoryPool[] pool;// 메모리 풀
    private GameObject[] hazard;
    private int rand_up;
    private int rand_down;
    float X_position;
    float spawnTime = 0f;
    private float startWait = 0;
    private float spawnWait_min = 0;
    private float spwanWait_max = 0;
    

    public GameObject[] Obstacle;
    private int nNumber_1;
    private int nNumber_2;
  

    // 배경 스왑 관련 변수
    public GameObject[] bg_Change;// 스테이지별 배경을 담고 있는 게임오브젝트 배열
    public GameObject[] cur_BG;// 현재 보여주고있는 배경( 그 안의 자식들을 담고 있는 배열 )

    public StageClass[] All_Stages;
    private bool is_fade_out;
    private Color bg_color;

    // 랜덤 캐릭터 생성
    public RuntimeAnimatorController[] anim_ctr;

    // 게임 점수를 위한 변수
    public GameObject Score_txt;
    public GameObject Score_Result;
    public static uint score_result;
    public MemoryPool Jump_Effect;


    // 더블 점프 카운트
    private int Double_cnt;
   
    // Back key PopUp UI
    public GameObject Pop_Up_UI;


    // 충돌 했을 경우 UI 생성
    public bool is_collider = false;
    public GameObject Game_Over = null;
    public GameObject Over_BG = null;
   

    // 테스트용 버튼
    public ButtonForTest ButttonForTest = null;
    public Text Text_Collider;
    public bool Collider_OnOff;


    // 사운드
    public AudioClip dead_sound;

    void Awake()
    {
        // 게임 초기화
        GPGSMng.GetInstance.InitializeGPGS();
        
        pool = new MemoryPool[10];
        nNumber_1 = 0;
        nNumber_2 = 2;
        for (int i = 0; i < 10; i++)
        {
            pool[i] = new MemoryPool(); 
            pool[i].Create(Obstacle[i], 8);
        }
        

        score_result = 0;
        helper_text = new string[3];

        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        // Unity Ads
        
#if UNITY_ANDROID
        Advertisement.Initialize("1074804", false);
#endif
#if UNITY_IPHONE
        Advertisement.Initialize("1074805", true);
#endif


    }



    void Start()
    {
        Play_count++;
        ButtonControl.gameState = Game_state.GAME;
        if(HomeControl.home_start)
            GameObject.Find("BGM").GetComponent<BGM>().PlayGameBGM();


        this.Btn_Control = this.gameObject.GetComponent<ButtonControl>();
        
        this.ButttonForTest = this.gameObject.GetComponent<ButtonForTest>();

        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMain>();
        this.player2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerMain>();

        // 저장한 볼륨값 대입
        this.player.GetComponent<AudioSource>().volume = ButtonControl.FX_Sound_Volume;
        this.player2.GetComponent<AudioSource>().volume = ButtonControl.FX_Sound_Volume;

        // 랜덤 캐릭터 생성
        this.Rand_Character();

        // 레벨 디자인 텍스트 로딩
        this.level_control = new LevelControl();
        this.level_control.initialize();
        this.level_control.loadLevelData(level_data_text);

        // 업적 : 게임 처음 시작 업적 언락
        this.UnLockAchievement(0);

        // 업적 : 게임 플레이 횟수에 따른 업적 언락
        this.UnLockAchievement(2);
        this.UnLockAchievement(3);
        this.UnLockAchievement(4);
        
        // 하단 버튼 이미지 동적으로 크기 할당
        this.board.rectTransform.sizeDelta =
            new Vector2(800, this.board.rectTransform.parent.GetComponent<RectTransform>().sizeDelta.y * 0.5f);
    

        this.gameRoot = this.gameObject.GetComponent<GameRoot>();

        bg_color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        this.hazard = new GameObject[2];

        this.cur_BG = All_Stages[0].Stages;
                                                                                                                                                                                                                                   
        for (int i = 0; i < cur_BG.Length; i++)
            this.cur_BG[i].GetComponent<Renderer>().material.SetColor("_TintColor", bg_color);


        this.ButttonForTest = this.gameObject.GetComponent<ButtonForTest>();
        this.ButttonForTest.ParseData();
        this.ButttonForTest.Initialize();
        this.Score_txt.GetComponent<Text>().text = "0";



        startCroutines();

        System.GC.Collect();

       
    }



    public void OnShowUnityAds()
    {
        Advertisement.Show(null, _ShowOpt);
    }
    

    public IEnumerator SpawnWaves_UP()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {

            


            X_position = 13.0f;
            spawnTime = Random.Range(spawnWait_min, spwanWait_max);
            Vector3 spawnPosition = new Vector3(X_position, 4f, -1.0f);

            rand_up = Random.Range(nNumber_1, nNumber_2);
            this.hazard[0] = pool[rand_up].NewItem();

            this.hazard[0].transform.position = spawnPosition;
            this.hazard[0].layer = LayerMask.NameToLayer("Enemy");

            yield return new WaitForSeconds(spawnTime  + Random.Range(-(spawnTime- spawnWait_min), spawnTime - spawnWait_min));
        }
    }

    public IEnumerator SpawnWaves_DOWN()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            X_position = 13.0f;
            Vector3 spawnPosition = new Vector3(X_position, 2.1f, -1.0f);

            
            rand_down = Random.Range(nNumber_1, nNumber_2);
            this.hazard[1] = pool[rand_down].NewItem();


            this.hazard[1].transform.position = spawnPosition;
            this.hazard[1].layer = LayerMask.NameToLayer("Enemy_2");
  

            //yield return new WaitForSeconds(spawnTime + Random.Range(0, spawnWait_min));
            yield return new WaitForSeconds(spawnTime + Random.Range(-(spawnTime- spawnWait_min), spawnTime - spawnWait_min));
        }
    }

    private void startCroutines()
    {
        if(!is_collider)
        {
            StartCoroutine(SpawnWaves_UP());
            StartCoroutine(SpawnWaves_DOWN());
        }
    }

    void Update()
    {
        // 게임의 첫 시작일 경우
        if(SetStartClass.is_first)
        {
            helper_text[0] = helper_01;
            helper_text[1] = helper_02;
            helper_text[2] = helper_03;

            help_popup_text.text = helper_text[Random.Range(0,3)];

            Ads_Count = Random.Range(7, 12);
            helper_popup.SetActive(true);
            Time.timeScale = 0;
            SetStartClass.is_first = false;
        }
           
        if (Play_count == Ads_Count && !ButtonControl.is_donation)
        {
            UnityAds_On = true;
        }
        

        /// 레벨 디자인
 
        this.level_control.update(this.gameRoot.getPlayTime());
        this.enemy = this.level_control.cur_enemy;

        this.spawnWait_min = this.enemy.spawnTime.min;
        this.spwanWait_max = this.enemy.spawnTime.max;

        /// <summary>
        /// 업적 : 게임 점수에 관련한 업적 언락
        /// </summary>
        if (score_result == 100)
            this.UnLockAchievement(10);
        else if (score_result == 300)
            this.UnLockAchievement(11);
        else if (score_result == 500)
            this.UnLockAchievement(12);
        else if (score_result == 800)
            this.UnLockAchievement(13);
        else if (score_result == 900)
            this.UnLockAchievement(14);
        else if (score_result == 1000)
            this.UnLockAchievement(15);
        /*-------------------------------*/

        /// <summary>
        /// 업적 : 동시 점프 횟수에 관련한 업적 언락
        /// </summary>
        if (Double_cnt == 10)
        {
            this.UnLockAchievement(7);
        }
        else if(Double_cnt == 50)
        {
            this.UnLockAchievement(8);
        }
        else if(Double_cnt == 100)
        {
            this.UnLockAchievement(9);
        }

        // 업적 : 동시 점프를 한 번도 누르지 않고 2번째 스테이지 도달
        if (this.level_control.level == 1 && this.Double_cnt == 0)
            this.UnLockAchievement(5);
        // 업적 : 동시 점프를 한 번도 누르지 않고 마지막 스테이지 도달
        else if (this.level_control.level == 4 && this.Double_cnt == 0)
            this.UnLockAchievement(6);
        /*-------------------------------*/


        /// <summary>
        /// 업적 : 스테이지에 관련된 업적
        /// </summary>
        if (this.level_control.level == 1)
        {
            this.UnLockAchievement(16);
        }
        else if(this.level_control.level == 2)
        {
            this.UnLockAchievement(17);
        }
        else if(this.level_control.level == 3)
        {
            this.UnLockAchievement(18);
        }
        else if(this.level_control.level == 4)
        {
            this.UnLockAchievement(19);
        }
      
        
        /// <summary>
        /// 레벨에 따른 배경 페이드인 & 페이드 아웃 적용
        /// </summary>
        if (this.cur_level != this.level_control.level)
        {

            StopAllCoroutines();
            Invoke("startCroutines", 1.5f);
            
            bg_color.a = 0.5f;
            this.bg_Change[this.level_control.level].SetActive(true);
            this.bg_Change[this.level_control.level].transform.position = new Vector3(0, 0, 1);
            this.cur_BG = All_Stages[this.level_control.level].Stages;
            

          
            for (int i = 0; i < cur_BG.Length; i++)
                this.cur_BG[i].GetComponent<Renderer>().material.SetColor("_TintColor", bg_color);


            nNumber_1 += 2;
            nNumber_2 += 2;
            if (nNumber_1 >= 10)
            {
                nNumber_1 = 0;
                nNumber_2 = 2;
            }
 
            this.cur_level = this.level_control.level;
           
            if(this.cur_level != 0)
                this.cur_BG = All_Stages[this.level_control.level-1].Stages;

            else
            { 
                this.cur_BG = All_Stages[4].Stages;
                game_turn_count++;
                if (game_turn_count == 1)
                    this.UnLockAchievement(20);
            }


            is_fade_out = true;
        }

        if(is_fade_out)
            StartCoroutine(BackGroundFade());


        //}

        // 게임 오버 팝업 생성
        if (is_collider)
        {
            Invoke("GameOver_PopUp", 1.5f);
            this.gameRoot.enabled = false;
        }

        

        //Application.platform == RuntimePlatform.Android && 
        // 안드로이드 기기에서 뒤로가기버튼 시
        if (Input.GetKeyUp(KeyCode.Escape) && Time.timeScale == 1)
        {
            Time.timeScale = 0;
            this.Over_BG.SetActive(true);
            Pop_Up_UI.SetActive(true);
        }
        
    }

    /// <summary>
    /// 리더보드에 점수 세팅
    /// </summary>
    /// <param name="score"></param>
    public void SetLeaderBoard(uint score)
    {
        GPGSMng.GetInstance.SetLeaderBoard(score);
    }
    /// <summary>
    /// 업적 언락
    /// 
    /// </summary>
    public void UnLockAchievement(int passNumber)
    {
        GPGSMng.GetInstance.UnLockAchievement(passNumber);    
    }

    /// <summary>
    /// 메모리풀 모든 객체를 쉬게 한다.
    /// </summary>
    public void ClearObstacle()
    {
        for(int i = 0; i < 10; i++)
            this.pool[i].ClearItem();
    }

    /// <summary>
    /// 뒤로 가기를 눌렀을 경우 버튼들에 대한 실행 코드
    /// </summary>
    /// <param name="nKey"></param>
    public void StopUIKey(int nKey)
    {
        switch (nKey)
        {
            case 1:
                Pop_Up_UI.SetActive(false);
                this.Over_BG.SetActive(false);
 
                break;
            case 2:
                GameObject.Find("BGM").GetComponent<BGM>().PlayHomeBGM();
                for (int i = 0; i < 10; i++)
                    this.pool[i].Dispose();
                SceneManager.LoadScene("Home");
               
                break;
            case 3:
                SetStartClass.is_first = false;
                helper_popup.SetActive(false);
                //Time.timeScale = 1;
                break;
        }
        Time.timeScale = 1;
    }

    /// <summary>
    /// 배경화면 페이드인 & 아웃
    /// </summary>
    public IEnumerator BackGroundFade()
    {
        if (!this.is_collider)
        {
            bg_color.a -= Time.deltaTime * 0.5f;

            if (bg_color.a <= 0)
            {
                if (this.cur_level != 0)
                {
                    this.bg_Change[this.level_control.level - 1].SetActive(false);
                    
                }
                else
                {
                    
                    this.bg_Change[4].SetActive(false);
                }
                this.bg_Change[this.level_control.level].transform.position = new Vector3(0, 0, -0.5f);

                is_fade_out = false;

            }
            for(int i = 0; i < cur_BG.Length; i++)
                this.cur_BG[i].GetComponent<Renderer>().material.SetColor("_TintColor", bg_color);
                
        }
        yield return null;
       
    }



    /// <summary>
    /// 두 플레이어가 착지한 상태인지 체크하는 함수.
    /// </summary>
    /// <returns> Boolean 변수 </returns>
    public bool Players_landed()
    {
        if (this.player.is_landed && this.player2.is_landed)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 더블 점프 판단
    /// </summary>
    /// <returns></returns>
    public bool Player_Double_Jmp()
    {
        if (this.player.is_double_jmp && this.player2.is_double_jmp)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 동시 점프 카운트
    /// </summary>
    public void DoubleJmp_cnt()
    {
        if (Player_Double_Jmp() && ButtonControl.gameState == Game_state.GAME)
        {
            Double_cnt++;
        }
    }

    /// <summary>
    /// 랜덤 캐릭터 생성
    /// </summary>
    public void Rand_Character()
    {
        this.player.ran = Random.Range(0, this.anim_ctr.Length);
        do
        {
            this.player2.ran = Random.Range(0, this.anim_ctr.Length);
        } while (this.player.ran == this.player2.ran);

        this.player.anim.runtimeAnimatorController = this.anim_ctr[this.player.ran];
        this.player2.anim.runtimeAnimatorController = this.anim_ctr[this.player2.ran];
    }



    /// <summary>
    /// 게임 오버 시 Invoke함수로 1초 뒤 호출
    /// </summary>
    public void GameOver_PopUp()
    {
        ButtonControl.gameState = Game_state.OVER;
        this.Over_BG.SetActive(true);
        this.Game_Over.SetActive(true);
        //this.is_collider = false;
     
        if (UnityAds_On)
        {
            OnShowUnityAds();
            UnityAds_On = false;
            Ads_Count = Random.Range(6, 11);
            Play_count = 0;
        }

        try
        {
            Resources.UnloadAsset(Btn_Left.image.sprite);

            Btn_Left.image.sprite = Resources.Load<Sprite>("Sprites/06 game over/btn_quit_01_basic");
            Btn_Center.image.sprite = Resources.Load<Sprite>("Sprites/04 setting/btn_home_01_basic");
            Btn_Right.image.sprite = Resources.Load<Sprite>("Sprites/06 game over/btn_restart_01_basic");

            SpriteState st = new SpriteState();
            st.pressedSprite = Resources.Load<Sprite>("Sprites/06 game over/btn_quit_02_pressed");
            Btn_Left.spriteState = st;


            st.pressedSprite = Resources.Load<Sprite>("Sprites/04 setting/btn_home_02_pressed");
            Btn_Center.spriteState = st;

            st.pressedSprite = Resources.Load<Sprite>("Sprites/06 game over/btn_restart_02_pressed");
            Btn_Right.spriteState = st;

        }
        catch
        {
            Debug.LogError("해당 리소스를 찾을 수 없습니다.");
        }
        this.SetLeaderBoard(score_result);
        this.Score_Result.GetComponent<Text>().text = score_result.ToString();
    }
}
