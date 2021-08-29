using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour {
    
    [HideInInspector]
    public string Version = "1.0";

    [HideInInspector]
    public static Game_Manager Instance;

    [HideInInspector]
    public Network_Manager Network_M; //포톤 네트워크관련 처리
    [HideInInspector]
    public Sound_Manager Sound_M; //사운드 관련처리    
    [HideInInspector]
    public UI_Manager UI_M; //UI 관련처리    
    [HideInInspector]
    public DB_Manager DB_M; //DB 관련 처리
    [HideInInspector]
    public Stage_Manager Stage_M; //스테이지 관련 처리
    [HideInInspector]
    public Input_Manager Input_M; // 키입력에 대한 처리
    [HideInInspector]
    public Timer_Manager Timer_M; // 타이머관련 처리
    [HideInInspector]
    public Scence_Manager Scence_M; // 씬 관련 처리
    [HideInInspector]
    public InGame_UI InGame_M; // 씬 관련 처리

    [HideInInspector]
    public GameObject My_Unit;
    [HideInInspector]
    public Character My_Character;

    public delegate void Default_Callback();

    float Volum = 0.5f;
    public float BGM_Volum
    {
        get
        {
            return Volum;
        }
        set
        {
            Volum = value;
            Sound_M.Set_Volum(Volum);
        }
    }

    public enum Game_Mode
    {
        ACADE,  // Connected State
        PVP // Not Connected State
    }
    Game_Mode Present_Game_Mode = Game_Mode.ACADE;
    public Game_Mode GameMode
    {
        get
        {
            return Present_Game_Mode;
        }
        set
        {
            Present_Game_Mode = value;
        }
    }

    public enum Game_State
    {
        Login,
        Lobby,
        Room,
        InGame
    }
    Game_State Present_Game_State = Game_State.Login;
    public Game_State GameState
    {
        get
        {
            return Present_Game_State;
        }
        set
        {
            Present_Game_State = value;
            Sound_M.BGM_Change(Present_Game_State);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (Network_M == null)
            Network_M = this.gameObject.AddComponent<Network_Manager>();

        if (Sound_M == null)
            Sound_M = this.gameObject.AddComponent<Sound_Manager>();
    
        if (Stage_M == null)
            Stage_M = this.gameObject.AddComponent<Stage_Manager>();

        if (DB_M == null)
            DB_M = this.gameObject.AddComponent<DB_Manager>();
        
        if (Input_M == null)
            Input_M = this.gameObject.AddComponent<Input_Manager>();
            
        if (Timer_M == null)
            Timer_M = this.gameObject.AddComponent<Timer_Manager>();

        if (Scence_M == null)
            Scence_M = this.gameObject.AddComponent<Scence_Manager>();

        Screen.SetResolution(1920, 1080, true);
    }

 
}
