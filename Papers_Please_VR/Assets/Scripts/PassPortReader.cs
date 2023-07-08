using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassPortReader : MonoBehaviour
{
    /// <summary>
    /// Checks if the pass is placed inside of it.
    /// If hit then it triggers a game event for the computer to display the info
    /// </summary>
    /// <param name="other"> object that collided with the trigger box</param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PassPort"))
        {
            GameEvents.current.TriggerInfo();
        }
        
    }

    /// <summary>
    /// Checks if the pass is removed from the reader.
    /// Then triggers game event to display a empty pass info
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("PassPort"))
        {
            GameEvents.current.Info(new PassPortData());
        }
    }
}
