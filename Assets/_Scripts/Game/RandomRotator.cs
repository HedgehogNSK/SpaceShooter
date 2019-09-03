using UnityEngine;
using System.Collections;

namespace Shooter
{
    public class RandomRotator : MonoBehaviour
    {
        public float tumble;

        void Start()
        {
            GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
        }
    }
}
