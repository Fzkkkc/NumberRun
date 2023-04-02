using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class OtherNumber : MonoBehaviour
{
    [HideInInspector] public TextMeshPro TextMeshProOtherNumbers;
    [HideInInspector]public int NumberOtherNumbers;
    [HideInInspector] public bool CanPickUp;
    [HideInInspector] public Color ColorOtherNumbers;
    
    private static List<OtherNumber> _allOtherNumbers = new List<OtherNumber>();

    private void Start()
    {
        TextMeshProOtherNumbers = GetComponent<TextMeshPro>();
        NumberOtherNumbers = int.Parse(TextMeshProOtherNumbers.text);
        CanPickUp = true;
        ColorOtherNumbers = new Color(22f / 255f, 0f, 255f / 255f, 1f);
        _allOtherNumbers.Add(this);
    }
    
    public static OtherNumber[] GetAllOtherNumbers()
    {
        return _allOtherNumbers.ToArray();
    }
}