using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// It defines the data of a passport
/// </summary>
public class PassPortData
{
    #region Enums
    
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
        Angola,
        Algeria,
        Italy,
        Canada,
        Russia,
        Finland,
        Australia,
        India,
        Congo,
        Brazil,
        Mexico,
        England,
        USA,
        Bolivia,
        Japan,
        SouthKorea,
        Turkey,
        China,
        Spain,
        France
    }
    
    #endregion

    #region Variables
    
    public Countries Country { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Vector3Int ExpirationDate { get; set; }
    public Vector3Int DateOfCreation { get; set; }
    public Vector3Int DateOfBirth { get; set; }
    public PassportTypes PassType { get; set; }
    public PassportColor PassColor { get; set; }
    public bool Wanted { get; set; }
    
    #endregion
    
    #region Constructors
    
    /// <summary>
    /// Constructor of a passport data object with standard values
    /// </summary>
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
        Wanted = false;
    }

    /// <summary>
    /// Constructor of a passport data object with given values
    /// </summary>
    /// <param name="country">Country of the passport <see cref="Countries"/></param>
    /// <param name="firstName">First name of the person</param>
    /// <param name="lastName">Last name of the person</param>
    /// <param name="expirationDate">Expiration date of the passport</param>
    /// <param name="dateOfCreation">Creation date of the passport</param>
    /// <param name="dateOfBirth">Date of birth of the person</param>
    /// <param name="passType">Type of the passport <see cref="PassportTypes"/></param>
    /// <param name="passColor">Color of the passport <see cref="PassportColor"/></param>
    /// <param name="wanted">Is the Person wanted or not</param>
    public PassPortData(Countries country, string firstName, string lastName, Vector3Int expirationDate, Vector3Int dateOfCreation, Vector3Int dateOfBirth, PassportTypes passType, PassportColor passColor, bool wanted)
    {
        Country = country;
        FirstName = firstName;
        LastName = lastName;
        ExpirationDate = expirationDate;
        DateOfCreation = dateOfCreation;
        DateOfBirth = dateOfBirth;
        PassType = passType;
        PassColor = passColor;
        Wanted = wanted;
    }
    
    #endregion
}
