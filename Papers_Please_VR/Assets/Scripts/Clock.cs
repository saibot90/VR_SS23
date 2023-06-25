using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;

public class Clock : MonoBehaviour
{
    public Transform minuteHand;
    public Transform hourHand;

    private const float MinutesToPlay = 1f;
    private const float SecondsToPlay = MinutesToPlay * 60;
    private const float WorkTimeInHours = 6;
    private float _deltaTime = 0.0f;
    private float _deltaTimePlayHour = 0.0f;
    private float _deltaTimePlaySeconds = 0.0f;
    private const float OnePlayHour = SecondsToPlay / WorkTimeInHours;
    private const float FivePlayMinutes = SecondsToPlay / WorkTimeInHours / 60.0f;
    private float _inGameHoursPlayed = 0;
    private float _inGameSecondsPlayed = 0;

    private const int HoursInDay = 24, MinutesInHour = 60;

    public float dayDuration = 30f;

    private float _totalTime = 0;
    private float _currentTime = 0;

    // Update is called once per frame
    void Update()
    {
        _totalTime += Time.deltaTime;
        _currentTime = _totalTime % dayDuration;
    }

    public float GetHour()
    {
        return _currentTime * HoursInDay / dayDuration;
    }

    public float GetMinutes()
    {
        return (_currentTime * HoursInDay * MinutesInHour / dayDuration) % MinutesInHour;
    }

    public void TestTime()
    {
        //DateTime currentTime = DateTime.Now;
        //float minutes = (float)currentTime.Minute;
        //float hours = (float)currentTime.Hour % 12;
        
        if (_deltaTime <= SecondsToPlay)
        {
            _deltaTime += Time.fixedDeltaTime;
            _deltaTimePlayHour += Time.fixedDeltaTime;
            _deltaTimePlaySeconds += Time.fixedDeltaTime;
        }
        if (_deltaTimePlayHour >= OnePlayHour)
        {
            _inGameHoursPlayed++;
            _deltaTimePlayHour = 0.0f;
        }
        if (_deltaTimePlaySeconds >= FivePlayMinutes)
        {
            _inGameSecondsPlayed += 1;
            _deltaTimePlaySeconds = 0.0f;
        }

        float minuteAngle = 360 * (_inGameSecondsPlayed  / 60.0f);

        float hoursAngle = 360 * ((6.0f / 12.0f) + (_inGameHoursPlayed / 12.0f));
        minuteHand.localRotation = Quaternion.Euler(0,0,minuteAngle);
        hourHand.localRotation = Quaternion.Euler(0,0,hoursAngle);

        
        Debug.Log(_deltaTime.ToString("N0"));
    }
}
