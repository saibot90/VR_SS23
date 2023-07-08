using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the time of the day
/// </summary>
public class TimeManager : MonoBehaviour
{
    private const int HoursInDay = 12, MinutesInHour = 60;

    private const float DayDuration = 120f; // in seconds

    private float _currentTime = 121;

    /// <summary>
    /// Is called when the script is called for the first time
    ///     -subscribes game events
    /// </summary>
    private void Start()
    {
        GameEvents.current.onTriggerNextDay += ResetDay;
    }

    /// <summary>
    /// Update is called once per frame
    /// - Updates the current time of the day
    /// </summary>
    private void Update()
    {
        if (_currentTime <= DayDuration)
        {
            _currentTime += Time.deltaTime;
        }
    }

    /// <summary>
    /// Returns the current hour of the day
    /// </summary>
    /// <returns>hour of the day</returns>
    public float GetHour()
    {
        return _currentTime * HoursInDay / DayDuration;
    }

    /// <summary>
    /// Returns the current minute of the the current hour of the day
    /// </summary>
    /// <returns>minute of the the current hour</returns>
    public float GetMinutes()
    {
        return (_currentTime * HoursInDay * MinutesInHour / DayDuration) % MinutesInHour;
    }

    /// <summary>
    /// Resets the time to start a new day day
    /// </summary>
    private void ResetDay()
    {
        _currentTime = 0;
    }

    /// <summary>
    /// Checks and returns if the current day has ended or not
    /// </summary>
    /// <returns>true if day ended, otherwise false</returns>
    public bool IsDayEnd()
    {
        return _currentTime > DayDuration;
    }
    
    /// <summary>
    /// Is called if this script is destroyed
    ///     -unsubscribe game events
    /// </summary>
    private void OnDestroy()
    {
        GameEvents.current.onTriggerNextDay -= ResetDay;
    }
}
