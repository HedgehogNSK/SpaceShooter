using UnityEngine;
using System.Collections;

namespace SpaceShooter
{
    public class EvasiveManeuver : MonoBehaviour
    {
#pragma warning disable CS0649

        [SerializeField] Boundary boundary;
        [SerializeField] float tilt;
        [SerializeField] float dodge;
        [SerializeField] float smoothing;
        [SerializeField] Vector2 startWait;
        [SerializeField] Vector2 maneuverTime;
        [SerializeField] Vector2 maneuverWait;
#pragma warning restore CS0649

        private float currentSpeed;
        private float targetManeuver;

        Rigidbody rigid;
        void Start()
        {
            rigid = GetComponent<Rigidbody>();
            currentSpeed = rigid.velocity.z;
            StartCoroutine(Evade());
        }

        IEnumerator Evade()
        {
            yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));
            while (true)
            {
                targetManeuver = Random.Range(1, dodge) * -Mathf.Sign(transform.position.x);
                yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
                targetManeuver = 0;
                yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));
            }
        }

        void FixedUpdate()
        {
            float newManeuver = Mathf.MoveTowards(rigid.velocity.x, targetManeuver, smoothing * Time.deltaTime);
            rigid.velocity = new Vector3(newManeuver, 0.0f, currentSpeed);
            rigid.position = new Vector3
            (
                Mathf.Clamp(rigid.position.x, boundary.xMin, boundary.xMax),
                0.0f,
                Mathf.Clamp(rigid.position.z, boundary.zMin, boundary.zMax)
            );

            rigid.rotation = Quaternion.Euler(0, 0, rigid.velocity.x * -tilt);
        }
    }

}
