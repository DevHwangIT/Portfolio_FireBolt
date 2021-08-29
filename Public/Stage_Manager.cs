using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage_Manager : MonoBehaviour {

    //스테이지 관련 처리를 담당하는 클래스
    public static Stage_Manager Instance;

    float Stage_Complete_Persent = 0f;

    //RankResources Image.
    string Rank_1_Sprite_Name = "128x128_Word_1st";
    string Rank_2_Sprite_Name = "128x128_Word_2nd";
    string Rank_3_Sprite_Name = "128x128_Word_3rd";
    string Rank_4_Sprite_Name = "128x128_Word_4th";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //Callback Register.
            PhotonPlayer_Extension.Player.Score_Callback += Set_Score_Callback;
        }
    }

    public void Initialized()
    {
        //Initialize
        InGame_UI.Instance.Lv_With_Nick_Name.text = PhotonPlayer_Extension.Player.Lv + "        " + PhotonPlayer_Extension.Player.NickName;
        InGame_UI.Instance.Score_Label.text = "0";

        GameObject My_Unit = null;

        try
        {
            if (PhotonNetwork.connected)
            {
                switch (PhotonPlayer_Extension.Player.Select_Character)
                {
                    case PhotonPlayer_Extension.Character_Type.Hoon:
                        My_Unit = PhotonNetwork.Instantiate("Character\\Huny\\Huny", Player_Create_Pos.Respon_Pos[PhotonPlayer_Extension.Player.List_Index].position, Quaternion.identity, 0);
                        break;

                    case PhotonPlayer_Extension.Character_Type.Pie:
                        break;

                    case PhotonPlayer_Extension.Character_Type.SIM_0:
                        break;

                    default:
                        My_Unit = PhotonNetwork.Instantiate("Character\\Huny\\Huny", Player_Create_Pos.Respon_Pos[PhotonPlayer_Extension.Player.List_Index].position, Quaternion.identity, 0);
                        break;
                }

                Game_Manager.Instance.My_Unit = My_Unit;
            }
            else
            {
                My_Unit = Instantiate(Resources.Load<GameObject>("Character\\Huny\\Huny"), Player_Create_Pos.Respon_Pos[0].position, Quaternion.identity);
                Destroy(My_Unit.GetComponent<PhotonTransformView>());
                Destroy(My_Unit.GetComponent<PhotonAnimatorView>());
                Destroy(My_Unit.GetComponent<PhotonView>());
            }
        }
        catch (System.NullReferenceException e)
        {
            Debug.Log(e.ToString());
            My_Unit = Instantiate(Resources.Load<GameObject>("Character\\Huny\\Huny"), Player_Create_Pos.Respon_Pos[0].position, Quaternion.identity);
            Destroy(My_Unit.GetComponent<PhotonView>());
        }
    }

    //Callback Method
    void Set_Score_Callback()
    {
        InGame_UI.Instance.Score_Label.text = PhotonPlayer_Extension.Player.Rank_Score.ToString();
        switch (Game_Manager.Instance.GameMode)
        {
            case Game_Manager.Game_Mode.ACADE:
                PhotonPlayer_Extension.Player.Rank_Score = PhotonPlayer_Extension.Player.Rank_Score;
                Stage_Manager.Instance.Rank_Compare();
                break;

            case Game_Manager.Game_Mode.PVP:
                Network_Manager.Instance.photonView.RPC("Send_Score", PhotonTargets.Others, PhotonNetwork.player.NickName, PhotonPlayer_Extension.Player.Rank_Score);
                break;

            default:
                PhotonPlayer_Extension.Player.Rank_Score = PhotonPlayer_Extension.Player.Rank_Score;
                Stage_Manager.Instance.Rank_Compare();
                break;
        }
    }

    //Callback -> Network_manager.cs -> Calling this.
    public void Rank_Compare()
    {
        int rank = 0;
        foreach (PhotonPlayer_Extension Player in Network_Manager.Join_Players)
        {
            if (Player.Rank_Score >= PhotonPlayer_Extension.Player.Rank_Score)
            {
                rank++;
            }
        }
        switch (rank)
        {
            case 0:
                InGame_UI.Instance.Rank_Sprite.spriteName = Rank_1_Sprite_Name;
                break;

            case 1:
                InGame_UI.Instance.Rank_Sprite.spriteName = Rank_2_Sprite_Name;
                break;

            case 2:
                InGame_UI.Instance.Rank_Sprite.spriteName = Rank_3_Sprite_Name;
                break;

            case 3:
                InGame_UI.Instance.Rank_Sprite.spriteName = Rank_4_Sprite_Name;
                break;

            default:
                InGame_UI.Instance.Rank_Sprite.spriteName = Rank_4_Sprite_Name;
                break;
        }
    }
}
