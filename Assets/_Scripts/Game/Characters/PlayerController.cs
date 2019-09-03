using UnityEngine;
using System.Collections;
using System;
using Hedge.UI;

[Serializable]
public class Boundary 
{
	public float xMin, xMax, zMin, zMax;
}

namespace SpaceShooter
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour, IHitable
    {
#pragma warning disable CS0649
        [SerializeField] GameObject explosion;
        [SerializeField] int baseHealth;
        [SerializeField] float speed;
        [SerializeField] float tilt;
        [SerializeField] Boundary boundary;

        [SerializeField] Projectile shotPrefab;
        [SerializeField] Transform shotSpawn;
        [SerializeField] float fireRate;
        [SerializeField] int damage;
#pragma warning restore CS0649

        int currentHealth;
        public int Health
        {
            get => currentHealth;
            protected set
            {
                if (value > 0)
                    currentHealth = value;
                else
                {
                    currentHealth = 0;
                }

                DataSpreader.OnUpdate(ParameterType.Health, currentHealth);
            }
        }

        private float nextFire;

        AudioSource audioSource;
        Rigidbody rigid;

        Vector2 movementVector;

        public event Action<HitArgs> OnDie;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            rigid = GetComponent<Rigidbody>();
        }

        public void Initialize()
        {
            Health = baseHealth;
        }
        private void Start()
        {
            Initialize();            
        }

        public void SetMovement(Vector2 direction)
        {
            movementVector = direction;
        }

        public void Fire()
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Projectile bolt =  Instantiate(shotPrefab, shotSpawn.position, shotSpawn.rotation);
                HitArgs hit = HitArgs.CreateBuilder().SetAttacker(gameObject).SetDamage(damage);                
                bolt.Settings(hit);
                audioSource.Play();
            }
        }

        void FixedUpdate()
        {
            rigid.velocity = movementVector * speed;
#if KEYBOARD
            movementVector = Vector2.zero;
#endif

            rigid.position = new Vector3
            (
                Mathf.Clamp(rigid.position.x, boundary.xMin, boundary.xMax),
                0.0f,
                0.0f
            );

            rigid.rotation = Quaternion.Euler(0.0f, 0.0f, rigid.velocity.x * -tilt);
        }

        public void HitObject(HitArgs hit)
        {
            Health -= hit.Damage;
            if (Health == 0)
            {
                OnDie?.Invoke(hit);
                Explode();
            }
        }


        protected void Explode()
        {
            DestroyAnimation();
            Destroy(gameObject);
        }

        protected void DestroyAnimation()
        {
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }
        }

        private void OnDestroy()
        {
            InputMoveController.OnMove-= SetMovement;
            InputFireController.OnFire-= Fire;
        }
    }

}
