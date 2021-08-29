using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer_Manager : MonoBehaviour {

    // 시간관련 처리를 담당하는 클래스
    
    public static Timer_Manager Instance;

    public bool Time_Start;
    public int Time_Sec = 0;
    public int Time_MSec = 0;

    public float Timer_Delay = 0.01f; //Next Call Timer Method Delay

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Timer_Run()
    {
        Timer(Game_Manager.Instance.GameMode);
    }

    public void Timer_Set(int Time_M)
    {
        Time_Sec = Time_M;
    }

    public void Timer_Stop()
    {
        switch (Game_Manager.Instance.GameMode)
        {
            case Game_Manager.Game_Mode.ACADE:
                if (IsInvoking("None_Network_Timer"))
                {
                    CancelInvoke("None_Network_Timer");
                }
                break;

            case Game_Manager.Game_Mode.PVP:
                if (IsInvoking("Network_Timer"))
                {
                    CancelInvoke("Network_Timer");
                }
                break;

            default:
                if (IsInvoking("None_Network_Timer"))
                {
                    CancelInvoke("None_Network_Timer");
                }
                break;
        }
    }

    public void Timer_Resume()
    {
        switch (Game_Manager.Instance.GameMode)
        {
            case Game_Manager.Game_Mode.ACADE:
                if (!IsInvoking("None_Network_Timer"))
                {
                    Invoke("None_Network_Timer", Timer_Delay);
                }
                break;

            case Game_Manager.Game_Mode.PVP:
                if (!IsInvoking("Network_Timer"))
                {
                    Invoke("Network_Timer", Timer_Delay);
                }
                break;

            default:
                if (!IsInvoking("None_Network_Timer"))
                {
                    Invoke("None_Network_Timer", Timer_Delay);
                }
                break;
        }
    }

    void Time_Over()
    {
        //Time Over Throw To Stage_Manager
        InGame_UI.Instance.InGame_Result_Scprit.Open();
    }

    void Timer(Game_Manager.Game_Mode Mode)
    {
        switch (Mode)
        {
            case Game_Manager.Game_Mode.ACADE:
                if (!IsInvoking("None_Network_Timer"))
                {
                    Invoke("None_Network_Timer", Timer_Delay);
                }
                break;

            case Game_Manager.Game_Mode.PVP:
                if (!IsInvoking("Network_Timer"))
                {
                    Invoke("Network_Timer", Timer_Delay);
                }
                break;

            default:
                if (!IsInvoking("None_Network_Timer"))
                {
                    Invoke("None_Network_Timer", Timer_Delay);
                }
                break;
        }
    }
   
    void None_Network_Timer()
    {
        if (Time_Sec <= 0) //시간 종료
        {
            if (IsInvoking("None_Network_Timer"))
            {
                CancelInvoke("None_Network_Timer");
                Time_Sec = 0;
                Time_MSec = 0;
                Time_Over();
                return;
            }
        }
        Time_MSec -= 1;

        if (Time_MSec <= 0)
        {
            Time_MSec = 60;
            Time_Sec--;
        }
        if (InGame_UI.Instance != null)
        {
            InGame_UI.Instance.Timer_Label.text = Time_Sec + "." + Time_MSec;
        }

        Invoke("None_Network_Timer", Timer_Delay);
    }

    void Network_Timer()
    {
        if (PhotonNetwork.isMasterClient)
        {
            if (Time_Sec <= 0) //시간 종료
            {
                if (IsInvoking("Network_Timer"))
                {
                    CancelInvoke("Network_Timer");
                    Time_Sec = 0;
                    Time_MSec = 0;
                    Time_Over();
                    return;
                }
            }
            Time_MSec -= 1;

            if (Time_MSec <= 0)
            {
                Time_MSec = 60;
                Time_Sec--;
            }
            Network_Manager.Instance.photonView.RPC("Send_Timer", PhotonTargets.All, Time_Sec + "." + Time_MSec);
        }
        Invoke("Network_Timer", Timer_Delay);
    }
}
