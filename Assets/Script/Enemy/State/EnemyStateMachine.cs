using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    private IEnemyState _currentState;

    public void ChangeState(IEnemyState _nextState)
    {
        _currentState?.Exit();

        _currentState = _nextState;

        _currentState.Enter();
    }

    public void Update()
    {
        _currentState.Update();
    }

}
