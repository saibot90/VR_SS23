using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helpful functions for the UI
/// </summary>
public static class UtilitiesUI
{
    /// <summary>
    /// Is used to make all characters the same width
    /// </summary>
    /// <typeparam name="T">Type of the given object</typeparam>
    /// <param name="original">Given object like e.g. String, int, float...</param>
    /// <param name="mspace">Width of the character, do not forget the type of mspace</param>
    /// <param name="enabled">Optional: deactivate the monospace</param>
    /// <returns>A Sting in which all characters have the same width</returns>
    public static string Monospace<T>(this T original, string mspace, bool enabled = true)
    {
        return enabled ? $"<mspace={mspace}>{original}</mspace>" : original.ToString();
    }
}
