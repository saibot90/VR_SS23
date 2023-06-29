using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ButtomVR : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    private GameObject _presser;
    private bool _isPressed;
    private Person _person;
    
    // Start is called before the first frame update
    void Start()
    {
        _isPressed = false;
        GameEvents.current.onSpawnNewPerson += FindPerson;
    }

    private void FindPerson()
    {
        _person = GameObject.FindObjectOfType<Person>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isPressed)
        {
            button.transform.localPosition = new Vector3(0, 0.003f, 0);
            _presser = other.gameObject;
            onPress.Invoke();
            _isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _presser)
        {
            button.transform.localPosition = new Vector3(0, 0.015f, 0);
            onRelease.Invoke();
            _isPressed = false;
        }
    }

    public void SpawnSphere()//ToDo: rename
    {
        if (_person.PersonPresent())
        {
            GameEvents.current.TriggerPassBack2();
        }  
    }
}
//Change VR Hands Layer to Hands
