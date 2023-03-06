using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerInputState { Game, Inventory, Paused, NavigationMenu, GameOver, Shop };


public class PlayerInputController : MonoBehaviour
{
    public NewGamepadInput mGamepadInput;
    //Eventually, the playres will have this component. It will handle transitions between different input states and allow swapping between keyboard and gamepad and rebinding input
    public PlayerInputState inputState = PlayerInputState.Game;
    public float[] axisInput = new float[(int)AxisInput.Count];
    public float[] previousAxisInput = new float[(int)AxisInput.Count];

    public bool[] buttonInput = new bool[(int)ButtonInput.Count];
    public bool[] previousButtonInput = new bool[(int)ButtonInput.Count];
    public PlayerController player;

    public void Start()
    {
        axisInput = new float[(int)AxisInput.Count];
        previousAxisInput = new float[(int)AxisInput.Count];

        buttonInput = new bool[(int)ButtonInput.Count];
        previousButtonInput = new bool[(int)ButtonInput.Count];
        player = GetComponent<PlayerController>();
        //SmoothFollow.instance.AddPlayer(GetComponent<PlayerController>());

    }

    /*
    public void Remove()
    {
        if(mGamepadInput != null)
            GamepadInputManager.instance.RemovePlayerAtIndex(mGamepadInput.input.playerIndex);


    }
    */

    public void SetGamepadInput(NewGamepadInput gamepad)
    {
        if(gamepad == null)
        {
            if(mGamepadInput != null)
            {
                mGamepadInput.player = null;
            }
            mGamepadInput = gamepad;
        }
        else
        {
            mGamepadInput = gamepad;
            mGamepadInput.player = player;
        }


    }

    void UpdatePreviousInputs()
    {
        var axisCount = (byte)AxisInput.Count;

        for (byte i = 0; i < axisCount; ++i)
        {
            previousAxisInput[i] = axisInput[i];
        }

        var buttonCount = (byte)ButtonInput.Count;

        for (byte i = 0; i < buttonCount; ++i)
        {
            previousButtonInput[i] = buttonInput[i];
        }
    }

    void ClearInputs()
    {
        var axisCount = (byte)AxisInput.Count;

        for (byte i = 0; i < axisCount; ++i)
        {
            previousAxisInput[i] = 0;
        }

        var buttonCount = (byte)ButtonInput.Count;

        for (byte i = 0; i < buttonCount; ++i)
        {
            previousButtonInput[i] = false;
        }
    }

    public bool GetButtonDown(ButtonInput button)
    {
        return (buttonInput[(int)button] && !previousButtonInput[(int)button]);
    }

    public bool GetButton(ButtonInput button)
    {
        return buttonInput[(int)button];
    }

    public float GetAxisValue(AxisInput axis)
    {
        return axisInput[(int)axis];
    }

    public bool GetLeftStickTapUp()
    {
        return (axisInput[(int)AxisInput.LeftStickY] >= 0.5f && previousAxisInput[(int)AxisInput.LeftStickY] <= 0.5f);
    }

    public bool GetLeftStickTapDown()
    {
        return (axisInput[(int)AxisInput.LeftStickY] <= -0.5f && previousAxisInput[(int)AxisInput.LeftStickY] >= -0.5f);
    }

    public bool GetLeftStickTapRight()
    {
        return (axisInput[(int)AxisInput.LeftStickX] >= 0.5f && previousAxisInput[(int)AxisInput.LeftStickX] <= 0.5f);
    }

    public bool GetLeftStickTapLeft()
    {
        return (axisInput[(int)AxisInput.LeftStickX] <= -0.5f && previousAxisInput[(int)AxisInput.LeftStickX] >= -0.5f);
    }

    public bool GetRightStickTapUp()
    {
        return (axisInput[(int)AxisInput.RightStickY] >= 0.5f && previousAxisInput[(int)AxisInput.RightStickY] <= 0.5f);
    }

    public bool GetRightStickTapDown()
    {
        return (axisInput[(int)AxisInput.RightStickY] <= -0.5f && previousAxisInput[(int)AxisInput.RightStickY] >= -0.5f);
    }

    public bool GetRightStickTapRight()
    {
        return (axisInput[(int)AxisInput.RightStickX] >= 0.5f && previousAxisInput[(int)AxisInput.RightStickX] <= 0.5f);
    }

    public bool GetRightStickTapLeft()
    {
        return (axisInput[(int)AxisInput.RightStickX] <= -0.5f && previousAxisInput[(int)AxisInput.RightStickX] >= -0.5f);
    }

    public bool GetLeftStickHoldDown()
    {
        return (axisInput[(int)AxisInput.LeftStickY] <= -0.75f && previousAxisInput[(int)AxisInput.LeftStickY] <= -0.75f);
    }

    public Vector2 GetLeftStickAim()
    {
        return new Vector2(GetAxisValue(AxisInput.LeftStickX), GetAxisValue(AxisInput.LeftStickY));
    }

    public Vector2 GetRightStickAim()
    {
        return new Vector2(GetAxisValue(AxisInput.RightStickX), GetAxisValue(AxisInput.RightStickY));
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //Delete all these states and just make more buttons...
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    public void Update()
    {
        if(!mGamepadInput)
        {
            ClearInputs();
            return;
        }
        UpdatePreviousInputs();
        //Debug.Log("Player input " + player.mPlayerIndex + " in " + inputState.ToString());
        switch (inputState)
        {
            case PlayerInputState.Game:
                //Update the axis inputs
                axisInput[(int)AxisInput.LeftStickX] = mGamepadInput.axisInputs[(int)GamepadAxis.LeftStickX];
                axisInput[(int)AxisInput.LeftStickY] = mGamepadInput.axisInputs[(int)GamepadAxis.LeftStickY];
                axisInput[(int)AxisInput.RightStickX] = mGamepadInput.axisInputs[(int)GamepadAxis.RightStickX];
                axisInput[(int)AxisInput.RightStickY] = mGamepadInput.axisInputs[(int)GamepadAxis.RightStickY];

                //Update the button inputs
                //These are for reading in "taps" of the stick buttons
                buttonInput[(int)ButtonInput.DPad_Left] = mGamepadInput.buttonInputs[(int)GamepadButtons.DpadLeft];
                buttonInput[(int)ButtonInput.DPad_Right] = mGamepadInput.buttonInputs[(int)GamepadButtons.DpadRight];
                buttonInput[(int)ButtonInput.Interact] = mGamepadInput.buttonInputs[(int)GamepadButtons.WestButton];
                buttonInput[(int)ButtonInput.DPad_Up] = mGamepadInput.buttonInputs[(int)GamepadButtons.DpadUp];

                buttonInput[(int)ButtonInput.Jump] = mGamepadInput.buttonInputs[(int)GamepadButtons.SouthButton];
                buttonInput[(int)ButtonInput.LightAttack] = mGamepadInput.buttonInputs[(int)GamepadButtons.EastButton];
                buttonInput[(int)ButtonInput.ActivateGadget] = mGamepadInput.buttonInputs[(int)GamepadButtons.NorthButton];
                //Attack Input
                buttonInput[(int)ButtonInput.Attack_Down] = mGamepadInput.buttonInputs[(int)GamepadButtons.EastButton] && mGamepadInput.axisInputs[(int)GamepadAxis.LeftStickY] == -1;
                buttonInput[(int)ButtonInput.Attack_Left] = mGamepadInput.buttonInputs[(int)GamepadButtons.EastButton] && mGamepadInput.axisInputs[(int)GamepadAxis.LeftStickX] == -1;
                buttonInput[(int)ButtonInput.Attack_Right] = mGamepadInput.buttonInputs[(int)GamepadButtons.EastButton] && mGamepadInput.axisInputs[(int)GamepadAxis.LeftStickX] == 1;
                buttonInput[(int)ButtonInput.Attack_Up] = mGamepadInput.buttonInputs[(int)GamepadButtons.EastButton] && mGamepadInput.axisInputs[(int)GamepadAxis.LeftStickY] == 1;

                buttonInput[(int)ButtonInput.Attack_Neutral] = mGamepadInput.buttonInputs[(int)GamepadButtons.EastButton];


                buttonInput[(int)ButtonInput.PlayerMenu] = mGamepadInput.buttonInputs[(int)GamepadButtons.SelectButton];
                buttonInput[(int)ButtonInput.Pause] = mGamepadInput.buttonInputs[(int)GamepadButtons.StartButton];
                buttonInput[(int)ButtonInput.Minimap] = mGamepadInput.buttonInputs[(int)GamepadButtons.DpadUp];
                buttonInput[(int)ButtonInput.Gadget1] = mGamepadInput.buttonInputs[(int)GamepadButtons.LeftBumper];
                buttonInput[(int)ButtonInput.Fire] = mGamepadInput.buttonInputs[(int)GamepadButtons.RightTrigger];
                buttonInput[(int)ButtonInput.SwapWeapon] = mGamepadInput.buttonInputs[(int)GamepadButtons.DpadLeft]
                    || mGamepadInput.buttonInputs[(int)GamepadButtons.DpadRight];
                buttonInput[(int)ButtonInput.BeamUp] = mGamepadInput.buttonInputs[(int)GamepadButtons.DpadDown];
                buttonInput[(int)ButtonInput.Consumable] = mGamepadInput.buttonInputs[(int)GamepadButtons.RightBumper];
                buttonInput[(int)ButtonInput.InventoryDrop] = false;
                buttonInput[(int)ButtonInput.InventoryMove] = false;
                buttonInput[(int)ButtonInput.InventorySort] = false;
                buttonInput[(int)ButtonInput.ChangeTabLeft] = false;
                buttonInput[(int)ButtonInput.ChangeTabRight] = false;
                buttonInput[(int)ButtonInput.Menu_Back] = false;
                buttonInput[(int)ButtonInput.FireMode] = mGamepadInput.buttonInputs[(int)GamepadButtons.RightStickPress];
                buttonInput[(int)ButtonInput.Roll] = mGamepadInput.buttonInputs[(int)GamepadButtons.LeftTrigger];
                buttonInput[(int)ButtonInput.CycleQuickUseLeft] = mGamepadInput.buttonInputs[(int)GamepadButtons.DpadLeft];
                buttonInput[(int)ButtonInput.CycleQuickUseRight] = mGamepadInput.buttonInputs[(int)GamepadButtons.DpadRight];

                break;

            case PlayerInputState.Inventory:
                //Update the axis inputs
                axisInput[(int)AxisInput.LeftStickX] = 0;
                axisInput[(int)AxisInput.LeftStickY] = 0;
                axisInput[(int)AxisInput.RightStickX] = 0;
                axisInput[(int)AxisInput.RightStickY] = 0;

                //Update the button inputs
                //These are for reading in "taps" of the stick buttons
                buttonInput[(int)ButtonInput.LeftStick_Left] = false;
                buttonInput[(int)ButtonInput.LeftStick_Right] = false;
                buttonInput[(int)ButtonInput.LeftStick_Down] = false;
                buttonInput[(int)ButtonInput.LeftStick_Up] = false;
                buttonInput[(int)ButtonInput.DPad_Left] = false;
                buttonInput[(int)ButtonInput.DPad_Right] = false;
                buttonInput[(int)ButtonInput.Interact] = false;
                buttonInput[(int)ButtonInput.DPad_Up] = false;

                buttonInput[(int)ButtonInput.Jump] = false;
                buttonInput[(int)ButtonInput.LightAttack] = false;
                buttonInput[(int)ButtonInput.ActivateGadget] = false;
                buttonInput[(int)ButtonInput.PlayerMenu] = mGamepadInput.buttonInputs[(int)GamepadButtons.SelectButton];
                buttonInput[(int)ButtonInput.Pause] = false;
                buttonInput[(int)ButtonInput.Minimap] = false;
                buttonInput[(int)ButtonInput.SkipLevel] = false;
                buttonInput[(int)ButtonInput.Gadget1] = false;
                buttonInput[(int)ButtonInput.Fire] = false;
                buttonInput[(int)ButtonInput.BeamUp] = false;
                buttonInput[(int)ButtonInput.SwapWeapon] = false;

                buttonInput[(int)ButtonInput.Consumable] = false;
                buttonInput[(int)ButtonInput.InventoryDrop] = mGamepadInput.buttonInputs[(int)GamepadButtons.EastButton];
                buttonInput[(int)ButtonInput.InventoryMove] = mGamepadInput.buttonInputs[(int)GamepadButtons.WestButton];
                buttonInput[(int)ButtonInput.InventorySort] = mGamepadInput.buttonInputs[(int)GamepadButtons.SelectButton];
                buttonInput[(int)ButtonInput.CycleQuickUseLeft] = false;
                buttonInput[(int)ButtonInput.CycleQuickUseRight] = false;
                buttonInput[(int)ButtonInput.ChangeTabLeft] = mGamepadInput.buttonInputs[(int)GamepadButtons.LeftBumper];
                buttonInput[(int)ButtonInput.ChangeTabRight] = mGamepadInput.buttonInputs[(int)GamepadButtons.RightBumper];
                buttonInput[(int)ButtonInput.Menu_Back] = false;
                buttonInput[(int)ButtonInput.FireMode] = false;
                buttonInput[(int)ButtonInput.Roll] = false;

                break;

            case PlayerInputState.NavigationMenu:
                //Update the axis inputs
                axisInput[(int)AxisInput.LeftStickX] = 0;
                axisInput[(int)AxisInput.LeftStickY] = 0;
                axisInput[(int)AxisInput.RightStickX] = 0;
                axisInput[(int)AxisInput.RightStickY] = 0;

                //Update the button inputs
                //These are for reading in "taps" of the stick buttons
                buttonInput[(int)ButtonInput.LeftStick_Left] = false;
                buttonInput[(int)ButtonInput.LeftStick_Right] = false;
                buttonInput[(int)ButtonInput.LeftStick_Down] = false;
                buttonInput[(int)ButtonInput.LeftStick_Up] = false;
                buttonInput[(int)ButtonInput.DPad_Left] = false;
                buttonInput[(int)ButtonInput.DPad_Right] = false;
                buttonInput[(int)ButtonInput.Interact] = false;
                buttonInput[(int)ButtonInput.DPad_Up] = false;
                buttonInput[(int)ButtonInput.BeamUp] = false;

                buttonInput[(int)ButtonInput.Jump] = false;
                buttonInput[(int)ButtonInput.LightAttack] = false;
                buttonInput[(int)ButtonInput.ActivateGadget] = false;
                buttonInput[(int)ButtonInput.PlayerMenu] = false;
                buttonInput[(int)ButtonInput.Pause] = false;
                buttonInput[(int)ButtonInput.Minimap] = false;
                buttonInput[(int)ButtonInput.SkipLevel] = false;
                buttonInput[(int)ButtonInput.Gadget1] = false;
                buttonInput[(int)ButtonInput.Fire] = false;
                buttonInput[(int)ButtonInput.SwapWeapon] = false;

                buttonInput[(int)ButtonInput.Consumable] = false;
                buttonInput[(int)ButtonInput.InventoryDrop] = false;
                buttonInput[(int)ButtonInput.InventoryMove] = false;
                buttonInput[(int)ButtonInput.InventorySort] = false;
                buttonInput[(int)ButtonInput.CycleQuickUseLeft] = false;
                buttonInput[(int)ButtonInput.CycleQuickUseRight] = false;
                buttonInput[(int)ButtonInput.ChangeTabLeft] = mGamepadInput.buttonInputs[(int)GamepadButtons.LeftBumper];
                buttonInput[(int)ButtonInput.ChangeTabRight] = mGamepadInput.buttonInputs[(int)GamepadButtons.RightBumper];
                buttonInput[(int)ButtonInput.Menu_Back] = mGamepadInput.buttonInputs[(int)GamepadButtons.EastButton];
                buttonInput[(int)ButtonInput.FireMode] = false;
                buttonInput[(int)ButtonInput.Roll] = false;

                break;
            case PlayerInputState.Shop:
                //Update the axis inputs
                axisInput[(int)AxisInput.LeftStickX] = 0;
                axisInput[(int)AxisInput.LeftStickY] = 0;
                axisInput[(int)AxisInput.RightStickX] = 0;
                axisInput[(int)AxisInput.RightStickY] = 0;

                //Update the button inputs
                //These are for reading in "taps" of the stick buttons
                buttonInput[(int)ButtonInput.LeftStick_Left] = false;
                buttonInput[(int)ButtonInput.LeftStick_Right] = false;
                buttonInput[(int)ButtonInput.LeftStick_Down] = false;
                buttonInput[(int)ButtonInput.LeftStick_Up] = false;
                buttonInput[(int)ButtonInput.DPad_Left] = false;
                buttonInput[(int)ButtonInput.DPad_Right] = false;
                buttonInput[(int)ButtonInput.Interact] = false;
                buttonInput[(int)ButtonInput.DPad_Up] = false;
                buttonInput[(int)ButtonInput.BeamUp] = false;

                buttonInput[(int)ButtonInput.Jump] = false;
                buttonInput[(int)ButtonInput.LightAttack] = false;
                buttonInput[(int)ButtonInput.ActivateGadget] = false;
                buttonInput[(int)ButtonInput.PlayerMenu] = false;
                buttonInput[(int)ButtonInput.Pause] = false;
                buttonInput[(int)ButtonInput.Minimap] = false;
                buttonInput[(int)ButtonInput.SkipLevel] = false;
                buttonInput[(int)ButtonInput.Gadget1] = false;
                buttonInput[(int)ButtonInput.Fire] = false;
                buttonInput[(int)ButtonInput.SwapWeapon] = false;

                buttonInput[(int)ButtonInput.Consumable] = false;
                buttonInput[(int)ButtonInput.InventoryDrop] = false;
                buttonInput[(int)ButtonInput.InventoryMove] = false;
                buttonInput[(int)ButtonInput.InventorySort] = false;
                buttonInput[(int)ButtonInput.CycleQuickUseLeft] = false;
                buttonInput[(int)ButtonInput.CycleQuickUseRight] = false;
                buttonInput[(int)ButtonInput.ChangeTabLeft] = mGamepadInput.buttonInputs[(int)GamepadButtons.LeftBumper];
                buttonInput[(int)ButtonInput.ChangeTabRight] = mGamepadInput.buttonInputs[(int)GamepadButtons.RightBumper];
                buttonInput[(int)ButtonInput.Menu_Back] = mGamepadInput.buttonInputs[(int)GamepadButtons.EastButton];
                buttonInput[(int)ButtonInput.FireMode] = false;
                buttonInput[(int)ButtonInput.Roll] = false;

                break;

            case PlayerInputState.Paused:
               //Update the axis inputs
                axisInput[(int)AxisInput.LeftStickX] = 0;
                axisInput[(int)AxisInput.LeftStickY] = 0;
                axisInput[(int)AxisInput.RightStickX] = 0;
                axisInput[(int)AxisInput.RightStickY] = 0;

                //Update the button inputs
                //These are for reading in "taps" of the stick buttons
                buttonInput[(int)ButtonInput.LeftStick_Left] = false;
                buttonInput[(int)ButtonInput.LeftStick_Right] = false;
                buttonInput[(int)ButtonInput.LeftStick_Down] = false;
                buttonInput[(int)ButtonInput.LeftStick_Up] = false;
                buttonInput[(int)ButtonInput.DPad_Left] = false;
                buttonInput[(int)ButtonInput.DPad_Right] = false;
                buttonInput[(int)ButtonInput.Interact] = false;
                buttonInput[(int)ButtonInput.DPad_Up] = false;
                buttonInput[(int)ButtonInput.BeamUp] = false;

                buttonInput[(int)ButtonInput.Jump] = false;
                buttonInput[(int)ButtonInput.LightAttack] = false;
                buttonInput[(int)ButtonInput.ActivateGadget] = false;
                buttonInput[(int)ButtonInput.PlayerMenu] = false;
                buttonInput[(int)ButtonInput.Pause] = mGamepadInput.buttonInputs[(int)GamepadButtons.StartButton];
                buttonInput[(int)ButtonInput.Minimap] = false;
                buttonInput[(int)ButtonInput.SkipLevel] = false;
                buttonInput[(int)ButtonInput.Gadget1] = false;
                buttonInput[(int)ButtonInput.Fire] = false;
                buttonInput[(int)ButtonInput.SwapWeapon] = false;

                buttonInput[(int)ButtonInput.Consumable] = false;
                buttonInput[(int)ButtonInput.InventoryDrop] = false;
                buttonInput[(int)ButtonInput.InventoryMove] = false;
                buttonInput[(int)ButtonInput.InventorySort] = false;
                buttonInput[(int)ButtonInput.CycleQuickUseLeft] = false;
                buttonInput[(int)ButtonInput.CycleQuickUseRight] = false;
                buttonInput[(int)ButtonInput.ChangeTabLeft] = false;
                buttonInput[(int)ButtonInput.ChangeTabRight] = false;
                buttonInput[(int)ButtonInput.Menu_Back] = mGamepadInput.buttonInputs[(int)GamepadButtons.EastButton];
                buttonInput[(int)ButtonInput.FireMode] = false;
                buttonInput[(int)ButtonInput.Roll] = false;

                break;
        }
        
    }
}
