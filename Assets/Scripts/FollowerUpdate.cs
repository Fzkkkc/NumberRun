using Character;
using UnityEngine;

namespace FollowObjects
{
    public class FollowerUpdate : MonoBehaviour
    {
        [HideInInspector] public float _forwardSpeed = 460f;

        [SerializeField] private MainNumberCollision _mainNumberCollisionController;
        [SerializeField] private MovementController _movementController;
        
        private FollowerUpdate _followerUpdateController;

        public void Initialize(FollowerUpdate followerUpdateController)
        {
            _followerUpdateController = followerUpdateController;
        }

        private void Start()
        {
            _mainNumberCollisionController.Initialize(_mainNumberCollisionController);
            _movementController.Initialize(_movementController);
        }

        private void Update()
        {
            if(_movementController._canMove)
                 MoveForward();
        }
        
        private void MoveForward()
        {
            if (!_mainNumberCollisionController.IsReducingNumbers && !_mainNumberCollisionController.IsRunOutOfNumbers)
            {
                Vector3 targetPosition = transform.position;
                targetPosition.z += _forwardSpeed * Time.deltaTime;

                transform.position = Vector3.Lerp(transform.position, targetPosition, 0.5f);
            }
        }
    }
}