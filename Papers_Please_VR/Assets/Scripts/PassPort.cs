using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEditor;
using UnityEngine;
using System.IO;


public class Name
{
    public string name;
}

public class Names
{
    public Name[] nameList;
}
public class LastName
{
    public string name;
}

public class LastNames
{
    public LastName[] lastNameList;
}

public class PassPort : MonoBehaviour
{
    [SerializeField] TextMeshPro m_TextMeshPro;
    [SerializeField] GameObject m_Picture;
    string passName;
    int passAge;
    Object [] m_Faces;
    string textDataName = "";

    void Start()
    {
        GameEvents.current.onTriggerInfo += ReaderHit;
        m_Faces = Resources.LoadAll("Faces", typeof(Renderer));
        passName = "Guenther";
        passAge = 32;
        m_TextMeshPro.text = "Name " + passName + "<br><br>";
        m_TextMeshPro.text += "Alter " + passAge;
        Debug.Log(m_Faces.Length);
        //m_Picture.GetComponent<Renderer>().material = (Material) m_Faces[0]; 

        
    }

    void loadJson()
    {
        string datapath = Application.dataPath + "/Resources/" + textDataName;
        Names peopleInfo;
        if (File.Exists(datapath))
        {
            string fileContents = File.ReadAllText(datapath);
            peopleInfo = JsonUtility.FromJson<Names>(fileContents);

            foreach (Name p in peopleInfo.nameList)
            {
                
            }
        }
        else
        {
            //text.text = "Cannot Read PlanetList";
        }
    }

    void ReaderHit()
    {
        GameEvents.current.Info(passName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
