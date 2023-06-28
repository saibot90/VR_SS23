using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
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
    [SerializeField] private GameObject m_BackSide;
    
    Material m_Faces;
    public static int mFaceIndex;
    private int lastWantedFace;
    private bool wantedUsed;
    string textDataName = "NameList2.json";//"NameList.json";
    string datapath;
    string passInfo;

    void Start()
    {
        GameEvents.current.onTriggerInfo += ReaderHit;
        GameEvents.current.onTriggerPassBack += getPassInfo;
        mFaceIndex = Person.mFaceIndex;
        string facePath = "Faces/face" + mFaceIndex;
        m_Faces = Resources.Load(facePath) as Material;
        int correctPass = Random.Range(1, 100);

        switch (correctPass)
        {
            case <85:
                expirationDate = new Vector3Int(Random.Range(1, 29), Random.Range(1, 12), Random.Range(2024, 2040));
                dateOfcreation = new Vector3Int(Random.Range(1, 29), Random.Range(1, 12), Random.Range(2015, 2022));
                dateOfBirth = new Vector3Int(Random.Range(1, 29), Random.Range(1, 12), Random.Range(1920, 2022));
                country = (PassPortData.Countries) Random.Range(1, (int) Enum.GetValues(typeof(PassPortData.Countries)).Cast<PassPortData.Countries>().Max() + 1);
                int temp = Random.Range(1, (int) Enum.GetValues(typeof(PassPortData.PassportTypes)).Cast<PassPortData.PassportTypes>().Max() + 1);
                passType = (PassPortData.PassportTypes)temp;
                passColor = (PassPortData.PassportColor)temp; //Random.Range(1, (int) Enum.GetValues(typeof(PassPortData.PassportColor)).Cast<PassPortData.PassportColor>().Max() + 1)
                break;
            case >=85:
                expirationDate = new Vector3Int(Random.Range(1, 31), Random.Range(1, 13), Random.Range(2000, 2022));
                dateOfcreation = new Vector3Int(Random.Range(1, 31), Random.Range(1, 13), Random.Range(1100, 4022));
                dateOfBirth = new Vector3Int(Random.Range(1, 31), Random.Range(1, 13), Random.Range(1120, 4022));
                country = (PassPortData.Countries) Random.Range(1, (int) Enum.GetValues(typeof(PassPortData.Countries)).Cast<PassPortData.Countries>().Max() + 1);
                passType = (PassPortData.PassportTypes)Random.Range(1, (int) Enum.GetValues(typeof(PassPortData.PassportTypes)).Cast<PassPortData.PassportTypes>().Max() + 1);
                passColor = (PassPortData.PassportColor)Random.Range(1, (int) Enum.GetValues(typeof(PassPortData.PassportColor)).Cast<PassPortData.PassportColor>().Max() + 1);
                break;
        }

        //Debug.Log(m_Faces.Length);
        m_Picture.GetComponent<Renderer>().material = m_Faces; 
        datapath = Application.dataPath + "/Resources/" + textDataName;

        var backSideRenderer = m_BackSide.GetComponent<Renderer>();
        switch (passColor)
        {
                case PassPortData.PassportColor.Red:
                    backSideRenderer.material.SetColor("_Color", new Color(0.5660378f,0.01868994f,0.01868994f,1));
                    break;
                case PassPortData.PassportColor.Brown:
                    backSideRenderer.material.SetColor("_Color", new Color(0.3301887f,0.158204f,0,1));
                    break;
                case PassPortData.PassportColor.Green:
                    backSideRenderer.material.SetColor("_Color", new Color(0,0.245283f,0.02102427f,1));
                    break;
                case PassPortData.PassportColor.LightRed:
                    backSideRenderer.material.SetColor("_Color", new Color(0.8679245f,0,0,1));
                    break;
                case PassPortData.PassportColor.Blue:
                    backSideRenderer.material.SetColor("_Color", new Color(0,0.05400867f,0.4716981f,1));
                    break;
                default: backSideRenderer.material.SetColor("_Color", new Color(0.5660378f,0.01868994f,0.01868994f,1));
                    break;    
            
        }

        Names names;
        LastNames lastNames;

        if (File.Exists(datapath))
        {
            string fileContents = File.ReadAllText(datapath);
            names = JsonUtility.FromJson<Names>(fileContents);
            lastNames = JsonUtility.FromJson<LastNames>(fileContents);
            
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
        GameEvents.current.Info(new PassPortData(country, passName, passLastName, expirationDate, dateOfcreation, dateOfBirth, passType, passColor, wanted));
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
