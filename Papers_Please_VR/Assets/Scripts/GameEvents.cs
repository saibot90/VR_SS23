using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action<string> onInfo;

    public void Info(string info)
    {
        if (onInfo != null)
        {
            onInfo(info);
        }
    }

    public event Action onTriggerInfo;

    public void TriggerInfo()
    {
        if(onTriggerInfo != null)
        {
            onTriggerInfo();
        }
    }
}
