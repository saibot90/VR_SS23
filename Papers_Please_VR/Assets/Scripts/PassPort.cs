using System;
using TMPro;
using UnityEngine;
using System.IO;
using System.Linq;
using Random = UnityEngine.Random;

#region Classes
//used to load the names from the json text file
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
    //passport data
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
    #endregion

    //different gameobjects used by the script
    [SerializeField] TextMeshPro m_TextMeshPro;
    [SerializeField] TextMeshPro backSideText;
    [SerializeField] GameObject m_Picture;
    [SerializeField] private GameObject m_BackSide;
    
    //variables used for the picture (face)
    Material m_Faces;
    public static int mFaceIndex;
    private int lastWantedFace;
    private bool wantedUsed;
    
    //for loading the json text file
    string textDataName = "NameList2.json";//"NameList.json";
    string datapath;

    void Start()
    {
        //Subscribe to events
        GameEvents.current.onTriggerInfo += ReaderHit;
        GameEvents.current.onTriggerPassBack += getPassInfo;
        
        //Loading the face of the person to be same on the pass
        mFaceIndex = Person.mFaceIndex;
        string facePath = "Faces/face" + mFaceIndex;
        m_Faces = Resources.Load(facePath) as Material;
        m_Picture.GetComponent<Renderer>().material = m_Faces; 
        datapath = Application.dataPath + "/Resources/" + textDataName;
        
        //initiating the pass data with correct data
        int correctPass = Random.Range(1, 100);
        expirationDate = new Vector3Int(Random.Range(1, 29), Random.Range(1, 12), Random.Range(2024, 2040));
        dateOfcreation = new Vector3Int(Random.Range(1, 29), Random.Range(1, 12), Random.Range(2015, 2022));
        dateOfBirth = new Vector3Int(Random.Range(1, 29), Random.Range(1, 12), Random.Range(1920, 2022));
        country = (PassPortData.Countries) Random.Range(1, (int) Enum.GetValues(typeof(PassPortData.Countries)).Cast<PassPortData.Countries>().Max() + 1);
        int temp = Random.Range(1, (int) Enum.GetValues(typeof(PassPortData.PassportTypes)).Cast<PassPortData.PassportTypes>().Max() + 1);
        passType = (PassPortData.PassportTypes)temp;
        passColor = (PassPortData.PassportColor)temp;
        
        //overwriting pass data with incorrect data if pass is wrong
        switch (correctPass)
        {
            case <85:
                break;
            case >=85 and <=87: expirationDate = new Vector3Int(Random.Range(1, 31), Random.Range(1, 13), Random.Range(2000, 2022));
                break;
            case <=90: dateOfcreation = new Vector3Int(Random.Range(1, 31), Random.Range(1, 13), Random.Range(1100, 4022));
                break;
            case <=93: dateOfBirth = new Vector3Int(Random.Range(1, 31), Random.Range(1, 13), Random.Range(1120, 4022));
                break;
            case <=96: country = (PassPortData.Countries) Random.Range(1, (int) Enum.GetValues(typeof(PassPortData.Countries)).Cast<PassPortData.Countries>().Max() + 1);
                break;
            case <=100: passType = (PassPortData.PassportTypes)Random.Range(1, (int) Enum.GetValues(typeof(PassPortData.PassportTypes)).Cast<PassPortData.PassportTypes>().Max() + 1);
                passColor = (PassPortData.PassportColor)Random.Range(1, (int) Enum.GetValues(typeof(PassPortData.PassportColor)).Cast<PassPortData.PassportColor>().Max() + 1);
                break;
        }

        //changing backside color to the right pass color
        var backSideRenderer = m_BackSide.GetComponent<Renderer>();
        switch (passColor)
        {
                case PassPortData.PassportColor.Red:
                    backSideRenderer.material.color = new Color(0.5660378f,0.01868994f,0.01868994f,1);
                    break;
                case PassPortData.PassportColor.Brown:
                    backSideRenderer.material.color = new Color(0.3301887f,0.158204f,0,1);
                    break;
                case PassPortData.PassportColor.Green:
                    backSideRenderer.material.color = new Color(0,0.245283f,0.02102427f,1);
                    break;
                case PassPortData.PassportColor.LightRed:
                    backSideRenderer.material.color = new Color(0.8679245f,0,0,1);
                    break;
                case PassPortData.PassportColor.Blue:
                    backSideRenderer.material.color = new Color(0,0.05400867f,0.4716981f,1);
                    break;
                default: backSideRenderer.material.color =  new Color(0.5660378f,0.01868994f,0.01868994f,1);
                    break;
        }
        
        //loading names from a json text file
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

        //writing pass data to textfield on the pass
        m_TextMeshPro.text = "Type " + passType.ToString() + "<br><br>";
        m_TextMeshPro.text += "LastName " + passLastName + "     Name " + passName + "<br><br>";
        m_TextMeshPro.text += "Date of Birth " + dateOfBirth.x + "/" + dateOfBirth.y +"/" + dateOfBirth.z + "<br><br>";
        m_TextMeshPro.text += "Expires "+ expirationDate.x + "/" + expirationDate.y +"/" + expirationDate.z + "<br><br>";
        m_TextMeshPro.text += "Issued " + dateOfcreation.x + "/" + dateOfcreation.y +"/" + dateOfcreation.z;
        backSideText.text = country.ToString();
    }

    /// <summary>
    /// Sending the current pass data to the computer script to display it on the monitor
    /// </summary>
    void ReaderHit()
    {
        GameEvents.current.Info(new PassPortData(country, passName, passLastName, expirationDate, dateOfcreation, dateOfBirth, passType, passColor, wanted));
    }

    /// <summary>
    /// Sending data to game manager script to be checked 
    /// </summary>
    void getPassInfo()
    {
        GameEvents.current.TriggerPassCheck(new PassPortData(country, passName, passLastName, expirationDate, dateOfcreation, dateOfBirth, passType, passColor, wanted));
    }

    //Unsubscribe from events
    private void OnDestroy()
    {
        GameEvents.current.onTriggerInfo -= ReaderHit;
        GameEvents.current.onTriggerPassBack -= getPassInfo;
    }
}
