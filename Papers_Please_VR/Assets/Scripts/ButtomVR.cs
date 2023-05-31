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
    
    // Start is called before the first frame update
    void Start()
    {
        _isPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isPressed)
        {
            button.transform.localPosition = new Vector3(0, 0.003f, 0);
            _presser = other.gameObject;
            onPress.Invoke();
            _isPressed = true;
            Debug.Log("Pressed");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _presser)
        {
            button.transform.localPosition = new Vector3(0, 0.015f, 0);
            onRelease.Invoke();
            _isPressed = false;
            Debug.Log("Depressed");
        }
    }

    public void SpawnSphere()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        sphere.transform.localPosition = new Vector3(0, 1, 2);
        sphere.AddComponent<Rigidbody>();
    }
}
//Change VR Hands Layer to Hands