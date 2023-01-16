using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkSouls
{
    public class PlayerManager : MonoBehaviour
    {
        [Header("Player Flags")]
        public bool isInteracting;
        public bool isInAir;
        public bool isGrounded;

        InputHandler inputHandler;
        Animator anim;
        CameraHandler cameraHandler;
        PlayerMovement playerMovement;

        void Start()
        {
            cameraHandler = CameraHandler.singleton;

            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerMovement = GetComponent<PlayerMovement>();
        }

        void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = anim.GetBool("isInteracting");


            inputHandler.TickInput(delta);

            playerMovement.HandleMovement(delta);
            playerMovement.HandleRotation(delta);
            playerMovement.HandleRollingAndSprinting(delta);
            playerMovement.HandleFalling(delta);
        }

        void FixedUpdate()
        {
            if (cameraHandler != null)
            {
                float delta = Time.fixedDeltaTime;

                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }
        }

        void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;
            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;

            if (isInAir)
            {
                playerMovement.inAirTimer += Time.deltaTime;
            }
        }
    }
}