using System.Collections;
using UnityEngine;

public class HandIn : MonoBehaviour
{
    private bool _isActive = false;

    private Coroutine _co;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onSpawnNewPerson += ResetHandIn;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PassPort")) //Pass und Visa muessen in Trigger liegen
        {
            Debug.Log("Entered Box");
            _co = StartCoroutine(StartCountdownForHandIn());
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PassPort"))
        {
            Debug.Log("Left Box");
            StopCoroutine(_co);
            _isActive = false;
        }
    }

    private IEnumerator StartCountdownForHandIn()
    {
        if (!_isActive) {
            _isActive = true;
            yield return new WaitForSeconds(4.9f);
            GameEvents.current.TriggerVisaCheck();
            yield return new WaitForSeconds(0.1f);
            Debug.Log("Do he be waiting");
            GameEvents.current.TriggerPassBack();
        }
        
    }

    private void ResetHandIn()
    {
        _isActive = false;
    }
}
