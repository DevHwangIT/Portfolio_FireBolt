using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creadit_UI : UI_Parent
{
    void Update()
    {
        if (Input.anyKey || Input.GetMouseButton(0))
        {
            Close();
        }
    }
}
