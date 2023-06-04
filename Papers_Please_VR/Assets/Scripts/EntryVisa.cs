using UnityEngine;
using TMPro;

public class EntryVisa : MonoBehaviour
{
    [SerializeField] private string approvedTag;
    [SerializeField] private string deniedTag;
    [SerializeField] private TextMeshProUGUI approveText;

    private readonly Color32 _approvedColor = new Color32(0, 185, 59, 255);
    private readonly Color32 _deniedColor = new Color32(255, 0, 0, 255);

    private bool _status = false; // zu CheckStatus

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onTriggerVisaCheck += GetVisaStatus;
    }

    private void GetVisaStatus()
    {
        GameEvents.current.VisaStatus(_status);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(approvedTag))
        {
            approveText.text = "Approved";
            approveText.color = _approvedColor;
            _status = true;
        }
        if (other.gameObject.CompareTag(deniedTag))
        {
            approveText.text = "Denied";
            approveText.color = _deniedColor;
            _status = false;
        }
    }
    
    private void OnDestroy()
    {
        GameEvents.current.onTriggerVisaCheck -= GetVisaStatus;
    }
}
