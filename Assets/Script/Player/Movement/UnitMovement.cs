using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;
//using static UnityEditor.Searcher.SearcherWindow.Alignment;

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

    public void UnitMove(float _speed, bool _isDodge, Vector3 _dodgeVec, float _yaw)
    {

        Quaternion rot = Quaternion.Euler(0f, _yaw, 0f);

        Vector3 forward = rot * Vector3.forward;
        Vector3 right = rot * Vector3.right;

        moveVec = (right * input.GetAxis[InputAction.Horizontal]) + (forward * input.GetAxis[InputAction.Vertical]);

        if (_isDodge) moveVec = _dodgeVec;

        moveVec = moveVec.normalized;

        rigid.MoveRotation(rot);
        rigid.velocity = new Vector3(moveVec.x * _speed, rigid.velocity.y, moveVec.z * _speed);
        anim.SetBool("Run", moveVec != Vector3.zero);
    }

    public void jump(float _jumpPower, PlayerInput _playerInput)
    {
        isGround = Physics.Raycast(unitTransform.position, Vector3.down, 0.5f, LayerMask.GetMask("Ground"));
        if (_playerInput.ButtonDown[InputAction.Jump] && isGround)
        {
            anim.SetBool("isJump", true);
            anim.SetTrigger("Jump");
            rigid.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
            _playerInput.ButtonDown[InputAction.Jump] = false;
        }
    }

}
