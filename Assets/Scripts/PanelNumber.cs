using UnityEngine;
using TMPro;

public class PanelNumber : MonoBehaviour
{
    public TextMeshPro TextMeshProPanel;
    public int NumberPanel;

    private void Start()
    {
        TextMeshProPanel = GetComponent<TextMeshPro>();
        NumberPanel = int.Parse(TextMeshProPanel.text);
    }
}