using System.Collections;
using UnityEngine;

namespace Behaviors.LichBoss.States
{
    public class AttackRitual : State
    {
        private LichBossController controller;
        private LichBossHelper helper;

        private float endAttackCooldown;

        public AttackRitual(LichBossController controller) : base("AttackRitual")
        {
            this.controller = controller;
            this.helper = controller.helper;
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Atacou com ritual");
            controller.stateMachine.ChangeState(controller.idleState);
            // Set variables
            endAttackCooldown = controller.attackRitualDuration;

            Debug.Log("Atacou com normal");
            controller.stateMachine.ChangeState(controller.idleState);

            // Update animator
            controller.thisAnimator.SetTrigger("tAttackRitual");

            // Schedule attacks
            helper.StartStateCoroutine(ScheduleAttack(controller.attackRitualDelay));
        }

        public override void Exit()
        {
            base.Exit();
            helper.ClearStateCoroutine();
        }
        
        public override void Update()
        {
            base.Update();
            
            // End attack
            if ((endAttackCooldown -= Time.deltaTime) <= 0f)
            {
                controller.stateMachine.ChangeState(controller.idleState);
                return;
            }
        }
        
        public override void LateUpdate()
        {
            base.LateUpdate();
        }        
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
        
        private IEnumerator ScheduleAttack(float delay)
        {
            // Create object
            yield return new WaitForSeconds(delay);
            var gameObject = Object.Instantiate(
            controller.ritualPrefab, 
            controller.staffBottom.position,
            controller.ritualPrefab.transform.rotation
            );

            // Schedule destruction
            Object.Destroy(gameObject, 10);

            // Damage player
            if (helper.GetDistanceToPlayer() <= controller.distanceToRitual)
            {
                var playerLife = GameManager.Instance.player.GetComponent<Life>();
                playerLife.InflictDamage(controller.gameObject, controller.attackDamage);
            }
        }
    }
}