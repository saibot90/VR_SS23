using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private const int HoursInDay = 12, MinutesInHour = 60;

    private const float DayDuration = 120f; // in seconds

    private float _currentTime = 0;

    private void Start()
    {
        GameEvents.current.onTriggerNextDay += ResetDay;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentTime <= DayDuration)
        {
            _currentTime += Time.deltaTime;
            //Debug.Log(_currentTime.ToString("N1"));
        }
        //_currentTime = _totalTime % dayDuration;
        //Debug.Log(_currentTime.ToString("N1"));
    }

    public float GetHour()
    {
        return _currentTime * HoursInDay / DayDuration;
    }

    public float GetMinutes()
    {
        return (_currentTime * HoursInDay * MinutesInHour / DayDuration) % MinutesInHour;
    }

    public void ResetDay()
    {
        _currentTime = 0;
        Debug.Log("Test");
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
