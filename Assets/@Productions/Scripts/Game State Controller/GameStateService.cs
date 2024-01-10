using System;
using Core;
using ForgeFun.AvarikSaga.Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Demyth.Gameplay
{
    public class GameStateService : SceneService
    {
        public StateMachine<GameState>.StateHooks this[GameState state] => _stateMachine[state];
        
        [field: SerializeField, ReadOnly]
        public GameState PreviousState { get; private set; }
        
        [field: SerializeField, ReadOnly]
        public GameState CurrentState { get; private set; }

        private GameState DelayedState { get; set; }
        
        private StateMachine<GameState> _stateMachine = new();

        private float _startTime;
        private float _delayTime;

        private void Update()
        {
            if (_delayTime > 0)
            {
                if (Time.time >= _startTime + _delayTime)
                {
                    _delayTime = 0;
                    SetState(DelayedState);
                }
            }
        }

        [Button]
        public void SetState(GameState state, float delay = 0)
        {
            if (delay == 0)
            {
                if (CurrentState == state) return;
                PreviousState = CurrentState;
                CurrentState = state;
                
                _stateMachine.Update(CurrentState, PreviousState);
            }
            else
            {
                _startTime = Time.time;
                _delayTime = delay;
                DelayedState = state;
            }
        }

        [Button]
        public void NextState(float delay = 0)
        {
            var nextState = CurrentState;
            
            if (CurrentState != GameState.GameEnd)
                nextState += 1;
            else
                Debug.LogError("Current State is GameEnd, cannot set to next state");

            if (delay > 0)
                SetState(nextState, delay);
            else
                SetState(nextState);
        }
        
        //////////////////////// Unsubscribe All ////////////////////////
        private void OnDestroy()
        {
            _stateMachine.UnsubscribeAll();
        }
    }
}
