using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalogClock : MonoBehaviour
{
    private TimeManager _tm;
    
    public Transform minuteHand;
    public Transform hourHand;

    private const float HoursToDegree = 360.0f / 12.0f;
    private const float MinutesToDegree = 360.0f / 60.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        _tm = FindObjectOfType<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        hourHand.localRotation = Quaternion.Euler(0, 0, (_tm.GetHour() + 6.0f) * HoursToDegree);
        minuteHand.localRotation = Quaternion.Euler(0, 0, _tm.GetMinutes() * MinutesToDegree);
    }
}
