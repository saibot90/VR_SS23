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

    public event Action onTriggerPassBack;

    public void TriggerPassBack()
    {
        if(onTriggerPassBack != null)
        {
            onTriggerPassBack();
        }
    }

    public event Action<PassPortData> onTriggerPassCheck;

    public void TriggerPassCheck(PassPortData pass)
    {
        if(onTriggerPassCheck != null)
        {
            onTriggerPassCheck(pass);
        }
    }

    public event Action<bool> onVisaStatus;

    public void VisaStatus(bool status)
    {
        if (onVisaStatus != null)
        {
            onVisaStatus(status);
        }
    }

    public event Action onSpawnNewPerson;

    public void SpawnNewPerson()
    {
        if(onSpawnNewPerson != null)
        { 
            onSpawnNewPerson(); 
        }
    }
}
