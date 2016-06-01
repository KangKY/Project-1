using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public enum Game_state
{
    NONE = -1,
    HOME,
    GAME,
    OVER,
    OPTION,
    NUM,
};

public enum Button_Pos
{
    NONE = -1,
    LEFT,
    RIGHT,
    CENTER,
    ACHIEVE,
    PAUSE,
    NUM,
};

public class ButtonControl : MonoBehaviour {

    public static Game_state gameState;
    private SettingControl Set_Control;

    private bool is_pause = false;
    public Button Pause_Btn;
    GameObject bgm_object;
    public static float BGM_Sound_Volume = 1.0f;
    public static float FX_Sound_Volume = 1.0f;
    public static bool is_donation = false;

    public GameObject LoadingScene;
    public GameObject LoadingText;
    private float percent;
    GameControl gameControl;
    string textPath = "Assets/Resources/";

    void Start()
    {
        GPGSMng.GetInstance.InitializeGPGS();
        this.Set_Control = this.GetComponent<SettingControl>();
        //if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.OSXPlayer)
        useGUILayout = false;
    }
   
    
    /// <summary>
    /// 게임 상태에 따른 버튼의 동작
    /// </summary>
    /// <param name="button"></param>
    public void UIButtonClick(int button)
    {
#if !UNITY_EDITOR
        if(gameState != Game_state.GAME)
        {
            GameObject.Find("Effect_Sound").GetComponent<Effect_Sound>().Button_Sound();
        }
#endif
        switch (gameState)
        {

          
            case Game_state.HOME:
                if (Button_Pos.LEFT == (Button_Pos)button)
                {
                    GPGSMng.GetInstance.ShowLeaderBoard();
                }

                else if (Button_Pos.RIGHT == (Button_Pos)button)
                {
                    //if(!this.GetComponent<AudioSource>().isPlaying)
                        SceneManager.LoadScene("Setting");
                }
                else if(Button_Pos.ACHIEVE == (Button_Pos)button)
                {
                    GPGSMng.GetInstance.ShowAchievementUI();
                }
                break;
               
            case Game_state.GAME:

                if(Button_Pos.PAUSE == (Button_Pos)button)
                {
                    if(!is_pause)
                    {
                        Time.timeScale = 0;
                        is_pause = true;

                        Pause_Btn.image.sprite = Resources.Load<Sprite>("Sprites/05 game play/btn_play_01_basic");

                        SpriteState st = new SpriteState();
                        st.pressedSprite = Resources.Load<Sprite>("Sprites/05 game play/btn_play_02_pressed");
                        Pause_Btn.spriteState = st;  
                    }
                    else
                    {
                        Time.timeScale = 1;
                        is_pause = false;

                        Pause_Btn.image.sprite = Resources.Load<Sprite>("Sprites/05 game play/btn_pause_01_basic");

                        SpriteState st = new SpriteState();
                        st.pressedSprite = Resources.Load<Sprite>("Sprites/05 game play/btn_pause_02_pressed");
                        Pause_Btn.spriteState = st;
                    }
                }
                break;
            case Game_state.OPTION:
                
                if (Button_Pos.LEFT == (Button_Pos)button)
                {
                    if(this.Set_Control.BGM_Sound.isOn)
                    {
                        bgm_object = GameObject.Find("BGM");
                        if (bgm_object.GetComponent<AudioSource>().volume > 0f)
                        {
                            bgm_object.GetComponent<AudioSource>().volume =
                                   bgm_object.GetComponent<AudioSource>().volume - 0.25f;
                            BGM_Sound_Volume -= 0.25f;
                        }
                    }
                    else if(this.Set_Control.FX_Sound.isOn)
                    {
                        if (FX_Sound_Volume > 0f)
                            FX_Sound_Volume -= 0.25f;
                    }
                    ChangeSpriteFromVolume();
                    string soundData;
                    soundData = BGM_Sound_Volume + "\t" + FX_Sound_Volume;
                    WriteData("sound",soundData);
                }
                else if (Button_Pos.CENTER == (Button_Pos)button)
                {
                    SceneManager.LoadScene("Home");
                }
                else if (Button_Pos.RIGHT == (Button_Pos)button)
                {
                    if (this.Set_Control.BGM_Sound.isOn)
                    {
                        bgm_object = GameObject.Find("BGM");
                        if (bgm_object.GetComponent<AudioSource>().volume < 1.0f)
                        { 
                            bgm_object.GetComponent<AudioSource>().volume
                                = bgm_object.GetComponent<AudioSource>().volume + 0.25f;
                            BGM_Sound_Volume += 0.25f;
                        }
                       
                    }
                    else if (this.Set_Control.FX_Sound.isOn)
                    {
                        if(FX_Sound_Volume < 1.0f)
                            FX_Sound_Volume += 0.25f; 
 
                    }
                    ChangeSpriteFromVolume();
                    string soundData;
                    soundData = BGM_Sound_Volume + "\t" + FX_Sound_Volume;
                    WriteData("sound",soundData);
                }
                break;
            case Game_state.OVER:
                
                this.gameControl =
           GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
                if (Button_Pos.LEFT == (Button_Pos)button)
                {
                    this.gameControl.Pop_Up_UI.SetActive(true);
                    //Application.Quit();
                }
                else if (Button_Pos.CENTER == (Button_Pos)button)
                {
                    GPGSMng.GetInstance.ShowLeaderBoard();
                    /*GameObject.Find("BGM").GetComponent<BGM>().PlayHomeBGM();
                    SceneManager.LoadScene("Home");*/
                }
                else if (Button_Pos.RIGHT == (Button_Pos)button)
                {
                    LoadLevel();
                    //SceneManager.LoadScene("Main");
                }
                break;
        }
    }

    /// <summary>
    /// 값 저장
    /// </summary>
    /// <param name="Data"></param>
    public void WriteData(string choice, string Data)
    {// \t 탭으로 구분
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.OSXPlayer)
        {

            switch(choice)
            {
                case "sound":
                        StreamWriter sw = new StreamWriter(File.Create(Application.persistentDataPath + "/set_text.txt"));
                        //sw.Dispose();
                        sw.WriteLine(Data);
                        sw.Flush();
                        sw.Close();
                  
                    break;

                case "donation":
                    StreamWriter sw2 = new StreamWriter(Application.persistentDataPath + "/donation_list.txt", true);
                    sw2.WriteLine(Data);
                    sw2.Flush();
                    sw2.Close();

                    break;
            }
            
        }
        else
        {
            switch (choice)
            {
                case "sound":
                        StreamWriter sw = new StreamWriter(File.Create(textPath + "set_text.txt"));

                        sw.WriteLine(Data);
                        sw.Flush();
                        sw.Close();
      

                    break;
                case "donation":
                        StreamWriter sw2 = new StreamWriter(textPath+"donation_list.txt", true);
                        sw2.WriteLine(Data);
                        sw2.Flush();
                        sw2.Close();
                    break;
            }
            
        }
            
    }
    
    /// <summary>
    /// 저장해놓은 값 파싱
    /// </summary>
    public void ParseData(string choice)
    {
         
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.OSXPlayer)
        {
            switch (choice)
            {
                case "sound":
                    try
                    {
                        StreamReader sr = System.IO.File.OpenText(Application.persistentDataPath + "/set_text.txt");

                        // 파일을 줄단위로 읽는다.
                        string input = "";

                        while (true)
                        {

                            //sr.Dispose();

                            input = sr.ReadLine();
                            if (input != null)
                            {
                                string[] words = input.Split('\t');

                                int n = 0;
                                foreach (var word in words)
                                {
                                    if (word == "")
                                        continue;

                                    switch (n)
                                    {
                                        case 0:
                                            BGM_Sound_Volume = float.Parse(word);
                                            break;
                                        case 1:
                                            FX_Sound_Volume = float.Parse(word);
                                            break;
                                    }
                                    n++;
                                }

                            }

                            else if (input == null)
                            {
                                break;
                            }

                        }

                        sr.Close(); // 파일 읽기후 반드시 해준다.
                    }
                    catch
                    {
                        Debug.Log("Can't find File.");
                    }


                    break;

                case "donation":
                    try
                    {
                        StreamReader sr2 = new StreamReader(Application.persistentDataPath + "/donation_list.txt");

                        while (sr2.Peek() >= 0)
                        {
                            if (sr2.ReadLine() == GPGSMng.GetInstance.GetIDGPGS())
                            {
                                is_donation = true;
                            }
                        }
                        sr2.Close();

                    }
                    catch
                    {
                        Debug.Log("Can't find File.");
                    }
                    

                    break;
            }
           
        }
        else
        {
            switch(choice)
            {
                case "sound":
                    try
                    {
                        StreamReader sr = System.IO.File.OpenText(textPath + "set_text.txt");

                        // 파일을 줄단위로 읽는다.
                        string input = "";

                        while (true)
                        {

                            //sr.Dispose();
                            input = sr.ReadLine();
                            if (input != null)
                            {
                                string[] words = input.Split('\t');

                                int n = 0;
                                foreach (var word in words)
                                {
                                    if (word == "")
                                        continue;


                                    switch (n)
                                    {
                                        case 0:
                                            BGM_Sound_Volume = float.Parse(word);
                                            break;
                                        case 1:
                                            FX_Sound_Volume = float.Parse(word);
                                            break;
                                    }
                                    n++;
                                }

                            }

                            else if (input == null)
                            {
                                break;
                            }

                        }

                        sr.Close(); // 파일 읽기후 반드시 해준다.
                    }
                    catch
                    {
                        Debug.Log("Can't find File.");
                    }
                    break;
                case "donation":
                    try
                    {
                        StreamReader sr2 = new StreamReader(textPath + "donation_list.txt");

                        while (sr2.Peek() >= 0)
                        {
                            if (sr2.ReadLine() == GPGSMng.GetInstance.GetIDGPGS())
                            {
                                is_donation = true;
                            }
                        }
                        sr2.Close();
                    }
                    catch
                    {
                       
                      Debug.Log("Can't find File.");
                      

                    }
                    break;
            } 
        }       
    }

    /// <summary>
    /// 각각의 Audio 소스의 Volume값을 통한 스프라이트 변경
    /// </summary>
    public void ChangeSpriteFromVolume()
    {
        if (BGM_Sound_Volume == 0f)
        {
            this.Set_Control.volume_BGM.sprite = Resources.Load<Sprite>("Sprites/04 setting/volume_04_inactive");
        }
        else if (BGM_Sound_Volume == 0.25f)
        {
            this.Set_Control.volume_BGM.sprite = Resources.Load<Sprite>("Sprites/04 setting/volume_04_active");
        }
        else if (BGM_Sound_Volume == 0.5f)
        {
            this.Set_Control.volume_BGM.sprite = Resources.Load<Sprite>("Sprites/04 setting/volume_03_active");
        }
        else if (BGM_Sound_Volume == 0.75f)
        {
            this.Set_Control.volume_BGM.sprite = Resources.Load<Sprite>("Sprites/04 setting/volume_02_active");
        }
        else if (BGM_Sound_Volume == 1f)
        {
            this.Set_Control.volume_BGM.sprite = Resources.Load<Sprite>("Sprites/04 setting/volume_01_active");
        }

        if (FX_Sound_Volume == 0f)
        {
            this.Set_Control.volume_FX.sprite = Resources.Load<Sprite>("Sprites/04 setting/volume_04_inactive");
        }
        else if (FX_Sound_Volume == 0.25f)
        {
            this.Set_Control.volume_FX.sprite = Resources.Load<Sprite>("Sprites/04 setting/volume_04_active");
        }
        else if (FX_Sound_Volume == 0.5f)
        {
            this.Set_Control.volume_FX.sprite = Resources.Load<Sprite>("Sprites/04 setting/volume_03_active");
        }
        else if (FX_Sound_Volume == 0.75f)
        {
            this.Set_Control.volume_FX.sprite = Resources.Load<Sprite>("Sprites/04 setting/volume_02_active");
        }
        else if (FX_Sound_Volume == 1f)
        {
            this.Set_Control.volume_FX.sprite = Resources.Load<Sprite>("Sprites/04 setting/volume_01_active");
        }

    }


    public void LoadLevel()
    {
        StartCoroutine(LevelCoroutine());
    }
    public IEnumerator LevelCoroutine()
    {

        AsyncOperation async = SceneManager.LoadSceneAsync(2);

        while (!async.isDone)
        {
            LoadingScene.SetActive(true);
            percent = (async.progress / 0.9f) * 100;

            LoadingText.GetComponent<Text>().text = percent.ToString("N0") + "%";

            yield return null;
        }

    }
}
