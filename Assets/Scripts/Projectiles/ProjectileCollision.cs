using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Projectiles
{
    public class ProjectileCollision : MonoBehaviour 
    {
        public GameObject hitEffect;
        
        [HideInInspector] public int damage;
        [HideInInspector] public GameObject attacker;

        private void Awake() 
        {
            
        }

        private void Start() 
        {
            
        }

        private void OnCollisionEnter(Collision collision) 
        {
            //Process player collision
            var hitObject = collision.gameObject;
            var hitLayer = hitObject.layer;
            var collidedWithPlayer = hitLayer == LayerMask.NameToLayer("Player");
            if (collidedWithPlayer)
            {
                var hitLife = hitObject.GetComponent<Life>();
                if (hitLife != null)
                {
                    hitLife.InflictDamage(attacker, damage);
                }
            }
            if (hitEffect != null)
            {
                var effect = Instantiate (hitEffect, transform.position, hitEffect.transform.rotation);
                Destroy (effect, 10);
            }
            // Destroy projectile
            Destroy(gameObject);
        }

    }

}