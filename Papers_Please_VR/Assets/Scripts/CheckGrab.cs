using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGrab : MonoBehaviour
{
    [SerializeField] private new string tag;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TagToTag()
    {
        transform.gameObject.tag = tag;
    }
    
    public void TagToUntagged()
    {
        transform.gameObject.tag = "Untagged";
    }
}
