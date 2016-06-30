using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


public class ButtonForTest : MonoBehaviour {
    public GameControl gameControl = null;
    //private bool Stop_CreateEnemy = false;

    private GameObject[] cur_BG;
    private MapCreator mapCreator;

    public GameObject back;
    public GameObject track;
    public GameObject near;
    public GameObject velo;
    public GameObject height;

    public static int track_velocity;
    public static int near_velocity;
    public int back_velocity;
    public static int velocity;
    public static int jumpHeight;
    public bool on_off = true;
    

    string textPath = "Assets/Resources/";
    
    void Start()
    {

        this.gameControl =
          GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();

    }


    public void WriteData(string strData)
    {// \t 탭으로 구분

        StreamWriter sw = new StreamWriter(File.Create(textPath + "test_text.txt"));
        
        //sw.Dispose();
        sw.WriteLine(strData);
        sw.Flush();
        sw.Close();
       
    }

    public void Initialize()
    {
        this.gameControl =
         GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();

        if (this.on_off)
        {
            this.gameControl.Collider_OnOff = true;
            this.gameControl.Text_Collider.text = "적 충돌 ON";
        }
        else
        {
            this.gameControl.Collider_OnOff = false;

            this.gameControl.Text_Collider.text = "적 충돌 OFF";
        }
    }

    public void ParseData()
    {
        try
        {
            StreamReader sr = System.IO.File.OpenText(textPath + "test_text.txt");

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
                                this.on_off = bool.Parse(word);

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

        }
         
        
    }


    public void OnClick(int nKey)
    {
        

        switch(nKey)
        {

            case 0:
                if(this.gameControl.Collider_OnOff)
                {
                    this.gameControl.Collider_OnOff = false;
                    this.gameControl.Text_Collider.text = "적 충돌 OFF";
                }
                else
                {
                    this.gameControl.Collider_OnOff = true;
                    
                    this.gameControl.Text_Collider.text = "적 충돌 ON";
                }
                
                break;
            /*case 1:
                
                this.cur_BG = GameObject.FindGameObjectsWithTag(this.gameControl.bg_Change[this.gameControl.level_control.level].name);

                foreach (GameObject bg_factor in cur_BG)
                {
                    this.mapCreator = bg_factor.GetComponent<MapCreator>();
                    this.mapCreator.moveSpeed += 0.1f;
                }
                back_velocity++;
                back.GetComponent<Text>().text = back_velocity.ToString();
                    break;
            case 2:

                //GameObject.Find("Background2").GetComponent<MapCreator>().moveSpeed += 0.1f;

                 this.cur_BG = GameObject.FindGameObjectsWithTag(this.gameControl.bg_Change[this.gameControl.level_control.level].name);
                
                foreach (GameObject bg_factor in cur_BG)
                {
                    if(bg_factor.name == "Background2")
                    {
                        this.mapCreator = bg_factor.GetComponent<MapCreator>();
                        this.mapCreator.moveSpeed += 0.1f;
                    }     
                }
                track_velocity++;
                track.GetComponent<Text>().text = track_velocity.ToString();
                break;
            case 3:
                this.cur_BG = GameObject.FindGameObjectsWithTag(this.gameControl.bg_Change[this.gameControl.level_control.level].name);
                foreach (GameObject bg_factor in cur_BG)
                {
                    if (bg_factor.name == "Background2")
                    {
                        this.mapCreator = bg_factor.GetComponent<MapCreator>();
                        if (this.mapCreator.moveSpeed > 0)
                            this.mapCreator.moveSpeed -= 0.1f;
                    } 
                }
                track_velocity--;
                track.GetComponent<Text>().text = track_velocity.ToString();
                break;
            case 4:
                this.player.JUMP_KEY_RELEASE_REDUCE++;
                this.player2.JUMP_KEY_RELEASE_REDUCE++;
                velocity++;
                this.velo.GetComponent<Text>().text = velocity.ToString();
                //velo.GetComponent<Text>().text = velocity.ToString();
                break;
            case 5:
                this.player.JUMP_KEY_RELEASE_REDUCE--;
                this.player2.JUMP_KEY_RELEASE_REDUCE--;
                velocity--;
                this.velo.GetComponent<Text>().text = velocity.ToString();
                //velo.GetComponent<Text>().text = velocity.ToString();
                break;
            case 6:

                this.player.JUMP_HEIGHT++;
                this.player2.JUMP_HEIGHT++;
                jumpHeight++;
                height.GetComponent<Text>().text = jumpHeight.ToString();
                break;
            case 7:
                this.player.JUMP_HEIGHT--;
                this.player2.JUMP_HEIGHT--;
                jumpHeight--;
                height.GetComponent<Text>().text = jumpHeight.ToString();
                break;
            case 8:
               /* this.cur_BG = GameObject.FindGameObjectsWithTag(this.gameControl.bg_Change[this.gameControl.level_control.level].name);

                foreach (GameObject bg_factor in cur_BG)
                {
                    this.mapCreator = bg_factor.GetComponent<MapCreator>();
                    this.mapCreator.moveSpeed -= 0.1f;
                }
                back_velocity--;
                //back.GetComponent<Text>().text = back_velocity.ToString();
                break;
            case 9:
                this.cur_BG = GameObject.FindGameObjectsWithTag(this.gameControl.bg_Change[this.gameControl.level_control.level].name);
                foreach (GameObject bg_factor in cur_BG)
                {
                    if (bg_factor.name == "grass" || bg_factor.name == "skull" || bg_factor.name == "Cloud_01")
                    {
                        this.mapCreator = bg_factor.GetComponent<MapCreator>();
                       
                            this.mapCreator.moveSpeed += 0.1f;
                    }
                }
                near_velocity++;
                near.GetComponent<Text>().text = near_velocity.ToString();
                break;
            case 10:
                this.cur_BG = GameObject.FindGameObjectsWithTag(this.gameControl.bg_Change[this.gameControl.level_control.level].name);
                foreach (GameObject bg_factor in cur_BG)
                {
                    if (bg_factor.name == "grass" || bg_factor.name == "skull" || bg_factor.name == "Cloud_01")
                    {
                        this.mapCreator = bg_factor.GetComponent<MapCreator>();
                        if (this.mapCreator.moveSpeed > 0)
                            this.mapCreator.moveSpeed -= 0.1f;
                    }
                }
                near_velocity--;
                near.GetComponent<Text>().text = near_velocity.ToString();
                break;  */
        }
            string testStr;
            testStr = "" + this.gameControl.Collider_OnOff;


        this.WriteData(testStr);
    }
}
