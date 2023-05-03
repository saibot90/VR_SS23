using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PassPortData;

public class GameManager : MonoBehaviour
{
    [Flags]
    private enum Rules
    {
        None        = 0b_0000_0000,
        Land        = 0b_0000_0001,
        PassType    = 0b_0000_0010,
        MultiDocs   = 0b_0000_0100
    }

    private Vector3Int currentDay = new Vector3Int(23, 4, 2023);

    private Rules currentRules = Rules.None;

    private List<Countries> currentCountryDenied = new List<Countries>();

    private List<PassPortData.PassportTypes> currentPpTypesDenied = new List<PassPortData.PassportTypes>();

    private float deltaTime = 0.0f;

    [SerializeField] private GameObject person;
    GameObject currentPerson;
    [SerializeField] private Transform personStart;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onSpawnNewPerson += spawnPerson;
        int ttt = 4;
        currentPpTypesDenied.Add(PassPortData.PassportTypes.None);
        PassPortData test = new PassPortData(Countries.Germany, "Dieter", "Müller", new Vector3Int(30, 12, 2035), 
            new Vector3Int(6, 1, 2010), new Vector3Int(12, 6, 1980), PassPortData.PassportTypes.P, PassportColor.LightRed);
        Debug.Log(PassportCheck(test));
        //Debug.Log((Rules)ttt);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        deltaTime += Time.deltaTime;
        if (deltaTime > 0.0001f)
        {
            deltaTime = 0.0f;
            nextDay();
            Debug.Log(currentDay.ToString());
        }
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

    bool PassportCheck (PassPortData passPortData)
    {
        bool accepted = true;

        // Check if passportData is null
        if (passPortData == null) { return false; }

        // Check if dates are valid
        if (passPortData.ExpirationDate[0] < 1 || passPortData.ExpirationDate[0] > 30) { accepted = false; }
        if (accepted && passPortData.ExpirationDate[1] < 1 || passPortData.ExpirationDate[1] > 12) { accepted = false; }

        if (accepted && passPortData.DateOfCreation[0] < 1 || passPortData.DateOfCreation[0] > 30) { accepted = false; }
        if (accepted && passPortData.DateOfCreation[1] < 1 || passPortData.DateOfCreation[1] > 12) { accepted = false; }

        if (accepted && passPortData.DateOfBirth[0] < 1 || passPortData.DateOfBirth[0] > 30) { accepted = false; }
        if (accepted && passPortData.DateOfBirth[1] < 1 || passPortData.DateOfBirth[1] > 12) { accepted = false; }

        Debug.Log("valid dates " + accepted);

        // Check if expiration date of passport is before the current date
        if (accepted && passPortData.ExpirationDate[2] < currentDay[2]) { accepted = false; }

        if (accepted && 
            passPortData.ExpirationDate[2] == currentDay[2] && 
            passPortData.ExpirationDate[1] < currentDay[1]) { accepted = false; }

        if (accepted && 
            passPortData.ExpirationDate[2] == currentDay[2] && 
            passPortData.ExpirationDate[1] == currentDay[1] && 
            passPortData.ExpirationDate[0] < currentDay[0]) { accepted = false; }

        Debug.Log("valid expiration date " + accepted);

        // Check if the date of creation of passport is after the current date
        if (accepted && passPortData.DateOfCreation[2] > currentDay[2]) { accepted = false; }

        if (accepted && 
            passPortData.DateOfCreation[2] == currentDay[2] &&
            passPortData.DateOfCreation[1] > currentDay[1]) { accepted = false; }

        if (accepted && 
            passPortData.DateOfCreation[2] == currentDay[2] &&
            passPortData.DateOfCreation[1] == currentDay[1] &&
            passPortData.DateOfCreation[0] > currentDay[0]) { accepted = false; }

        Debug.Log("valid date of creation " + accepted);

        // Check if the date of birth of passport is after the current date
        if (accepted && passPortData.DateOfBirth[2] > currentDay[2]) { accepted = false; }

        if (accepted && 
            passPortData.DateOfBirth[2] == currentDay[2] &&
            passPortData.DateOfBirth[1] > currentDay[1]) { accepted = false; }

        if (accepted && 
            passPortData.DateOfBirth[2] == currentDay[2] &&
            passPortData.DateOfBirth[1] == currentDay[1] &&
            passPortData.DateOfBirth[0] > currentDay[0]) { accepted = false; }

        Debug.Log("valid date of birth " + accepted);

        // Check if Passport has the right Color

        if ((int)passPortData.PassType != (int)passPortData.PassColor) { accepted = false; }

        Debug.Log("valid Color " + accepted);

        // Check if country is forbidden
        if (currentRules == Rules.Land)
        {
            if (accepted && currentCountryDenied.Contains(passPortData.Country)) { accepted = false; }
        }

        Debug.Log("valid country " + accepted);

        // Check if passport type is forbidden
        if (currentRules == Rules.PassType)
        {
            if (accepted && currentPpTypesDenied.Contains(passPortData.PassType)) { accepted = false; }
        }

        Debug.Log("valid passport type " + accepted);

        return accepted;
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
