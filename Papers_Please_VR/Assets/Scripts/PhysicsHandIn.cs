using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Template.VR;
using UnityEngine.Serialization;

public class PhysicsHandIn : MonoBehaviour
{
    public LayerMask mLayerMask;
    private bool _isActive = false;

    private Coroutine _co;

    // Start is called before the first frame update
    private void Start()
    {
        GameEvents.current.onSpawnNewPerson += ResetHandIn;
        GameEvents.current.onVisaStatus += VisaNotReady;

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        MyCollisions();
    }

    private void MyCollisions()
    {
        //Use the OverlapBox to detect if there are any other colliders within this box area.
        //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2,
            Quaternion.identity, mLayerMask);
        
        if (hitColliders.Length == 2 )
        {
            if (!_isActive)
            {
                _co = StartCoroutine(StartCountdownForHandIn());
            }
        }
        else
        {
            if (_co != null) 
            {
                StopCoroutine(_co);
                _isActive = false;
            }
        }
        
    }

    private void VisaNotReady(CheckStatus ready)
    {
        if (ready == CheckStatus.None && _co != null)
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
            yield return new WaitForSeconds(0.3f);
            GameEvents.current.TriggerPassBack();
        }
        
    }

    private void ResetHandIn()
    {
        _isActive = false;
    }

}
