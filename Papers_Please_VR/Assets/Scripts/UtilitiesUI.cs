using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// N�tzliche funktionen f�r die UI
/// </summary>
public static class UtilitiesUI
{
    /// <summary>
    /// Sorgt daf�r, dass alle Zeichen die gleiche Breite haben
    /// </summary>
    /// <typeparam name="T">Typ vom �bergebenen Objekt</typeparam>
    /// <param name="original">�bergebenes Objekt wie z.B. String, int, float...</param>
    /// <param name="mspace">Breite der Zeichen</param>
    /// <param name="enabled">Optional: deaktiviert den Monospace</param>
    /// <returns>Einen Sting in dem alle Zeichen die gleiche Breite haben</returns>
    public static string Monospace<T>(this T original, float mspace, bool enabled = true)
    {
        return  enabled ? $"<mspace={mspace}px>{original}</mspace>" : original.ToString();
    }

    /// <summary>
    /// Sorgt daf�r, dass alle Zeichen die gleiche Breite haben
    /// </summary>
    /// <typeparam name="T">Typ vom �bergebenen Objekt</typeparam>
    /// <param name="original">�bergebenes Objekt wie z.B. String, int, float...</param>
    /// <param name="mspace">Breite der Zeichen</param>
    /// <param name="enabled">Optional: deaktiviert den Monospace</param>
    /// <returns>Einen Sting in dem alle Zeichen die gleiche Breite haben</returns>
    public static string Monospace<T>(this T original, string mspace, bool enabled = true)
    {
        return enabled ? $"<mspace={mspace}>{original}</mspace>" : original.ToString();
    }
}
