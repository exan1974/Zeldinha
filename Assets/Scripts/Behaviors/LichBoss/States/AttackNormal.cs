using System.Collections;
using Projectiles;
using UnityEngine;

namespace Behaviors.LichBoss.States
{
    public class AttackNormal : State
    {
        private LichBossController controller;
        private LichBossHelper helper;

        private float endAttackCooldown;

        public AttackNormal(LichBossController controller) : base("AttackNormal")
        {
            this.controller = controller;
            this.helper = controller.helper;
        }

        public override void Enter()
        {
            base.Enter();

            // Set variables
            endAttackCooldown = controller.attackNormalDuration;

            Debug.Log("Atacou com normal");
            controller.stateMachine.ChangeState(controller.idleState);

            // Update animator
            controller.thisAnimator.SetTrigger("tAttackNormal");

            // Schedule attacks
           helper.StartStateCoroutine(ScheduleAttack(controller.attackNormalDelay));
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
            controller.fireballPrefab, 
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
            var vectorToPlayer = (player.transform.position  + controller.aimOffset - spawnTransform.position).normalized;
            forceVector.y = vectorToPlayer.y;
            forceVector *= controller.attackNormalImpulse;
            projectileRigidbody.AddForce(forceVector, ForceMode.Impulse);

            // Schedule destruction
            Object.Destroy(projectile, 20);
        }

            
        
    }
}