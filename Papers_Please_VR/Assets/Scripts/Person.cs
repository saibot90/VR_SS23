using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Person : MonoBehaviour
{
    private NavMeshAgent navMeshAgend;
    [SerializeField] private Transform[] movePositionTransform;
    [SerializeField] private Transform passPosition;
    [SerializeField] private Transform visaPosition;
    [SerializeField] private GameObject passPort;
    [SerializeField] private GameObject visa;
    private int index = 0;
    Boolean isActive = false;
    Vector3 passPortStart = Vector3.zero;
    Vector3 visaStart = Vector3.zero;
    Vector3 passPortStop = Vector3.zero;
    Vector3 visaStop = Vector3.zero;
    GameObject passPortObject;
    GameObject visaObject;
    bool gotBack = false;


    // Movement speed in units per second.
    public float speed = 1.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLengthPass;
    private float journeyLengthVisa;

    private void Awake()
    {
        GameEvents.current.onTriggerPassBack += getPassBack;
        navMeshAgend = GetComponent<NavMeshAgent>();
        passPortStop = passPosition.position;
        visaStop = visaPosition.position;
    }

    private void Update()
    {
        navMeshAgend.destination = movePositionTransform[index].position;
        
        if (transform.position.x == movePositionTransform[index].position.x && transform.position.z == movePositionTransform[index].position.z)
        {
            if (index == 0)
            {
                spawnPass();

                // Distance moved equals elapsed time times speed..
                float distCovered = (Time.time - startTime) * speed;

                // Fraction of journey completed equals current distance divided by total distance.
                float fractionOfJourneyPass = distCovered / journeyLengthPass;
                float fractionOfJourneyVisa = distCovered / journeyLengthVisa;
                if(passPortObject != null && passPortObject.GetComponentInChildren<Rigidbody>().useGravity == false) 
                {
                    passPortObject.transform.position = Vector3.Lerp(passPortStart, passPortStop, fractionOfJourneyPass);
                }
                
                if(visaObject != null && visaObject.GetComponent<Rigidbody>().useGravity == false)
                {
                    visaObject.transform.position = Vector3.Lerp(visaStart, visaStop, fractionOfJourneyVisa);
                }
                

                if (passPortObject != null && passPortObject.transform.position == passPosition.position)
                {
                    passPortObject.GetComponentInChildren<Rigidbody>().useGravity = true;
                }

                if (visaObject != null && visaObject.transform.position == visaPosition.position)
                {
                    visaObject.GetComponent<Rigidbody>().useGravity = true;
                }

                if (passPortObject != null && (passPortObject.transform.position == passPortStop) && gotBack)
                {
                    Destroy(passPortObject);
                }

                if (visaObject != null && (visaObject.transform.position == visaStop) && gotBack)
                {
                    Destroy(visaObject);
                    index++;
                }
            }
            else
            {
                GameEvents.current.SpawnNewPerson();
                Destroy(gameObject);
            }
        }

    }

    void nextPerson()
    {
        index++;
    }

    void spawnPass()
    {
        if(!isActive)
        {
            isActive = true;
            passPort.GetComponentInChildren<Rigidbody>().useGravity = false;
            visa.GetComponent<Rigidbody>().useGravity = false;
            Vector3 pos = new Vector3(transform.position.x, 0.914f, transform.position.z);
            passPortObject = Instantiate(passPort, pos + new Vector3 (0, 0, 0.2f), Quaternion.Euler(90, 90, 0));
            visaObject = Instantiate(visa, pos - new Vector3(0, 0, 0.2f), Quaternion.Euler(-90, 90, 0));
            passPortStart = passPortObject.transform.position;
            visaStart = visaObject.transform.position;
            // Keep a note of the time the movement started.
            startTime = Time.time;

            // Calculate the journey length.
            journeyLengthPass = Vector3.Distance(passPortStart, passPosition.position);
            journeyLengthVisa = Vector3.Distance(passPortStart, passPosition.position);

        }
    }

    void getPassBack()
    {
        passPortStart = passPortObject.transform.position;
        visaStart = visaObject.transform.position;
        passPortStop = new Vector3 (transform.position.x, transform.position.y, transform.position.z + 0.2f);
        visaStop = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.2f);
        passPortObject.GetComponentInChildren<Rigidbody>().useGravity = false;
        visaObject.GetComponent<Rigidbody>().useGravity = false;
        gotBack = true;
        startTime = Time.time;
        //deactivate grabbable Object
    }

    public GameObject getPassPort() { return passPortObject; }
    public GameObject getVisa() {return visaObject; }
}
