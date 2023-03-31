using System.Collections;
using UnityEngine;
using TMPro;


public class ChangeNumber : MonoBehaviour
{
    public TextMeshPro TextMeshProMain;
    public int PreviousNumberMain;
    private bool _canPickUp = true;
    public bool IsReducingNumbers = false;
    
    private void Start()
    {
        TextMeshProMain = GetComponent<TextMeshPro>();
        PreviousNumberMain = int.Parse(TextMeshProMain.text);
        UpdateColors();
    }

    private void OnCollisionEnter(Collision collision)
    {
        MainNumberCollisionOtherNumber(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        PanelNumber panelNumber = collision.gameObject.GetComponent<PanelNumber>();
        if (panelNumber != null)
        {
            if (!IsReducingNumbers)
            {
                StartCoroutine(ReduceNumbers(panelNumber));
            }
        }
    }

    private IEnumerator ReduceNumbers(PanelNumber panelNumber)
    {
        IsReducingNumbers = true;

        while (PreviousNumberMain > 0 && panelNumber.NumberPanel > 0)
        {
            PreviousNumberMain--;
            panelNumber.NumberPanel--;
            TextMeshProMain.text = PreviousNumberMain.ToString();
            panelNumber.TextMeshProPanel.text = panelNumber.NumberPanel.ToString();
            if (panelNumber.NumberPanel == 0)
            {
                Destroy(panelNumber.transform.parent.gameObject);
            }

            yield return new WaitForSeconds(0.03f);
        }

        IsReducingNumbers = false;
        UpdateColors();
    }
    
    private void UpdateColors()
    {
        OtherNumber[] others = OtherNumber.GetAllOtherNumbers();

        foreach (OtherNumber other in others)
        {
            if (other.CanPickUp && PreviousNumberMain < other.NumberOtherNumbers)
            {
                other.TextMeshProOtherNumbers.color = Color.red;
            }
            else if (other.CanPickUp && PreviousNumberMain > other.NumberOtherNumbers)
            {
                other.TextMeshProOtherNumbers.color = new Color(22f / 255f, 0f, 255f / 255f, 1f);
            }
        }
    }

    private void MainNumberCollisionOtherNumber(Collision collision)
    {
        OtherNumber other = collision.gameObject.GetComponent<OtherNumber>();
        if (other != null)
        {
            if (_canPickUp && PreviousNumberMain >= other.NumberOtherNumbers && other.CanPickUp)
            {
                PreviousNumberMain += other.NumberOtherNumbers;
                TextMeshProMain.text = PreviousNumberMain.ToString();
                other.CanPickUp = false;
                Destroy(other.gameObject);
            }

            UpdateColors();
        }
    }
}