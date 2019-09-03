using UnityEngine;
using System.Collections;
using System;

namespace SpaceShooter
{
    public class Hazard : MonoBehaviour,IHitable
    {
#pragma warning disable CS0649
        [SerializeField] GameObject explosion;
        [SerializeField] int rewardValue;
        [SerializeField] int health;
        [SerializeField] int collisionDmg;
#pragma warning restore CS0649

        public int Health
        {
            get => health;
            protected set
            {
                if (value > 0)
                    health = value;
                else
                {
                    health = 0;
                }

            }
        }
        public int Reward => rewardValue;
        public event Action<HitArgs> OnDie;
        void Start()
        {
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Boundary" || other.tag == "Enemy")
            {
                return;
            }                 

            IHitable hitable = other.GetComponent<IHitable>();
            if (hitable!=null)
            {
                HitArgs hitArgs = HitArgs.CreateBuilder().SetAttacker(gameObject).SetDamage(collisionDmg);
                hitable.HitObject(hitArgs);
                Explode();
            }
            
        }

        public void HitObject(HitArgs hitArgs)
        {
            Health -= hitArgs.Damage;
           
            if (Health == 0)
            {
                hitArgs.Victim = gameObject;
                OnDie?.Invoke(hitArgs);
                Explode();
            }
            else
            {
                HitAnimation();
            }

        }

        protected void HitAnimation()
        {
        }

        protected void Explode()
        {
            DestroyAnimation();
            Destroy(gameObject);
            Destroy(this);
        }

        protected void DestroyAnimation()
        {
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }
        }
    }
}