using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkSouls
{
    public class AnimateHandler : MonoBehaviour
    {
        public Animator anim;
        public InputHandler inputHandler;
        public PlayerMovement playerMovement;
        public bool canRotate;

        int vertical;
        int horizontal;

        void Start()
        {
            anim = GetComponent<Animator>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerMovement = GetComponentInParent<PlayerMovement>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
        {
            anim.SetFloat(vertical, ClampMovement(verticalMovement), 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal, ClampMovement(horizontalMovement), 0.1f, Time.deltaTime);
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
            if (!inputHandler.isInteracting)
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