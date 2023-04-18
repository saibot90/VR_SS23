using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using TMPro;

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

    private Rules currentRules = Rules.None;

    // Start is called before the first frame update
    void Start()
    {
        PassPortData test = new PassPortData();
        test.Country = "Germany";
        Debug.Log(test.Country);
    }

    // Update is called once per frame
    void Update()
    {

    }

}
