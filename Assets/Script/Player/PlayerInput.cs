using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerInput
{

    // 이동 관련
    private float horizontal;
    public float GetHorizontal { get { return horizontal; } }

    private float vertical;
    public float GetVertical { get { return vertical; } }

    // 점프
    private bool jumpCheck;
    public bool JumpCheck { get { return jumpCheck; } set { jumpCheck = value; } }

    // 마우스 관련
    private float mouseX;
    public float GetMouseX { get { return mouseX; } }

    private float mouseY;
    public float GetMouseY { get { return mouseY; } }

    // 공격 관련
    private bool fireDown;
    public bool GetFireDown { get { return fireDown; } }

    private bool fireHold;
    public bool GetFireHold { get { return fireHold; } }

    private bool leftShift;
    public bool GetLeftShift { get { return leftShift; } }

    //무기 관련
    private bool weapon1;
    public bool GetWeapon1 { get { return weapon1; } }

    private bool weapon2;
    public bool GetWeapon2 { get { return weapon2; } }

    private bool fCheck;
    public bool FCheck { get { return fCheck; } }

    public void ReadInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        leftShift = Input.GetButtonDown("LeftShift");
        jumpCheck = Input.GetButtonDown("Jump");

        fireDown = Input.GetMouseButtonDown(0);
        fireHold = Input.GetMouseButton(0);

        weapon1 = Input.GetKeyDown(KeyCode.Alpha1);
        weapon2 = Input.GetKeyDown(KeyCode.Alpha2);
        fCheck = Input.GetKeyDown(KeyCode.F);
    }



}
