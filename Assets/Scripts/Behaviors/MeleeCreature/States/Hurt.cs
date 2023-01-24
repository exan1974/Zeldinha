using UnityEngine;

namespace Zeldinha.Assets.Scripts.Behaviors.MeleeCreature.States
{
    public class Hurt : State
    {
        private MeleeCreatureController controller;
        private MeleeCreatureHelper helper;

        private float timePassed;

        public Hurt(MeleeCreatureController controller) : base("Hurt")
        {
            this.controller = controller;
            this.helper = controller.helper;
        }

        public override void Enter()
        {
            base.Enter();

            // Reset timer
            timePassed = 0;

            // Pause damage
            controller.thisLife.isVulnerable = false;

            // Shift object control from navmesh to physics
            controller.thisAgent.enabled = false;
            controller.thisRigidbody.isKinematic = false;

            // Shift object control from physics to navmesh
            controller.thisAgent.enabled = true;
            controller.thisRigidbody.isKinematic = true;
        }

        public override void Exit()
        {
            base.Exit();
            controller.thisLife.isVulnerable = true;

        }
        
        public override void Update()
        {
            base.Update();

            // Update timer
            timePassed += Time.deltaTime;

            // Switch states
            if (timePassed >= controller.hurtDuration)
            {
                if(controller.thisLife.IsDead())
                {
                    controller.stateMachine.ChangeState(controller.deadState);   
                }
                else
                {
                    // Go back to idle
                    controller.stateMachine.ChangeState(controller.idleState);
                }
                return;
            }

            // Update animator
            controller.thisAnimator.SetTrigger("tHurt");
        }
        
        public override void LateUpdate()
        {
            base.LateUpdate();
        }        
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
        
    }
}