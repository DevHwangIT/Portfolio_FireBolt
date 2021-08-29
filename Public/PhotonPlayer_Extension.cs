using System.Collections;
using System.Collections.Generic;

//실제 Extension이 아닌 Custom으로 Player 정보 처리.
public class PhotonPlayer_Extension
{
    public static PhotonPlayer_Extension Player = new PhotonPlayer_Extension(); // My infomation

    public Player Player_; // Character Hp,EXP,Lv...
    Player Player_C
    {
        get
        {
            try
            {
                Player_ = Game_Manager.Instance.My_Character.Player_;
                return Player_;
            }
            catch
            {
                return null;
            }
        }
    }

    public enum Character_Type
    {
        Hoon,
        Pie,
        SIM_0
    }
    public Character_Type Select_Character;

    //Photonnetwork 안에서의 고유 키 값 저장.
    public string Key = "";

    string Id_ = "";
    public string User_ID
    {
        get
        {
            return Hashing_Get(Id_);
        }
        set
        {
            Id_ = Hashing_Set(value);
        }
    }

    string Pw_ = "";
    public string User_Pw
    {
        get
        {
            return Hashing_Get(Pw_);
        }
        set
        {
            Pw_ = Hashing_Set(value);
        }
    }

    string NickName_ = "";
    public string NickName
    {
        get
        {
            return Hashing_Get(NickName_);
        }
        set
        {
            NickName_ = Hashing_Set(value);
        }
    }

    int Lv_ = 1;
    public int Lv
    {
        get
        {
            return Hashing_Get(Lv_);
        }
        set
        {
            Lv_ = Hashing_Set(value);
        }
    }

    public float Exp
    {
        get
        {
            return Hashing_Get(Player_C.Exp);
        }
        set
        {
            Player_C.Exp = Hashing_Set(value);
        }
    }

    //Using InGame_UI
    public Game_Manager.Default_Callback Score_Callback;
    public int Rank_Score
    {
        get
        {
            return Hashing_Get(Player_C.Score);
        }
        set
        {
            Score_Callback();
            Player_C.Score = Hashing_Set(value);
        }
    }

    //Using InGame_UI
    public Game_Manager.Default_Callback Hp_Callback;
    public float Hp_Value
    {
        get
        {
            return Hashing_Get(Player_C.Helth_Value);
        }
        set
        {
            Hp_Callback();
            Player_C.Helth_Value = Hashing_Set(value);
        }
    }

    //Using InGame_UI
    public Game_Manager.Default_Callback Exp_Callback;
    public float Exp_Value
    {
        get
        {
            return Hashing_Get(Player_C.Exp);
        }
        set
        {
            Exp_Callback();
            Player_C.Exp = Hashing_Set(value);
        }
    }

    //Syc_List_Index
    public int List_Index = 0;

    //Ready State Check
    public bool Ready = false;

    //Room Master Check
    public bool Room_Master = false;

    public T Hashing_Set<T>(T value_)
    {
        //추후 구현
        return value_;
    }

    public T Hashing_Get<T>(T value_)
    {
        //추후 구현
        return value_;
    }
}

