using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Template.VR;

public class PhysicsHandIn : MonoBehaviour
{
    public LayerMask m_LayerMask;
    private bool _isActive = false;

    private Coroutine _co;

    private bool started = false; // TODO delete
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onSpawnNewPerson += ResetHandIn;
        GameEvents.current.onVisaStatus += VisaNotReady;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MyCollisions();
    }

    void MyCollisions()
    {
        //Use the OverlapBox to detect if there are any other colliders within this box area.
        //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2,
            Quaternion.identity, m_LayerMask);
        
        if (hitColliders.Length == 2 )
        {
            //if (!started)
            //{
            if (!_isActive)
            {
                _co = StartCoroutine(StartCountdownForHandIn());
            }
                
                //started = true;
            //}
            
        }
        else
        {
            //Debug.Log("sollte abbrechen");
            if (_co != null) //started && 
            {
                Debug.Log("stopped because not both");
                StopCoroutine(_co);
                _isActive = false;
                //started = false;
            }
        }
        
    }
    
    void VisaNotReady(CheckStatus ready)
    {
        Debug.Log(_co != null);
        Debug.Log(_co);
        
        Debug.Log("sollte abbrechen");
        if (ready == CheckStatus.None && _co != null)
        {
            Debug.Log("stopped because no Visa");
            StopCoroutine(_co);
            _isActive = false;
            //started = false;
            //HandInBox color change? To indicate something is wrong?
        }
    }
    
    private IEnumerator StartCountdownForHandIn()
    {
        if (!_isActive) {
            Debug.Log("started");
            _isActive = true;
            yield return new WaitForSeconds(4.9f);//TODO 3 Sekunden besser?
            GameEvents.current.TriggerVisaCheck();
            yield return new WaitForSeconds(0.3f);
            GameEvents.current.TriggerPassBack();
            //started = false;
        }
        
    }

    private void ResetHandIn()
    {
        _isActive = false;
    }

}
