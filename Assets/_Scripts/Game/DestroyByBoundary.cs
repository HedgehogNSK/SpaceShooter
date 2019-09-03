using UnityEngine;
using System.Collections;

namespace SpaceShooter
{
    public class DestroyByBoundary : MonoBehaviour
    {
        void OnTriggerExit(Collider other)
        {

            Transform target = other.transform.parent;
            if (target != null)
            { while (target.parent!=null && target.parent.tag.Equals(other.tag))
                {
                    target = target.parent;
                }
            
            Destroy(target.gameObject);
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }
}