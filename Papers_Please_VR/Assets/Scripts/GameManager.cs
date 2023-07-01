using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using static PassPortData;
using System.Collections;
using Random = UnityEngine.Random;
using CheckStatus = Unity.Template.VR.CheckStatus;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Material glowCorrect;
    [SerializeField] private Material glowWrong;
    [SerializeField] private Material glowNone;
    [SerializeField] private GameObject checkLight;
    [SerializeField] private TextMeshPro ruleTableCountriesText;
    [SerializeField] private TextMeshPro ruleTableAdditionText;
    [SerializeField] private TextMeshPro totalScoreText;
    [SerializeField] private TextMeshPro todayScoreText;
    [SerializeField] private TextMeshPro yesterdayScoreText;
    [SerializeField] private TextMeshPro[] currentDayDisplay;

    private CheckStatus _visaStatus = CheckStatus.None;

    [SerializeField] private GameObject person;
    [SerializeField] private GameObject nextDayButton;
    [SerializeField] private GameObject startButton;
    GameObject _currentPerson;
    [SerializeField] private Transform personStart;

    [Flags]
    private enum Rules
    {
        None        = 0b_0000_0000,
        Land        = 0b_0000_0001,
        PassType    = 0b_0000_0010
    }

    private Vector3Int _currentDay = new Vector3Int(1, 1, 2023);

    private Rules _currentRules = Rules.None;

    private readonly List<Countries> _currentCountryDenied = new List<Countries>();

    private readonly List<PassportTypes> _currentPpTypesDenied = new List<PassportTypes>();

    private PassPortData _test = new PassPortData(Countries.Germany, "Dieter", "Mueller", new Vector3Int(30, 12, 2035),
            new Vector3Int(6, 1, 2010), new Vector3Int(12, 6, 1980), PassportTypes.P, PassportColor.Red, false);

    private PassPortData _currentPassportData;

    private int _timesRulesChanged = 0;
    private readonly List<Score> _scores = new List<Score>();
    private int _correctToday = 0;
    private int _totalToday = 0;
    public TimeManager tm;
    

    // Start is called before the first frame update
    private void Start()
    {
        GameEvents.current.onSpawnNewPerson += SpawnPerson;
        GameEvents.current.onVisaStatus += ChangeVisaStatus;
        GameEvents.current.onTriggerPassCheck += PassportCheck;
        GameEvents.current.onTriggerPassBack2 += WantedPerson;
        DisplayDay();
        ShowScore();
    }

    private void ShowScore()
    {
        // Score Today
        
            todayScoreText.text = "<align=left>Correct:<line-height=0>\n"
                                      + "<align=\"right\">" + _correctToday.Monospace("0.6em")
                                      + "<line-height=1em>\n" + "<align=left>Total:<line-height=0>\n" + "<align=\"right\">"
                                      + _totalToday.Monospace("0.6em") + "<line-height=1em>";
        
        // Score Yesterday
        if (_scores.Count >= 1)
        {
            yesterdayScoreText.text = "<align=left>Correct:<line-height=0>\n"
                                      + "<align=\"right\">" + _scores.ElementAt(_scores.Count - 1).Correct.Monospace("0.6em")
                                      + "<line-height=1em>\n" + "<align=left>Total:<line-height=0>\n" + "<align=\"right\">"
                                      + _scores.ElementAt(_scores.Count - 1).Total.Monospace("0.6em") + "<line-height=1em>";
        }
        else
        {
            yesterdayScoreText.text = "<align=left>Correct:<line-height=0>\n"
                                      + "<align=\"right\">" + 0
                                      + "<line-height=1em>\n" + "<align=left>Total:<line-height=0>\n"
                                      + "<align=\"right\">" + 0 + "<line-height=1em>";
        }

        // Score Total
        int correctTotal = 0;
        int totalTotal = 0;

        foreach (var score in _scores)
        {
            correctTotal += score.Correct;
            totalTotal += score.Total;
        }

        correctTotal += _correctToday;
        totalTotal += _totalToday;

        totalScoreText.text = "<align=left>Correct:<line-height=0>\n" 
            + "<align=\"right\">" + correctTotal.Monospace("0.6em") + "<line-height=1em>\n" 
            + "<align=left>Total:<line-height=0>\n" 
            + "<align=\"right\">" 
            + totalTotal.Monospace("0.6em") + "<line-height=1em>";
    }

    public void StartGame()
    {
        startButton.SetActive(false);
        GameEvents.current.TriggerNextDay();
        GameEvents.current.SpawnNewPerson();//SpawnPerson();
        _correctToday = 0;
        _totalToday = 0;
        DisplayDay();
        ShowScore();
    }

    private void DisplayDay()
    {
        foreach (var oneDisplay in currentDayDisplay)
        {
            oneDisplay.text = "Today: " + _currentDay.x + "/" +  _currentDay.y + "/" + _currentDay.z;
        }
    }

    /**
     * Sets the day to the next one
     */
    public void NextDay()
    {
        nextDayButton.SetActive(false);
        GameEvents.current.TriggerNextDay();
        GameEvents.current.SpawnNewPerson();//SpawnPerson();
        _scores.Add(new Score(_correctToday, _totalToday));
        _currentDay[0]++;
        if (_currentDay[0] > 30)
        {
            _currentDay[0] = 1;
            _currentDay[1]++;
        }
        if (_currentDay[1] > 12)
        {
            _currentDay[1] = 1;
            _currentDay[2]++;
        }

        _correctToday = 0;
        _totalToday = 0;
        AddNewRules();
        DisplayDay();
        ShowScore();
    }

    private void AddNewRules()
    {
        _timesRulesChanged++;
        // delete rules
        int test = Random.Range(1, 11);
        Debug.Log("Durchlauf: " + _timesRulesChanged + " " + "Random number to delete rule" + test);
        if (test <= 4)
        {
            int random = Random.Range(1, 11);
            Debug.Log("Durchlauf: " + _timesRulesChanged + " " + "Random number to decide delete rule" + random);
            switch (random)
            {
                case <= 5:
                    Countries lastCountry = Enum.GetValues(typeof(Countries)).Cast<Countries>().Max();
                    Countries countryToRemove = (Countries)Random.Range(1, (int)lastCountry);
                    _currentCountryDenied.Remove(countryToRemove);
                    if (_currentCountryDenied.Count == 0)
                    {
                        _currentRules |= ~ Rules.Land;
                    }
                    Debug.Log("Durchlauf: " + _timesRulesChanged + " " + "removed " + countryToRemove);
                    break;
                case > 5:
                    PassportTypes lastType = Enum.GetValues(typeof(PassportTypes)).Cast<PassportTypes>().Max();
                    PassportTypes typeToRemove = (PassportTypes)Random.Range(1, (int)lastType);
                    _currentPpTypesDenied.Remove(typeToRemove);
                    if (_currentPpTypesDenied.Count == 0)
                    {
                        _currentRules |= ~ Rules.PassType;
                    }
                    Debug.Log("Durchlauf: " + _timesRulesChanged + " " + "removed " + typeToRemove);
                    break;
            }
        }

        int test2 = Random.Range(1, 11);
        Debug.Log("Durchlauf: " + _timesRulesChanged + " " +"Random number to add rule" + test2);
        // add rules
        if (test2 <= 6)
        {
            int random = Random.Range(1, 11);
            Debug.Log("Durchlauf: " + _timesRulesChanged + " " +"Random number to decide add rule" + random);
            switch (random)
            {
                case <= 5:
                    Countries lastCountry = Enum.GetValues(typeof(Countries)).Cast<Countries>().Max();
                    Countries countryToAdd = (Countries)Random.Range(1, (int)lastCountry + 1);
                    if (_currentCountryDenied.IndexOf(countryToAdd) == -1)
                    {
                        _currentCountryDenied.Add(countryToAdd);
                        _currentRules |= Rules.Land;
                    }
                    Debug.Log("Durchlauf: " + _timesRulesChanged + " " + "added " + countryToAdd);
                    break;
                case > 5:
                    PassportTypes lastType = Enum.GetValues(typeof(PassportTypes)).Cast<PassportTypes>().Max();
                    PassportTypes typeToAdd = (PassportTypes)Random.Range(1, (int)lastType + 1);
                    if (_currentPpTypesDenied.IndexOf(typeToAdd) == -1)
                    {
                        _currentPpTypesDenied.Add(typeToAdd);
                        _currentRules |= Rules.PassType;
                    }
                    Debug.Log("Durchlauf: " + _timesRulesChanged + " " + "added " + typeToAdd);
                    break;
            }
        }

        ruleTableCountriesText.text = string.Join("\n", _currentCountryDenied);
        ruleTableAdditionText.text = string.Join(", ", _currentPpTypesDenied);
        
        Debug.Log("Durchlauf: " + _timesRulesChanged + " " + string.Join(", ", _currentCountryDenied));
        Debug.Log("Durchlauf: " + _timesRulesChanged + " " + string.Join(", ", _currentPpTypesDenied));
    }

    private void AddScore(CheckStatus checkStatus)
    {
        switch (checkStatus)
        {
            case CheckStatus.Correct:
                _correctToday++;
                _totalToday++;
                break;
            case CheckStatus.Wrong:
                _totalToday++;
                break;
        }
    }

    private void ChangeCheckLight(CheckStatus checkStatus)
    {
        Material checkLightMaterial = checkStatus switch
        {
            CheckStatus.None => glowNone,
            CheckStatus.Correct => glowCorrect,
            CheckStatus.Wrong => glowWrong,
            _ => throw new ArgumentOutOfRangeException(nameof(checkStatus), checkStatus, null)
        };
        checkLight.GetComponent<MeshRenderer>().material = checkLightMaterial;
    }

    private void ChangeVisaStatus(CheckStatus status)
    {
        _visaStatus = status;
    }

    private void PassportCheck(PassPortData passPortData)
    {
        CheckStatus checkStatus = CheckStatus.Correct;
        // Check if passportData is null
        if (passPortData == null)
        {
            Debug.Log("Should not have happened!");
            return; 
        }

        // Check if dates are valid
        if (passPortData.ExpirationDate[0] < 1 || passPortData.ExpirationDate[0] > 30) { checkStatus =  CheckStatus.Wrong; }
        if (checkStatus != CheckStatus.Wrong && 
            (passPortData.ExpirationDate[1] < 1 || passPortData.ExpirationDate[1] > 12)) { checkStatus =  CheckStatus.Wrong; }

        if (checkStatus != CheckStatus.Wrong && 
            (passPortData.DateOfCreation[0] < 1 || passPortData.DateOfCreation[0] > 30)) { checkStatus =  CheckStatus.Wrong; }
        if (checkStatus != CheckStatus.Wrong && 
            (passPortData.DateOfCreation[1] < 1 || passPortData.DateOfCreation[1] > 12)) { checkStatus =  CheckStatus.Wrong; }

        if (checkStatus != CheckStatus.Wrong && 
            (passPortData.DateOfBirth[0] < 1 || passPortData.DateOfBirth[0] > 30)) { checkStatus =  CheckStatus.Wrong; }
        if (checkStatus != CheckStatus.Wrong && 
            (passPortData.DateOfBirth[1] < 1 || passPortData.DateOfBirth[1] > 12)) { checkStatus =  CheckStatus.Wrong; }

        // Check if expiration date of passport is before the current date
        if (checkStatus != CheckStatus.Wrong && 
            passPortData.ExpirationDate[2] < _currentDay[2]) { checkStatus =  CheckStatus.Wrong; }

        if (checkStatus != CheckStatus.Wrong && 
            passPortData.ExpirationDate[2] == _currentDay[2] &&
            passPortData.ExpirationDate[1] < _currentDay[1]) { checkStatus =  CheckStatus.Wrong; }

        if (checkStatus != CheckStatus.Wrong && 
            passPortData.ExpirationDate[2] == _currentDay[2] &&
            passPortData.ExpirationDate[1] == _currentDay[1] &&
            passPortData.ExpirationDate[0] < _currentDay[0]) { checkStatus =  CheckStatus.Wrong; }

        // Check if the date of creation of passport is after the current date
        if (checkStatus != CheckStatus.Wrong && 
            passPortData.DateOfCreation[2] > _currentDay[2]) { checkStatus =  CheckStatus.Wrong; }

        if (checkStatus != CheckStatus.Wrong && 
            passPortData.DateOfCreation[2] == _currentDay[2] &&
            passPortData.DateOfCreation[1] > _currentDay[1]) { checkStatus =  CheckStatus.Wrong; }

        if (checkStatus != CheckStatus.Wrong && 
            passPortData.DateOfCreation[2] == _currentDay[2] &&
            passPortData.DateOfCreation[1] == _currentDay[1] &&
            passPortData.DateOfCreation[0] > _currentDay[0]) { checkStatus =  CheckStatus.Wrong; }

        // Check if the date of birth of passport is after the current date
        if (checkStatus != CheckStatus.Wrong && 
            passPortData.DateOfBirth[2] > _currentDay[2]) { checkStatus =  CheckStatus.Wrong; }

        if (checkStatus != CheckStatus.Wrong && 
            passPortData.DateOfBirth[2] == _currentDay[2] &&
            passPortData.DateOfBirth[1] > _currentDay[1]) { checkStatus =  CheckStatus.Wrong; }

        if (checkStatus != CheckStatus.Wrong && 
            passPortData.DateOfBirth[2] == _currentDay[2] &&
            passPortData.DateOfBirth[1] == _currentDay[1] &&
            passPortData.DateOfBirth[0] > _currentDay[0]) { checkStatus =  CheckStatus.Wrong; }
        
        // Check if the date of birth of passport is after date of creation
        if (checkStatus != CheckStatus.Wrong && 
            passPortData.DateOfBirth[2] > passPortData.DateOfCreation[2]) { checkStatus =  CheckStatus.Wrong; }

        if (checkStatus != CheckStatus.Wrong && 
            passPortData.DateOfBirth[2] == passPortData.DateOfCreation[2] &&
            passPortData.DateOfBirth[1] > passPortData.DateOfCreation[1]) { checkStatus =  CheckStatus.Wrong; }

        if (checkStatus != CheckStatus.Wrong && 
            passPortData.DateOfBirth[2] == passPortData.DateOfCreation[2] &&
            passPortData.DateOfBirth[1] == passPortData.DateOfCreation[1] &&
            passPortData.DateOfBirth[0] > passPortData.DateOfCreation[0]) { checkStatus =  CheckStatus.Wrong; }
        
        // Check if expiration date of passport is before date of creation
        if (checkStatus != CheckStatus.Wrong && 
            passPortData.ExpirationDate[2] < passPortData.DateOfCreation[2]) { checkStatus =  CheckStatus.Wrong; }

        if (checkStatus != CheckStatus.Wrong && 
            passPortData.ExpirationDate[2] == passPortData.DateOfCreation[2] &&
            passPortData.ExpirationDate[1] < passPortData.DateOfCreation[1]) { checkStatus =  CheckStatus.Wrong; }

        if (checkStatus != CheckStatus.Wrong && 
            passPortData.ExpirationDate[2] == passPortData.DateOfCreation[2] &&
            passPortData.ExpirationDate[1] == passPortData.DateOfCreation[1] &&
            passPortData.ExpirationDate[0] < passPortData.DateOfCreation[0]) { checkStatus =  CheckStatus.Wrong; }

        // Check if Passport has the right Color
        //if (checkStatus != CheckStatus.Wrong && 
            //(int)passPortData.PassType != (int)passPortData.PassColor) { checkStatus =  CheckStatus.Wrong; }

        // Check if country is forbidden
        if (checkStatus != CheckStatus.Wrong && (_currentRules & Rules.Land) != 0)
        {
            if (_currentCountryDenied.Contains(passPortData.Country)) { checkStatus =  CheckStatus.Wrong; }
        }

        // Check if passport type is forbidden
        if (checkStatus != CheckStatus.Wrong && (_currentRules & Rules.PassType) != 0)
        {
            if (_currentPpTypesDenied.Contains(passPortData.PassType)) { checkStatus =  CheckStatus.Wrong; }
        }

        checkStatus = checkStatus == _visaStatus ? CheckStatus.Correct : CheckStatus.Wrong;
        ChangeCheckLight(checkStatus);
        AddScore(checkStatus);
        ShowScore();

        StartCoroutine(StartCountdownLightOff());
    }

    private IEnumerator StartCountdownLightOff()
    {
        yield return new WaitForSeconds(3);
        ChangeCheckLight(CheckStatus.None);
    }

    void WantedPerson()
    {
        CheckStatus checkStatus = CheckStatus.None;
        //int faceofpass = 0; //TODO
        if (Wanted.MFaceCount != PassPort.mFaceIndex)
        {
            checkStatus = CheckStatus.Wrong;
        }else checkStatus = CheckStatus.Correct;

        ChangeCheckLight(checkStatus);
        AddScore(checkStatus);
        ShowScore();

        StartCoroutine(StartCountdownLightOff());
    }

    void SpawnPerson()
    {
        if (!tm.DayEnd())
        {
            Vector3 start = new Vector3(personStart.position.x, 0.4350001f, personStart.position.z);
            _currentPerson = Instantiate(person, start, Quaternion.Euler(0,180,0));
            GameEvents.current.NewPersonSpawned(_currentPerson.GetComponent<Person>());
        }
        else
        {
            nextDayButton.SetActive(true);
        }  
    }
    
    

    private void OnDestroy()
    {
        GameEvents.current.onSpawnNewPerson -= SpawnPerson;
        GameEvents.current.onVisaStatus -= ChangeVisaStatus;
        GameEvents.current.onTriggerPassCheck -= PassportCheck;
    }

}
