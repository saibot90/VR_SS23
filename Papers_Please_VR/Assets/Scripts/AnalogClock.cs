using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Update the hand position of the clock
/// </summary>
public class AnalogClock : MonoBehaviour
{
    #region Variables
    
    private TimeManager _tm;
    
    [SerializeField] private Transform minuteHand;
    [SerializeField] private Transform hourHand;

    private const float HoursToDegree = 360.0f / 12.0f;
    private const float MinutesToDegree = 360.0f / 60.0f;
    
    #endregion
    
    #region Functions
    
    /// <summary>
    /// Is called before the first frame update
    /// </summary>
    void Start()
    {
        _tm = FindObjectOfType<TimeManager>();
    }

    /// <summary>
    /// Is called once per frame
    /// </summary>
    void Update()
    {
        hourHand.localRotation = Quaternion.Euler(0, 0, (_tm.GetHour() + 6.0f) * HoursToDegree);
        minuteHand.localRotation = Quaternion.Euler(0, 0, _tm.GetMinutes() * MinutesToDegree);
    }
    
    #endregion
}
