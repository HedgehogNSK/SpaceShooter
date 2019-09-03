using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class Projectile : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField] GameObject hitEffect;
        [SerializeField] AudioClip hitSound;
#pragma warning restore CS0649
        HitArgs hit;

        public void Settings(HitArgs hit)
        {
            this.hit = hit;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Boundary" ||(hit.Attacker!=null && other.tag.Equals(hit.Attacker.tag))) return;

            IEnumerable<IHitable> hitable = other.GetComponentsInParent<IHitable>();
            foreach(var instance in hitable)
            {
                instance.HitObject(hit);

            }
            Vector3 collisionPoint = other.ClosestPoint(transform.position);
            HitAnimation(collisionPoint);
            HitSound(collisionPoint);
            Destroy(gameObject);

        }

        private void HitAnimation(Vector3 collisionPoint)
        {
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, transform.rotation);
            }
            
        }
        private void HitSound(Vector3 collisionPoint)
        {
            if(hitSound!=null)
            {
                AudioSource.PlayClipAtPoint(hitSound, collisionPoint);
            }
            
        }
    }

}
