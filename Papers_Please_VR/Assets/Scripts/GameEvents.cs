using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CheckStatus = Unity.Template.VR.CheckStatus;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action<PassPortData> onInfo;

    public void Info(PassPortData info)
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
    public event Action onTriggerPassBack2;

    public void TriggerPassBack2()
    {
        if(onTriggerPassBack2 != null)
        {
            onTriggerPassBack2();
        }
    }
    
    public event Action onTriggerVisaCheck;

    public void TriggerVisaCheck()
    {
        if(onTriggerVisaCheck != null)
        {
            onTriggerVisaCheck();
        }
    }
    
    public event Action<CheckStatus> onVisaStatus;

    public void VisaStatus(CheckStatus status)
    {
        if (onVisaStatus != null)
        {
            onVisaStatus(status);
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

    

    public event Action onSpawnNewPerson;

    public void SpawnNewPerson()
    {
        if(onSpawnNewPerson != null)
        { 
            onSpawnNewPerson(); 
        }
    }
    
    public event Action onTriggerNextDay;

    public void TriggerNextDay()
    {
        if(onTriggerNextDay != null)
        {
            onTriggerNextDay();
        }
    }
    
    public event Action<Person> onNewPersonSpawned;

    public void NewPersonSpawned(Person person)
    {
        if(onNewPersonSpawned != null)
        {
            onNewPersonSpawned(person);
        }
    }
    
    public event Action<CheckStatus> onVisaCheckSound;

    public void VisaCheckSound(CheckStatus status)
    {
        if (onVisaCheckSound != null)
        {
            onVisaCheckSound(status);
        }
    }

    public event Action onNewWantedPerson;

    public void NewWantedPerson()
    {
        if (onNewWantedPerson != null)
        {
            onNewWantedPerson();
        }
    }
}
