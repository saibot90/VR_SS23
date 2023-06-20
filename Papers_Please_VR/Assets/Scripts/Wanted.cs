using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Wanted : MonoBehaviour
{
    Material _mFaces;
    [SerializeField] GameObject mPicture;
    public static int MFaceCount { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        NewFaceForWanted();
    }

    void NewFaceForWanted()
    {
        MFaceCount = Random.Range(0, 3);
        _mFaces = Resources.Load("Faces/face1") as Material;
        mPicture.GetComponent<Renderer>().material = _mFaces;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetFace() { return MFaceCount; }
}
