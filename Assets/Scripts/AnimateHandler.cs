using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkSouls
{
    public class AnimateHandler : MonoBehaviour
    {
        public Animator anim;
        InputHandler inputHandler;
        PlayerMovement playerMovement;
        PlayerManager playerManager;
        public bool canRotate;

        int vertical;
        int horizontal;

        void Start()
        {
            inputHandler = GetComponentInParent<InputHandler>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerMovement = GetComponentInParent<PlayerMovement>();
            anim = GetComponent<Animator>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            float v = ClampMovement(verticalMovement);
            float h = ClampMovement(horizontalMovement);

            if (v != 0 && isSprinting)
            {
                v = 2;
            }

            anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void CanRotate()
        {
            canRotate = true;
        }

        public void StopRotattion()
        {
            canRotate = false;
        }

        private float ClampMovement(float movement)
        {
            float m = 0;

            if (movement > 0 && movement < 0.55f)
            {
                m = 0.5f;
            }
            else if (movement > 0.55f)
            {
                m = 1;
            }
            else if (movement < 0 && movement > -0.55f)
            {
                m = -0.5f;
            }
            else if (movement < -0.55f)
            {
                m = -1;
            }

            return m;
        }

        public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnimation, 0.2f);
        }

        private void OnAnimatorMove()
        {
            if (!playerManager.isInteracting)
            {
                return;
            }

            float delta = Time.deltaTime;

            playerMovement.myRigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerMovement.myRigidbody.velocity = velocity;
        }
    }
}