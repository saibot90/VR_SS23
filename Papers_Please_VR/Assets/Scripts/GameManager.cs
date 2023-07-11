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
    #region Variables
    
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
    [SerializeField] private GameObject person;
    [SerializeField] private GameObject nextDayButton;
    [SerializeField] private Transform personStart;

    [Flags]
    private enum Rules
    {
        None        = 0b_0000_0000,
        Land        = 0b_0000_0001,
        PassType    = 0b_0000_0010
    }
    
    private GameObject _currentPerson;

    private Vector3Int _currentDay = new Vector3Int(1, 1, 2023);

    private Rules _currentRules = Rules.None;

    private readonly List<Countries> _currentCountryDenied = new List<Countries>();

    private readonly List<PassportTypes> _currentPpTypesDenied = new List<PassportTypes>();

    private PassPortData _currentPassportData;
    
    private readonly List<Score> _scores = new List<Score>();
    private int _correctToday;
    private int _totalToday;
    public TimeManager tm;
    private CheckStatus _visaStatus = CheckStatus.None;
    
    #endregion
    
    #region Functions
    
    /// <summary>
    /// Is called before the first frame update
    ///     - Sets game events
    ///     - Init the scene
    /// </summary>
    private void Start()
    {
        GameEvents.current.onSpawnNewPerson += SpawnPerson;
        GameEvents.current.onVisaStatus += ChangeVisaStatus;
        GameEvents.current.onTriggerPassCheck += PassportCheck;
        GameEvents.current.onTriggerPassBack2 += WantedPerson;
        DisplayDay();
        ShowScore();
    }

    /// <summary>
    /// Show the score on the text fields of the scoreboard
    /// </summary>
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

    /// <summary>
    /// Set the actions that happen when the game start
    /// </summary>
    public void StartGame()
    {
        GameEvents.current.TriggerNextDay();
        GameEvents.current.SpawnNewPerson();//SpawnPerson();
        _correctToday = 0;
        _totalToday = 0;
        DisplayDay();
        ShowScore();
    }

    /// <summary>
    /// Sets the time to the given text meshes
    /// </summary>
    private void DisplayDay()
    {
        foreach (var oneDisplay in currentDayDisplay)
        {
            oneDisplay.text = "Today: " + _currentDay.x + "/" +  _currentDay.y + "/" + _currentDay.z;
        }
    }

    /// <summary>
    /// Sets the day to the next one and the actions that follows whit it
    /// </summary>
    public void NextDay()
    {
        nextDayButton.SetActive(false);
        GameEvents.current.TriggerNextDay();
        GameEvents.current.SpawnNewPerson();//SpawnPerson();
        _scores.Add(new Score(_correctToday, _totalToday));
        _currentDay[0]++;
        // count the day up
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

        // Reset score
        _correctToday = 0;
        _totalToday = 0;
        
        // Ready the new day
        AddNewRules();
        DisplayDay();
        ShowScore();
    }

    /// <summary>
    /// Add and remove countries and passport types which are denied after this
    /// </summary>
    private void AddNewRules()
    {
        // delete rules
        if (Random.Range(1, 11) <= 4)
        {
            switch (Random.Range(1, 11))
            {
                case <= 5:
                    // countries
                    Countries lastCountry = Enum.GetValues(typeof(Countries)).Cast<Countries>().Max();
                    Countries countryToRemove = (Countries)Random.Range(1, (int)lastCountry);
                    _currentCountryDenied.Remove(countryToRemove);
                    if (_currentCountryDenied.Count == 0)
                    {
                        _currentRules |= ~ Rules.Land;
                    }
                    break;
                case > 5:
                    // passport type
                    PassportTypes lastType = Enum.GetValues(typeof(PassportTypes)).Cast<PassportTypes>().Max();
                    PassportTypes typeToRemove = (PassportTypes)Random.Range(1, (int)lastType);
                    _currentPpTypesDenied.Remove(typeToRemove);
                    if (_currentPpTypesDenied.Count == 0)
                    {
                        _currentRules |= ~ Rules.PassType;
                    }
                    break;
            }
        }

        // add rules
        if (Random.Range(1, 11) <= 6)
        {
            switch (Random.Range(1, 11))
            {
                case <= 5:
                    // countries
                    Countries lastCountry = Enum.GetValues(typeof(Countries)).Cast<Countries>().Max();
                    Countries countryToAdd = (Countries)Random.Range(1, (int)lastCountry + 1);
                    if (_currentCountryDenied.IndexOf(countryToAdd) == -1)
                    {
                        _currentCountryDenied.Add(countryToAdd);
                        _currentRules |= Rules.Land;
                    }
                    break;
                case > 5:
                    // passport type
                    PassportTypes lastType = Enum.GetValues(typeof(PassportTypes)).Cast<PassportTypes>().Max();
                    PassportTypes typeToAdd = (PassportTypes)Random.Range(1, (int)lastType + 1);
                    if (_currentPpTypesDenied.IndexOf(typeToAdd) == -1)
                    {
                        _currentPpTypesDenied.Add(typeToAdd);
                        _currentRules |= Rules.PassType;
                    }
                    break;
            }
        }

        // add denied countries and passport types to the rule table to show them in game
        ruleTableCountriesText.text = string.Join("\n", _currentCountryDenied);
        ruleTableAdditionText.text = string.Join(", ", _currentPpTypesDenied);
    }

    /// <summary>
    /// Adds the score to the daily score
    /// </summary>
    /// <param name="checkStatus">defines if the player was correct or not</param>
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

    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// Changes the color of the check light
    /// </summary>
    /// <param name="checkStatus">status to change the color</param>
    /// <exception cref="ArgumentOutOfRangeException">In case a value is added which is not thought of</exception>
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

    /// <summary>
    /// Changes the local visa status
    /// </summary>
    /// <param name="status">status that the local visa should have</param>
    private void ChangeVisaStatus(CheckStatus status)
    {
        _visaStatus = status;
    }

    /// <summary>
    /// Checks the content of the given passport data and returns the validation of it
    /// </summary>
    /// <param name="passportData">data to check</param>
    /// <returns>Valid state of the data</returns>
    private CheckStatus PassportDataCheck(PassPortData passportData)
    {
        CheckStatus checkStatus = CheckStatus.Correct;
        
        // Check if passportData is null
        if (passportData == null)
        {
            print("Should not have happened!");
            return CheckStatus.None; 
        }
        
        // Check if dates are valid
        if (passportData.ExpirationDate[0] < 1 || passportData.ExpirationDate[0] > 30) { checkStatus =  CheckStatus.Wrong; }
        if (checkStatus != CheckStatus.Wrong && 
            (passportData.ExpirationDate[1] < 1 || passportData.ExpirationDate[1] > 12)) { checkStatus =  CheckStatus.Wrong; }

        if (checkStatus != CheckStatus.Wrong && 
            (passportData.DateOfCreation[0] < 1 || passportData.DateOfCreation[0] > 30)) { checkStatus =  CheckStatus.Wrong; }
        if (checkStatus != CheckStatus.Wrong && 
            (passportData.DateOfCreation[1] < 1 || passportData.DateOfCreation[1] > 12)) { checkStatus =  CheckStatus.Wrong; }

        if (checkStatus != CheckStatus.Wrong && 
            (passportData.DateOfBirth[0] < 1 || passportData.DateOfBirth[0] > 30)) { checkStatus =  CheckStatus.Wrong; }
        if (checkStatus != CheckStatus.Wrong && 
            (passportData.DateOfBirth[1] < 1 || passportData.DateOfBirth[1] > 12)) { checkStatus =  CheckStatus.Wrong; }

        // Check if expiration date of passport is before the current date
        if (checkStatus != CheckStatus.Wrong && 
            passportData.ExpirationDate[2] < _currentDay[2]) { checkStatus =  CheckStatus.Wrong; }

        if (checkStatus != CheckStatus.Wrong && 
            passportData.ExpirationDate[2] == _currentDay[2] &&
            passportData.ExpirationDate[1] < _currentDay[1]) { checkStatus =  CheckStatus.Wrong; }

        if (checkStatus != CheckStatus.Wrong && 
            passportData.ExpirationDate[2] == _currentDay[2] &&
            passportData.ExpirationDate[1] == _currentDay[1] &&
            passportData.ExpirationDate[0] < _currentDay[0]) { checkStatus =  CheckStatus.Wrong; }

        // Check if the date of creation of passport is after the current date
        if (checkStatus != CheckStatus.Wrong && 
            passportData.DateOfCreation[2] > _currentDay[2]) { checkStatus =  CheckStatus.Wrong; }

        if (checkStatus != CheckStatus.Wrong && 
            passportData.DateOfCreation[2] == _currentDay[2] &&
            passportData.DateOfCreation[1] > _currentDay[1]) { checkStatus =  CheckStatus.Wrong; }

        if (checkStatus != CheckStatus.Wrong && 
            passportData.DateOfCreation[2] == _currentDay[2] &&
            passportData.DateOfCreation[1] == _currentDay[1] &&
            passportData.DateOfCreation[0] > _currentDay[0]) { checkStatus =  CheckStatus.Wrong; }

        // Check if the date of birth of passport is after the current date
        if (checkStatus != CheckStatus.Wrong && 
            passportData.DateOfBirth[2] > _currentDay[2]) { checkStatus =  CheckStatus.Wrong; }

        if (checkStatus != CheckStatus.Wrong && 
            passportData.DateOfBirth[2] == _currentDay[2] &&
            passportData.DateOfBirth[1] > _currentDay[1]) { checkStatus =  CheckStatus.Wrong; }

        if (checkStatus != CheckStatus.Wrong && 
            passportData.DateOfBirth[2] == _currentDay[2] &&
            passportData.DateOfBirth[1] == _currentDay[1] &&
            passportData.DateOfBirth[0] > _currentDay[0]) { checkStatus =  CheckStatus.Wrong; }
        
        // Check if the date of birth of passport is after date of creation
        if (checkStatus != CheckStatus.Wrong && 
            passportData.DateOfBirth[2] > passportData.DateOfCreation[2]) { checkStatus =  CheckStatus.Wrong; }

        if (checkStatus != CheckStatus.Wrong && 
            passportData.DateOfBirth[2] == passportData.DateOfCreation[2] &&
            passportData.DateOfBirth[1] > passportData.DateOfCreation[1]) { checkStatus =  CheckStatus.Wrong; }

        if (checkStatus != CheckStatus.Wrong && 
            passportData.DateOfBirth[2] == passportData.DateOfCreation[2] &&
            passportData.DateOfBirth[1] == passportData.DateOfCreation[1] &&
            passportData.DateOfBirth[0] > passportData.DateOfCreation[0]) { checkStatus =  CheckStatus.Wrong; }
        
        // Check if expiration date of passport is before date of creation
        if (checkStatus != CheckStatus.Wrong && 
            passportData.ExpirationDate[2] < passportData.DateOfCreation[2]) { checkStatus =  CheckStatus.Wrong; }

        if (checkStatus != CheckStatus.Wrong && 
            passportData.ExpirationDate[2] == passportData.DateOfCreation[2] &&
            passportData.ExpirationDate[1] < passportData.DateOfCreation[1]) { checkStatus =  CheckStatus.Wrong; }

        if (checkStatus != CheckStatus.Wrong && 
            passportData.ExpirationDate[2] == passportData.DateOfCreation[2] &&
            passportData.ExpirationDate[1] == passportData.DateOfCreation[1] &&
            passportData.ExpirationDate[0] < passportData.DateOfCreation[0]) { checkStatus =  CheckStatus.Wrong; }

        // Check if Passport has the right Color
        if (checkStatus != CheckStatus.Wrong && 
            (int)passportData.PassType != (int)passportData.PassColor) { checkStatus =  CheckStatus.Wrong; }

        // Check if country is forbidden
        if (checkStatus != CheckStatus.Wrong && (_currentRules & Rules.Land) != 0)
        {
            if (_currentCountryDenied.Contains(passportData.Country)) { checkStatus =  CheckStatus.Wrong; }
        }

        // Check if passport type is forbidden
        if (checkStatus != CheckStatus.Wrong && (_currentRules & Rules.PassType) != 0)
        {
            if (_currentPpTypesDenied.Contains(passportData.PassType)) { checkStatus =  CheckStatus.Wrong; }
        }
        
        // Check if passport person is wanted
        if (checkStatus != CheckStatus.Wrong && passportData.Wanted)
        {
            checkStatus = CheckStatus.Wrong;
        }
        
        // Check if passport person and passport have the same face
        if (checkStatus != CheckStatus.Wrong && passportData.IncorrectFace)
        {
            checkStatus = CheckStatus.Wrong;
        }

        return checkStatus;
    }

    /// <summary>
    /// Checks the content of the given passport data and show the validation result
    /// </summary>
    /// <param name="passportData">data to check</param>
    private void PassportCheck(PassPortData passportData)
    {
        // Check if passportData is null
        if (passportData == null)
        {
            print("Passport data empty!");
            return; 
        }

        // Check passport data
        CheckStatus checkStatus = PassportDataCheck(passportData);

        if (checkStatus == CheckStatus.None)
        {
            print("Passport data empty after data check!");
            return;
        }

        // shows the result whether the data is valid or not
        checkStatus = checkStatus == _visaStatus ? CheckStatus.Correct : CheckStatus.Wrong;
        ChangeCheckLight(checkStatus);
        GameEvents.current.VisaCheckSound(checkStatus);
        AddScore(checkStatus);
        ShowScore();

        StartCoroutine(StartCountdownLightOff());
    }

    /// <summary>
    /// Deactivate the check light after a specific time
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartCountdownLightOff()
    {
        yield return new WaitForSeconds(3);
        ChangeCheckLight(CheckStatus.None);
    }

    /// <summary>
    /// Checks if the current person is wanted or not
    /// </summary>
    void WantedPerson()
    {
        CheckStatus checkStatus = Wanted.MFaceCount != PassPort.MFaceIndex ? CheckStatus.Wrong : CheckStatus.Correct;
        if (checkStatus == CheckStatus.Correct)
        {
            GameEvents.current.NewWantedPerson();
        }
        ChangeCheckLight(checkStatus);
        GameEvents.current.VisaCheckSound(checkStatus);
        AddScore(checkStatus);
        ShowScore();

        StartCoroutine(StartCountdownLightOff());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// Spawns a new person
    /// </summary>
    void SpawnPerson()
    {
        if (!tm.IsDayEnd())
        {
            var position = personStart.position;
            Vector3 start = new Vector3(position.x, 0.4350001f, position.z);
            _currentPerson = Instantiate(person, start, Quaternion.Euler(0,180,0));
            GameEvents.current.NewPersonSpawned(_currentPerson.GetComponent<Person>());
        }
        else
        {
            nextDayButton.SetActive(true);
        }  
    }

    /// <summary>
    /// Is called if this script is destroyed
    ///     -unsubscribe game events 
    /// </summary>
    private void OnDestroy()
    {
        GameEvents.current.onSpawnNewPerson -= SpawnPerson;
        GameEvents.current.onVisaStatus -= ChangeVisaStatus;
        GameEvents.current.onTriggerPassCheck -= PassportCheck;
    }
    
    #endregion
}
