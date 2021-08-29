using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Sound_Manager : MonoBehaviour {

    //사운드 관련 처리를 담당하는 매니저 클래스
    public static Sound_Manager Instance;
    
    [SerializeField] private AudioClip Login_Clip;
    [SerializeField] private AudioClip Lobby_Clip;
    [SerializeField] private AudioClip Room_Clip;
    [SerializeField] private AudioClip InGame1;
    [SerializeField] private AudioClip InGame2;
    [SerializeField] private AudioSource Audio_;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        Audio_ = this.GetComponent<AudioSource>();
        Audio_.loop = true;
        Finding_Sound();
        Set_Volum(Game_Manager.Instance.BGM_Volum);
    }

    public void Set_Volum(float Volum)
    {
        Audio_.volume = Volum;
    }

    public void Finding_Sound()
    {
        Login_Clip = Resources.Load<AudioClip>("Music\\BGM\\Login");
        Lobby_Clip = Resources.Load<AudioClip>("Music\\BGM\\Lobby");
        Room_Clip = Resources.Load<AudioClip>("Music\\BGM\\Room");
        InGame1 = Resources.Load<AudioClip>("Music\\BGM\\InGame1");
        InGame2 = Resources.Load<AudioClip>("Music\\BGM\\InGame2");
        BGM_Change(Game_Manager.Instance.GameState);
    }

    public void BGM_Change(Game_Manager.Game_State Type)
    {
        switch (Type)
        {
            case Game_Manager.Game_State.Login:
                if (Audio_.isPlaying)
                {
                    Audio_.Stop();
                }
                Audio_.clip = Login_Clip;
                Audio_.Play();
                break;

            case Game_Manager.Game_State.Lobby:
                if (Audio_.isPlaying)
                {
                    Audio_.Stop();
                }
                Audio_.clip = Lobby_Clip;
                Audio_.Play();
                break;
                
            case Game_Manager.Game_State.Room:
                if (Audio_.isPlaying)
                {
                    Audio_.Stop();
                }
                Audio_.clip = Room_Clip;
                Audio_.Play();
                break;

            case Game_Manager.Game_State.InGame:
                if (Audio_.isPlaying)
                {
                    Audio_.Stop();
                }
                Audio_.clip = InGame1;
                Audio_.Play();
                break;
        }
    }
}
