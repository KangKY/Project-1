using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScript : MonoBehaviour {

    public GameObject Score_txt;
    public GameObject User_txt;

    void Start()
    {
       // GPGSMng.GetInstance.InitializeGPGS(); // 초기화
    }

    void Update() {


        if (GPGSMng.GetInstance.bLogin == false)
        {
            Score_txt.GetComponent<Text>().text = "Login";
            
        }
        else
        {
            Score_txt.GetComponent<Text>().text = "Logout";
            SceneManager.LoadScene("Main");
        }
	}

    public void ClickEvent()
    {
        if (GPGSMng.GetInstance.bLogin == false)
        {
           // GPGSMng.GetInstance.LoginGPGS(); // 로그인
        }
        else
        {
           // GPGSMng.GetInstance.LogoutGPGS(); // 로그아웃
        }
       
    }

}
