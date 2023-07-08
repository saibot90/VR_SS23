using System.Collections;
using UnityEngine;
using Unity.Template.VR;

public class PhysicsHandIn : MonoBehaviour
{
    public LayerMask mLayerMask;
    private bool _isActive = false;

    //Coroutine so we can stop the same coroutine as we started
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

    /// <summary>
    /// Alternative to OnTriggerEnter because we needed to check for two objects at the same time.
    /// Uses physics to create a box which is not rendered that in turn can check every object that is overlapping with.
    /// We don't want every object so we just check a certain layer (it has only the two wanted objects in it).
    /// When both objects are in the defined area the coroutine is started and when one or both are taken out the coroutine is stopped again.
    /// </summary>
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

    /// <summary>
    /// Stops coroutine if the visa is not stamped
    /// </summary>
    /// <param name="ready"> status on if the visa is stamped</param>
    private void VisaNotReady(CheckStatus ready)
    {
        if (ready == CheckStatus.None && _co != null)
        {
            StopCoroutine(_co);
            _isActive = false;
        }
    }
    /// <summary>
    /// Starts coroutine with countdown befor pass and visa go back to the person
    /// </summary>
    /// <returns>IEnumerator (standard return value of a coroutine)</returns>
    private IEnumerator StartCountdownForHandIn()
    {
        if (!_isActive) {
            _isActive = true;
            yield return new WaitForSeconds(4.9f);
            GameEvents.current.TriggerVisaCheck();
            yield return new WaitForSeconds(0.3f);
            GameEvents.current.TriggerPassBack();
        }
        
    }

    /// <summary>
    /// Resetting bool for when the next person comes
    /// </summary>
    private void ResetHandIn()
    {
        _isActive = false;
    }

    /// <summary>
    /// Unsubscribe from game events
    /// </summary>
    private void OnDestroy()
    {
        GameEvents.current.onSpawnNewPerson -= ResetHandIn;
        GameEvents.current.onVisaStatus -= VisaNotReady;
    }
}
