using System.Collections;
using Projectiles;
using UnityEngine;

namespace Behaviors.LichBoss.States
{
    public class AttackSuper : State
    {
        private LichBossController controller;
        private LichBossHelper helper;

        private float endAttackCooldown;

        public AttackSuper(LichBossController controller) : base("AttackSuper")
        {
            this.controller = controller;
            this.helper = controller.helper;
        }

        public override void Enter()
        {
            base.Enter();

            // Set variables
            endAttackCooldown = controller.attackSuperDuration;

            // Update animator
            controller.thisAnimator.SetTrigger("tAttackSuper");

            // Schedule attacks
                var delayStep = controller.attackSuperMagicDuration / (controller.attackSuperMagicCount - 1);
            for (int i = 0; i < controller.attackSuperMagicCount - 1; i++)
            {
                var delay = controller.attackSuperMagicDelay + delayStep * i;
                helper.StartStateCoroutine(ScheduleAttack(delay));
            }
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
            yield return new WaitForSeconds(delay);

            // Create object
            var spawnTransform = controller.staffTop;
            var projectile = Object.Instantiate(
            controller.energyBallPrefab, 
            spawnTransform.position,
            spawnTransform.rotation
            //controller.fireballPrefab.transform.rotation
            );

            // Populate ProjectileCollision
            var projectileCollision = projectile.GetComponent<ProjectileCollision>();
            projectileCollision.damage = controller.attackDamage;
            projectileCollision.attacker = controller.gameObject;

            // Get stuff
            var projectileRigidbody = projectile.GetComponent<Rigidbody>();
            var player = GameManager.Instance.player;

            // Aplly impulse
            var forceVector = spawnTransform.rotation * Vector3.forward;
            var vectorToPlayer = (player.transform.position + controller.aimOffset - spawnTransform.position).normalized;
            forceVector.y = vectorToPlayer.y;
            forceVector *= controller.attackSuperImpulse;
            projectileRigidbody.AddForce(forceVector, ForceMode.Impulse);

            // Schedule destruction
            Object.Destroy(projectile, 20);
        }
    }
}