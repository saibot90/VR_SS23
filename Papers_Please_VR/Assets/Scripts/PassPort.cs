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
    Material _mFaces;
    public static int MFaceIndex;
    private int _lastWantedFace;
    private bool _wantedUsed;
    private bool _mIncorrectFace = false;
    
    //for loading the json text file
    string textDataName = "NameList2.json";//"NameList.json";
    string datapath;

    void Start()
    {
        //Subscribe to events
        GameEvents.current.onTriggerInfo += ReaderHit;
        GameEvents.current.onTriggerPassBack += getPassInfo;
        
        //Taking the face of the person so they are the same
        MFaceIndex = Person.mFaceIndex;
        
        
        //initiating the pass data with correct data
        int correctPass = Random.Range(1, 101);
        expirationDate = new Vector3Int(Random.Range(1, 30), Random.Range(1, 13), Random.Range(2024, 2040));
        dateOfcreation = new Vector3Int(Random.Range(1, 29), Random.Range(1, 13), Random.Range(2015, 2023));
        dateOfBirth = new Vector3Int(Random.Range(1, 29), Random.Range(1, 12), Random.Range(1920, 2023));
        country = (PassPortData.Countries) Random.Range(1, (int) Enum.GetValues(typeof(PassPortData.Countries)).Cast<PassPortData.Countries>().Max() + 1);
        int temp = Random.Range(1, (int) Enum.GetValues(typeof(PassPortData.PassportTypes)).Cast<PassPortData.PassportTypes>().Max() + 1);
        passType = (PassPortData.PassportTypes)temp;
        passColor = (PassPortData.PassportColor)temp;
        
        //overwriting pass data with incorrect data if pass is wrong
        switch (correctPass)
        {
            case <82:
                break;
            case >=82 and <=84:
                while (MFaceIndex == Person.mFaceIndex)
                {
                    MFaceIndex = Random.Range(1, 33);
                    _mIncorrectFace = true;
                }
                break;
            case <=87: expirationDate = new Vector3Int(Random.Range(1, 31), Random.Range(1, 13), Random.Range(2000, 2022));
                break;
            case <=90: dateOfcreation = new Vector3Int(Random.Range(1, 31), Random.Range(1, 13), Random.Range(1800, 3020));
                break;
            case <=93: dateOfBirth = new Vector3Int(Random.Range(1, 31), Random.Range(1, 13), Random.Range(1800, 3020));
                break;
            case <=96: country = (PassPortData.Countries) Random.Range(1, (int) Enum.GetValues(typeof(PassPortData.Countries)).Cast<PassPortData.Countries>().Max() + 1);
                break;
            case <=100: passType = (PassPortData.PassportTypes)Random.Range(1, (int) Enum.GetValues(typeof(PassPortData.PassportTypes)).Cast<PassPortData.PassportTypes>().Max() + 1);
                passColor = (PassPortData.PassportColor)Random.Range(1, (int) Enum.GetValues(typeof(PassPortData.PassportColor)).Cast<PassPortData.PassportColor>().Max() + 1);
                break;
        }
        
        //
        string facePath = "Faces/face" + MFaceIndex;
        _mFaces = Resources.Load(facePath) as Material;
        m_Picture.GetComponent<Renderer>().material = _mFaces; 
        datapath = Application.dataPath + "/Resources/" + textDataName;

        //changing backside color to the right pass color
        var backSideRenderer = m_BackSide.GetComponent<Renderer>();
        switch (passColor)
        {
                case PassPortData.PassportColor.Red:
                    backSideRenderer.material.color = new Color(0.5188679f,0.002447496f,0.002447496f,1);
                    break;
                case PassPortData.PassportColor.Brown:  
                    backSideRenderer.material.color = new Color(0.7547169f,0.359389f,0,1);
                    break;
                case PassPortData.PassportColor.Green:
                    backSideRenderer.material.color = new Color(0,0.245283f,0.02102427f,1);
                    break;
                case PassPortData.PassportColor.LightRed:
                    backSideRenderer.material.color = new Color(1,0,0,1);
                    break;
                case PassPortData.PassportColor.Blue:
                    backSideRenderer.material.color = new Color(0,0.05400867f,0.4716981f,1);
                    break;
                default: backSideRenderer.material.color =  new Color(0.5188679f,0.002447496f,0.002447496f,1);
                    break;
        }
        
        //loading names from a json text file
        if (File.Exists(datapath))
        {
            string fileContents = File.ReadAllText(datapath);
            Names names = JsonUtility.FromJson<Names>(fileContents);
            LastNames lastNames = JsonUtility.FromJson<LastNames>(fileContents);
            
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
        m_TextMeshPro.text += "LastName " + passLastName + "<br><br>" + "Name " + passName + "<br><br>";
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
        GameEvents.current.Info(new PassPortData(country, passName, passLastName, expirationDate, dateOfcreation, dateOfBirth, passType, passColor, wanted, _mIncorrectFace));
    }

    /// <summary>
    /// Sending data to game manager script to be checked 
    /// </summary>
    void getPassInfo()
    {
        GameEvents.current.TriggerPassCheck(new PassPortData(country, passName, passLastName, expirationDate, dateOfcreation, dateOfBirth, passType, passColor, wanted, _mIncorrectFace));
    }

    //Unsubscribe from events
    private void OnDestroy()
    {
        GameEvents.current.onTriggerInfo -= ReaderHit;
        GameEvents.current.onTriggerPassBack -= getPassInfo;
    }
}
