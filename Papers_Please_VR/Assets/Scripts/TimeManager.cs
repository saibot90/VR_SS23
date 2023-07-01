using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private const int HoursInDay = 12, MinutesInHour = 60;

    private const float DayDuration = 120f; // in seconds

    private float _currentTime = 121;

    private void Start()
    {
        GameEvents.current.onTriggerNextDay += ResetDay;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_currentTime <= DayDuration)
        {
            _currentTime += Time.deltaTime;
        }
    }

    public float GetHour()
    {
        return _currentTime * HoursInDay / DayDuration;
    }

    public float GetMinutes()
    {
        return (_currentTime * HoursInDay * MinutesInHour / DayDuration) % MinutesInHour;
    }

    private void ResetDay()
    {
        _currentTime = 0;
    }

    public bool DayEnd()
    {
        return _currentTime > DayDuration;
    }
    
    private void OnDestroy()
    {
        GameEvents.current.onTriggerNextDay -= ResetDay;
    }
}
