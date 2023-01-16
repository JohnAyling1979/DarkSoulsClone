using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkSouls
{
    public class PlayerMovement : MonoBehaviour
    {
        public Rigidbody myRigidbody;
        GameObject normalCamera;
        Transform myTransform;
        Transform cameraObject;
        InputHandler inputHandler;
        Vector3 moveDirection;
        AnimateHandler animateHandler;
        PlayerManager playerManager;

        [Header("Movement Stats")]
        [SerializeField]
        float movementSpeed = 5;
        [SerializeField]
        float sprintSpeed = 10;
        [SerializeField]
        float rotationSpeed = 10;

        void Start()
        {
            playerManager = GetComponent<PlayerManager>();
            myRigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            animateHandler = GetComponentInChildren<AnimateHandler>();
            cameraObject = Camera.main.transform;
            myTransform = transform;
        }

        #region Movement

        Vector3 normalVector;
        Vector3 targetPosition;

        public void HandleRotation(float delta)
        {
            if (animateHandler.canRotate)
            {
                Vector3 targetDir = cameraObject.forward * inputHandler.vertical + cameraObject.right * inputHandler.horizontal;

                targetDir.Normalize();
                targetDir.y = 0;

                if (targetDir == Vector3.zero)
                {
                    targetDir = myTransform.forward;
                }

                myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(targetDir), rotationSpeed * delta);
            }
        }

        public void HandleMovement(float delta)
        {
            if (inputHandler.rollFlag)
            {
                return;
            }

            moveDirection = cameraObject.forward * inputHandler.vertical + cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;

            if (inputHandler.sprintFlag)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
            }

            moveDirection *= movementSpeed;

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            myRigidbody.velocity = projectedVelocity;

            animateHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);
        }

        public void HandleRollingAndSprinting(float delta)
        {
            if (animateHandler.anim.GetBool("isInteracting"))
            {
                return;
            }

            if (inputHandler.rollFlag)
            {
                moveDirection = cameraObject.forward * inputHandler.vertical + cameraObject.right * inputHandler.horizontal;
                moveDirection.Normalize();
                moveDirection.y = 0;

                if (inputHandler.moveAmount > 0)
                {
                    animateHandler.PlayTargetAnimation("Rolling", true);

                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);

                    transform.rotation = rollRotation;
                }
                else
                {
                    animateHandler.PlayTargetAnimation("StepBack", true);
                }
            }
        }

        #endregion
    }
}