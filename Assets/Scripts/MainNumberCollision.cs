using System.Collections;
using Character;
using UnityEngine;
using TMPro;
using DG.Tweening;
using FollowObjects;


public class MainNumberCollision : MonoBehaviour
{
    [HideInInspector] public TextMeshPro TextMeshProMain;
    [HideInInspector] public bool IsReducingNumbers = false;
    [HideInInspector] public bool IsOnFinishLine = false;
    [HideInInspector] public bool IsRunOutOfNumbers = false;
    
    private int _previousNumberMain;
    private bool _canPickUp = true;

    [SerializeField] private MovementController _movementController;
    [SerializeField] private FollowerUpdate _followerUpdateController;
    [SerializeField] private ScenesMethods _scenesMethodsController;
    
    private MainNumberCollision _mainNumberCollisionController;

    public void Initialize(MainNumberCollision mainNumberCollisionController)
    {
        _mainNumberCollisionController = mainNumberCollisionController;
    }
    
    private void Start()
    {
        IsOnFinishLine = false;
        TextMeshProMain = GetComponent<TextMeshPro>();
        _previousNumberMain = int.Parse(TextMeshProMain.text);
        _movementController.Initialize(_movementController);
        _followerUpdateController.Initialize(_followerUpdateController);
        _scenesMethodsController.Initialize(_scenesMethodsController);
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
                FX.Instance.PlayGoalExplosionFX(panelNumber.transform.position);
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
                FX.Instance.PlayGoalExplosionFX(collision.transform.position);
                Destroy(otherNumber.gameObject);
            }
            else if (_previousNumberMain < otherNumber.NumberOtherNumbers)
            {
                _scenesMethodsController.RestartLevel();
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
            _movementController._forwardSpeed = 600f;
            _followerUpdateController._forwardSpeed = 1200f;
            transform.parent.DOMoveX(-54.9f, 1f);
        }
    }

    private void MainNumberCollisionFinishPanel(Collision collision)
    {
        FinishPanel finishPanel = collision.gameObject.GetComponent<FinishPanel>();
        if (finishPanel != null)
        {
            if (_previousNumberMain >= finishPanel.NumberFinishPanel)
            {
                /*_previousNumberMain -= finishPanel.NumberFinishPanel;
                TextMeshProMain.text = _previousNumberMain.ToString();*/
                FX.Instance.PlayGoalExplosionFX(finishPanel.transform.parent.gameObject.transform.position);
                Destroy(finishPanel.transform.parent.gameObject);
                _movementController._forwardSpeed += 100f;
                _followerUpdateController._forwardSpeed += 150;
            }
            else if (_previousNumberMain < finishPanel.NumberFinishPanel)
            {
                IsRunOutOfNumbers = true;
                EndAnimation();
                _movementController._forwardSpeed = 230f;
                _followerUpdateController._forwardSpeed = 460;
            }

            UpdateColors();
        }
    }

    private void PickingNumbersAnimation()
    {
        Vector3 targetScale = new Vector3(1.427f, 1.3516f, _movementController.NumberTransform.localScale.z);
        _movementController.NumberTransform.DOScale(targetScale, 0.3f).OnStart(() => {
                _movementController.NumberTransform.transform.position += new Vector3(0, 3, 0);
            })
            .OnComplete(() => {
                _movementController.NumberTransform.DOScale(_movementController.OriginalScale, 0.3f);
                _movementController.NumberTransform.transform.position -= new Vector3(0, 3, 0);
            });
    }

    private void EndAnimation()
    {
        Transform objectTransform = transform.parent;
        Vector3 currentPosition = objectTransform.position;
        Vector3 newPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z - 130f);
        objectTransform.DOMove(newPosition, 1f);
        objectTransform.DOLocalRotate(new Vector3(0f, 360f, 0f), 1f, RotateMode.FastBeyond360);
    }
}