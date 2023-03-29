using UnityEngine;
using TMPro;

public class ChangeNumber : MonoBehaviour
{
    public TextMeshPro TextMeshPro;
    private int _previousNumber;
    private bool _canPickUp = true;
    private OtherNumber _otherNumberScript;

    private void Start()
    {
        TextMeshPro = GetComponent<TextMeshPro>();
        _previousNumber = int.Parse(TextMeshPro.text);
        _otherNumberScript = GetComponent<OtherNumber>();
        UpdateColors();
    }

    private void OnCollisionEnter(Collision collision)
    {
        OtherNumber other = collision.gameObject.GetComponent<OtherNumber>();
        if (other != null)
        {
            if (_canPickUp && _previousNumber >= other.Number && other.CanPickUp)
            {
                _previousNumber += other.Number;
                TextMeshPro.text = _previousNumber.ToString();
                other.CanPickUp = false;
                Destroy(other.gameObject);
            }

            UpdateColors();
        }
    }

    private void UpdateColors()
    {
 
        OtherNumber[] others = FindObjectsOfType<OtherNumber>();

        foreach (OtherNumber other in others)
        {
            if (other.CanPickUp && _previousNumber < other.Number)
            {
                other.TextMeshPro.color = Color.red;
            }
            else
            {
                other.TextMeshPro.color = new Color(22f / 255f, 0f, 255f / 255f, 1f);
            }
        }
    }

    public void SetCanPickUp(bool value)
    {
        _canPickUp = value;
        UpdateColors();
    }
}