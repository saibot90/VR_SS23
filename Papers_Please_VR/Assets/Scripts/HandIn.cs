using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandIn : MonoBehaviour
{
    bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PassPort") //Pass und Visa muessen in Trigger liegen
        {
            StartCoroutine(startCountdownForHandIn());
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PassPort")
        {
            StopCoroutine(startCountdownForHandIn());
        }
    }

    IEnumerator startCountdownForHandIn()
    {
        if (!isActive) {
            isActive = true;
            yield return new WaitForSeconds(5);
            Debug.Log("Do he be waiting");
            GameEvents.current.TriggerPassBack();
        }
        
    }
}
