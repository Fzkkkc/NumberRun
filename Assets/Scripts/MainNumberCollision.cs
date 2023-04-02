using System.Collections;
using Character;
using UnityEngine;
using TMPro;
using DG.Tweening;


public class MainNumberCollision : MonoBehaviour
{
    [HideInInspector] public TextMeshPro TextMeshProMain;
    [HideInInspector] public bool IsReducingNumbers = false;
    [HideInInspector] public bool IsOnFinishLine = false;
    [HideInInspector] public bool IsRunOutOfNumbers = false;
    
    private int _previousNumberMain;
    private bool _canPickUp = true;
    
    private void Start()
    {
        TextMeshProMain = GetComponent<TextMeshPro>();
        _previousNumberMain = int.Parse(TextMeshProMain.text);
        UpdateColors();
    }

    private void OnCollisionEnter(Collision collision)
    {
        MainNumberCollisionOtherNumber(collision);
        MainNumberCollisionFinishPanel(collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        MainNumberEnterFinishLine(other);
    }

    private void OnCollisionStay(Collision collision)
    {
        MainNumberCollisionPanel(collision);
    }

    private IEnumerator ReduceNumbers(PanelNumber panelNumber)
    {
        IsReducingNumbers = true;

        while (_previousNumberMain > 0 && panelNumber.NumberPanel > 0)
        {
            _previousNumberMain--;
            panelNumber.NumberPanel--;
            TextMeshProMain.text = _previousNumberMain.ToString();
            panelNumber.PanelText.text = panelNumber.NumberPanel.ToString();
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

        foreach (OtherNumber otherNumber in others)
        {
            if (otherNumber.CanPickUp && _previousNumberMain < otherNumber.NumberOtherNumbers)
            {
                otherNumber.TextMeshProOtherNumbers.color = Color.red;
            }
            else if (otherNumber.CanPickUp && _previousNumberMain > otherNumber.NumberOtherNumbers)
            {
                otherNumber.TextMeshProOtherNumbers.color = new Color(22f / 255f, 0f, 255f / 255f, 1f);
            }
        }
    }

    private void MainNumberCollisionOtherNumber(Collision collision)
    {
        OtherNumber otherNumber = collision.gameObject.GetComponent<OtherNumber>();
        if (otherNumber != null)
        {
            if (_canPickUp && _previousNumberMain >= otherNumber.NumberOtherNumbers && otherNumber.CanPickUp)
            {
                _previousNumberMain += otherNumber.NumberOtherNumbers;
                TextMeshProMain.text = _previousNumberMain.ToString();
                PickingNumbersAnimation();
                otherNumber.CanPickUp = false;
                Destroy(otherNumber.gameObject);
            }

            UpdateColors();
        }
    }

    private void MainNumberCollisionPanel(Collision collision)
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
    
    private void MainNumberEnterFinishLine(Collider other)
    {
        if (other.gameObject.CompareTag("FinishLine"))
        {
            IsOnFinishLine = true;
            MovementController movementController = FindObjectOfType<MovementController>();
            movementController._forwardSpeed = 600f;
        }
    }

    private void MainNumberCollisionFinishPanel(Collision collision)
    {
        FinishPanel finishPanel = collision.gameObject.GetComponent<FinishPanel>();
        if (finishPanel != null)
        {
            if (_previousNumberMain >= finishPanel.NumberFinishPanel)
            {
                _previousNumberMain -= finishPanel.NumberFinishPanel;
                TextMeshProMain.text = _previousNumberMain.ToString();
                Destroy(finishPanel.transform.parent.gameObject);
            }
            else if (_previousNumberMain < finishPanel.NumberFinishPanel)
            {
                IsRunOutOfNumbers = true;
            }

            UpdateColors();
        }
    }

    private void PickingNumbersAnimation()
    {
        MovementController movementController = FindObjectOfType<MovementController>();
        Vector3 targetScale = new Vector3(1.427f, 1.3516f, movementController.NumberTransform.localScale.z);
        movementController.NumberTransform.DOScale(targetScale, 0.3f).OnStart(() => {
                movementController.NumberTransform.transform.position += new Vector3(0, 3, 0);
            })
            .OnComplete(() => {
                movementController.NumberTransform.DOScale(movementController.OriginalScale, 0.3f);
                movementController.NumberTransform.transform.position -= new Vector3(0, 3, 0);
            });
    }
}