using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkSouls
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        PlayerControls inputactions;
        CameraHandler cameraHandler;

        Vector2 movementInput;
        Vector2 cameraInput;

        void Awake()
        {
            cameraHandler = CameraHandler.singleton;
        }

        public void OnEnable()
        {
            if (inputactions == null)
            {
                inputactions = new PlayerControls();

                inputactions.PlayerMovement.Movement.performed += input => movementInput = input.ReadValue<Vector2>();
                inputactions.PlayerMovement.Camera.performed += input => cameraInput = input.ReadValue<Vector2>();
            }

            inputactions.Enable();
        }

        void FixedUpdate()
        {
            if (cameraHandler != null)
            {
                float delta = Time.fixedDeltaTime;

                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
            }
        }

        private void OnDisable()
        {
            inputactions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
        }

        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }
    }
}
