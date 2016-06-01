using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

public class HomeControl : MonoBehaviour {

    public static bool home_start = false;
    private ButtonControl Btn_Control;
    public GameObject Pop_Up_UI;
    public GameObject Button_Play;
    public GameObject Button_Ranking;
    public GameObject Button_Achieve;
    public GameObject Button_Setting;


    // 에드몹 광고 인스턴스
    public static BannerView bannerView;
    string textPath = "Assets/Resources/";

    public AudioClip yap_sound;

    // Use this for initialization
    void Start () {


        
        this.Btn_Control = this.GetComponent<ButtonControl>();
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
        AdRequest.Builder builder = new AdRequest.Builder();
        // 테스트 디바이스 등록 ( 테스트 디바이스에서는 결제가 안된다 )
        // request 요청 정보를 담는다.
        AdRequest request =
            builder.AddTestDevice(AdRequest.TestDeviceSimulator).
            AddTestDevice("9088800B314FB5FE").Build();  //9088800B314FB5FE : 디바이스 아이디
            bannerView.LoadAd(request);
#endif
#if UNITY_IPHONE

#endif

            //  AdRequest adRequest = new AdRequest.Builder().Build();

            bannerView.Show();  // 배너 광고 Show*/

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
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Pop_Up_UI.SetActive(true);
            Button_Play.GetComponent<Button>().enabled = false;
            Button_Ranking.GetComponent<Button>().enabled = false;
            Button_Achieve.GetComponent<Button>().enabled = false;
            Button_Setting.GetComponent<Button>().enabled = false;
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
