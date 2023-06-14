using UnityEngine;

public class Computer : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshPro textMeshPro;
    // Start is called before the first frame update
    private void Start()
    {
        GameEvents.current.onInfo += DisplayInfo;
    }

    //Country country,  firstName,  lastName,  expirationDate,  dateOfCreation,  dateOfBirth, PassportTypes passType, PassportColor passColor, wanted
    private void DisplayInfo(PassPortData info)
    {
        textMeshPro.text = info.PassType.ToString() + "     " + info.Country.ToString() + "<br><br>";
        textMeshPro.text += "LastName " + info.LastName + "  Name " + info.FirstName + "<br><br>";
        textMeshPro.text += "Date of Birth " + info.DateOfBirth.x + "/" + info.DateOfBirth.y +"/" + info.DateOfBirth.z + "<br><br>";
        textMeshPro.text += "Expires "+ info.ExpirationDate.x + "/" + info.ExpirationDate.y +"/" + info.ExpirationDate.z + "<br><br>";
        textMeshPro.text += "Issued " + info.DateOfCreation.x + "/" + info.DateOfCreation.y +"/" + info.DateOfCreation.z;
    }

    private void OnDestroy()
    {
        GameEvents.current.onInfo -= DisplayInfo;
    }
}
