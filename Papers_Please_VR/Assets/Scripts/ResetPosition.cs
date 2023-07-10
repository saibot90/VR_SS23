using UnityEngine;

/// <summary>
/// Resets the position of colliding objects
/// </summary>
public class ResetPosition : MonoBehaviour
{
    #region Variables
    
    [SerializeField] private GameObject resetPoint;
    [SerializeField] private string resetTag;
    private static Vector3 _offset = new Vector3(0, 0, 0);
    
    #endregion
    
    #region Functions
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(resetTag)) return;
        other.GetComponent<Rigidbody>().position = Vector3.zero;
        other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        other.transform.position = resetPoint.transform.position + _offset;
        SetOffset();
    }

    /// <summary>
    /// Sets an offset to each object which resets
    /// </summary>
    private static void SetOffset()
    {
        _offset.z += 0.2f;
        if (_offset.z > 1.2)
        {
            _offset.z = 0;
        }
    }
    
    #endregion
}
