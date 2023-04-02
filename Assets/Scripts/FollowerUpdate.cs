using UnityEngine;

namespace FollowObjects
{
    public class FollowerUpdate : MonoBehaviour
    {
        [SerializeField] private float _forwardSpeed = 400f;
        
        private MainNumberCollision _mainNumberCollision;

        private void Start()
        {
            _mainNumberCollision = FindObjectOfType<MainNumberCollision>();
        }

        private void Update()
        {
            MoveForward();

            if (_mainNumberCollision.IsOnFinishLine)
                _forwardSpeed = 1200f;
        }
        
        private void MoveForward()
        {
            if (!_mainNumberCollision.IsReducingNumbers && !_mainNumberCollision.IsRunOutOfNumbers)
            {
                Vector3 targetPosition = transform.position;
                targetPosition.z += _forwardSpeed * Time.deltaTime;

                transform.position = Vector3.Lerp(transform.position, targetPosition, 0.5f);
            }
        }
    }
}