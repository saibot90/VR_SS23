using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using System.IO;
using Random = UnityEngine.Random;
//using System;

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
    PassPortData.Countries country;
    string passName;
    string passLastName;
    int passAge;
    Vector3Int expirationDate;
    Vector3Int dateOfcreation;
    Vector3Int dateOfBirth;
    PassPortData.PassportTypes passType;
    PassPortData.PassportColor passColor;
    //DateTime date = new DateTime(2000, 13, 1);
    #endregion

    [SerializeField] TextMeshPro m_TextMeshPro;
    [SerializeField] TextMeshPro backSideText;
    [SerializeField] GameObject m_Picture;
    
    Material m_Faces;
    string textDataName = "NameList2.json";//"NameList.json";
    string datapath;
    string passInfo;

    void Start()
    {
        GameEvents.current.onTriggerInfo += ReaderHit;
        GameEvents.current.onTriggerPassBack += getPassInfo;
        m_Faces = Resources.Load("Faces/face1") as Material;

        //passAge = Random.Range(5, 92);
        expirationDate = new Vector3Int(Random.Range(1, 30), Random.Range(1, 12), Random.Range(2015, 2035));
        dateOfcreation = new Vector3Int(Random.Range(1, 30), Random.Range(1, 12), Random.Range(2015, 2035));
        dateOfBirth = new Vector3Int(Random.Range(1, 30), Random.Range(1, 12), Random.Range(1920, 2035));
        country = (PassPortData.Countries) Random.Range(1, 3); //Anpassen wenn Laenderliste erweitert wird
        passType = (PassPortData.PassportTypes)Random.Range(1, 5);
        passColor = (PassPortData.PassportColor)Random.Range(1, 5);


        //Debug.Log(m_Faces.Length);
        m_Picture.GetComponent<Renderer>().material = m_Faces; 
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
        }
        else
        {
            Debug.Log("Cannot Read PlanetList");
        }

        m_TextMeshPro.text = "Type " + passType.ToString() + "<br><br>";
        m_TextMeshPro.text += "Name " + passLastName + "     Vorname " + passName + "<br><br>";
        m_TextMeshPro.text += "Geburtsdatum " + dateOfBirth + "<br><br>";
        m_TextMeshPro.text += "Ablaufdatum "+ expirationDate + "<br><br>" + "Erstelldatum " + dateOfcreation;
        passInfo = m_TextMeshPro.text;
        backSideText.text = country.ToString();
    }

    void ReaderHit()
    {
        GameEvents.current.Info(passInfo);
    }

    void getPassInfo()
    {
        GameEvents.current.TriggerPassCheck(new PassPortData(country, passName, passLastName, expirationDate, dateOfcreation, dateOfBirth, passType, passColor));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        GameEvents.current.onTriggerInfo -= ReaderHit;
        GameEvents.current.onTriggerPassBack -= getPassInfo;
    }
}
