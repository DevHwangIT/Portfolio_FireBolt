using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyInfo_UI : UI_Parent
{
    public GameObject Character_Select_View;

    [SerializeField] private UISprite Character_Texture;
    [SerializeField] private UILabel Nick_Name;
    [SerializeField] private UILabel Lv;
    [SerializeField] private UISlider Hp_Bar;
    [SerializeField] private UISlider ATK_Bar;
    [SerializeField] private UISlider DEF_Bar;
    [SerializeField] private UISlider SPD_Bar;

    public new void Open()
    {
        this.gameObject.SetActive(true);
        Character_Select_View.SetActive(false);
        Show_Character_Stat();
    }

    public new void Close()
    {
        this.gameObject.SetActive(false);
        Character_Select_View.SetActive(false);
    }

    void Show_Character_Stat()
    {
        switch (PhotonPlayer_Extension.Player.Select_Character)
        {
            case PhotonPlayer_Extension.Character_Type.Hoon:
                StartCoroutine(Info_Slider_Animation(0.5f, 0.5f, 0.4f, 0.4f));
                Character_Texture.spriteName = "768x768_ICON_Cha0";
                break;

            case PhotonPlayer_Extension.Character_Type.Pie:
                StartCoroutine(Info_Slider_Animation(0.3f, 0.9f, 0.3f, 0.7f));
                Character_Texture.spriteName = "768x768_ICON_Cha1";
                break;

            case PhotonPlayer_Extension.Character_Type.SIM_0:
                StartCoroutine(Info_Slider_Animation(0.8f, 0.4f, 0.7f, 0.2f));
                Character_Texture.spriteName = "768x768_ICON_Cha1";
                break;

            default:
                StartCoroutine(Info_Slider_Animation(0.5f, 0.5f, 0.4f, 0.4f));
                Character_Texture.spriteName = "768x768_ICON_Cha0";
                break;
        }

        Nick_Name.text = PhotonPlayer_Extension.Player.NickName;
        Lv.text = PhotonPlayer_Extension.Player.Lv.ToString();
    }

    IEnumerator Info_Slider_Animation(float HP, float ATK, float DEF, float SPD)
    {
        Hp_Bar.transform.GetChild(0).GetComponent<TweenWidth>().style = UITweener.Style.PingPong;
        Hp_Bar.transform.GetChild(0).GetComponent<TweenPosition>().style = UITweener.Style.PingPong;
        Hp_Bar.transform.GetChild(0).GetComponent<TweenWidth>().PlayForward();
        Hp_Bar.transform.GetChild(0).GetComponent<TweenPosition>().PlayForward();

        ATK_Bar.transform.GetChild(0).GetComponent<TweenWidth>().style = UITweener.Style.PingPong;
        ATK_Bar.transform.GetChild(0).GetComponent<TweenPosition>().style = UITweener.Style.PingPong;
        ATK_Bar.transform.GetChild(0).GetComponent<TweenWidth>().PlayForward();
        ATK_Bar.transform.GetChild(0).GetComponent<TweenPosition>().PlayForward();

        DEF_Bar.transform.GetChild(0).GetComponent<TweenWidth>().style = UITweener.Style.PingPong;
        DEF_Bar.transform.GetChild(0).GetComponent<TweenPosition>().style = UITweener.Style.PingPong;
        DEF_Bar.transform.GetChild(0).GetComponent<TweenWidth>().PlayForward();
        DEF_Bar.transform.GetChild(0).GetComponent<TweenPosition>().PlayForward();

        SPD_Bar.transform.GetChild(0).GetComponent<TweenWidth>().style = UITweener.Style.PingPong;
        SPD_Bar.transform.GetChild(0).GetComponent<TweenPosition>().style = UITweener.Style.PingPong;
        SPD_Bar.transform.GetChild(0).GetComponent<TweenWidth>().PlayForward();
        SPD_Bar.transform.GetChild(0).GetComponent<TweenPosition>().PlayForward();
        yield return new WaitForSeconds(0.5f);
        Hp_Bar.transform.GetChild(0).GetComponent<TweenWidth>().style = UITweener.Style.Once;
        Hp_Bar.transform.GetChild(0).GetComponent<TweenPosition>().style = UITweener.Style.Once;
        Hp_Bar.transform.GetChild(0).GetComponent<TweenWidth>().PlayReverse();
        Hp_Bar.transform.GetChild(0).GetComponent<TweenPosition>().PlayReverse();

        ATK_Bar.transform.GetChild(0).GetComponent<TweenWidth>().style = UITweener.Style.Once;
        ATK_Bar.transform.GetChild(0).GetComponent<TweenPosition>().style = UITweener.Style.Once;
        ATK_Bar.transform.GetChild(0).GetComponent<TweenWidth>().PlayReverse();
        ATK_Bar.transform.GetChild(0).GetComponent<TweenPosition>().PlayReverse();

        DEF_Bar.transform.GetChild(0).GetComponent<TweenWidth>().style = UITweener.Style.Once;
        DEF_Bar.transform.GetChild(0).GetComponent<TweenPosition>().style = UITweener.Style.Once;
        DEF_Bar.transform.GetChild(0).GetComponent<TweenWidth>().PlayReverse();
        DEF_Bar.transform.GetChild(0).GetComponent<TweenPosition>().PlayReverse();

        SPD_Bar.transform.GetChild(0).GetComponent<TweenWidth>().style = UITweener.Style.Once;
        SPD_Bar.transform.GetChild(0).GetComponent<TweenPosition>().style = UITweener.Style.Once;
        SPD_Bar.transform.GetChild(0).GetComponent<TweenWidth>().PlayReverse();
        SPD_Bar.transform.GetChild(0).GetComponent<TweenPosition>().PlayReverse();

        Hp_Bar.value = HP;
        ATK_Bar.value = ATK;
        DEF_Bar.value = DEF;
        SPD_Bar.value = SPD;
    }

    public void Character_Select_Btn()
    {
        Character_Select_View.SetActive(true);
    }

    public void Hoon_Select_Btn()
    {
        if (Game_Manager.Instance.GameState == Game_Manager.Game_State.Lobby)
        {
            PhotonPlayer_Extension.Player.Select_Character = PhotonPlayer_Extension.Character_Type.Hoon;
            Show_Character_Stat();
        }
        else
        {
            Debug.Log("방에 입장한 상태로는 캐릭터를 변경할 수 없음. 추후 구현바람");
        }
        Character_Select_View.SetActive(false);
        return;
    }

    public void Pie_Select_Btn()
    {
        if (Game_Manager.Instance.GameState == Game_Manager.Game_State.Lobby)
        {
            PhotonPlayer_Extension.Player.Select_Character = PhotonPlayer_Extension.Character_Type.Pie;
            Show_Character_Stat();
        }
        else
        {
            Debug.Log("방에 입장한 상태로는 캐릭터를 변경할 수 없음. 추후 구현바람");
        }
        Character_Select_View.SetActive(false);
    }

    public void SIM_Select_Btn()
    {
        if (Game_Manager.Instance.GameState == Game_Manager.Game_State.Lobby)
        {
            PhotonPlayer_Extension.Player.Select_Character = PhotonPlayer_Extension.Character_Type.SIM_0;
            Show_Character_Stat();
        }
        else
        {
            Debug.Log("방에 입장한 상태로는 캐릭터를 변경할 수 없음. 추후 구현바람");
        }
        Character_Select_View.SetActive(false);
    }
}
