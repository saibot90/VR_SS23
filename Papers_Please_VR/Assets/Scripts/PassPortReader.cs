using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassPortReader : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PassPort")
        {
            GameEvents.current.TriggerInfo();
        }
        
    }
}
