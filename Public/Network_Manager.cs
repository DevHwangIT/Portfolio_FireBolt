using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Network_Manager : Photon.PunBehaviour {

    //포톤 네트워크와의 연결을 관리하는 매니저 클래스

    public static Network_Manager Instance;
    public static List<GameObject> Players_Unit = new List<GameObject>();
    public static List<PhotonPlayer_Extension> Join_Players = new List<PhotonPlayer_Extension>();
    
    [HideInInspector]
    public PhotonView Pv_;
    public RoomInfo[] Room_List;

    public string Present_Name = ""; //룸에 접속확인.
    public string Present_Pass = ""; //룸에 접속확인.

    public int Joined_Maximum= 4; //룸에 접속확인.
    public int Room_Mode = 0; //룸에 접속확인.

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance == null)
        {
            Instance = this;
            Pv_ = this.gameObject.AddComponent<PhotonView>();
            Pv_.viewID = 999;
        }
    }

    public void PVP_Connected_Try()
    {
        if (PhotonNetwork.connected == false && PhotonNetwork.connecting == false)
        {
            PhotonNetwork.PhotonServerSettings.JoinLobby = true;
            PhotonNetwork.ConnectUsingSettings("Version_ " + Game_Manager.Instance.Version);
        }
    }

    public void Get_RoomList()
    {
        Room_List = PhotonNetwork.GetRoomList();
        Invoke("Get_RoomList", 2.0f);
    }

    public void Joined_Notice_Player()
    {
        List<string> Key_array = new List<string>();
        List<int> Character_Image = new List<int>();
        List<string> Id_array = new List<string>();
        List<string> name_array = new List<string>();
        List<int> Lv_array = new List<int>();
        List<bool> Ready_array = new List<bool>();
        List<bool> Master_array = new List<bool>();

        //Master_Client Info Insert
        Key_array.Add(PhotonPlayer_Extension.Player.Key);
        Character_Image.Add((int)PhotonPlayer_Extension.Player.Select_Character);
        Id_array.Add(PhotonPlayer_Extension.Player.User_ID);
        name_array.Add(PhotonPlayer_Extension.Player.NickName);
        Lv_array.Add(PhotonPlayer_Extension.Player.Lv);
        Ready_array.Add(PhotonPlayer_Extension.Player.Ready);
        Master_array.Add(PhotonPlayer_Extension.Player.Room_Master);

        foreach (PhotonPlayer_Extension player in Join_Players)
        {
            Key_array.Add(player.Key);
            Character_Image.Add((int)player.Select_Character);
            Id_array.Add(player.User_ID);
            name_array.Add(player.NickName);
            Lv_array.Add(player.Lv);
            Ready_array.Add(player.Ready);
            Master_array.Add(player.Room_Master);
        }
        
        //기존 접속해있는 유저 정보 넘김. Player Count + 1 <= Master Client Add.
        photonView.RPC("Recive_Init_ToClient", PhotonTargets.Others,
            Join_Players.Count,
            Key_array.ToArray(),
            Character_Image.ToArray(),
            Id_array.ToArray(),
            name_array.ToArray(),
            Lv_array.ToArray(),
            Ready_array.ToArray(),
            Master_array.ToArray()
            );
    }

    [PunRPC]
    public void Requested_Player_Info() // Network_Manager.cs -> Callback
    {
        photonView.RPC("Recive_Init_ToMaster", PhotonTargets.MasterClient, PhotonPlayer_Extension.Player.Key, (int)PhotonPlayer_Extension.Player.Select_Character, PhotonPlayer_Extension.Player.User_ID, PhotonPlayer_Extension.Player.NickName, PhotonPlayer_Extension.Player.Lv, PhotonNetwork.player, false);
    }

    [PunRPC]
    public void Client_Ready_State(string key,bool ready) // Room_UI.cs -> Only Send To MasterClient
    {
        foreach (PhotonPlayer_Extension player in Network_Manager.Join_Players)
        {
            if (player.Key == key) 
            {
                player.Ready = ready;
            }
        }

        Joined_Notice_Player();
    }

    [PunRPC]
    public void Recive_Init_ToMaster(string key, int C_Image, string id, string name, int lv, PhotonPlayer Client, bool Ready) // Network_Manager.cs -> Requested_Player_Info()
    {
        PhotonPlayer_Extension New_Join_Player = new PhotonPlayer_Extension();
        New_Join_Player.Key = key;
        New_Join_Player.Select_Character = (PhotonPlayer_Extension.Character_Type)C_Image;
        New_Join_Player.User_ID = id;
        New_Join_Player.NickName= name;
        New_Join_Player.Lv = lv;
        New_Join_Player.Ready = Ready;

        Join_Players.Add(New_Join_Player);
        Joined_Notice_Player();
        UI_Manager.Instance.Room_UI_Script.Refresh_Joined_PlayerInfo(); // 룸 UI 화면 새로고침
    }

    [PunRPC]
    public void Recive_Init_ToClient(int Player_Count, string[] key, int[] C_Image, string[] id, string[] name, int[] lv, bool[] ready, bool[] Master) // Network_Manager.cs -> Joined_Notice_Player() or Another Player Disconected callback!!!
    {
        Join_Players.Clear();
        
        for (int i = 0; i < Player_Count+1; i++) // First Index is Master_Client 
        {
            Debug.Log("리스트중하나 => " + key[i] + "내 키값->" + PhotonPlayer_Extension.Player.Key);
            if (key[i] != PhotonPlayer_Extension.Player.Key) //받은 리스트 중 내 정보인 것을 제외한 나머지 정보일 경우
            {
                Debug.Log(i);
                PhotonPlayer_Extension Player_Info = new PhotonPlayer_Extension();
                Player_Info.Key = key[i];
                Player_Info.Select_Character = (PhotonPlayer_Extension.Character_Type)C_Image[i];
                Player_Info.User_ID = id[i];
                Player_Info.NickName = name[i];
                Player_Info.Lv = lv[i];
                Player_Info.Ready = ready[i];
                Player_Info.Room_Master = Master[i];
                
                Join_Players.Add(Player_Info);
            }
            else
            {
                Debug.Log("호출은되냐..?");
                PhotonPlayer_Extension.Player.List_Index = i;
            }
        }
        
        UI_Manager.Instance.Room_UI_Script.Refresh_Joined_PlayerInfo();
    }

    [PunRPC]
    public void Chat_Send_Call(string Message, string Sender) // Room_UI.cs -> Chat_Btn() Calling
    {
        if (Game_Manager.Instance.GameState == Game_Manager.Game_State.Room)
        {
            In_Room_Chat_Send(Message, Sender);
        }

        if (Game_Manager.Instance.GameState == Game_Manager.Game_State.InGame)
        {
            In_Game_Chat_Send(Message, Sender);
        }
    }
    
    public void In_Room_Chat_Send(string Message, string Sender)
    {
        UILabel Message_Object = GameObject.Instantiate(Resources.Load<GameObject>("Event_UI\\Chat_Label")).GetComponent<UILabel>();

        if (Sender == PhotonPlayer_Extension.Player.NickName)
        {
            Message_Object.text = "[FF0000]" + Sender + "[-] : " + Message;
        }
        else
            Message_Object.text = "[0010FF]" + Sender + "[-] : " + Message;

        if (Message_Object)
        {
            UI_Manager.Instance.Room_UI_Script.Chat_Field.GetComponent<UIInput>().value = "";
            Message_Object.transform.parent = UI_Manager.Instance.Room_UI_Script.Parent_Chat;
            Message_Object.transform.localPosition = Vector3.zero;
            Message_Object.transform.localScale = Vector3.one;
            Message_Object.transform.parent.GetComponent<UIGrid>().Reposition();
        }
    }

    //----------------------- 인 게임에서의 RPC -----------------------------------

    [PunRPC]
    public void InGame_UI_Scene_Addtive(int main,int sub)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("PVP" + main + "-" + sub);
        UnityEngine.SceneManagement.SceneManager.LoadScene(Scence_Manager.Instance.InGame_UI, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    [PunRPC]
    public void Send_Score(string Key, int Rank_Point)
    {
        foreach (PhotonPlayer_Extension Player in Join_Players)
        {
            if (Player.Key == Key)
            {
                Player.Rank_Score = Rank_Point;
            }
        }
        Stage_Manager.Instance.Rank_Compare();
    }

    [PunRPC]
    public void Send_Timer(string Time)
    {
        if (InGame_UI.Instance != null)
        {
            InGame_UI.Instance.Timer_Label.text = Time;
        }
    }

    [PunRPC]
    public void Syc_Attack(int OwnerID, Input_Manager.Mouse_Event Event_)
    {
        GameObject Unit = null;

        foreach (GameObject T_Unit in Players_Unit)
        {
            if (T_Unit.GetComponent<PhotonView>().ownerId == OwnerID)
            {
                Unit = T_Unit;
                break;
            }
        }

        try
        {
            switch (Event_)
            {
                case Input_Manager.Mouse_Event.Left_Mouse:
                    Unit.GetComponent<Character>().Fire_Bullet_One();
                    Unit.GetComponent<Character>().Move_Rotate_SpeedDown(true);
                    Unit.GetComponent<Character>().Player_.animator.SetTrigger("Attack");
                    break;

                case Input_Manager.Mouse_Event.Left_Mouse_Up:
                    Unit.GetComponent<Character>().Move_Rotate_SpeedDown(false);
                    break;

                case Input_Manager.Mouse_Event.Right_Mouse:
                    Unit.GetComponent<Character>().Fire_Bullet_Two();
                    break;
            }
        }
        catch (System.NullReferenceException e)
        {
            return;
        }
    }

    [PunRPC]
    public void In_Game_Chat_Send(string Message, string Sender)
    {
        //인 게임내에서 채팅이 존재할 경우 추후 구현
    }

    //------------------------ 포톤 콜백 처리 -------------------------------------

        //포톤에 연결되었을때.
    public override void OnConnectedToPhoton()
    {
        Debug.Log("모든 플레이어가 고유의 키값을 가질수 있는 방법에 대해 고려 - 해시 등..");
        PhotonPlayer_Extension.Player.Key = System.Convert.ToChar(Random.Range(0, 127)) + Random.Range(1, 999999).ToString() + System.Convert.ToChar(Random.Range(0, 127)) + Random.Range(1, 999999).ToString();
        PhotonNetwork.player.NickName = PhotonPlayer_Extension.Player.Key; //PhotonNetwork 의 Player Userid는 고유 키 값을 저장한다. -> Photon의 Callback은 Player를 전달하기 때문.

        //추후에 연결이 일시적으로 재연결상황 고려
    }

    //포톤과의 연결이 종료되었을 경우
    public override void OnDisconnectedFromPhoton()
    {
        UI_Manager.Instance.Open_Login_Call();
    }

    //방에 입장하는것을 실패하였을 경우
    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        Debug.LogError("방 입장에 실패하였습니다.");
    }

    //로비에 입장되었을 때
    public override void OnJoinedLobby()
    {
        UI_Manager.Instance.Open_PVP_Call();
        if (IsInvoking("Get_RoomList") == false) //Try Get RoomList
        {
            Get_RoomList();
        }
    }

    //로비에 나갔을 경우
    public override void OnLeftLobby()
    {
        if (IsInvoking("Get_RoomList"))
            CancelInvoke("Get_RoomList");
    }

    //방을 생성하였을 경우
    public override void OnCreatedRoom()
    {
        Join_Players.Clear(); // 초기화.
        PhotonPlayer_Extension.Player.List_Index = 0;
    }

    //룸에 입장하였을 때
    public override void OnJoinedRoom()
    {
        Network_Manager.Instance.Joined_Maximum = PhotonNetwork.room.MaxPlayers;
        UI_Manager.Instance.Open_Room_Call(Present_Name, Present_Pass, Network_Manager.Instance.Joined_Maximum, Network_Manager.Instance.Room_Mode);
    }
    
    //방 생성에 실패하였을 경우
    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {

    }

    //오브젝트가 생성되었을 경우
    public override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Players_Unit.Clear();
        Character[] List = GameObject.FindObjectsOfType<Character>();

        foreach(Character Unit in List)
        {
            Players_Unit.Add(Unit.gameObject);
        }
    }

    //다른 플레이어가 방을 접속한 경우
    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        if (PhotonNetwork.isMasterClient == true && Game_Manager.Instance.GameState == Game_Manager.Game_State.Room)
        {
            photonView.RPC("Requested_Player_Info", newPlayer); // 접속한 유저에게 정보를 요구한다.
        }
    }
    
    //다른 플레이어가 방을 나갔을 경우
    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        UI_Manager.Instance.Room_UI_Script.Set_Btn_Event();

        if (Game_Manager.Instance.GameState == Game_Manager.Game_State.Room)
        {
            int index = 0;
            foreach (PhotonPlayer_Extension Player in Join_Players)
            {
                if (Player.Key == otherPlayer.NickName)
                {
                    Join_Players.RemoveAt(index);
                    break;
                }
                index++;
            }
            Joined_Notice_Player(); //다른 클라이언트에게 알림
            UI_Manager.Instance.Room_UI_Script.Refresh_Joined_PlayerInfo(); // 룸 UI 화면 새로고침
        }

        if (Game_Manager.Instance.GameState == Game_Manager.Game_State.InGame && PhotonNetwork.isMasterClient) // 인게임 상황에서 내가 마스터가 되었을 경우
        {
            //if Room Master Leave In Game
        }
    }
}
