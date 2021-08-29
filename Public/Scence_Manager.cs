using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scence_Manager : MonoBehaviour 
{
    public static Scence_Manager Instance;

    [HideInInspector]
    public string Main_UI = "Main";
    [HideInInspector]
    public string InGame_UI = "InGame";
    public string Scence;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Call_LoadStage(string Scence)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(Scence);
    }

    public void Call_LoadStage(int Main_Stage,int Sub_Stage)
    {
        switch (Game_Manager.Instance.GameMode)
        {
            case Game_Manager.Game_Mode.ACADE:
                UnityEngine.SceneManagement.SceneManager.LoadScene("Stage" + Main_Stage + "-" + Sub_Stage);
                UnityEngine.SceneManagement.SceneManager.LoadScene(Scence_Manager.Instance.InGame_UI, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                break;

            case Game_Manager.Game_Mode.PVP:
                Network_Manager.Instance.Pv_.RPC("InGame_UI_Scene_Addtive", PhotonTargets.All, Main_Stage, Sub_Stage);
                break;

            default:
                UnityEngine.SceneManagement.SceneManager.LoadScene("Stage" + Main_Stage + "-" + Sub_Stage);
                UnityEngine.SceneManagement.SceneManager.LoadScene(Scence_Manager.Instance.InGame_UI, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                break;
        }
    }
}
