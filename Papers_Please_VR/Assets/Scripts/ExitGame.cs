using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to exit the Game
/// </summary>
public class ExitGame : MonoBehaviour
{
    /// <summary>
    /// Quits the Game with different handling for Unity editor and build application
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
