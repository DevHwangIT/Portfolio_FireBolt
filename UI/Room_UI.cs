using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_UI : UI_Parent
{
    public GameObject Master_Icon;

    [Header("Chat_View")]
    [SerializeField] private GameObject Chat_Instaiate;
    [SerializeField] private Transform Parent_Chat;
    [SerializeField] private UIInput Chat_Field;

    [Header("Room_Info")]
    [SerializeField] private UILabel Room_Name;
    [SerializeField] private UILabel Room_Pw;
    [SerializeField] private UIToggle Number_2;
    [SerializeField] private UIToggle Number_3;
    [SerializeField] private UIToggle Number_4;
    [SerializeField] private UIToggle Team_Mode;
    [SerializeField] private UIToggle Death_Mode;
    [SerializeField] private UIButton Ready_OR_Start_Btn;

    [Header("Player")]
    public GameObject[] Player_Slot=new GameObject[3];

    bool Ready_Check = false;

    public new void Open()
    {
        //Mode Set Join Room
        this.gameObject.SetActive(true);
        Game_Manager.Instance.GameState = Game_Manager.Game_State.Room;
    }

    public void Init_Room_Info(string Name, string Pw, int number, int mode)
    {
        Player_Slot[0].SetActive(false);
        Player_Slot[1].SetActive(false);
        Player_Slot[2].SetActive(false);

        //Delete Chat_Log
        for (int child = 0; child < Parent_Chat.childCount; child++)
        {
            Destroy(Parent_Chat.GetChild(child).gameObject);
        }

        Room_Name.text = Name;
        Room_Pw.text = Pw;

        switch (number)
        {
            case 2:
                Number_2.value = true;
                Number_3.value = false;
                Number_4.value = false;
                Player_Slot[0].transform.parent.gameObject.SetActive(true);
                Player_Slot[1].transform.parent.gameObject.SetActive(false);
                Player_Slot[2].transform.parent.gameObject.SetActive(false);
                break;

            case 3:
                Number_2.value = false;
                Number_3.value = true;
                Number_4.value = false;
                Player_Slot[0].transform.parent.gameObject.SetActive(true);
                Player_Slot[1].transform.parent.gameObject.SetActive(true);
                Player_Slot[2].transform.parent.gameObject.SetActive(false);
                break;

            case 4:
                Number_2.value = false;
                Number_3.value = false;
                Number_4.value = true;
                Player_Slot[0].transform.parent.gameObject.SetActive(true);
                Player_Slot[1].transform.parent.gameObject.SetActive(true);
                Player_Slot[2].transform.parent.gameObject.SetActive(true);
                break;

            default:
                Number_2.value = true;
                Number_3.value = false;
                Number_4.value = false;
                Player_Slot[0].transform.parent.gameObject.SetActive(true);
                Player_Slot[1].transform.parent.gameObject.SetActive(false);
                Player_Slot[2].transform.parent.gameObject.SetActive(false);
                break;
        }
        switch (mode)
        {
            case 0:
                Team_Mode.value = true;
                Death_Mode.value = false;
                break;

            case 1:
                Team_Mode.value = false;
                Death_Mode.value = true;
                break;

            default:
                Team_Mode.value = true;
                Death_Mode.value = false;
                break;
        }
        Set_Btn_Event();
    }

    public void Set_Btn_Event()
    {
        Ready_OR_Start_Btn.onClick.Clear();
        if (PhotonNetwork.isMasterClient == true) // Room_Master
        {
            EventDelegate Start_Event = new EventDelegate(Start_Btn);
            Ready_OR_Start_Btn.onClick.Add(Start_Event);
            Master_Icon.SetActive(true);
            Ready_OR_Start_Btn.normalSprite = "512x128_Button_Start_Normal";
        }
        else // Player
        {
            EventDelegate Ready_Event = new EventDelegate(Ready_Btn);
            Ready_OR_Start_Btn.onClick.Add(Ready_Event);
            Master_Icon.SetActive(false);
            Ready_OR_Start_Btn.normalSprite = "384X128_Button_Ready_L";
        }
    }
    
    public void Refresh_Joined_PlayerInfo()
    {
        for(int index=0; index< Network_Manager.Instance.Joined_Maximum; index++)
        {
            if (index < Network_Manager.Join_Players.Count)
            {
                Player_Slot[index].SetActive(true);

                //Character_Image Set
                switch (Network_Manager.Join_Players[index].Select_Character)
                {
                    case PhotonPlayer_Extension.Character_Type.Hoon:
                        Player_Slot[index].transform.GetChild(0).GetComponent<UISprite>().spriteName = "768x768_ICON_Cha0";
                        break;

                    case PhotonPlayer_Extension.Character_Type.Pie:
                        Player_Slot[index].transform.GetChild(0).GetComponent<UISprite>().spriteName = "768x768_ICON_Cha1";
                        break;

                    case PhotonPlayer_Extension.Character_Type.SIM_0:
                        Player_Slot[index].transform.GetChild(0).GetComponent<UISprite>().spriteName = "768x768_ICON_Cha1";
                        break;

                    default:
                        Player_Slot[index].transform.GetChild(0).GetComponent<UISprite>().spriteName = "768x768_ICON_Cha0";
                        break;
                }

                //Lv_Label Set
                Player_Slot[index].transform.GetChild(1).GetComponent<UILabel>().text = Network_Manager.Join_Players[index].Lv.ToString();
                //Nickname_Label Set
                Player_Slot[index].transform.GetChild(2).GetComponent<UILabel>().text = Network_Manager.Join_Players[index].NickName;

                //ReadyState Icon Set
                if (Network_Manager.Join_Players[index].Ready == true)
                {
                    Player_Slot[index].transform.GetChild(3).gameObject.SetActive(true);
                    Ready_Check = true;
                }
                else
                {
                    Player_Slot[index].transform.GetChild(3).gameObject.SetActive(false);
                    Ready_Check = false;
                }

                //Room Master Icon Set
                if (Network_Manager.Join_Players[index].Room_Master == true)
                    Player_Slot[index].transform.GetChild(4).gameObject.SetActive(true);
                else
                    Player_Slot[index].transform.GetChild(4).gameObject.SetActive(false);
            }
            else
            {
                Player_Slot[index].SetActive(false);
            }
        }
    }

    public void Chat_Btn()
    {
        Network_Manager.Instance.photonView.RPC("Chat_Send_Call", PhotonTargets.All, Chat_Field.value, PhotonPlayer_Extension.Player.NickName);
    }

    public void Back_Lobby_Btn()
    {
        if (PhotonNetwork.inRoom == true)
        {
            if (PhotonNetwork.isMasterClient)
            {
                if (PhotonNetwork.playerList.Length > 1) // 다른 플레이어가 존재할 경우 방장을 넘김.
                {
                    PhotonNetwork.SetMasterClient(PhotonNetwork.playerList[1]); //First is Mine
                }
            }
            PhotonNetwork.LeaveRoom();
        }
    }

    public void Ready_Btn()
    {
        //마스터 클라이언트에게 Ready 상태 알림.
        Ready_Check = !Ready_Check;
        Network_Manager.Instance.photonView.RPC("Client_Ready_State", PhotonTargets.All, PhotonPlayer_Extension.Player.Key, Ready_Check);
    }

    public void Start_Btn()
    {
        //Throw Stage Level
        UI_Manager.Instance.Open_InGame_Call(1,1);
    }
}
