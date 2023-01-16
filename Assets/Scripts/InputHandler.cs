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
        public bool rb_Input;
        public bool rt_Input;
        public bool rollFlag;
        public bool sprintFlag;
        public float rollInputTimer;

        PlayerControls inputactions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;

        Vector2 movementInput;
        Vector2 cameraInput;

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

        public void Start()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
        }

        private void OnDisable()
        {
            inputactions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
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
                rollInputTimer += delta;
                sprintFlag = true;
            } else {
                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }

        private void HandleAttackInput(float delta)
        {
            inputactions.PlayerActions.RB.performed += i => rb_Input = true;
            inputactions.PlayerActions.RT.performed += i => rt_Input = true;

            if (rb_Input)
            {
                playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
            }

            if (rt_Input)
            {
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }
        }
    }
}
