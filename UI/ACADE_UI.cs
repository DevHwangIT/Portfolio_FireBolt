using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACADE_UI : UI_Parent
{
    [SerializeField] private UIButton Stage_1;
    [SerializeField] private UIButton Stage_2;
    [SerializeField] private UIButton Stage_3;

    int Main_S = 1;
    int Sub_S = 1;

    UIButton Selected_Btn;
    string Swap_Image;

    private void Awake()
    {
        Selected_Btn = Stage_1;
        Stage_One_Btn();
    }

    public new void Open()
    {
        this.gameObject.SetActive(true);
        Game_Manager.Instance.GameState = Game_Manager.Game_State.Lobby;
        Game_Manager.Instance.GameMode = Game_Manager.Game_Mode.ACADE;
    }

    public void Stage_One_Btn()
    {
        Main_S = 1;
        Sub_S = 1;
    }

    public void Stage_Two_Btn()
    {
        Main_S = 2;
        Sub_S = 1;
    }

    public void Stage_Three_Btn()
    {
        Main_S = 3;
        Sub_S = 1;
    }

    public void Stage_Start_Btn()
    {
        Game_Manager.Instance.Scence_M.Call_LoadStage(Main_S, Sub_S);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    public void Return_Start_Btn()
    {
        UI_Manager.Instance.Open_Login_Call();
    }
}
