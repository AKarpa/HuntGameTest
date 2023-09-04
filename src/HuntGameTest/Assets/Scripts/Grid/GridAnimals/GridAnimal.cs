using System;
using System.Collections.Generic;
using MergeGrid;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Grid.GridAnimals
{
    public class GridAnimal : MonoBehaviour, IPoolable<int, IMemoryPool>, IDisposable
    {
        [SerializeField] private GameObject[] levelModels;
        private IMemoryPool _pool;
        private Transform _transform;
        private Vector3 _startingDelta;
        private Camera _camera;
        private float _normalHeight;
        private float _dragHeight;

        public event Action<GridAnimal, GridElement> DroppedOverGridElement;
        
        public int Level { get; private set; }

        public void SetLevel(int level)
        {
            Level = level;
            for (int i = 0; i < levelModels.Length; i++)
            {
                levelModels[i].SetActive(i + 1 == level);
            }
        }

        public void Dispose()
        {
            _transform.SetParent(null);
            _pool.Despawn(this);
        }

        void IPoolable<int, IMemoryPool>.OnSpawned(int level, IMemoryPool pool)
        {
            _pool = pool;
            SetLevel(level);
        }

        void IPoolable<int, IMemoryPool>.OnDespawned()
        {
            _pool = null;
        }

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _camera = Camera.main;
            _normalHeight = _transform.position.y;
            _dragHeight = _normalHeight + 1f;
        }

        private Vector3 GetPositionOnScreen()
        {
            return _camera.WorldToScreenPoint(_transform.position);
        }

        private void OnMouseDown()
        {
            _startingDelta = Input.mousePosition - GetPositionOnScreen();
        }

        private void OnMouseDrag()
        {
            Vector3 screenPosition = _camera.ScreenToWorldPoint(Input.mousePosition - _startingDelta);
            float screenHeight = screenPosition.y;
            Vector3 cameraPosition = _camera.transform.position;
            _transform.position = screenPosition + (_dragHeight - screenHeight) / (cameraPosition.y - screenHeight) *
                (cameraPosition - screenPosition);
        }

        private void OnMouseUp()
        {
            PointerEventData pointerData = new(EventSystem.current)
            {
                position = Input.mousePosition
            };
            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(pointerData, results);
            foreach (RaycastResult raycastResult in results)
            {
                if (!raycastResult.gameObject.TryGetComponent(out GridElement gridElement)) continue;
                DroppedOverGridElement?.Invoke(this, gridElement);
                return;
            }
            
            _transform.localPosition = Vector3.zero;
        }
    }
}