using UnityEngine;
using System.Collections;

namespace SpaceShooter
{
    public class Mover : MonoBehaviour
    {
        public float speed;

        void Start()
        {
            GetComponent<Rigidbody>().velocity = transform.forward * speed;
        }
    }
}