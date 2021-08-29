using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Top_UI : UI_Parent
{
    public void Mail_Box_Open_Btn()
    {
        UI_Manager.Instance.Open_MailBox_Call();
    }

    public void Setting_Open_Btn()
    {
        UI_Manager.Instance.Open_Setting_Call();
    }
}
