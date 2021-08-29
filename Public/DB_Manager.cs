using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Manager : MonoBehaviour
{
    public static DB_Manager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        Set_My_Info();
    }

    void Set_My_Info()
    {
        PhotonPlayer_Extension.Player.User_ID = "Unknow";
        PhotonPlayer_Extension.Player.User_Pw = "Unknow";
        PhotonPlayer_Extension.Player.NickName = "Player_" + Random.Range(0, 99999);
        PhotonPlayer_Extension.Player.Lv = Random.Range(1, 50);
    }
}
