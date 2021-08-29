using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_Menu : UI_Parent
{
    public static InGame_Menu Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
