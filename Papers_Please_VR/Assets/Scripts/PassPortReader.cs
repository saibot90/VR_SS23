using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassPortReader : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PassPort"))
        {
            GameEvents.current.TriggerInfo();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("PassPort"))
        {
            GameEvents.current.Info(new PassPortData());
        }
    }
}
