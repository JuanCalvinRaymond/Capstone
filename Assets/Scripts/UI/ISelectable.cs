using UnityEngine;
using System.Collections;

/*
Description: Interface used by CSelectingMenu script in order to call events, like OnHover,OnUnHover, 
, OnClick, annd OnUnClick
Creator: Alvaro Chavez Mixco
Creation Date: Tuesday, December 27, 2016
*/
public interface ISelectable
{
    /*
    Description: Function to know if the current selectable object is currently being selected by any object
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, March 28th, 2016
    */
    bool GetIsSelected();

    /*
    Description: Function call when the object is constantly being pressed (Pressed)
    Parameters: Vector3 aHitPosition - The world space position where the object was "Pointed"/selected
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, December 27, 2016
    */
    void OnPress(Vector3 aHitPosition);

    /*
    Description: Function call when the object has been clicked once (Pressed Down)
    Parameters: Vector3 aHitPosition - The world space position where the object was "Pointed"/selected
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, December 27, 2016
    */
    void OnClick(Vector3 aHitPosition);

    /*
    Description: Function call when the object has been unclicked once (Pressed Up)
    Parameters: Vector3 aHitPosition - The world space position where the object was "Pointed"/selected
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, December 27, 2016
    */
    void OnUnClick(Vector3 aHitPosition);

    /*
    Description: Function call when the object is being currently being aimed at/hovered by the player.
    Parameters: Vector3 aHitPosition - The world space position where the object was "Pointed"/selected
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, December 27, 2016
    */
    void OnHover(Vector3 aHitPosition);

    /*
    Description: Function call when the object that was previously hovering, stops being aimed at
    Parameters: Vector3 aHitPosition - The world space position where the object was "Pointed"/selected
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, December 27, 2016
    */
    void OnUnHover();
}
