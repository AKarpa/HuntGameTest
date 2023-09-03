using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Hunt
{
    public class HuntInput : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
    {
        private readonly ReactiveProperty<Vector2> _deltaPosition = new(Vector2.zero);
        private Vector2 _startPosition;

        public event Action PointerDown;
        public event Action PointerUp;

        public IReadOnlyReactiveProperty<Vector2> DeltaPosition => _deltaPosition;

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (eventData.pointerId > 0) return;
            PointerDown?.Invoke();
            _startPosition = eventData.position;
        }

        void IPointerMoveHandler.OnPointerMove(PointerEventData eventData)
        {
            if (eventData.pointerId > 0) return;
            Vector2 delta = eventData.position - _startPosition;
            _deltaPosition.Value = new Vector2(delta.x / Screen.width, delta.y / Screen.height);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (eventData.pointerId > 0) return;
            PointerUp?.Invoke();
            _deltaPosition.Value = Vector2.zero;
        }
    }
}