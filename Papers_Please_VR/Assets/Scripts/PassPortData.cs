using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassPortData
{
    public enum PassportTypes
    {
        None,
        P,  
        PC,
        PP,
        PO,
        PD
    }

    public enum PassportColor
    {
        None,
        Red,
        Brown,
        Green,
        LightRed,
        Blue
    }

    public enum Countries
    {
        None,
        Germany,
        Italy,
        France
    }

    public Countries Country { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Vector3Int ExpirationDate { get; set; }
    public Vector3Int DateOfCreation { get; set; }
    public Vector3Int DateOfBirth { get; set; }
    public PassportTypes PassType { get; set; }
    public PassportColor PassColor { get; set; }
    
    public PassPortData()
    {
        Country = Countries.None;
        FirstName = "";
        LastName = "";
        ExpirationDate = new Vector3Int(0, 0, 0);
        DateOfCreation = new Vector3Int(0, 0, 0);
        DateOfBirth = new Vector3Int(0, 0, 0);
        PassType = PassportTypes.None;
        PassColor = PassportColor.None;
    }

    public PassPortData(Countries country, string firstName, string lastName, Vector3Int expirationDate, Vector3Int dateOfCreation, Vector3Int dateOfBirth, PassportTypes passType, PassportColor passColor)
    {
        Country = country;
        FirstName = firstName;
        LastName = lastName;
        ExpirationDate = expirationDate;
        DateOfCreation = dateOfCreation;
        DateOfBirth = dateOfBirth;
        PassType = passType;
        PassColor = passColor;
    }
}
