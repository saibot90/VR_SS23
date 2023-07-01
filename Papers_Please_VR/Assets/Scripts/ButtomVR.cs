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
    private bool _pressedOnce = false;
    
    // Start is called before the first frame update
    private void Start()
    {
        _isPressed = false;
        GameEvents.current.onNewPersonSpawned += FindPerson;
    }

    private void FindPerson(Person person)
    {
        _person = person;
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

    public void WantedPressed()
    {
        if (_person.PersonPresent())
        {
            if (_pressedOnce) return;
            GameEvents.current.TriggerPassBack2();
            _pressedOnce = true;
        }
        else _pressedOnce = false;
    }
}
//Change VR Hands Layer to Hands
