using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GamepadInput;
public class SettingControl : MonoBehaviour {

    private ButtonControl Btn_Control;

    public Image board;
    public Image bg;
    public Toggle BGM_Sound;
    public Toggle FX_Sound;
   
    public SpriteRenderer volume_BGM;
    public SpriteRenderer volume_FX;
    GamepadState state;
    GameObject myEventSystem;
    // Use this for initialization
    void Start () {
        this.Btn_Control = this.GetComponent<ButtonControl>();
        myEventSystem = GameObject.Find("EventSystem");
        ButtonControl.gameState = Game_state.OPTION;

        this.board.rectTransform.sizeDelta =
            new Vector2(800,
            this.board.rectTransform.parent.GetComponent<RectTransform>().sizeDelta.y * 0.5f);
        Btn_Control.ChangeSpriteFromVolume();

        this.bg.rectTransform.sizeDelta =new Vector2(800, this.board.rectTransform.parent.GetComponent<RectTransform>().sizeDelta.y * 0.43f);
    }

    void Update()
    {
        state = GamePad.GetState((GamePad.Index)2);
        //Debug.Log(state.Down);

        if (myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject.GetComponent<Toggle>())
            myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject.GetComponent<Toggle>().isOn = true;
        else
        {
            BGM_Sound.isOn = false;
            FX_Sound.isOn = false;
        }
            
    }

}
