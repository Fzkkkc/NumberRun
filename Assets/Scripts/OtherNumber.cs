using UnityEngine;
using TMPro;

    public class OtherNumber : MonoBehaviour
    {
        public TextMeshPro TextMeshPro;
        public int Number;
        public bool CanPickUp;
        public Color Color;

        private void Start()
        {
            TextMeshPro = GetComponent<TextMeshPro>();
            Number = int.Parse(TextMeshPro.text);
            CanPickUp = true;
            Color = new Color(22f / 255f, 0f, 255f / 255f, 1f);
        }
    }