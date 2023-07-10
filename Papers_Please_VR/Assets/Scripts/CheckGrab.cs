using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Changes the tag of the specific game object
/// </summary>
public class CheckGrab : MonoBehaviour
{
    #region Variables
    
    [SerializeField] private new string tag;
    
    #endregion

    #region Functions
    
    /// <summary>
    /// Sets the Tag to the tag which is defined in Unity
    /// </summary>
    public void TagToTag()
    {
        transform.gameObject.tag = tag;
    }
    
    /// <summary>
    /// Sets the tag of the game object to untagged.
    /// </summary>
    public void TagToUntagged()
    {
        transform.gameObject.tag = "Untagged";
    }
    
    #endregion
}
