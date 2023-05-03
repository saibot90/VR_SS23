using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EntryVisa : MonoBehaviour
{
    [SerializeField] private string approvedTag;
    [SerializeField] private string deniedTag;
    [SerializeField] private TextMeshProUGUI approveText;

    private Color32 approvedColor = new Color32(0, 185, 59, 255);
    private Color32 deniedColor = new Color32(255, 0, 0, 255);

    bool status = false;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onTriggerPassBack += getVisaStatus;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void getVisaStatus()
    {
        GameEvents.current.VisaStatus(status);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(approvedTag))
        {
            approveText.text = "Approved";
            approveText.color = approvedColor;
            status = true;
        }
        if (other.gameObject.CompareTag(deniedTag))
        {
            approveText.text = "Denied";
            approveText.color = deniedColor;
            status = false;
        }
    }
}
