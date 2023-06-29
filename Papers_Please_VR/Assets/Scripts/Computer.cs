using UnityEngine;

public class Computer : MonoBehaviour
{
    [SerializeField] GameObject m_Picture;
    [SerializeField] private TMPro.TextMeshPro textMeshPro;
    Material m_Faces;

    private Person person;
    // Start is called before the first frame update
    private void Start()
    {
        //person = new Person();
        GameEvents.current.onInfo += DisplayInfo;
        //int num = person.GetFaceofPerson();
    }

    //Country country,  firstName,  lastName,  expirationDate,  dateOfCreation,  dateOfBirth, PassportTypes passType, PassportColor passColor, wanted
    private void DisplayInfo(PassPortData info)
    {
        string facePath = "Faces/face" + PassPort.mFaceIndex;
        m_Faces = Resources.Load(facePath) as Material;
        m_Picture.GetComponent<Renderer>().material = m_Faces; 
        textMeshPro.text = info.PassType.ToString() + "     " + info.Country.ToString() + "<br><br>";
        textMeshPro.text += "LastName " + info.LastName + "<br><br>" + "Name " + info.FirstName + "<br><br>";
        textMeshPro.text += "Date of Birth " + info.DateOfBirth.x + "/" + info.DateOfBirth.y +"/" + info.DateOfBirth.z + "<br><br>";
        textMeshPro.text += "Expires "+ info.ExpirationDate.x + "/" + info.ExpirationDate.y +"/" + info.ExpirationDate.z + "<br><br>";
        textMeshPro.text += "Issued " + info.DateOfCreation.x + "/" + info.DateOfCreation.y +"/" + info.DateOfCreation.z;
    }

    private void OnDestroy()
    {
        GameEvents.current.onInfo -= DisplayInfo;
    }
}
