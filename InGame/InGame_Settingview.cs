using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_Settingview : UI_Parent
{
    public static InGame_Settingview Instance;

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
}
