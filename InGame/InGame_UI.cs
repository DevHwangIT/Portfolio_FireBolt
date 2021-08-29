using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_UI : UI_Parent
{
    public static InGame_UI Instance;

    public InGame_Result InGame_Result_Scprit;
    public InGame_Menu InGame_Menu_Scprit;
    public InGame_Settingview InGame_Setting_Scprit;

    [SerializeField] private UILabel Timer_Label;
    [SerializeField] private UILabel Score_Label;
    [SerializeField] private UISprite Rank_Sprite;
    [SerializeField] private UILabel Lv_With_Nick_Name;
    [SerializeField] private UILabel Notice_Label;
    
    [Header("Character_State")]
    [SerializeField] private UISlider Hp_Bar;
    [SerializeField] private UILabel Now_Hp_Label;
    [SerializeField] private UILabel Max_Hp_Label;
    [SerializeField] private UISlider Exp_Bar;
    [SerializeField] private UILabel Now_Exp_Label;
    [SerializeField] private UILabel Max_Exp_Label;
    [SerializeField] private UISprite Skill_One;
    [SerializeField] private UISprite Skill_Two;


    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Game_Manager.Instance.InGame_M = this;
            Game_Manager.Instance.GameState = Game_Manager.Game_State.InGame;

            Game_Manager.Instance.Stage_M.Initialized();
            Game_Manager.Instance.Timer_M.Timer_Set(200); // 200 Sec.
            Game_Manager.Instance.Timer_M.Timer_Run();
        }
    }

    void LateUpdate()
    {
        if (Game_Manager.Instance.My_Character != null)
        {
            Max_Hp_Label.text = ((int)Game_Manager.Instance.My_Character.Player_.Maximum_Helth_Value).ToString();
            Now_Hp_Label.text = ((int)Game_Manager.Instance.My_Character.Player_.Helth_Value).ToString();
            Max_Exp_Label.text = ((int)Game_Manager.Instance.My_Character.Player_.Maximum_Exp).ToString();
            Now_Exp_Label.text = ((int)Game_Manager.Instance.My_Character.Player_.Exp).ToString();

            if (Game_Manager.Instance.My_Character.Player_.Helth_Value != 0)
                Hp_Bar.value = Game_Manager.Instance.My_Character.Player_.Helth_Value / Game_Manager.Instance.My_Character.Player_.Maximum_Helth_Value;
            else
                Hp_Bar.value = 0;

            if (Game_Manager.Instance.My_Character.Player_.Exp != 0)
                Exp_Bar.value = Game_Manager.Instance.My_Character.Player_.Exp / Game_Manager.Instance.My_Character.Player_.Maximum_Exp;
            else
                Exp_Bar.value = 0;

            if (Game_Manager.Instance.My_Character.Player_.Bullet2_Ready)
            {
                Skill_One.fillAmount = 1.0f;
            }
            else
            {
                if (!IsInvoking("Skill_Two_Dealy"))
                {
                    Skill_One.fillAmount = 0f;
                    Invoke("Skill_Two_Dealy", 0f);
                }
            }

            if (Game_Manager.Instance.My_Character.Player_.Invasion_Ready)
            {
                Skill_Two.fillAmount = 1.0f;
            }
            else
            {
                if (!IsInvoking("Invasion_Dealy"))
                {
                    Skill_Two.fillAmount = 0f;
                    Invoke("Invasion_Dealy", 0f);
                }
            }
        }
    }

    void Skill_Two_Dealy()
    {
        Skill_One.fillAmount += (Game_Manager.Instance.My_Character.Player_.Bullet_Delay_2 / 1000) * 0.1f;
        if (Skill_One.fillAmount < 1.0f)
        {
            Invoke("Skill_Two_Dealy", (Game_Manager.Instance.My_Character.Player_.Bullet_Delay_2 / 1000) * 0.1f);
        }
    }

    void Invasion_Dealy()
    {
        Skill_Two.fillAmount += (1 / (Game_Manager.Instance.My_Character.Player_.invansion_Delay / 1000));
        if (Skill_Two.fillAmount < 1.0f)
        {
            Invoke("Skill_Two_Dealy", (1 / (Game_Manager.Instance.My_Character.Player_.invansion_Delay / 1000)));
        }
    }

    public void Notice_To_Players(string Text, float Notice_Time)
    {
        Notice_Label.text = Text;
        Notice_Label.parent.gameObject.SetActive(true);
        Invoke("Close_Notice", Notice_Time);
    }

    void Close_Notice()
    {
        Notice_Label.parent.gameObject.SetActive(false);
    }
}
