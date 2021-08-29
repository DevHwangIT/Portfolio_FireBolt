using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting_Set_UI : UI_Parent
{
    public static Setting_Set_UI Instance;
    [SerializeField] private UISlider Sound_Bar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public new void Open()
    {
        this.gameObject.SetActive(true);
        Sound_Bar.value = Game_Manager.Instance.BGM_Volum;
    }

    public void Save_Btn()
    {
        Game_Manager.Instance.BGM_Volum = Sound_Bar.value;
        Close();
    }
    
    void Start()
    {
        Show_Setting_Key();
    }
    
    public void Show_Setting_Key()
    {
        Up_Key_Label.text = Input_Manager.Instance.UP_Move_Key.ToString();
        Down_Key_Label.text = Input_Manager.Instance.Down_Move_Key.ToString();
        Left_Key_Label.text = Input_Manager.Instance.Left_Move_Key.ToString();
        Right_Key_Label.text = Input_Manager.Instance.Right_Move_Key.ToString();

        Fire1_Key_Label.text = Input_Manager.Instance.Fire1_Key.ToString();
        Fire2_Key_Label.text = Input_Manager.Instance.Fire2_Key.ToString();

        Evasion_Key_Label.text = Input_Manager.Instance.Evasion_Key.ToString();
    }
    
    KeyCode Read_Key()
    {
        foreach (KeyCode kc in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(kc))
            {
                return kc;
            }
        }
        return KeyCode.None;
    }

    public void Up_Key_Btn()
    {
        Input_Manager.Instance.UP_Move_Key = Read_Key();
        Show_Setting_Key();
    }

    public void Down_Key_Btn()
    {
        Input_Manager.Instance.Down_Move_Key = Read_Key();
        Show_Setting_Key();
    }

    public void Left_Key_Btn()
    {
        Input_Manager.Instance.Left_Move_Key = Read_Key();
        Show_Setting_Key();
    }

    public void Right_Key_Btn()
    {
        Input_Manager.Instance.Right_Move_Key = Read_Key();
        Show_Setting_Key();
    }

    public void Fire1_Key_Btn()
    {
        Input_Manager.Instance.Fire1_Key = Read_Key();
        Show_Setting_Key();
    }

    public void Fire2_Key_Btn()
    {
        Input_Manager.Instance.Fire2_Key = Read_Key();
        Show_Setting_Key();
    }

    public void Evasion_Key_Btn()
    {
        Input_Manager.Instance.Evasion_Key = Read_Key();
        Show_Setting_Key();
    }
}
