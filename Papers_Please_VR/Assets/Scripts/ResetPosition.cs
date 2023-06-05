using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ResetPosition : MonoBehaviour
{
    [SerializeField] private GameObject resetPoint;
    [SerializeField] private string resetTag;
    private static Vector3 _offset = new Vector3(0, 0, 0);
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(resetTag)) return;
        other.GetComponent<Rigidbody>().position = Vector3.zero;
        other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        other.transform.position = resetPoint.transform.position + _offset;
        SetOffset();
    }

    private static void SetOffset()
    {
        _offset.z += 0.2f;
        if (_offset.z > 1.2)
        {
            _offset.z = 0;
        }
    }
}
