using System;
using System.Collections.Generic;
using Data.DataProxies;
using MergeGrid.GridAnimals;
using UnityEngine;
using Zenject;

namespace MergeGrid
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] private Transform[] huntingPackGridElements;
        [SerializeField] private Transform[] otherGridElements;
        [SerializeField] private int gridColumns;
        private GridDataProxy _gridDataProxy;
        private GridAnimalFactory _gridAnimalFactory;
        private readonly Dictionary<Vector2Int, GridAnimal> _huntingPackGridAnimals = new();
        private readonly Dictionary<Vector2Int, GridAnimal> _otherGridAnimals = new();

        [Inject]
        public void Construct(GridDataProxy gridDataProxy, GridAnimalFactory gridAnimalFactory)
        {
            _gridAnimalFactory = gridAnimalFactory;
            _gridDataProxy = gridDataProxy;
        }

        private void Start()
        {
            foreach (GridElementInfo gridElementInfo in _gridDataProxy.HuntingPack)
            {
                SpawnAnimal(huntingPackGridElements, _huntingPackGridAnimals,
                    new Vector2Int(gridElementInfo.X, gridElementInfo.Y), gridElementInfo.Level);
            }
            
            foreach (GridElementInfo gridElementInfo in _gridDataProxy.Other)
            {
                SpawnAnimal(otherGridElements, _otherGridAnimals,
                    new Vector2Int(gridElementInfo.X, gridElementInfo.Y), gridElementInfo.Level);
            }
        }

        public void SpawnNewGridAnimal()
        {
            if (TryToSpawnGridAnimal(huntingPackGridElements, _huntingPackGridAnimals, true))
            {
                return;
            }

            if (TryToSpawnGridAnimal(otherGridElements, _otherGridAnimals, false))
            {
                return;
            }

            throw new InvalidOperationException("Trying to spawn new animal on filled grid");
        }

        private bool TryToSpawnGridAnimal(IReadOnlyList<Transform> gridElements,
            IDictionary<Vector2Int, GridAnimal> gridAnimals, bool isHuntingPack)
        {
            int huntingPackCount = gridElements.Count;
            int huntingPackRows = Mathf.CeilToInt((float) huntingPackCount / gridColumns);
            for (int x = 0; x < huntingPackRows; x++)
            {
                int columns = Mathf.Min(gridColumns, huntingPackCount - x * gridColumns);
                for (int y = 0; y < columns; y++)
                {
                    const int level = 1;
                    Vector2Int coords = new(x, y);
                    if (gridAnimals.ContainsKey(coords)) continue;
                    SpawnAnimal(gridElements, gridAnimals, coords, level);
                    _gridDataProxy.AddAnimal(x, y, level, isHuntingPack);
                    return true;
                }
            }

            return false;
        }

        private void SpawnAnimal(IReadOnlyList<Transform> gridElements, IDictionary<Vector2Int, GridAnimal> gridAnimals,
            Vector2Int coords, int level)
        {
            int x = coords.x;
            int y = coords.y;
            GridAnimal gridAnimal = _gridAnimalFactory.Create(level);
            gridAnimal.transform.SetParent(gridElements[x * gridColumns + y]);
            gridAnimals.Add(coords, gridAnimal);
        }
    }
}