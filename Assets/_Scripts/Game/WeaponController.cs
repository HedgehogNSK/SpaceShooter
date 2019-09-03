using UnityEngine;
using System.Collections;

namespace SpaceShooter
{
    public class WeaponController : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField] Projectile boltPrefab;
        [SerializeField] Transform[] weaponMuzzles;
        [SerializeField] float fireRate;
        [SerializeField] float delay;
        [SerializeField] int damage;
#pragma warning restore CS0649

        new AudioSource audio;        
        void Start()
        {
            audio = GetComponent<AudioSource>();
            StartCoroutine(Fire());
        }


        IEnumerator Fire()
        {
            yield return new WaitForSeconds(delay);
            while(true)
            {
                Transform currentMuzzle = weaponMuzzles[Random.Range(0, weaponMuzzles.Length)];
                Projectile bolt = Instantiate(boltPrefab, currentMuzzle.position, currentMuzzle.rotation);
                HitArgs hit = HitArgs.CreateBuilder().SetAttacker(gameObject).SetDamage(damage);
                bolt.Settings(hit);

                audio.Play();
                yield return new WaitForSeconds(fireRate);

            }
        }
    }
}