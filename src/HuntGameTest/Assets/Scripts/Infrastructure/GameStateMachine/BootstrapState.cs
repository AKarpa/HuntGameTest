﻿using System;
using System.Reflection;
using Data;
using Data.Models;
using Scenes;
using StateMachines;
using UnityEditor;

namespace Infrastructure.GameStateMachine
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly DataController _dataController;

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, DataController dataController)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _dataController = dataController;
        }

        public void Enter()
        {
            _sceneLoader.LoadScene(SceneName.BootScene, OnBootSceneLoaded);
        }

        private void OnBootSceneLoaded()
        {
#if UNITY_EDITOR
            Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
            Type logEntries = assembly.GetType("UnityEditor.LogEntries");
            logEntries.GetMethod("Clear")?.Invoke(new object(), null);
#endif
            _dataController.Initialize();

            _gameStateMachine.Enter<LoadLevelState, SceneName>(SceneName.MergeScene);
        }

        public void Exit()
        {
        }
    }
}