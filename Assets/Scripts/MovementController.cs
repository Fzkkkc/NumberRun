using UnityEngine;

namespace Character
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private float _speed = 0.7f;
        [SerializeField] private float _xLimit = 140f;
        public Transform NumberTransform;

        public float _forwardSpeed = 200f;
        
        private Vector3 _firstPos;
        private Vector3 _endPos;
        private MainNumberCollision _mainNumberCollision;
        
        [HideInInspector] public Vector3 OriginalScale;
        
        private void Start()
        {
            _mainNumberCollision = FindObjectOfType<MainNumberCollision>();
            OriginalScale = transform.localScale;
        }

        private void Update()
        {
            Dragging();
            MoveForward();
        }

        private void Dragging()
        {
            if(!_mainNumberCollision.IsRunOutOfNumbers)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _firstPos = Input.mousePosition;
                }
                else if (Input.GetMouseButton(0))
                {
                    _endPos = Input.mousePosition;

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

                if (Input.GetMouseButtonUp(0))
                {
                    _firstPos = Vector3.zero;
                    _endPos = Vector3.zero;
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