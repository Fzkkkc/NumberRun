using Character;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MoveImage : MonoBehaviour
{
    [SerializeField] private MovementController _movementController;

    public Image image;
    public float moveDistance = 500f;
    public float moveDuration = 2f;

    private Tweener _moveTween;
    private bool _movingRight = true;

    private void Start()
    {
        _movementController.Initialize(_movementController);
        image.rectTransform.anchoredPosition = new Vector2(-moveDistance, 0f);
    }

    private void OnDisable()
    {
        StopMoveAnimation();
    }

    private void Update()
    {
        if (!_movementController._canMove && _moveTween == null)
        {
            StartMoveAnimation();
            image.gameObject.SetActive(true);
        }
        else if (_movementController._canMove)
        {
            StopMoveAnimation();
            image.gameObject.SetActive(false);
        }
    }

    private void StartMoveAnimation()
    {
        float targetPosX = _movingRight ? moveDistance : -moveDistance;
        _moveTween = image.rectTransform.DOAnchorPosX(targetPosX, moveDuration)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo)
            .OnComplete(ChangeDirection);
    }

    private void StopMoveAnimation()
    {
        _moveTween.Kill();
        _moveTween = null;
    }

    private void ChangeDirection()
    {
        _movingRight = !_movingRight;
        StartMoveAnimation();
    }
}