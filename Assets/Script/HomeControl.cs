using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;
using GamepadInput;
using UnityEngine.EventSystems;
public class HomeControl : MonoBehaviour {

    public static bool home_start = false;
    private ButtonControl Btn_Control;
    public GameObject Pop_Up_UI;
    private bool first_touch;
    public GameObject Button_Play;
    public GameObject Button_Ranking;
    public GameObject Button_Achieve;
    public GameObject Button_Setting;
    GamepadState state;
    AxisEventData baseEvent;
    // 에드몹 광고 인스턴스
    public static BannerView bannerView;
    string textPath = "Assets/Resources/";
    GameObject myEventSystem;
    public AudioClip yap_sound;
    public GameObject cancle;
    // Use this for initialization
    void Start () {
        first_touch = false;
        this.Btn_Control = this.GetComponent<ButtonControl>();
        myEventSystem = GameObject.Find("EventSystem");
        
        ButtonControl.gameState = Game_state.HOME;
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.OSXPlayer)
        {
            if(File.Exists(Application.persistentDataPath + "/set_text.txt"))
                this.Btn_Control.ParseData("sound");
            if (File.Exists(Application.persistentDataPath + "/donation_list.txt"))
                this.Btn_Control.ParseData("donation");
        }
        else
        {
            if (File.Exists(textPath + "set_text.txt"))
                this.Btn_Control.ParseData("sound");
            if (File.Exists(textPath + "donation_list.txt"))
                this.Btn_Control.ParseData("donation");
        }
            

        //--------------------- 광고 ------------------
        if (!ButtonControl.is_donation)
        {
       bannerView =
        new BannerView("ca-app-pub-6990052909825792/3969777261", AdSize.SmartBanner, AdPosition.Bottom);



#if UNITY_ANDROID
        //서버 광고 요청
        //AdRequest.Builder builder = new AdRequest.Builder();
  
            AdRequest request =
                new AdRequest.Builder().Build();
            //AddTestDevice("9088800B314FB5FE").Build();  //9088800B314FB5FE : 디바이스 아이디
            bannerView.LoadAd(request);

            bannerView.Show();  // 배너 광고 Show*/
#endif
#if UNITY_IPHONE

#endif

            //  AdRequest adRequest = new AdRequest.Builder().Build();



        }

        if(!home_start)
        {
            this.GetComponent<AudioSource>().clip = yap_sound;
            this.GetComponent<AudioSource>().volume = ButtonControl.BGM_Sound_Volume;
            this.GetComponent<AudioSource>().Play();
        }
        
        home_start = true;
    }

    // Update is called once per frame
    void Update () {
        state = GamePad.GetState((GamePad.Index)2);
        //Debug.Log(state.Down);
        if (Input.GetKeyUp(KeyCode.Escape) || state.Back)
        {
            Pop_Up_UI.SetActive(true);
            myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(cancle);
            Button_Play.GetComponent<Button>().enabled = false;
            Button_Ranking.GetComponent<Button>().enabled = false;
            Button_Achieve.GetComponent<Button>().enabled = false;
            Button_Setting.GetComponent<Button>().enabled = false;
            
        }

        if (Pop_Up_UI.activeSelf)
        {
            Debug.Log(myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject.name);
            //myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(cancle);
            if (myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject)
                myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject.GetComponent<Button>().Select();
        }

        else
        {
            if (!first_touch)
            {
                if (state.Down | state.Left | state.Right | state.Up)
                {
                    Button_Play.GetComponent<Button>().Select();
                    myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(Button_Play);
                    first_touch = true;
                }

            }
            else
            {
                //Debug.Log(myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject.name);
                if (myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject)
                    myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject.GetComponent<Button>().Select();

            }
        }
        

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
                Button_Play.GetComponent<Button>().enabled = true;
                Button_Ranking.GetComponent<Button>().enabled = true;
                Button_Achieve.GetComponent<Button>().enabled = true;
                Button_Setting.GetComponent<Button>().enabled = true;
                break;
            case 2:
                Application.Quit();
                break;
        }
    }
}
