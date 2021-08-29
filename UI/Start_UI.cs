using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_UI : UI_Parent
{
    public new void Open()
    {
        this.gameObject.SetActive(true);
        Game_Manager.Instance.GameState = Game_Manager.Game_State.Login;
    }

    public void Acade_Game_Btn()
    {
        UI_Manager.Instance.Open_ACADE_Call();
    }

    public void Pvp_Game_Btn()
    {
        Network_Manager.Instance.PVP_Connected_Try();
    }

    public void Option_Btn()
    {

        UI_Manager.Instance.Open_Setting_Call();
    }

    public void Credit_Btn()
    {
        UI_Manager.Instance.Open_Creadit_Call();
    }

    public void Exit_Btn()
    {
        Application.Quit();
    }
}
