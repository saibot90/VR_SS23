using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using static PassPortData;
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

    [SerializeField] private CheckStatus checkPassport = CheckStatus.None;
    [SerializeField] private CheckStatus addScoreStatus = CheckStatus.None;
    [SerializeField] private bool checkRules = false;
    [SerializeField] private bool forceNextDay = false;

    [SerializeField] private GameObject person;
    GameObject currentPerson;
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

    private Vector3Int currentDay = new Vector3Int(23, 4, 2023);

    private Rules currentRules = Rules.None;

    private List<Countries> currentCountryDenied = new List<Countries>();

    private List<PassportTypes> currentPpTypesDenied = new List<PassportTypes>();

    private float deltaTime = 0.0f;

    private PassPortData test = new PassPortData(Countries.Germany, "Dieter", "Mueller", new Vector3Int(30, 12, 2035),
            new Vector3Int(6, 1, 2010), new Vector3Int(12, 6, 1980), PassportTypes.P, PassportColor.Red);

    private int timesRulesChanged = 0;
    private int scoreTest = 0;
    private List<Score> scores = new List<Score>();
    private Score scoreToday = new Score();

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onSpawnNewPerson += spawnPerson;
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

    }

    private void FixedUpdate()
    {
        //deltaTime += Time.deltaTime;
        //if (deltaTime > 0.0001f)
        //{
        //    deltaTime = 0.0f;
        //    nextDay();
        //    Debug.Log(currentDay.ToString());
        //}
        deltaTime += Time.deltaTime;
        if (deltaTime > 3.0f)
        {
            //deltaTime = 0.0f;
            //checkPassport = CheckStatus.None;
            //Debug.Log(checkPassport);
        }
        else
        {
            checkPassport = PassportCheck(test);
        }
        if (addScoreStatus != checkPassport)
        {
            addScoreStatus = checkPassport;
            AddScore(addScoreStatus);
            Debug.Log("Correct: " + scoreToday.Correct + " Total: " + scoreToday.Total + "\n");
        }

        ChangeCheckLight(checkPassport);

        if (checkRules)
        {
            newDay();
            checkRules = false;
        }
        if (forceNextDay)
        {
            scores.Add(new Score(scoreToday.Correct, scoreToday.Total));
            scoreToday.Reset();
            ShowScore();
            forceNextDay = false;
        }
    }

    private void ShowScore()
    {
        // Score Yesterday
        if (scores.Count >= 1)
        {
            yesterdayScoreText.text = "<align=left>Correct:<line-height=0>\n"
            + "<align=\"right\">" + scores.ElementAt(scores.Count - 1).Correct.Monospace("0.6em")
            + "<line-height=1em>\n" + "<align=left>Total:<line-height=0>\n" + "<align=\"right\">"
            + scores.ElementAt(scores.Count - 1).Total.Monospace("0.6em") + "<line-height=1em>";
        }
        else
        {
            yesterdayScoreText.text = "<align=left>Correct:<line-height=0>\n"
            + "<align=\"right\">" + 0
            + "<line-height=1em>\n" + "<align=left>Total:<line-height=0>\n"
            + "<align=\"right\">" + 0 + "<line-height=1em>";
        }
        // Score before Yesterday
        if (scores.Count >= 2)
        {
            beforeYesterdayScoreText.text = "<align=left>Correct:<line-height=0>\n"
            + "<align=\"right\">" + scores.ElementAt(scores.Count - 2).Correct.Monospace("0.6em")
            + "<line-height=1em>\n" + "<align=left>Total:<line-height=0>\n" + "<align=\"right\">"
            + scores.ElementAt(scores.Count - 2).Total.Monospace("0.6em") + "<line-height=1em>";
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

        foreach (var score in scores)
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
    private void nextDay()
    {
        currentDay[0]++;
        if (currentDay[0] > 30)
        {
            currentDay[0] = 1;
            currentDay[1]++;
        }
        if (currentDay[1] > 12)
        {
            currentDay[1] = 1;
            currentDay[2]++;
        }
    }

    private void newDay()
    {
        timesRulesChanged++;
        // entferne Regel
        if (Random.Range(0, 100) <= 33)
        {
            int random = Random.Range(0, 100);
            switch (random)
            {
                case <= 60:
                    Countries lastCountry = Enum.GetValues(typeof(Countries)).Cast<Countries>().Max();
                    Countries countryToRemove = (Countries)Random.Range(1, (int)lastCountry);
                    currentCountryDenied.Remove(countryToRemove);
                    Debug.Log("Durschlauf: " + timesRulesChanged + " " + "removed " + countryToRemove);
                    break;
                case > 60:
                    PassportTypes lastType = Enum.GetValues(typeof(PassportTypes)).Cast<PassportTypes>().Max();
                    PassportTypes typeToRemove = (PassportTypes)Random.Range(1, (int)lastType);
                    currentPpTypesDenied.Remove(typeToRemove);
                    Debug.Log("Durschlauf: " + timesRulesChanged + " " + "removed " + typeToRemove);
                    break;
            }
        }

        // fuege Regel hinzu
        if (Random.Range(0, 100) <= 33)
        {
            int random = Random.Range(0, 100);
            switch (random)
            {
                case <= 60:
                    Countries lastCountry = Enum.GetValues(typeof(Countries)).Cast<Countries>().Max();
                    Countries countryToAdd = (Countries)Random.Range(1, (int)lastCountry);
                    if (currentCountryDenied.IndexOf(countryToAdd) == -1)
                    {
                        currentCountryDenied.Add(countryToAdd);
                    }
                    Debug.Log("Durschlauf: " + timesRulesChanged + " " + "added " + countryToAdd);
                    break;
                case > 60:
                    PassportTypes lastType = Enum.GetValues(typeof(PassportTypes)).Cast<PassportTypes>().Max();
                    PassportTypes typeToAdd = (PassportTypes)Random.Range(1, (int)lastType);
                    if (currentPpTypesDenied.IndexOf(typeToAdd) == -1)
                    {
                        currentPpTypesDenied.Add(typeToAdd);
                    }
                    Debug.Log("Durschlauf: " + timesRulesChanged + " " + "added " + typeToAdd);
                    break;
            }
        }

        ruleTableCountriesText.text = string.Join("\n", currentCountryDenied);
        ruleTableAdditionText.text = string.Join(", ", currentPpTypesDenied);
        
        Debug.Log("Durschlauf: " + timesRulesChanged + " " + string.Join(", ", currentCountryDenied));
        Debug.Log("Durschlauf: " + timesRulesChanged + " " + string.Join(", ", currentPpTypesDenied));
    }

    private void AddScore(CheckStatus checkStatus)
    {
        switch (checkStatus)
        {
            case CheckStatus.Correct:
                scoreToday.Correct++;
                scoreToday.Total++;
                break;
            case CheckStatus.Wrong:
                scoreToday.Total++;
                break;
        }
    }

    private void ChangeCheckLight(CheckStatus checkStatus)
    {
        switch (checkStatus)
        {
            case CheckStatus.None:
                checkLight.GetComponent<MeshRenderer>().material = glowNone;
                break;
            case CheckStatus.Correct:
                checkLight.GetComponent<MeshRenderer>().material = glowCorrect;
                break;
            case CheckStatus.Wrong:
                checkLight.GetComponent<MeshRenderer>().material = glowWrong;
                break;
        }
    }

    CheckStatus PassportCheck(PassPortData passPortData)
    {
        // Check if passportData is null
        if (passPortData == null) { return CheckStatus.Wrong; }

        // Check if dates are valid
        if (passPortData.ExpirationDate[0] < 1 || passPortData.ExpirationDate[0] > 30) { return CheckStatus.Wrong; }
        if (passPortData.ExpirationDate[1] < 1 || passPortData.ExpirationDate[1] > 12) { return CheckStatus.Wrong; }

        if (passPortData.DateOfCreation[0] < 1 || passPortData.DateOfCreation[0] > 30) { return CheckStatus.Wrong; }
        if (passPortData.DateOfCreation[1] < 1 || passPortData.DateOfCreation[1] > 12) { return CheckStatus.Wrong; }

        if (passPortData.DateOfBirth[0] < 1 || passPortData.DateOfBirth[0] > 30) { return CheckStatus.Wrong; }
        if (passPortData.DateOfBirth[1] < 1 || passPortData.DateOfBirth[1] > 12) { return CheckStatus.Wrong; }

        // Check if expiration date of passport is before the current date
        if (passPortData.ExpirationDate[2] < currentDay[2]) { return CheckStatus.Wrong; }

        if (passPortData.ExpirationDate[2] == currentDay[2] &&
            passPortData.ExpirationDate[1] < currentDay[1]) { return CheckStatus.Wrong; }

        if (passPortData.ExpirationDate[2] == currentDay[2] &&
            passPortData.ExpirationDate[1] == currentDay[1] &&
            passPortData.ExpirationDate[0] < currentDay[0]) { return CheckStatus.Wrong; }

        // Check if the date of creation of passport is after the current date
        if (passPortData.DateOfCreation[2] > currentDay[2]) { return CheckStatus.Wrong; }

        if (passPortData.DateOfCreation[2] == currentDay[2] &&
            passPortData.DateOfCreation[1] > currentDay[1]) { return CheckStatus.Wrong; }

        if (passPortData.DateOfCreation[2] == currentDay[2] &&
            passPortData.DateOfCreation[1] == currentDay[1] &&
            passPortData.DateOfCreation[0] > currentDay[0]) { return CheckStatus.Wrong; }

        // Check if the date of birth of passport is after the current date
        if (passPortData.DateOfBirth[2] > currentDay[2]) { return CheckStatus.Wrong; }

        if (passPortData.DateOfBirth[2] == currentDay[2] &&
            passPortData.DateOfBirth[1] > currentDay[1]) { return CheckStatus.Wrong; }

        if (passPortData.DateOfBirth[2] == currentDay[2] &&
            passPortData.DateOfBirth[1] == currentDay[1] &&
            passPortData.DateOfBirth[0] > currentDay[0]) { return CheckStatus.Wrong; }

        // Check if Passport has the right Color

        if ((int)passPortData.PassType != (int)passPortData.PassColor) { return CheckStatus.Wrong; }

        // Check if country is forbidden
        if (currentRules == Rules.Land)
        {
            if (currentCountryDenied.Contains(passPortData.Country)) { return CheckStatus.Wrong; }
        }

        // Check if passport type is forbidden
        if (currentRules == Rules.PassType)
        {
            if (currentPpTypesDenied.Contains(passPortData.PassType)) { return CheckStatus.Wrong; }
        }


        return CheckStatus.Correct; ;
    }

    void spawnPerson()
    {
        Vector3 start = new Vector3(personStart.position.x, 0.4350001f, personStart.position.z);
        currentPerson = Instantiate(person, start, Quaternion.identity);
    }

    private void OnDestroy()
    {
        GameEvents.current.onSpawnNewPerson -= spawnPerson;
    }

}
