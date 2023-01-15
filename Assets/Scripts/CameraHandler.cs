using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkSouls
{
    public class CameraHandler : MonoBehaviour
    {
        public Transform targetTransform;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        Vector3 cameraTransformPosition  = Vector3.zero;
        Vector3 cameraFollowVelocity = Vector3.zero;
        LayerMask ignoreLayers;

        public static CameraHandler singleton;

        public float lookSpeed = 0.1f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;
        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minimumCollisionOffset = 0.2f;

        float targetPosition;
        float defaultPosition;
        float lookAngle;
        float pivotAngle;
        float minimumPivot = -35;
        float maximumPivot = 35;

        void Awake() {
            singleton = this;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        }

        public void FollowTarget(float delta)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed
            );

            HandleCameraCollisions(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            lookAngle += (mouseXInput * lookSpeed) / delta;
            pivotAngle -= (mouseYInput * pivotSpeed) / delta;

            pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);

            transform.rotation = targetRotation;


            rotation = Vector3.zero;
            rotation.x = pivotAngle;

            targetRotation = Quaternion.Euler(rotation);

            cameraPivotTransform.localRotation = targetRotation;
        }

        void HandleCameraCollisions(float delta)
        {
            targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (
                Physics.SphereCast(
                    cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers
                )
            ) {
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);

                targetPosition = -(dis - cameraCollisionOffset);
            }

            if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
            {
                targetPosition = -minimumCollisionOffset;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);

            cameraTransform.localPosition = cameraTransformPosition;
        }
    }
}