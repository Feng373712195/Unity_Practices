using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PingSystem.AddPing(UtilsClass.GetMouseWorldPosition());
        }

        PingSystem.Update();
    }
   
}
