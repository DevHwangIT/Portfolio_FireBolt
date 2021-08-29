using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Manager : MonoBehaviour
{
    public static Input_Manager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public KeyCode UP_Move_Key = KeyCode.W;
    public KeyCode Down_Move_Key = KeyCode.S;
    public KeyCode Left_Move_Key = KeyCode.A;
    public KeyCode Right_Move_Key = KeyCode.D;
    public KeyCode Fire1_Key = KeyCode.Mouse0;
    public KeyCode Fire2_Key = KeyCode.Mouse1;
    public KeyCode Evasion_Key = KeyCode.Space;

    Dictionary<KeyCode, System.Action> keyDictionary;

    public enum Mouse_Event
    {
        Left_Mouse_Down=0,
        Left_Mouse = 1,
        Left_Mouse_Up =2,
        Right_Mouse_Down = 3,
        Right_Mouse = 4,
        Right_Mouse_Up = 5,
        Wheel_Mouse=6,
        Wheel_Mouse_Down=7,
        Wheel_Mouse_Up = 8
    }

    void Key_Change()
    {
        keyDictionary = new Dictionary<KeyCode, System.Action>
        {
            { UP_Move_Key, Key_Up },
            { Down_Move_Key, Key_Down },
            { Left_Move_Key, Key_Left },
            { Right_Move_Key, Key_Right },
            { KeyCode.LeftControl, Test_PowerUp },
            { KeyCode.P, Test_Dead },
        };
    }

    void Start()
    {
        Key_Change();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Game_Manager.Instance.My_Unit != null) //캐릭터가 존재 할 경우
        {
            Game_Manager.Instance.My_Character.Rotate();
            //플랫폼에 따른 키입력 방식 고려. 탱크 형태의 움직임. 상하 몸체 따로, 포탄따로 회전
            
            #region Invasion&Move
            if (Input.anyKey)
            {
                foreach (var Frame in keyDictionary)
                {
                    if (Input.GetKey(Frame.Key))
                    {
                        Frame.Value();
                    }
                }
            }
            else
            {
                Game_Manager.Instance.My_Character.Player_.animator.SetInteger("AnimState", 0);
            }
            #endregion

            if (Input.GetMouseButton(0)) //포탄 1에 대한 호출
            {
                try
                {
                    if(PhotonNetwork.connected)
                    {
                        Network_Manager.Instance.photonView.RPC("Syc_Attack", PhotonTargets.All, Network_Manager.Instance.Pv_.ownerId, Mouse_Event.Left_Mouse_Down);
                    }else
                    {
                        Game_Manager.Instance.My_Character.Fire_Bullet_One();
                        Game_Manager.Instance.My_Character.Move_Rotate_SpeedDown(true);
                        Game_Manager.Instance.My_Character.Player_.animator.SetTrigger("Attack");
                    }
                }
                catch (System.NullReferenceException e)
                {
                    Game_Manager.Instance.My_Character.Fire_Bullet_One();
                    Game_Manager.Instance.My_Character.Move_Rotate_SpeedDown(true);
                    Game_Manager.Instance.My_Character.Player_.animator.SetTrigger("Attack");
                }

            }
            if (Input.GetMouseButtonUp(0))
            {
                try
                {
                    if (PhotonNetwork.connected)
                    {
                        Network_Manager.Instance.photonView.RPC("Syc_Attack", PhotonTargets.All, Network_Manager.Instance.Pv_.ownerId, Mouse_Event.Left_Mouse_Up);
                    }
                    else
                    {
                        Game_Manager.Instance.My_Character.Move_Rotate_SpeedDown(false);
                    }
                }
                catch (System.NullReferenceException e)
                {
                    Game_Manager.Instance.My_Character.Move_Rotate_SpeedDown(false);
                }
            }
            if (Input.GetMouseButton(1)) //포탄 2에 대한 호출
            {
                try
                {
                    if (PhotonNetwork.connected)
                    {
                        Network_Manager.Instance.photonView.RPC("Syc_Attack", PhotonTargets.All, Network_Manager.Instance.Pv_.ownerId, Mouse_Event.Right_Mouse);
                    }
                    else
                    {
                        Game_Manager.Instance.My_Character.Fire_Bullet_Two();
                    }
                }
                catch (System.NullReferenceException e)
                {
                    Game_Manager.Instance.My_Character.Fire_Bullet_Two();
                }
            }
        }
    }

    void Key_Up()
    {
        Game_Manager.Instance.My_Character.Player_.animator.SetInteger("AnimState", 1);
        Game_Manager.Instance.My_Character.Move(1, 0);
        if (Input.GetKey(KeyCode.Space)) //회피에 대한 호출
        {
            Game_Manager.Instance.My_Character.Invasion(1, 0);
        }
    }

    void Key_Down()
    {
        Game_Manager.Instance.My_Character.Player_.animator.SetInteger("AnimState", 1);
        Game_Manager.Instance.My_Character.Move(-1, 0);
        if (Input.GetKey(KeyCode.Space)) //회피에 대한 호출
        {
            Game_Manager.Instance.My_Character.Invasion(-1, 0);
        }
    }
    void Key_Left()
    {
        Game_Manager.Instance.My_Character.Player_.animator.SetInteger("AnimState", 1);
        Game_Manager.Instance.My_Character.Move(0, -1);
        if (Input.GetKey(KeyCode.Space)) //회피에 대한 호출
        {
            Game_Manager.Instance.My_Character.Invasion(0, -1);
        }
    }
    void Key_Right()
    {
        Game_Manager.Instance.My_Character.Player_.animator.SetInteger("AnimState", 1);
        Game_Manager.Instance.My_Character.Move(0, 1);
        if (Input.GetKey(KeyCode.Space)) //회피에 대한 호출
        {
            Game_Manager.Instance.My_Character.Invasion(0, 1);
        }
    }

    void Test_PowerUp()
    {
        Game_Manager.Instance.My_Character.PowerUp();
    }

    void Test_Dead()
    {
        if (Game_Manager.Instance.My_Character.isDead)
        {
            Game_Manager.Instance.My_Character.ReSpawn();
        }
    }
}
