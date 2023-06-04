using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;

public class Clock : MonoBehaviour
{
    public Transform minuteHand;
    public Transform hourHand;

    // Update is called once per frame
    void Update()
    {
        DateTime currentTime = DateTime.Now;
        float minutes = (float)currentTime.Minute;
        float hours = (float)currentTime.Hour % 12;

        float minuteAngle = 360 * (minutes  / 60);
        float hoursAngle = 360 * (hours / 12);
        
        minuteHand.localRotation = Quaternion.Euler(0,0,minuteAngle);
        hourHand.localRotation = Quaternion.Euler(0,0,hoursAngle);
    }
}
