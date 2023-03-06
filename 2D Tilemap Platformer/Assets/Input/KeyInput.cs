using UnityEngine;
using System.Collections;

/// <summary>
/// Key input enumeration for easy input sending.
/// </summary>
public enum ButtonInput
{
    LeftStick_Left = 0,
    LeftStick_Right,
    LeftStick_Down,
    LeftStick_Up,
    DPad_Left,
    DPad_Right,
    Interact,
    DPad_Up,
    Jump,
    LightAttack,
    ActivateGadget,
    PlayerMenu,
    Pause,
    Minimap,
    SkipLevel,
    Gadget1,
    BeamUp,
    Fire,
    SwapWeapon,
    Consumable,
    InventoryDrop,
    InventoryMove,
    InventorySort,
    CycleQuickUseLeft,
    CycleQuickUseRight,
    ChangeTabLeft,
    ChangeTabRight,
    Menu_Back,
    FireMode,
    Roll,
    Attack_Left,
    Attack_Right,
    Attack_Up,
    Attack_Down,
    Attack_Neutral,
    Count
}

public enum AxisInput
{
    LeftStickX = 0,
    LeftStickY,
    RightStickX,
    RightStickY,
    Count
}