using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SpaceShooter
{
    public class InputMoveController : MonoBehaviour, IPointerDownHandler,IPointerUpHandler,IMoveHandler
    {
#pragma warning disable CS0649
        [SerializeField] Vector2 moveVector;
#pragma warning restore CS0649

        public static event System.Action<Vector2> OnMove;
        public void OnPointerDown(PointerEventData eventData)
        {
            OnMove?.Invoke(moveVector);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnMove?.Invoke(Vector2.zero);
        }

        void IMoveHandler.OnMove(AxisEventData eventData)
        {
            throw new System.NotImplementedException();
        }


#if KEYBOARD
        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            if (horizontal < 0)
                OnMove?.Invoke(Vector2.left);
            else if (horizontal > 0)
                OnMove?.Invoke(Vector2.right);
            
        }
#endif
    }

}



