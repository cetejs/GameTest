using GameFramework.EventPoolService;
using GameFramework.Generic;
using UnityEngine;

namespace Escape
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed = 10f;
        [SerializeField]
        private float turnSpeed = 10f;
        [SerializeField]
        private float sprintMultiplier = 1.5f;
        [SerializeField]
        private float slowMultiplier = 0.5f;
        [SerializeField]
        private EventId dieEventId;
        [SerializeField]
        private PlayerEnergy energy;
        private PlayerInputs input;
        private CharacterController cc;
        private ScreenFadeInOut fade;

        private void Awake()
        {
            input = GetComponentInParent<PlayerInputs>();
            cc = GetComponent<CharacterController>();
            Global.GetService<EventManager>().Register((int) dieEventId, Die);
            energy.ResumeToMax();
        }

        private void OnDestroy()
        {
            if (Global.IsApplicationQuitting)
            {
                return;
            }

            Global.GetService<EventManager>().Unregister((int) dieEventId, Die);
        }

        private void Update()
        {
            Move();
            ResumeEnergy();
        }

        private void Move()
        {
            if (input.Move.sqrMagnitude > 0.01f)
            {
                Vector2 move = input.Move.normalized;
                Vector3 velocity = new Vector3(move.x, 0f, move.y) * GetMoveSpeed();
                cc.SimpleMove(velocity);

                Quaternion lookRot = Quaternion.LookRotation(velocity);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, turnSpeed * Time.deltaTime);
            }
        }

        private void ResumeEnergy()
        {
            if (!(input.Move.sqrMagnitude > 0.01f && input.IsSprint))
            {
                energy.Resume();
            }
        }

        private float GetMoveSpeed()
        {
            if (input.IsSprint && energy.Consume())
            {
                return moveSpeed * sprintMultiplier;
            }

            if (input.IsSlow)
            {
                return moveSpeed * slowMultiplier;
            }

            return moveSpeed;
        }

        private void Die(EventBody body)
        {
            if (!fade)
            {
                fade = FindObjectOfType<ScreenFadeInOut>();
            }

            input.enabled = false;
            cc.SimpleMove(Vector3.zero);
            fade.StartFadeScreen(() =>
            {
                Global.GetService<EventManager>().Send((int) EventId.Reborn);
            }, () =>
            {
                input.enabled = true;
            });
        }
    }
}