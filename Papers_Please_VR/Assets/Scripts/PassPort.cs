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

    private bool wanted = false;
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
        int correctPass = Random.Range(1, 100);
        
        switch (correctPass)
        {
            case <85:
                expirationDate = new Vector3Int(Random.Range(1, 29), Random.Range(1, 12), Random.Range(2024, 2040));
                dateOfcreation = new Vector3Int(Random.Range(1, 29), Random.Range(1, 12), Random.Range(2015, 2022));
                dateOfBirth = new Vector3Int(Random.Range(1, 29), Random.Range(1, 12), Random.Range(1920, 2022));
                country = (PassPortData.Countries) Random.Range(1, 3); //Anpassen wenn Laenderliste erweitert wird
                passType = (PassPortData.PassportTypes)Random.Range(1, 5);
                passColor = (PassPortData.PassportColor)Random.Range(1, 5);
                break;
            case >=85:
                expirationDate = new Vector3Int(Random.Range(1, 31), Random.Range(1, 13), Random.Range(2000, 2022));
                dateOfcreation = new Vector3Int(Random.Range(1, 31), Random.Range(1, 13), Random.Range(1100, 4022));
                dateOfBirth = new Vector3Int(Random.Range(1, 31), Random.Range(1, 13), Random.Range(1120, 4022));
                country = (PassPortData.Countries) Random.Range(1, 3); //Anpassen wenn Laenderliste erweitert wird
                passType = (PassPortData.PassportTypes)Random.Range(1, 5);
                passColor = (PassPortData.PassportColor)Random.Range(1, 5);
                break;
            default: break;
        }

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
            Debug.Log("Cannot Read");
        }

        m_TextMeshPro.text = "Type " + passType.ToString() + "<br><br>";
        m_TextMeshPro.text += "LastName " + passLastName + "     Name " + passName + "<br><br>";
        m_TextMeshPro.text += "Date of Birth " + dateOfBirth.x + "/" + dateOfBirth.y +"/" + dateOfBirth.z + "<br><br>";
        m_TextMeshPro.text += "Expires "+ expirationDate.x + "/" + expirationDate.y +"/" + expirationDate.z + "<br><br>";
        m_TextMeshPro.text += "Issued " + dateOfcreation.x + "/" + dateOfcreation.y +"/" + dateOfcreation.z;
        passInfo = m_TextMeshPro.text;
        backSideText.text = country.ToString();
    }

    void ReaderHit()
    {
        GameEvents.current.Info(passInfo);
    }

    void getPassInfo()
    {
        GameEvents.current.TriggerPassCheck(new PassPortData(country, passName, passLastName, expirationDate, dateOfcreation, dateOfBirth, passType, passColor, wanted));
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
