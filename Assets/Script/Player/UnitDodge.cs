using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class UnitDodge : MonoBehaviour
{

    private bool dodogeStart = false;
    [SerializeField] private float dodgeCool;
    public float GetDodgeCool { get { return dodgeCool; } }

    public void dodge(PlayerInput _input, Unit _unit, UnitMovement _unitMovement, float _speed, Vector3 _moveVec)
    {
        if (_input.GetLeftShift && !dodogeStart && _unitMovement.GetMoveVec != Vector3.zero)
        {
            dodogeStart = true;
            _unit.IsDodge = true;
            StartCoroutine(cDodge(_unit, _speed,_moveVec));
        }

    }

    private IEnumerator cDodge(Unit _unit, float _speed,Vector3 _moveVec)
    {
        _unit.DodgeVec = _moveVec;
        _unit.SetSpeed *= 2;
        yield return new WaitForSeconds(0.3f);
        _unit.SetSpeed *= 0.5f;

        _unit.IsDodge = false;

        yield return new WaitForSeconds(dodgeCool - 0.3f);
        dodogeStart = false;

    }
}
