using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshPro m_TextMeshPro;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onInfo += displayInfo;
    }

    void displayInfo(string Info)
    {
        m_TextMeshPro.text = Info;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        GameEvents.current.onInfo -= displayInfo;
    }
}
