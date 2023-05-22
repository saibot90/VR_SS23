using System.Collections;
using UnityEngine;

public class HandIn : MonoBehaviour
{
    private bool _isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onSpawnNewPerson += ResetHandIn;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PassPort")) //Pass und Visa muessen in Trigger liegen
        {
            StartCoroutine(StartCountdownForHandIn());
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PassPort"))
        {
            StopCoroutine(StartCountdownForHandIn());
        }
    }

    private IEnumerator StartCountdownForHandIn()
    {
        if (!_isActive) {
            _isActive = true;
            yield return new WaitForSeconds(5);
            Debug.Log("Do he be waiting");
            GameEvents.current.TriggerPassBack();
        }
        
    }

    private void ResetHandIn()
    {
        _isActive = false;
    }
}
