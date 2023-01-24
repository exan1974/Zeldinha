using System.Collections;
using UnityEngine;

namespace Zeldinha.Assets.Scripts.Behaviors.MeleeCreature.States
{
    public class Attack : State
    {
        private MeleeCreatureController controller;
        private MeleeCreatureHelper helper;
        private float endAttackCooldown;
        private IEnumerator attackCoroutine;

        public Attack(MeleeCreatureController controller) : base("Attack")
        {
            this.controller = controller;
            this.helper = controller.helper;
        }

        public override void Enter()
        {
            base.Enter();

            // Set variables
            endAttackCooldown = controller.attackDuration;

            // Schedule attack
            attackCoroutine = ScheduleAttack();
            controller.StartCoroutine(attackCoroutine);
            Debug.Log("schedule attacking!!!");

            // Update animator
            controller.thisAnimator.SetTrigger("tAttack");
        }

        public override void Exit()
        {
            base.Exit();

            // Cancel attack
            if(attackCoroutine != null)
            {
            controller.StopCoroutine(attackCoroutine);
            Debug.Log("cancel attacking!!!");
            }
        }
        
        public override void Update()
        {
            base.Update();

            // Face player
            helper.FacePlayer();
            
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

        private IEnumerator ScheduleAttack()
        {
            yield return new WaitForSeconds(controller.damageDelay);
            PerformAttack();
        }

        private void PerformAttack()
        {
            Debug.Log("attacking!!!");

            // Get variables
            var origin = controller.transform.position;
            var radius = controller.attackRadius;
            var direction = controller.transform.rotation * Vector3.forward;
            var maxDistance = controller.attackRadius;

            // OverlapSphere
            var attackPosition = direction * radius + origin;
            var layerMask = LayerMask.GetMask("Player");
            Collider[] colliders = Physics.OverlapSphere(attackPosition, maxDistance, layerMask);
            foreach (var collider in colliders) 
            {
                var hitObject = collider.gameObject;              
                Debug.Log("hit: " + hitObject.name);

                    // Perform attack
                    var hitLife = hitObject.GetComponent<Life>();
                    if (hitLife != null)
                    {
                        var attacker = controller.gameObject;
                        var attackDamage = controller.attackDamage;
                        hitLife.InflictDamage(attacker, attackDamage);
                    }
                
            }
            
        }
        
    }
}