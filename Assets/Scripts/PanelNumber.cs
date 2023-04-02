using UnityEngine;
using TMPro;

public class PanelNumber : MonoBehaviour
{
    [HideInInspector] public TextMeshPro PanelText;
    [HideInInspector] public int NumberPanel;

    private void Start()
    {
        PanelText = GetComponent<TextMeshPro>();
        NumberPanel = int.Parse(PanelText.text);
    }
}