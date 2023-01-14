using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkSouls
{
    public class AnimateHandler : MonoBehaviour
    {
        public Animator anim;
        public bool canRotate;

        int vertical;
        int horizontal;

        void Start()
        {
            anim = GetComponent<Animator>();
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
    }
}