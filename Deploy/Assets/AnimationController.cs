﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimationController : MonoBehaviour
{
    public Animation anim;
    public Transform[] attackTransforms;
    public string[] attackAnims = new string[] { };
    public string[] loopingAnims = new string[] { };
    public string currentLoopingAnim = "";
    public string currentOverlayAnim = "";

    // Start is called before the first frame update
    void Start()
    {
        foreach (string animStr in attackAnims)
        {
            if(anim[animStr] == null){
                Debug.Log(animStr + " is broken");
            }
            anim.AddClip(anim[animStr].clip, animStr + "-a");
            anim[animStr].wrapMode = WrapMode.ClampForever;
            anim[animStr + "-a"].wrapMode = WrapMode.ClampForever;
            foreach (Transform t in attackTransforms)
            {
                anim[animStr + "-a"].AddMixingTransform(t);
            }
            anim[animStr + "-a"].layer = 1;
        }
        foreach (string animStr in loopingAnims)
        {
            anim[animStr].wrapMode = WrapMode.Loop;
        }
    }

    DateTime overlayAnimStart = DateTime.MinValue;
    DateTime overlayAnimEnd = DateTime.MinValue;
    int overlayIndex = -1;
    string currentStopAnim = "";
    bool cancel = false;

    int gameObjectId = -1;

    void Update()
    {
        if(Server.serverExists && gameObjectId == -1){
            for(int i = 0; i < Server.gameObjectsToUpdate.Count; i++){
                if(Server.gameObjectsToUpdate[i].Equals(gameObject)){
                    gameObjectId = i;
                    break;
                }
            }
        }

        if ((DateTime.Now - start).TotalSeconds < duration)
        {
            PlayOverlayAnim(animation, strength);
        }
        else
        {
            animation = "";
        }

        if (!currentOverlayAnim.Equals("") && ((DateTime.Now - overlayAnimStart).TotalSeconds > 0.05f))
        {
            anim.Blend(currentOverlayAnim + "-a", 0.0f, 0.3f);
            anim.Blend(currentOverlayAnim, 0.0f, 0.3f);
            anim.Blend(currentLoopingAnim, 1.0f, 0.3f);
            overlayAnimStart = DateTime.MaxValue;
            overlayIndex = -1;
            currentStopAnim = currentOverlayAnim;
            currentOverlayAnim = "";
            overlayAnimEnd = DateTime.Now;
        }

        if (!currentStopAnim.Equals("") && (DateTime.Now - overlayAnimEnd).TotalSeconds > 0.3f)
        {
            anim[currentStopAnim].time = 0.0f;
            anim[currentStopAnim + "-a"].time = 0.0f;
            anim.Sample();
            anim.Stop(currentStopAnim);
            anim.Stop(currentStopAnim + "-a");
            overlayAnimEnd = DateTime.MinValue;
            currentStopAnim = "";
        }
    }

    public void PlayLoopingAnim(string animation)
    {
        anim.Blend(animation, 1.0f, 0.1f);
        anim.CrossFade(animation, 0.3f);
        currentLoopingAnim = animation;
        if(Server.serverExists){
            Server.instance.SendAnimation(gameObjectId, animation, false);
        }
    }

    private void PlayOverlayAnim(string overlayAnim, float strength)
    {
        anim.Blend(overlayAnim + "-a", 1.0f, 0.5f);
        anim.Blend(overlayAnim, 1.0f, 0.5f);

        anim.Blend(currentLoopingAnim, 1 - strength, 0.3f);
        anim.Blend(overlayAnim, strength, 0.3f);
        PlayCompleteAnim(overlayAnim + "-a");
        currentOverlayAnim = overlayAnim;
        for (int i = 0; i < attackAnims.Length; i++)
        {
            if (attackAnims[i].Equals(overlayAnim))
            {
                overlayIndex = i;
                break;
            }
        }
        overlayAnimStart = DateTime.Now;
    }

    private void PlayCompleteAnim(string completeAnim)
    {
        if (completeAnim.Equals(currentStopAnim) || completeAnim.Equals(currentStopAnim + "-a"))
        {
            anim[currentStopAnim].time = 0.0f;
            anim[currentStopAnim + "-a"].time = 0.0f;
            anim.Blend(currentLoopingAnim, 1.0f, 0.0f);
            anim.Sample();
            anim.Stop(currentStopAnim);
            anim.Stop(currentStopAnim + "-a");
            overlayAnimEnd = DateTime.MinValue;
            currentStopAnim = "";
        }
        anim.CrossFade(completeAnim, 0.3f);
    }


    string animation = "";
    float strength = 0; float duration = 0;
    DateTime start = DateTime.MinValue;
    public void StartOverlayAnim(string animation, float strength, float duration)
    {
        if (!this.animation.Equals(animation) && !this.animation.Equals(""))
        {
            anim.Blend(currentOverlayAnim + "-a", 0.0f, 0.3f);
            anim.Blend(currentOverlayAnim, 0.0f, 0.3f);
            anim.Blend(currentLoopingAnim, 1.0f, 0.3f);
            overlayAnimStart = DateTime.MaxValue;
            overlayIndex = -1;
            currentStopAnim = currentOverlayAnim;
            currentOverlayAnim = "";
            overlayAnimEnd = DateTime.Now;
        }
        this.animation = animation;
        this.strength = strength;
        this.duration = duration;
        start = DateTime.Now;        
        if(Server.serverExists){
            Server.instance.SendAnimation(gameObjectId, animation, true, strength, duration);
        }            
    }
}
