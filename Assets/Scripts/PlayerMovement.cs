using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkSouls
{
    public class PlayerMovement : MonoBehaviour
    {
        public Rigidbody myRigidbody;
        public float inAirTimer;

        GameObject normalCamera;
        Transform myTransform;
        Transform cameraObject;
        InputHandler inputHandler;
        Vector3 moveDirection;
        AnimateHandler animateHandler;
        PlayerManager playerManager;
        LayerMask ignoreForGroundCheck;

        [Header("Ground & Air Detection")]
        [SerializeField]
        float groundDetectionRayStartPoint = 0.5f;
        [SerializeField]
        float minimumDistanceNeededToBeginFall = 1f;

        [Header("Movement Stats")]
        [SerializeField]
        float movementSpeed = 5;
        [SerializeField]
        float sprintSpeed = 10;
        [SerializeField]
        float rotationSpeed = 10;
        [SerializeField]
        float fallingSpeed = 80;

        void Start()
        {
            playerManager = GetComponent<PlayerManager>();
            myRigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            animateHandler = GetComponentInChildren<AnimateHandler>();
            cameraObject = Camera.main.transform;
            myTransform = transform;

            playerManager.isGrounded = true;
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
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
            if (inputHandler.rollFlag || playerManager.isInAir)
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

            moveDirection *= speed;

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

        public void HandleFalling(float delta)
        {
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionRayStartPoint;

            if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
            {
                moveDirection = Vector3.zero;
            }

            if (playerManager.isInAir)
            {
                myRigidbody.AddForce(-Vector3.up * fallingSpeed);
                myRigidbody.AddForce(moveDirection * fallingSpeed / 15f);
            }

            targetPosition = myTransform.position;

            Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);

            if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPosition.y = tp.y;

                if (playerManager.isInAir)
                {
                    if (inAirTimer > 0.5f)
                    {
                        animateHandler.PlayTargetAnimation("Land", true);
                    }
                    else
                    {
                        animateHandler.PlayTargetAnimation("Locomotion", false);
                    }

                    playerManager.isInAir = false;
                    inAirTimer = 0;
                }

                myTransform.position = targetPosition;
            }
            else
            {
                if (playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                if (playerManager.isInAir == false)
                {
                    if (playerManager.isInteracting == false)
                    {
                        animateHandler.PlayTargetAnimation("Falling", true);
                    }

                    playerManager.isInAir = true;
                }
            }
        }

        #endregion
    }
}