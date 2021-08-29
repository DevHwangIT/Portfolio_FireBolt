using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance;

    [SerializeField] private Start_UI Start_UI_Script;
    [SerializeField] private Top_UI Top_UI_Script;
    [SerializeField] private MyInfo_UI MyInfo_UI_Script;
    [SerializeField] private ACADE_UI ACADE_UI_Script;
    [SerializeField] private PVP_UI PVP_UI_Script;
    [SerializeField] private Room_UI Room_UI_Script;
    [SerializeField] private Mail_UI Mail_UI_Script;
    [SerializeField] private Setting_Set_UI Setting_UI_Script;
    [SerializeField] private Creadit_UI Creadit_UI_Script;

    public delegate void UI_Delegate();

    UI_Delegate All_Close_UI;

    // Start Open Close
    UI_Delegate Open_Start_UI;
    UI_Delegate Close_Start_UI;

    // Join ACADE
    UI_Delegate Open_ACADE_UI;
    UI_Delegate Close_ACDE_UI;

    // Join PVP Set
    UI_Delegate Open_PVP_UI;
    UI_Delegate Close_PVP_UI;

    // Join Room Set
    UI_Delegate Open_Room_UI;
    UI_Delegate Close_Room_UI;

    // MyInfo Open,Close
    UI_Delegate Open_MyInfo_UI;
    UI_Delegate Close_MyInfo_UI;

    // Mail Open,Close
    UI_Delegate Open_Mail_UI;
    UI_Delegate Close_Mail_UI;
    
    // Setting Open, Close
    UI_Delegate Open_Setting_UI;
    UI_Delegate Close_Setting_UI;

    // Top_UI Open, Close
    UI_Delegate Open_Top_UI;
    UI_Delegate Close_Top_UI;

    // Creadit Open, Close
    UI_Delegate Open_Creadit_UI;
    UI_Delegate Close_Creadit_UI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        All_Close_UI += Start_UI_Script.Close;
        All_Close_UI += Top_UI_Script.Close;
        All_Close_UI += MyInfo_UI_Script.Close;
        All_Close_UI += ACADE_UI_Script.Close;
        All_Close_UI += PVP_UI_Script.Close;
        All_Close_UI += Room_UI_Script.Close;
        All_Close_UI += Mail_UI_Script.Close;
        All_Close_UI += Setting_UI_Script.Close;
        All_Close_UI += Creadit_UI_Script.Close;

        Open_Start_UI += Start_UI_Script.Open;
        Close_Start_UI += Start_UI_Script.Close;

        //호출되면 DB불러오기
        Open_ACADE_UI += ACADE_UI_Script.Open;
        Open_ACADE_UI += Top_UI_Script.Open;
        Open_ACADE_UI += MyInfo_UI_Script.Open;
        Close_ACDE_UI += ACADE_UI_Script.Close;
        Close_ACDE_UI += Top_UI_Script.Close;
        Close_ACDE_UI += MyInfo_UI_Script.Close;

        Open_PVP_UI += PVP_UI_Script.Open;
        Open_PVP_UI += Top_UI_Script.Open;
        Open_PVP_UI += MyInfo_UI_Script.Open;
        Close_PVP_UI += PVP_UI_Script.Close;
        Close_PVP_UI += Top_UI_Script.Close;
        Close_PVP_UI += MyInfo_UI_Script.Close;

        Open_Top_UI += Top_UI_Script.Open;
        Close_Top_UI += Top_UI_Script.Close;

        Open_MyInfo_UI += MyInfo_UI_Script.Open;
        Close_MyInfo_UI += MyInfo_UI_Script.Close;

        Open_Room_UI += Room_UI_Script.Open;
        Open_Room_UI += Top_UI_Script.Open;
        Open_Room_UI += MyInfo_UI_Script.Open;
        Close_Room_UI += Room_UI_Script.Close;
        Close_Room_UI += Top_UI_Script.Close;
        Close_Room_UI += MyInfo_UI_Script.Close;

        Open_Mail_UI += Mail_UI_Script.Open;
        Close_Mail_UI += Mail_UI_Script.Close;

        Open_Setting_UI += Setting_UI_Script.Open;
        Close_Setting_UI += Setting_UI_Script.Close;

        Open_Creadit_UI += Creadit_UI_Script.Open;
        Close_Creadit_UI += Creadit_UI_Script.Close;

        All_Close_UI();
        Open_Start_UI();
    }

    void Start()
    {
        Game_Manager.Instance.UI_M = this;
    }

    //로그인 화면 호출하기.
    public void Open_Login_Call()
    {
        All_Close_UI();
        Open_Start_UI();
    }

    //호출되면 DB불러오기 
    public void Open_ACADE_Call()
    {
        Close_Start_UI();
        Open_ACADE_UI();
    }

    //호출되면 DB불러오기 및 포톤연결 추가하기 전까지 구동안됨.
    public void Open_PVP_Call()
    {
        Close_Start_UI();
        Close_Room_UI();
        Open_PVP_UI();
    }

    //방에 접속시 처리해야 할 항목 일괄 처리
    public void Open_Room_Call(string Name,string Pw,int MaxNumber,int Mode)
    {
        Close_PVP_UI();
        Open_Room_UI();

        Room_UI_Script.Init_Room_Info(Name, Pw, MaxNumber, Mode);
    }

    //방에 접속시 처리해야 할 항목 일괄 처리
    public void Open_InGame_Call(int Stage,int Sub_Stage)
    {
        Scence_Manager.Instance.Call_LoadStage(Stage, Sub_Stage);
    }

    //메일 정보 가져오기등 일괄처리
    public void Open_MailBox_Call()
    {
        Open_Mail_UI();
    }

    //셋팅 창 열기
    public void Open_Setting_Call()
    {
        Open_Setting_UI();
    }

    //Creadit창 열기
    public void Open_Creadit_Call()
    {
        Open_Creadit_UI();
    }
}
