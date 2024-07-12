using UnityEngine;

namespace UnboringFuture
{
    public class CameraMovementSkeleton : MonoBehaviour
    {
        public float movementSpeed = 0.1f;
        public float rotationSpeed = 5f;

        private float desiredRotation = 0f;
        private Quaternion newRotation;

        // Start is called before the first frame update
        void Start()
        {
            newRotation = transform.rotation;
        }

        // Update is called once per frame
        void Update()
        {
            float xAxisValue = Input.GetAxis("Horizontal") * movementSpeed;
            float zAxisValue = Input.GetAxis("Vertical") * movementSpeed;

            transform.Translate(new Vector3(xAxisValue, 0.0f, zAxisValue));

            if (Input.GetKeyUp(KeyCode.Q))
            {
                desiredRotation = -45f;
                newRotation *= Quaternion.Euler(0, desiredRotation, 0);
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                desiredRotation = 45f;
                newRotation *= Quaternion.Euler(0, desiredRotation, 0);
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
