using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PassPort : MonoBehaviour
{
    [SerializeField] TextMeshPro m_TextMeshPro;
    [SerializeField] GameObject m_Picture;
    string passName;
    int passAge;
    Object [] m_Faces;
    // Start is called before the first frame update
    void Start()
    {
        m_Faces = Resources.LoadAll("Faces", typeof(Renderer));
        passName = "Guenther";
        passAge = 32;
        m_TextMeshPro.text = "Name " + passName + "<br><br>";
        m_TextMeshPro.text += "Alter " + passAge;
        Debug.Log(m_Faces.Length);
        //m_Picture.GetComponent<Renderer>().material = (Material) m_Faces[0]; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
