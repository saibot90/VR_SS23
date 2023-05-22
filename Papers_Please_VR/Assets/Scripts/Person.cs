using System;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Person : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    [SerializeField] private Transform[] movePositionTransform;
    [SerializeField] private Transform passPosition;
    [SerializeField] private Transform visaPosition;
    [SerializeField] private GameObject passPort;
    [SerializeField] private GameObject visa;
    private int _index;
    private bool _isActive;
    private Vector3 _passPortStart = Vector3.zero;
    private Vector3 _visaStart = Vector3.zero;
    private Vector3 _passPortStop = Vector3.zero;
    private Vector3 _visaStop = Vector3.zero;
    private GameObject _passPortObject;
    private GameObject _visaObject;
    private bool _gotBack = false;

    private const double Tolerance = 0.5;
    
    private Quaternion _from;
    private Quaternion _to;

    private const float SpeedTurning = 1f;
    private float _timeCount;

    // Movement speed in units per second.
    public float speed = 1.0F;

    // Time when the movement started.
    private float _startTime;

    // Total distance between the markers.
    private float _journeyLengthPass;
    private float _journeyLengthVisa;
    private bool _isPassPortObjectNotNull;
    private bool _passPortObjectNotNull;
    private bool _isVisaObjectNotNull;
    private bool _portObjectNotNull;
    private bool _visaObjectNotNull;
    private bool _objectNotNull;

    private void Start()
    {
        GameEvents.current.onTriggerPassBack += GetPassBack;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _passPortStop = passPosition.position;
        _visaStop = visaPosition.position;
        _from = transform.rotation;
        _to = Quaternion.Euler(0,90,0);
    }

    private void Update()
    {
        _navMeshAgent.destination = movePositionTransform[_index].position;


        if (_index is 1 or 2)
        {
            transform.rotation = Quaternion.Lerp(_from, _to, _timeCount * SpeedTurning);
            _timeCount = _timeCount + Time.deltaTime;
        }

        
        if (Math.Abs(transform.position.x - movePositionTransform[_index].position.x) < Tolerance && Math.Abs(transform.position.z - movePositionTransform[_index].position.z) < Tolerance)
        {
            

            if (_index == (movePositionTransform.Length - 2))
            {
                SpawnPass();

                // Distance moved equals elapsed time times speed..
                float distCovered = (Time.time - _startTime) * speed;

                // Fraction of journey completed equals current distance divided by total distance.
                var fractionOfJourneyPass = distCovered / _journeyLengthPass;
                var fractionOfJourneyVisa = distCovered / _journeyLengthVisa;
                if(_passPortObject != null && _passPortObject.GetComponentInChildren<Rigidbody>().useGravity == false) 
                {
                    _passPortObject.transform.position = Vector3.Lerp(_passPortStart, _passPortStop, fractionOfJourneyPass);

                }
                
                if(_visaObject != null && _visaObject.GetComponent<Rigidbody>().useGravity == false)
                {
                    _visaObject.transform.position = Vector3.Lerp(_visaStart, _visaStop, fractionOfJourneyVisa);
                }
                
                if (_passPortObject != null && _passPortObject.transform.position == passPosition.position)
                {
                    _passPortObject.GetComponentInChildren<Rigidbody>().useGravity = true;
                }

                if (_visaObject != null && _visaObject.transform.position == visaPosition.position)
                {
                    _visaObject.GetComponent<Rigidbody>().useGravity = true;
                }

                if (_passPortObject != null && (_passPortObject.transform.position == _passPortStop) && _gotBack)
                {
                    Destroy(_passPortObject);
                }

                if (_visaObject != null && (_visaObject.transform.position == _visaStop) && _gotBack)
                {
                    Destroy(_visaObject);
                    _index = (movePositionTransform.Length - 1);
                    _from = transform.rotation;
                    _timeCount = 0.0f;
                    _to = Quaternion.Euler(0,180,0);
                    transform.rotation = Quaternion.Lerp(_from, _to, _timeCount * SpeedTurning);
                    _timeCount = _timeCount + Time.deltaTime;
                }
            }
            else
            {
                if (_index == (movePositionTransform.Length - 1))
                {
                    GameEvents.current.SpawnNewPerson();
                    Destroy(gameObject);
                }
                else _index++;
                
            }
        }

    }

    private void SpawnPass()
    {
        if(!_isActive)
        {
            _isActive = true;
            passPort.GetComponentInChildren<Rigidbody>().useGravity = false;
            visa.GetComponent<Rigidbody>().useGravity = false;
            var transform1 = transform;
            var position1 = transform1.position;
            Vector3 pos = new Vector3(position1.x, 0.914f, position1.z);
            _passPortObject = Instantiate(passPort, pos + new Vector3 (0, 0, 0.2f), Quaternion.Euler(90, 90, 0));
            _visaObject = Instantiate(visa, pos - new Vector3(0, 0, 0.2f), Quaternion.Euler(-90, 90, 0));
            _passPortStart = _passPortObject.transform.position;
            _visaStart = _visaObject.transform.position;
            // Keep a note of the time the movement started.
            _startTime = Time.time;

            // Calculate the journey length.
            var position = passPosition.position;
            _journeyLengthPass = Vector3.Distance(_passPortStart, position);
            _journeyLengthVisa = Vector3.Distance(_passPortStart, position);

        }
    }

    private void GetPassBack()
    {

        _passPortStart = _passPortObject.transform.position;
        _visaStart = _visaObject.transform.position;
        var transform1 = transform;
        var position = transform1.position;
        _passPortStop = new Vector3 (position.x, position.y, position.z + 0.2f);
        _visaStop = new Vector3(position.x, position.y, position.z - 0.2f);
        _passPortObject.GetComponentInChildren<Rigidbody>().useGravity = false;
        _visaObject.GetComponent<Rigidbody>().useGravity = false;
        _gotBack = true;
        _startTime = Time.time;
        //deactivate grabbable Object
    }

    public GameObject GetPassPort() { return _passPortObject; }
    public GameObject GetVisa() {return _visaObject; }

    private void OnDestroy()
    {
        GameEvents.current.onTriggerPassBack -= GetPassBack;
    }
}
