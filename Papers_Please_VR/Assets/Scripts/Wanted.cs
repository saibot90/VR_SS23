using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Wanted : MonoBehaviour
{
    //Variables for the picture of the face
    Material _mFaces;
    [SerializeField] GameObject mPicture;
    public static int MFaceCount { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onNewWantedPerson += NewFaceForWanted;
        NewFaceForWanted();
    }

    //Sets a new face for the wanted poster
    void NewFaceForWanted()
    {
        MFaceCount = Random.Range(1, 33);
        _mFaces = Resources.Load("Faces/face" + MFaceCount) as Material;
        mPicture.GetComponent<Renderer>().material = _mFaces;
    }

    public int GetFace() { return MFaceCount; }
}
