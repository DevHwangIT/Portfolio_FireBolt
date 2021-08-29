using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_Result : UI_Parent
{
    [SerializeField] private UIButton QuitTitle;
    [SerializeField] private UIButton Continue;

    [SerializeField] private UISprite Continue_Image;
    [SerializeField] private UILabel Score_Label;    

    public void Open()
    {
        try
        {
            if (PhotonNetwork.connected)
            {
                Continue_Image.gameObject.SetActive(false);
                Score_Label.gameObject.SetActive(true);
                InGame_UI.Instance.Lv_With_Nick_Name.text = PhotonPlayer_Extension.Player.Lv + "        " + PhotonPlayer_Extension.Player.NickName;
                InGame_UI.Instance.Score_Label.text = Game_Manager.Instance.My_Character.Player_.Score.ToString();
            }
            else
            {
                Continue_Image.gameObject.SetActive(true);
                Score_Label.gameObject.SetActive(false);
            }
        }
        catch (System.NullReferenceException e)
        {
            Continue_Image.gameObject.SetActive(true);
            Score_Label.gameObject.SetActive(false);
        }
    }

    public void Quit_Btn()
    {
        Close();
        Game_Manager.Instance.Scence_M.Call_LoadStage("Main");
    }

    public void Continue_Btn()
    {
        //Initialize        
        Game_Manager.Instance.My_Character.Player_.Helth_Value = Game_Manager.Instance.My_Character.Player_.Maximum_Helth_Value;
        Game_Manager.Instance.My_Unit.transform.position = Player_Create_Pos.Respon_Pos[0].position;
        Close();
    }
}
