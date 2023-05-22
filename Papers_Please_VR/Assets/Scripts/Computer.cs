using UnityEngine;

public class Computer : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshPro textMeshPro;
    // Start is called before the first frame update
    private void Start()
    {
        GameEvents.current.onInfo += DisplayInfo;
    }

    private void DisplayInfo(string info)
    {
        textMeshPro.text = info;
    }

    private void OnDestroy()
    {
        GameEvents.current.onInfo -= DisplayInfo;
    }
}
