using UnityEngine;

namespace FollowObjects
{
    public class FollowerUpdate : MonoBehaviour
    {
        [SerializeField] private float _forwardSpeed = 200f;
        private ChangeNumber _changeNumber;

        private void Start()
        {
            _changeNumber = FindObjectOfType<ChangeNumber>();
        }

        private void Update()
        {
            if(_changeNumber.IsReducingNumbers == false)
                MoveForward();
        }

        private void MoveForward()
        {
            Vector3 targetPosition = transform.position;
            targetPosition.z += _forwardSpeed * Time.deltaTime;

            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.5f);
        }
    }
}