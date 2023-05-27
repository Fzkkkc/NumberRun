using UnityEngine;

namespace Character
{
    public class MovementController : MonoBehaviour
    {
        [HideInInspector] public float _forwardSpeed = 230f;
        [HideInInspector] public bool _canMove;
        [HideInInspector] public Vector3 OriginalScale;
        
        public Transform NumberTransform;
        
        private float _speed = 0.7f;
        private float _xLimit = 140f;
        private Vector3 _firstPos;
        private Vector3 _endPos;
        
        [SerializeField] private MainNumberCollision _mainNumberCollision;
        
        private MovementController _movementController;
        
        public void Initialize(MovementController movementController)
        {
            _movementController = movementController;
        }
        
        private void Start()
        {
            _canMove = false;
            _mainNumberCollision.Initialize(_mainNumberCollision);
            OriginalScale = transform.localScale;
            _forwardSpeed = 230f;
        }

        private void Update()
        {
            /*if (Input.GetMouseButtonDown(0))
                _canMove = true;*/
            
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                    _canMove = true;
            }

            if (_canMove)
            {
                if(!_mainNumberCollision.IsOnFinishLine)
                    Dragging();
                
                MoveForward();
            }
        }

        private void Dragging()
        {
            if (!_mainNumberCollision.IsRunOutOfNumbers)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        _firstPos = touch.position;
                    }
                    else if (touch.phase == TouchPhase.Moved)
                    {
                        _endPos = touch.position;

                        float farkX = _endPos.x - _firstPos.x;

                        if (Mathf.Abs(transform.position.x + farkX * Time.deltaTime * _speed) <= _xLimit)
                        {
                            transform.Translate(farkX * Time.deltaTime * _speed, 0, 0);
                        }
                        else
                        {
                            float newXPos = Mathf.Clamp(transform.position.x + farkX * Time.deltaTime * _speed, -_xLimit,
                                _xLimit);
                            transform.position = new Vector3(newXPos, transform.position.y, transform.position.z);
                        }
                    }
                }
            }
        }

        private void MoveForward()
        {
            if (!_mainNumberCollision.IsReducingNumbers && !_mainNumberCollision.IsRunOutOfNumbers)
            {
                transform.Translate(Vector3.forward * _forwardSpeed * Time.deltaTime);
            }
        }
    }
}