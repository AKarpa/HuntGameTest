using System;
using UnityEngine;

namespace Hunt.Prey
{
    [RequireComponent(typeof(PreyView))]
    public class PreyPresenter : MonoBehaviour
    {
        private PreyView _view;

        public Transform[] FollowPositions => _view.FollowTransforms;

        private void Awake()
        {
            _view = GetComponent<PreyView>();
        }

        private void Start()
        {
            _view.StartRunning();
        }
    }
}