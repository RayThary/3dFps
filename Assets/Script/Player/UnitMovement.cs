using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class UnitMovement
{
    //이동
    private Vector3 moveVec;
    public Vector3 GetMoveVec { get { return moveVec; } }

    //점프
    private bool isGround;

    //기본
    private Transform unitTransform;
    private Animator anim;
    private Rigidbody rigid;

    private PlayerInput input;

    public void SetUp(Transform _transform, Animator _anim, Rigidbody _rigid, PlayerInput _input)
    {
        unitTransform = _transform;
        anim = _anim;
        rigid = _rigid;
        input = _input;
    }

    public void UnitMove(float _speed,bool _isDodge,Vector3 _dodgeVec)
    {
        moveVec = (unitTransform.right * input.GetHorizontal) + (unitTransform.forward * input.GetVertical);

        if (_isDodge) moveVec = _dodgeVec;

        unitTransform.position += moveVec * _speed * Time.deltaTime;

        anim.SetBool("Run", moveVec != Vector3.zero);
    }

    public void jump(float _jumpPower,PlayerInput _playerInput)
    {
        isGround = Physics.Raycast(unitTransform.position, Vector3.down, 0.5f, LayerMask.GetMask("Ground"));
        if (_playerInput.JumpCheck && isGround)
        {
            anim.SetBool("isJump", true);
            anim.SetTrigger("Jump");
            rigid.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
            _playerInput.JumpCheck = false;
        }
    }

    public bool dodge()
    {
        if (input.GetLeftShift)
        {
            return true;
        }
        return false;
    }


  
}
