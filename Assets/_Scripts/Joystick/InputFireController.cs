using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SpaceShooter
{
    public class InputFireController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public static event System.Action OnFire;

        Coroutine fire;
        public void OnPointerDown(PointerEventData eventData)
        {
            fire = StartCoroutine(Fire());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            StopCoroutine(fire);
        }

        IEnumerator Fire()
        {
            while(true)                
            {
                OnFire?.Invoke();
                yield return new WaitForFixedUpdate();
            }
            
        }

#if KEYBOARD
        private void Update()
        {
            if (Input.GetButton("Fire1"))
                OnFire?.Invoke();
        }
#endif
    }
}