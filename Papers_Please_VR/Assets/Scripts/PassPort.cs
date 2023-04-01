using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using System.IO;

#region Classes
[System.Serializable]
public class Name
{
    public string name;
}

public class Names
{
    public Name[] nameList;
}

[System.Serializable]
public class LastName
{
    public string name;
}

public class LastNames
{
    public LastName[] lastNameList;
}

[System.Serializable]
public class Country
{
    public string name;
}

public class Countrys
{
    public Country[] countryList;
}

[System.Serializable]
public class PassTyp
{
    public string name;
}

public class PassTypes
{
    public PassTyp[] passTypList;
}
#endregion

public class PassPort : MonoBehaviour
{
    #region PassInfo
    string country;
    string passName;
    string passLastName;
    int passAge;
    Vector3Int expirationDate;
    Vector3Int dateOfcreation;
    Vector3Int dateOfBirth;
    string passType;
    #endregion

    [SerializeField] TextMeshPro m_TextMeshPro;
    [SerializeField] TextMeshPro backSideText;
    [SerializeField] GameObject m_Picture;
    
    Object [] m_Faces;
    string textDataName = "NameList2.json";//"NameList.json";
    string datapath;
    string passInfo;

    void Start()
    {
        GameEvents.current.onTriggerInfo += ReaderHit;
        //m_Faces = Resources.LoadAll("Faces", typeof(Renderer));

        //passAge = Random.Range(5, 92);
        expirationDate = new Vector3Int(Random.Range(1, 30), Random.Range(1, 12), Random.Range(2015, 2035));
        dateOfcreation = new Vector3Int(Random.Range(1, 30), Random.Range(1, 12), Random.Range(2015, 2035));
        dateOfBirth = new Vector3Int(Random.Range(1, 30), Random.Range(1, 12), Random.Range(1920, 2035));

        //Debug.Log(m_Faces.Length);
        //m_Picture.GetComponent<Renderer>().material = (Material) m_Faces[0]; 
        datapath = Application.dataPath + "/Resources/" + textDataName;

        Names names;
        LastNames lastNames;
        Countrys countrys;
        PassTypes passTypes;

        if (File.Exists(datapath))
        {
            string fileContents = File.ReadAllText(datapath);
            names = JsonUtility.FromJson<Names>(fileContents);
            lastNames = JsonUtility.FromJson<LastNames>(fileContents);
            countrys = JsonUtility.FromJson<Countrys>(fileContents);
            passTypes = JsonUtility.FromJson<PassTypes>(fileContents);

            int rand = Random.Range(0, names.nameList.Length - 1);
            passName = names.nameList[rand].name;
            rand = Random.Range(0, lastNames.lastNameList.Length - 1);
            passLastName = lastNames.lastNameList[rand].name;
            rand = Random.Range(0, countrys.countryList.Length - 1);
            country = countrys.countryList[rand].name;
            rand = Random.Range(0, passTypes.passTypList.Length - 1);
            passType = passTypes.passTypList[rand].name;
        }
        else
        {
            Debug.Log("Cannot Read PlanetList");
        }

        m_TextMeshPro.text = "Type " + passType + "<br><br>";
        m_TextMeshPro.text += "Name " + passLastName + "     Vorname " + passName + "<br><br>";
        m_TextMeshPro.text += "Geburtsdatum " + dateOfBirth + "<br><br>";
        m_TextMeshPro.text += "Ablaufdatum "+ expirationDate + "<br><br>" + "Erstelldatum " + dateOfcreation;
        passInfo = m_TextMeshPro.text;
        backSideText.text = country;
    }

    void ReaderHit()
    {
        GameEvents.current.Info(passInfo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
