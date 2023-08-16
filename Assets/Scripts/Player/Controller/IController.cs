using UnityEngine;
using System.Collections;

/*
Description:Enums for the possible controller types the player may have
Creator: Alvaro Chavez Mixco
Creation Date: Wednesday, October 19th, 2016
*/
public enum EControllerTypes
{
    ViveController,
    GamepadController,
    MouseAndKeyboardController,
}

/*
Description: Enum to easily know in which hand is the current CWeaponControlInput in, and match controls accordingly
Creator: Alvaro Chavez Mixco
Creation Date: Thursday, October 10th, 2016
*/
public enum EWeaponHand
{
    None,
    RightHand,
    LeftHand,
    BothHands
}

/*
Description:Interface used to check for player inputs (axis and button presses) of the main player actions.
Creator: Alvaro Chavez Mixco
Creation Date: Wednesday, October 19th, 2016
*/
public interface IController
{
    CWeaponControlInput PRightWeaponControl { get; }
    CWeaponControlInput PLeftWeaponControl { get; }

    void UpdateController();

    Vector3 GetMoveInput();
    Vector3 GetLookInput();

    bool GetPause();

    bool GetAnyKeyPressed();

    bool GetInterruptKeyPressed();

    void SetRumbleController(float aDuration, ushort aStrength, EWeaponHand aHand = EWeaponHand.RightHand);

    //These are not properties so that we can suscribe to events
    void SetInputSensitivity(float aInputSensitivity);
    void SetInvertedYAxis(bool aIsInverted);
}
