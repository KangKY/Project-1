using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroStart : MonoBehaviour {
    void Start()
    {
        GPGSMng.GetInstance.InitializeGPGS();
    }

    // Update is called once per frame
    void Update () {
        Invoke("LoadHome", 1.5f);
    }

    private void LoadHome()
    {
        if (GPGSMng.GetInstance.bLogin == false)
        {
            GPGSMng.GetInstance.LoginGPGS(); // 로그인
        }
        else
        {
            SceneManager.LoadScene("Home");
        }
        
    }
}
