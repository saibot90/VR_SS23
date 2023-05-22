using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Nützliche funktionen für die UI
/// </summary>
public static class UtilitiesUI
{
    /// <summary>
    /// Sorgt dafür, dass alle Zeichen die gleiche Breite haben
    /// </summary>
    /// <typeparam name="T">Typ vom übergebenen Objekt</typeparam>
    /// <param name="original">Übergebenes Objekt wie z.B. String, int, float...</param>
    /// <param name="mspace">Breite der Zeichen</param>
    /// <param name="enabled">Optional: deaktiviert den Monospace</param>
    /// <returns>Einen Sting in dem alle Zeichen die gleiche Breite haben</returns>
    public static string Monospace<T>(this T original, float mspace, bool enabled = true)
    {
        return  enabled ? $"<mspace={mspace}px>{original}</mspace>" : original.ToString();
    }

    /// <summary>
    /// Sorgt dafür, dass alle Zeichen die gleiche Breite haben
    /// </summary>
    /// <typeparam name="T">Typ vom übergebenen Objekt</typeparam>
    /// <param name="original">Übergebenes Objekt wie z.B. String, int, float...</param>
    /// <param name="mspace">Breite der Zeichen</param>
    /// <param name="enabled">Optional: deaktiviert den Monospace</param>
    /// <returns>Einen Sting in dem alle Zeichen die gleiche Breite haben</returns>
    public static string Monospace<T>(this T original, string mspace, bool enabled = true)
    {
        return enabled ? $"<mspace={mspace}>{original}</mspace>" : original.ToString();
    }
}
