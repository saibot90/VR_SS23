using System;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Person : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    
    //Different gameobjects used by the script
    [SerializeField] private Transform[] movePositionTransform;
    [SerializeField] private Transform passPosition;
    [SerializeField] private Transform visaPosition;
    [SerializeField] private GameObject passPort;
    [SerializeField] private GameObject visa;
    [SerializeField] GameObject m_Picture;
    
    //index for at what point of the movement the person is
    private int _index;
    private bool _isActive;
    
    //Start and stop for pass and visa to be flying from and to
    private Vector3 _passPortStart = Vector3.zero;
    private Vector3 _visaStart = Vector3.zero;
    private Vector3 _passPortStop = Vector3.zero;
    private Vector3 _visaStop = Vector3.zero;
    
    //Used to hold the instantiated pass and visa
    private GameObject _passPortObject;
    private GameObject _visaObject;
    
    private bool _gotBack = false;

    private int _faceNumber;

    //Tolerance for the movement of the person because sometimes it doesn't hit the position perfectly
    private const double Tolerance = 0.5;
    
    //for turning of the person (nav mesh turning didnt work the way we wanted it so we had to do it on our own)
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

    private bool _reachedPositionPassport = false;
    private bool _reachedPositionVisa = false;
    
    
    Material m_Faces;
    public static int mFaceIndex;

    private void Start()
    {
        //Subscribes to game events
        GameEvents.current.onTriggerPassBack += GetPassBack;
        GameEvents.current.onTriggerPassBack2 += GetPassBack;
        
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _passPortStop = passPosition.position;
        _visaStop = visaPosition.position;
        _from = transform.rotation;
        _to = Quaternion.Euler(0,90,0);
        
        //Loading a random face material for the face
        mFaceIndex = UnityEngine.Random.Range(1,33);
        string facePath = "Faces/face" + mFaceIndex;
        m_Faces = Resources.Load(facePath) as Material;
        m_Picture.GetComponent<Renderer>().material = m_Faces;
    }

    private void Update()
    {
        //Using the nav mesh from unity we let the person run over certain position to create our own movement
        _navMeshAgent.destination = movePositionTransform[_index].position;

        //Turning of the person because the automatic turning of the person didn't work as we wanted
        if (_index is 1 or 2)
        {
            transform.rotation = Quaternion.Lerp(_from, _to, _timeCount * SpeedTurning);
            _timeCount = _timeCount + Time.deltaTime;
        }

        //if person hits on one of the defined positions
        if (Math.Abs(transform.position.x - movePositionTransform[_index].position.x) < Tolerance && Math.Abs(transform.position.z - movePositionTransform[_index].position.z) < Tolerance)
        {
            
            //if the person stand directly in front of the glass
            if (_index == (movePositionTransform.Length - 2))
            {
                SpawnPass();

                // Distance moved equals elapsed time times speed..
                float distCovered = (Time.time - _startTime) * speed;

                // Fraction of journey completed equals current distance divided by total distance.
                var fractionOfJourneyPass = distCovered / _journeyLengthPass;
                var fractionOfJourneyVisa = distCovered / _journeyLengthVisa;
                
                //moves pass and visa from a start position to a destination when the pass/visa doesn't use gravity
                if(_passPortObject != null && _passPortObject.GetComponent<Rigidbody>().useGravity == false 
                                           && !_reachedPositionPassport) 
                {
                    _passPortObject.transform.position = Vector3.Lerp(_passPortStart, _passPortStop, fractionOfJourneyPass);
                }
                
                if(_visaObject != null && _visaObject.GetComponent<Rigidbody>().useGravity == false 
                                       && !_reachedPositionVisa)
                {
                    _visaObject.transform.position = Vector3.Lerp(_visaStart, _visaStop, fractionOfJourneyVisa);
                }
                
                //if pass and visa have reached their destination turn on the gravity
                if (_passPortObject != null && _passPortObject.transform.position == passPosition.position)
                {
                    _passPortObject.GetComponent<Rigidbody>().useGravity = true;
                    _reachedPositionPassport = true;
                }

                if (_visaObject != null && _visaObject.transform.position == visaPosition.position)
                {
                    _visaObject.GetComponent<Rigidbody>().useGravity = true;
                    _reachedPositionVisa = true;
                }
                
                //if the pass and visa are given back to the person destroy them and the person moves on including turning back to moving direction
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
                //if the person reached the last position spawn a new person and delete the old one
                if (_index == (movePositionTransform.Length - 1))
                {
                    GameEvents.current.SpawnNewPerson();
                    Destroy(gameObject);
                }
                else _index++;
                
            }
        }

    }

    /// <summary>
    /// Spawns a pass and visa then sets variables for them to fly from the person to the desk
    /// </summary>
    private void SpawnPass()
    {
        if(!_isActive)
        {
            _isActive = true;
            passPort.GetComponent<Rigidbody>().useGravity = false;
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

    /// <summary>
    /// Sets variables for pass and visa so they fly back from the desk to the person
    /// </summary>
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
        _reachedPositionPassport = false;
        _reachedPositionVisa = false;
        //deactivate grabbable Object
    }

    /// <summary>
    /// Bool for if the person stands directly in front of the glass
    /// </summary>
    /// <returns>bool if the person is in front</returns>
    public bool PersonPresent()
    {
        return _index == (movePositionTransform.Length - 2);
    }

    public GameObject GetPassPort() { return _passPortObject; }
    public GameObject GetVisa() {return _visaObject; }
    
    public int GetFaceofPerson() {return _faceNumber; }

    //Unsubscribe from game events
    private void OnDestroy()
    {
        GameEvents.current.onTriggerPassBack -= GetPassBack;
        GameEvents.current.onTriggerPassBack2 -= GetPassBack;
    }
}
