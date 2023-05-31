using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using static PassPortData;
using System.Collections;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Material glowCorrect;
    [SerializeField] private Material glowWrong;
    [SerializeField] private Material glowNone;
    [SerializeField] private GameObject checkLight;
    [SerializeField] private TextMeshPro ruleTableCountriesText;
    [SerializeField] private TextMeshPro ruleTableAdditionText;
    [SerializeField] private TextMeshPro totalScoreText;
    [SerializeField] private TextMeshPro yesterdayScoreText;
    [SerializeField] private TextMeshPro beforeYesterdayScoreText;

    [SerializeField] private CheckStatus visaStatus = CheckStatus.None;
    [SerializeField] private CheckStatus addScoreStatus = CheckStatus.None;
    [SerializeField] private bool checkRules = false;
    [SerializeField] private bool forceNextDay = false;

    [SerializeField] private GameObject person;
    GameObject _currentPerson;
    [SerializeField] private Transform personStart;

    [Flags]
    private enum Rules
    {
        None        = 0b_0000_0000,
        Land        = 0b_0000_0001,
        PassType    = 0b_0000_0010,
        MultiDocs   = 0b_0000_0100
    }

    private enum CheckStatus
    {
        None = 0,
        Correct = 1,
        Wrong = 2
    }

    private Vector3Int _currentDay = new Vector3Int(1, 1, 2023);

    private Rules _currentRules = Rules.None;

    private readonly List<Countries> _currentCountryDenied = new List<Countries>();

    private readonly List<PassportTypes> _currentPpTypesDenied = new List<PassportTypes>();

    private float _deltaTime = 0.0f;

    private PassPortData _test = new PassPortData(Countries.Germany, "Dieter", "Mueller", new Vector3Int(30, 12, 2035),
            new Vector3Int(6, 1, 2010), new Vector3Int(12, 6, 1980), PassportTypes.P, PassportColor.Red);

    private PassPortData _currentPassportData;

    private int _timesRulesChanged = 0;
    private int scoreTest = 0;
    private readonly List<Score> _scores = new List<Score>();
    private readonly Score _scoreToday = new Score();

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onSpawnNewPerson += SpawnPerson;
        GameEvents.current.onVisaStatus += ChangeVisaStatus;
        GameEvents.current.onTriggerPassCheck += PassportCheck;
        //int ttt = 4;
        //Debug.Log(PassportCheck(test));
        //Debug.Log((Rules)ttt);
        //scores.Add(new Score(2, 2));
        //scores.Add(new Score(4, 8));
        //scores.Add(new Score(10, 10));
        ShowScore();
    }

    // Update is called once per frame
    void Update()
    {
        //deltaTime += Time.deltaTime;
        //if (deltaTime > 0.0001f)
        //{
        //    deltaTime = 0.0f;
        //    nextDay();
        //    Debug.Log(currentDay.ToString());
        //}
        _deltaTime += Time.deltaTime;
        if (_deltaTime > 10.0f)
        {
            //deltaTime = 0.0f;
            //checkPassport = CheckStatus.Wrong;
            //Debug.Log(checkPassport);
        }
        else
        {
            //checkPassport = PassportCheck();
        }
        /*if (addScoreStatus != checkPassport)
        {
            addScoreStatus = checkPassport;
            AddScore(addScoreStatus);
            Debug.Log("Correct: " + _scoreToday.Correct + " Total: " + _scoreToday.Total + "\n");
        }

        ChangeCheckLight(checkPassport);*/

        if (checkRules)
        {
            NewDay();
            checkRules = false;
        }
        if (forceNextDay)
        {
            _scores.Add(new Score(_scoreToday.Correct, _scoreToday.Total));
            _scoreToday.Reset();
            ShowScore();
            forceNextDay = false;
        }
    }

    private void ShowScore()
    {
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
        // Score before Yesterday
        if (_scores.Count >= 2)
        {
            beforeYesterdayScoreText.text = "<align=left>Correct:<line-height=0>\n"
            + "<align=\"right\">" + _scores.ElementAt(_scores.Count - 2).Correct.Monospace("0.6em")
            + "<line-height=1em>\n" + "<align=left>Total:<line-height=0>\n" + "<align=\"right\">"
            + _scores.ElementAt(_scores.Count - 2).Total.Monospace("0.6em") + "<line-height=1em>";
        } 
        else
        {
            beforeYesterdayScoreText.text = "<align=left>Correct:<line-height=0>\n"
            + "<align=\"right\">" + 0
            + "<line-height=1em>\n" + "<align=left>Total:<line-height=0>\n" 
            + "<align=\"right\">"+ 0 + "<line-height=1em>";
        }

        // Score Total
        int correctTotal = 0;
        int totalTotal = 0;

        foreach (var score in _scores)
        {
            correctTotal += score.Correct;
            totalTotal += score.Total;
        }

        totalScoreText.text = "<align=left>Correct:<line-height=0>\n" 
            + "<align=\"right\">" + correctTotal.Monospace("0.6em") + "<line-height=1em>\n" 
            + "<align=left>Total:<line-height=0>\n" 
            + "<align=\"right\">" 
            + totalTotal.Monospace("0.6em") + "<line-height=1em>";
    }

    /**
     * Sets the day to the next one
     */
    private void NextDay()
    {
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
    }

    private void NewDay()
    {
        _timesRulesChanged++;
        // delete rules
        if (Random.Range(0, 100) <= 33)
        {
            int random = Random.Range(0, 100);
            switch (random)
            {
                case <= 60:
                    Countries lastCountry = Enum.GetValues(typeof(Countries)).Cast<Countries>().Max();
                    Countries countryToRemove = (Countries)Random.Range(1, (int)lastCountry);
                    _currentCountryDenied.Remove(countryToRemove);
                    Debug.Log("Durchlauf: " + _timesRulesChanged + " " + "removed " + countryToRemove);
                    break;
                case > 60:
                    PassportTypes lastType = Enum.GetValues(typeof(PassportTypes)).Cast<PassportTypes>().Max();
                    PassportTypes typeToRemove = (PassportTypes)Random.Range(1, (int)lastType);
                    _currentPpTypesDenied.Remove(typeToRemove);
                    Debug.Log("Durchlauf: " + _timesRulesChanged + " " + "removed " + typeToRemove);
                    break;
            }
        }

        // add rules
        if (Random.Range(0, 100) <= 66)
        {
            int random = Random.Range(0, 100);
            switch (random)
            {
                case <= 60:
                    Countries lastCountry = Enum.GetValues(typeof(Countries)).Cast<Countries>().Max();
                    Countries countryToAdd = (Countries)Random.Range(1, (int)lastCountry);
                    if (_currentCountryDenied.IndexOf(countryToAdd) == -1)
                    {
                        _currentCountryDenied.Add(countryToAdd);
                    }
                    Debug.Log("Durchlauf: " + _timesRulesChanged + " " + "added " + countryToAdd);
                    break;
                case > 60:
                    PassportTypes lastType = Enum.GetValues(typeof(PassportTypes)).Cast<PassportTypes>().Max();
                    PassportTypes typeToAdd = (PassportTypes)Random.Range(1, (int)lastType);
                    if (_currentPpTypesDenied.IndexOf(typeToAdd) == -1)
                    {
                        _currentPpTypesDenied.Add(typeToAdd);
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
                _scoreToday.Correct++;
                _scoreToday.Total++;
                break;
            case CheckStatus.Wrong:
                _scoreToday.Total++;
                break;
        }
    }

    private void ChangeCheckLight(CheckStatus checkStatus)
    {
        Material checklightMaterial = checkStatus switch
        {
            CheckStatus.None => glowNone,
            CheckStatus.Correct => glowCorrect,
            CheckStatus.Wrong => glowWrong,
            _ => throw new ArgumentOutOfRangeException(nameof(checkStatus), checkStatus, null)
        };
        checkLight.GetComponent<MeshRenderer>().material = checklightMaterial;
    }

    private void ChangeVisaStatus(bool status)
    {
        visaStatus = status ? CheckStatus.Correct : CheckStatus.Wrong;
        Debug.Log("ChangeVisaStatus");
    }

    private void PassportCheck(PassPortData passPortData)
    {
        Debug.Log("PassportCheck");
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

        // Check if Passport has the right Color
        //if (checkStatus != CheckStatus.Wrong && 
            //(int)passPortData.PassType != (int)passPortData.PassColor) { checkStatus =  CheckStatus.Wrong; }

        // Check if country is forbidden
        if (checkStatus != CheckStatus.Wrong && _currentRules == Rules.Land)
        {
            if (_currentCountryDenied.Contains(passPortData.Country)) { checkStatus =  CheckStatus.Wrong; }
        }

        // Check if passport type is forbidden
        if (checkStatus != CheckStatus.Wrong && _currentRules == Rules.PassType)
        {
            if (_currentPpTypesDenied.Contains(passPortData.PassType)) { checkStatus =  CheckStatus.Wrong; }
        }
        
        Debug.Log(visaStatus);
        Debug.Log(checkStatus);
        Debug.Log(checkStatus == visaStatus);

        checkStatus = checkStatus == visaStatus ? CheckStatus.Correct : CheckStatus.Wrong;
        ChangeCheckLight(checkStatus);
        AddScore(checkStatus);
        Debug.Log("Correct: " + _scoreToday.Correct + " Total: " + _scoreToday.Total + "\n");

        StartCoroutine(StartCountdownLightOff());
    }

    private IEnumerator StartCountdownLightOff()
    {
        yield return new WaitForSeconds(5);
        ChangeCheckLight(CheckStatus.None);
        Debug.Log("Lights Off!");
    }

    void SpawnPerson()
    {
        Vector3 start = new Vector3(personStart.position.x, 0.4350001f, personStart.position.z);
        _currentPerson = Instantiate(person, start, Quaternion.Euler(0,180,0));
    }

    private void OnDestroy()
    {
        GameEvents.current.onSpawnNewPerson -= SpawnPerson;
        GameEvents.current.onVisaStatus -= ChangeVisaStatus;
        GameEvents.current.onTriggerPassCheck -= PassportCheck;
    }

}
