using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVP_UI : UI_Parent
{
    class Room_Slot
    {
        string R_Name;
        string R_Pass;
        bool Lock;
        int R_Maximum;
    }

    [Header("PVP_Mode_Pannel")]
    Room_Slot[] Room_Slot_List = new Room_Slot[4];
    public GameObject[] Room_Slot_Object = new GameObject[4];

    [SerializeField] private Transform Index_Button_Parent;
    UIButton[] Index_Button_List = new UIButton[10];
    int Present_Page_Index = 1; //현재 활성화된 페이지 값
    int Page_Count = 1; //페이지 총 개수
    int Present_Page_Session = 0; // 현재 선택된 페이지 단락.

    [Header("Game_Create_Pannel")]
    [SerializeField] private GameObject Room_Create_Pannel;
    [SerializeField] private UIInput R_Name_InputField;
    [SerializeField] private UIInput R_Pw_InputField;
    [SerializeField] private UIToggle Play_Number_2;
    [SerializeField] private UIToggle Play_Number_3;
    [SerializeField] private UIToggle Play_Number_4;
    [SerializeField] private UIToggle Team_Mode;
    [SerializeField] private UIToggle Death_Mode;

    private void Awake()
    {
        for(int i=0; i<10; i++)
        {
            Index_Button_List[i] = Index_Button_Parent.GetChild(i).GetComponent<UIButton>();
        }
    }

    public new void Open()
    {
        this.gameObject.SetActive(true);

        //Create_Pannel
        Room_Create_Pannel.SetActive(false);
        Number2_Btn();
        Team_Mode_Btn();

        Refresh_Info();
        Game_Manager.Instance.GameState = Game_Manager.Game_State.Lobby;
        Game_Manager.Instance.GameMode = Game_Manager.Game_Mode.PVP;
    }

    public new void Close()
    {
        this.gameObject.SetActive(false);
        Room_Create_Pannel.SetActive(false);
        if (IsInvoking("Refresh_Info"))
            CancelInvoke("Refresh_Info");
    }

    void Refresh_Info()
    {
        Refresh_RoomList();
        Refresh_RankingList();
        Invoke("Refresh_Info", 1.0f);
    }

    public void Refresh_RoomList()
    {
        if (Network_Manager.Instance.Room_List == null)
        {
            return;
        }

        if (Network_Manager.Instance.Room_List.Length >= 1) //방이 한개라도 존재할 경우
        {
            //방 Setting
            for (int i = 0; i < Room_Slot_Object.Length; i++)
            {
                if (Network_Manager.Instance.Room_List.Length > ((Present_Page_Index - 1) * Room_Slot_Object.Length) + i)
                {
                    string Full_Name = Network_Manager.Instance.Room_List[((Present_Page_Index - 1) * Room_Slot_Object.Length) + i].Name;

                    string[] Name_And_Pw = Full_Name.Split('\\');

                    int Now_Count = Network_Manager.Instance.Room_List[((Present_Page_Index - 1) * Room_Slot_Object.Length) + i].PlayerCount;
                    int Max_Count = (int)Network_Manager.Instance.Room_List[((Present_Page_Index - 1) * Room_Slot_Object.Length) + i].MaxPlayers;

                    //Room Name
                    Room_Slot_Object[i].transform.GetChild(0).GetComponent<UILabel>().text = Name_And_Pw[0];
                    //Roome Joined_Count
                    Room_Slot_Object[i].transform.GetChild(1).GetComponent<UILabel>().text = Now_Count + " / " + Max_Count;
                    //Lcok_Icon
                    if (Name_And_Pw[1] == "") //비밀번호가 존재하지 않음.
                    {
                        Room_Slot_Object[i].transform.GetChild(2).gameObject.SetActive(false);
                    }
                    else
                    {
                        Room_Slot_Object[i].transform.GetChild(2).gameObject.SetActive(true);
                    }
                    Room_Slot_Object[i].SetActive(true);
                }
                else
                {
                    Room_Slot_Object[i].SetActive(false);
                }
            }

            //하단 페이지 버튼 Setting
            Page_Count = Network_Manager.Instance.Room_List.Length / Room_Slot_Object.Length; // 총 페이지 개수 계산

            if (Page_Count < (Present_Page_Session + 1) * 10)  // 페이지 개수가 세션 단위보다 작을 경우
            {
                for (int index = 0; index < 10; index++)
                {
                    if (index < Page_Count)
                    {
                        Index_Button_List[index].gameObject.SetActive(true);
                        Index_Button_List[index].transform.parent = Index_Button_Parent;
                    }
                    else
                    {
                        Index_Button_List[index].gameObject.SetActive(false);
                        Index_Button_List[index].transform.parent = null;
                    }
                    Index_Button_List[index].GetComponent<UILabel>().text = ((index + 1) * (Present_Page_Session * 10)).ToString();
                }
            }
            else
            {
                for (int index = 0; index < Index_Button_List.Length; index++)
                {
                    Index_Button_List[index].gameObject.SetActive(true);
                    Index_Button_List[index].GetComponent<UILabel>().text = ((index + 1) * (Present_Page_Session * 10)).ToString();
                }
            }
            Index_Button_Parent.GetComponent<UIGrid>().Reposition();
        }
        else // 생성된 방이 아예 존재 하지않을 경우
        {
            Page_Count = 1;
            Present_Page_Index = 1;

            foreach (UIButton Index in Index_Button_List)
            {
                Index.gameObject.SetActive(false);
                Index.transform.parent = null;
            }
            Index_Button_List[0].gameObject.SetActive(true);
            Index_Button_List[0].transform.parent = Index_Button_Parent;


            foreach (GameObject Index in Room_Slot_Object)
            {
                Index.SetActive(false);
            }

            Index_Button_Parent.GetComponent<UIGrid>().Reposition();
        }
    }

    public void Page_Left_Arrow_Btn()
    {
        if (Present_Page_Session > 1)
        {
            if ((Page_Count / 10) < Present_Page_Session)
            {
                --Present_Page_Session;
                Present_Page_Index = Present_Page_Session * 10 + 1;
                Refresh_RoomList();
            }
        }
    }

    public void Page_Right_Arrow_Btn()
    {
        if (Present_Page_Session < (Page_Count / 10))
        {
            if ((Page_Count / 10) > Present_Page_Session)
            {
                ++Present_Page_Session;
                Present_Page_Index = Present_Page_Session * 10 + 1;
                Refresh_RoomList();
            }
        }
    }

    //해당 구현은 기능 구현안함. 추후구현
    public void Refresh_RankingList()
    {

    }

    public void Join_Room_Btn(UILabel Room_Name)
    {
        PhotonNetwork.JoinRoom(Room_Name.text + "\\");
        Debug.Log("비밀번호 방에 대한 기능도 추가해주세요.");
    }

    public void Index_Btn(int Page_Index)
    {

    }

    public void Return_Start_Btn()
    {
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.Disconnect();
        }
    }

    public void Show_Create_Room_Pannel_Btn()
    {
        Room_Create_Pannel.SetActive(true);
    }

    //---------------Create Pannel-------------------------------------------------
    
    public void Return_Pannel_Btn()
    {
        Room_Create_Pannel.SetActive(false);
    }

    public void Create_Room_Btn()
    {
        if (PhotonNetwork.inRoom == false)
        {
            Network_Manager.Instance.Present_Name = R_Name_InputField.value;
            Network_Manager.Instance.Present_Pass = R_Pw_InputField.value;

            RoomOptions Option = new RoomOptions();
            TypedLobby Type_Lobby = new TypedLobby();
            Option.MaxPlayers = (byte)Network_Manager.Instance.Joined_Maximum;
            Option.IsOpen = true;

            PhotonNetwork.CreateRoom(Network_Manager.Instance.Present_Name + "\\" + Network_Manager.Instance.Present_Pass, Option, Type_Lobby);
        }
    }

    public void Pw_Toggle()
    {
        if (R_Pw_InputField.gameObject.activeInHierarchy == true)
        {
            R_Pw_InputField.gameObject.SetActive(false);
            R_Pw_InputField.value = "";
        }
        else
        {
            R_Pw_InputField.gameObject.SetActive(true);
        }
    }

    public void Number2_Btn()
    {
        Play_Number_2.value = true;
        Play_Number_3.value = false;
        Play_Number_4.value = false;
        Network_Manager.Instance.Joined_Maximum = 2;
    }

    public void Number3_Btn()
    {
        Play_Number_2.value = false;
        Play_Number_3.value = true;
        Play_Number_4.value = false;
        Network_Manager.Instance.Joined_Maximum = 3;
    }

    public void Number4_Btn()
    {
        Play_Number_2.value = false;
        Play_Number_3.value = false;
        Play_Number_4.value = true;
        Network_Manager.Instance.Joined_Maximum = 4;
    }

    public void Team_Mode_Btn()
    {
        Team_Mode.value = true;
        Death_Mode.value = false;
        Network_Manager.Instance.Room_Mode = 0;
    }

    public void Death_Mode_Btn()
    {
        Team_Mode.value = false;
        Death_Mode.value = true;
        Network_Manager.Instance.Room_Mode = 1;
    }
}
