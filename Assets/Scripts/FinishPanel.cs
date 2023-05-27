using UnityEngine;
using TMPro;

public class FinishPanel : MonoBehaviour
{
    [HideInInspector] public TextMeshPro TextFinishPanel;
    [HideInInspector] public int NumberFinishPanel;
    
    private void Start()
    {
        TextFinishPanel = GetComponent<TextMeshPro>();
        NumberFinishPanel = int.Parse(TextFinishPanel.text);
    }
}
