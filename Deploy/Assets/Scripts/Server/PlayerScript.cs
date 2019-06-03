using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    int[] abilities = null;
    private string keyInput = "";
    private bool m1Down = false;
    private bool m2Down = false;
    private Quaternion camRot = Quaternion.identity;
    private Vector3 camPos = Vector3.zero;

    public void HandleInput(string keyIn, Quaternion cameraRotation, Vector3 cameraPosition, bool[] mouseIn){
        camRot = cameraRotation;
        camPos = cameraPosition;
        keyInput = keyIn;
        m1Down = mouseIn[0];
        m2Down = mouseIn[1];
    }

    //player input stuff
    public Quaternion CameraRotation(){
        return camRot;
    }

    public Vector3 CameraPosition(){
        return camPos;
    }

    public bool M2Down(){
        return m2Down;
    }
    public bool M1Down(){
        return m1Down;
    }

    public bool IsPressed(char val){
        return keyInput.Contains(val + "");
    }

    public bool IsPressed(string val){
        return keyInput.Contains(val);
    }

    public bool IsPressedDown(string val){
        return IsPressed(val);
    }

    //abilities
    public int[] GetAbilities(){
        return abilities;
    }

    public void SetAbilities(int[] abilities){
        this.abilities = abilities;
    }
}