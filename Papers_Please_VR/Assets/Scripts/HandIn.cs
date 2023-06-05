using System.Collections;
using Unity.Template.VR;
using UnityEngine;

public class HandIn : MonoBehaviour
{
    private bool _isActive = false;

    private Coroutine _co;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onSpawnNewPerson += ResetHandIn;
        GameEvents.current.onVisaStatus += VisaNotReady;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PassPort")) //Pass und Visa muessen in Trigger liegen
        {
            _co = StartCoroutine(StartCountdownForHandIn());
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PassPort"))
        {
            StopCoroutine(_co);
            _isActive = false;
        }
    }

    void VisaNotReady(CheckStatus ready)
    {
        if (ready == CheckStatus.None)
        {
            StopCoroutine(_co);
            _isActive = false; 
            //HandInBox color change? To indicate something is wrong?
        }
    }

    private IEnumerator StartCountdownForHandIn()
    {
        if (!_isActive) {
            _isActive = true;
            yield return new WaitForSeconds(4.9f);//TODO 3 Sekunden besser?
            GameEvents.current.TriggerVisaCheck();
            yield return new WaitForSeconds(0.1f);
            GameEvents.current.TriggerPassBack();
        }
        
    }

    private void ResetHandIn()
    {
        _isActive = false;
    }
}
