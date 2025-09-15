using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputAction
{
    Horizontal,
    Vertical,
    MouseX,
    MouseY,
    LeftShift,
    Jump,
    Fire,
    Reload,
    Zoom,
    Weapon1,
    Weapon2,
    CursorToggle,
    FCheck,
}

public class PlayerInput
{
    private Dictionary<InputAction, float> axisValues = new();
    public Dictionary<InputAction, float> GetAxis { get { return axisValues; } }

    private Dictionary<InputAction, bool> buttonDown = new();
    public Dictionary<InputAction, bool> ButtonDown { get { return buttonDown; } }

    private Dictionary<InputAction, bool> buttonHold = new();
    public Dictionary<InputAction, bool> ButtonHold { get { return buttonHold; } }

    private Dictionary<InputAction, bool> buttonUp = new();
    public Dictionary<InputAction, bool> ButtonUp { get { return buttonUp; } }

    public void ReadInput()
    {
        axisValues[InputAction.Horizontal] = Input.GetAxisRaw("Horizontal");
        axisValues[InputAction.Vertical] = Input.GetAxisRaw("Vertical");
        axisValues[InputAction.MouseX] = Input.GetAxis("Mouse X");
        axisValues[InputAction.MouseY] = Input.GetAxis("Mouse Y");

        buttonDown[InputAction.LeftShift] = Input.GetButtonDown("LeftShift");
        buttonDown[InputAction.Jump] = Input.GetButtonDown("Jump");

        buttonDown[InputAction.Fire] = Input.GetMouseButtonDown(0);
        buttonHold[InputAction.Fire] = Input.GetMouseButton(0);

        buttonDown[InputAction.Weapon1] = Input.GetKeyDown(KeyCode.Alpha1);
        buttonDown[InputAction.Weapon2] = Input.GetKeyDown(KeyCode.Alpha2);
        buttonDown[InputAction.FCheck] = Input.GetKeyDown(KeyCode.F);

        buttonDown[InputAction.Reload] = Input.GetKeyDown(KeyCode.R);

        buttonDown[InputAction.Zoom] = Input.GetMouseButtonDown(1);
        buttonUp[InputAction.Zoom] = Input.GetMouseButtonUp(1);
    }
    public PlayerInput()
    {
        foreach (InputAction action in System.Enum.GetValues(typeof(InputAction)))
        {
            axisValues[action] = 0f;
            buttonDown[action] = false;
            buttonHold[action] = false;
            buttonUp[action] = false;
        }
    }


}
