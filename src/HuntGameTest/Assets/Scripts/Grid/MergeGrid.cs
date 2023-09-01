using System;
using System.Collections.Generic;
using System.Linq;
using Data.DataProxies;
using MergeGrid.GridAnimals;
using ModestTree;
using UnityEngine;
using Zenject;

namespace MergeGrid
{
    public class MergeGrid : MonoBehaviour
    {
        [SerializeField] private GridElement[] gridElements;
        [SerializeField] private int gridColumns;
        private GridDataProxy _gridDataProxy;
        private GridAnimalFactory _gridAnimalFactory;
        private readonly Dictionary<Vector2Int, GridAnimal> _gridAnimals = new();

        [Inject]
        public void Construct(GridDataProxy gridDataProxy, GridAnimalFactory gridAnimalFactory)
        {
            _gridAnimalFactory = gridAnimalFactory;
            _gridDataProxy = gridDataProxy;
        }

        public void SpawnNewGridAnimal()
        {
            if (TryToSpawnNewGridAnimal())
            {
                return;
            }

            throw new InvalidOperationException("Trying to spawn new animal on filled grid");
        }

        private void Start()
        {
            foreach (GridElementInfo gridElementInfo in _gridDataProxy.GridElementInfos)
            {
                SpawnAnimal(new Vector2Int(gridElementInfo.X, gridElementInfo.Y), gridElementInfo.Level);
            }
        }

        private bool TryToSpawnNewGridAnimal()
        {
            const int spawnLevel = 1;

            for (int i = 0; i < gridElements.Length; i++)
            {
                int x = i / gridColumns;
                int y = i % gridColumns;
                Vector2Int coords = new(x, y);
                if (_gridAnimals.ContainsKey(coords)) continue;
                SpawnAnimal(coords, spawnLevel);
                _gridDataProxy.AddAnimal(coords, spawnLevel);
                return true;
            }

            return false;
        }

        private void SpawnAnimal(Vector2Int coords, int level)
        {
            int x = coords.x;
            int y = coords.y;
            GridAnimal gridAnimal = _gridAnimalFactory.Create(level);
            Transform animalTransform = gridAnimal.transform;
            animalTransform.SetParent(gridElements[x * gridColumns + y].transform);
            animalTransform.localPosition = Vector3.zero;
            gridAnimal.DroppedOverGridElement += OnDroppedOverGridElement;
            _gridAnimals.Add(coords, gridAnimal);
        }

        private void OnDroppedOverGridElement(GridAnimal gridAnimal, GridElement gridElement)
        {
            int elementIndex = gridElements.IndexOf(gridElement);
            int x = elementIndex / gridColumns;
            int y = elementIndex % gridColumns;
            Vector2Int coords = new(x, y);
            Transform animalTransform = gridAnimal.transform;
            Vector2Int prevCoords = _gridAnimals.First(pair => pair.Value == gridAnimal).Key;
            if (_gridAnimals.TryGetValue(coords, out GridAnimal dropOverAnimal))
            {
                if (dropOverAnimal.Level == gridAnimal.Level)
                {
                    dropOverAnimal.SetLevel(dropOverAnimal.Level + 1);
                    gridAnimal.DroppedOverGridElement -= OnDroppedOverGridElement;
                    gridAnimal.Dispose();
                    _gridAnimals.Remove(prevCoords);
                    _gridDataProxy.MergeAnimals(prevCoords, coords);
                }
                else
                {
                    animalTransform.localPosition = Vector3.zero;
                }
                return;
            }

            _gridAnimals.Remove(prevCoords);
            animalTransform.SetParent(gridElement.transform);
            animalTransform.localPosition = Vector3.zero;
            _gridAnimals.Add(coords, gridAnimal);
            _gridDataProxy.MoveAnimal(prevCoords, coords);
        }
    }
}