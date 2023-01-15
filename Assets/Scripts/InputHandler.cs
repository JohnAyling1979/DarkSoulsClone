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
        public bool b_Input;
        public bool rollFlag;
        public bool isInteracting;

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
            HandleRollInput(delta);
        }

        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        private void HandleRollInput(float delta)
        {
            b_Input = inputactions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

            if (b_Input)
            {
                rollFlag = true;
            }
        }
    }
}
