using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraSoundManager : MonoBehaviour
{
    public bool enemyInRange = false;
    public bool jupiterPullCast = false;
    public bool isJumping = false;
    public bool hasLanded = false;

    public AudioSource srcTrack;
    public AudioSource srcAttacksAbilities;
    public AudioSource srcMovement;

    public AudioClip worldTrack;
    public AudioClip battleTrack;
    public AudioClip jup_pull;
    public AudioClip jumpAir;
    public AudioClip jumpLand;

    bool wasEnemyInRange = false;
    // Start is called before the first frame update
    void Start()
    {

        srcTrack.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyInRange)
        {
            Debug.Log("Enemy in range");
            if (!wasEnemyInRange)
            {
                Debug.Log("Playing in audio");
                srcTrack.Stop();
                srcTrack.clip = battleTrack;
                srcTrack.Play();
            }
            wasEnemyInRange = enemyInRange;
        }
        else
        {
           
            if (wasEnemyInRange)
            {
                srcTrack.Stop();
                srcTrack.clip = worldTrack;
                srcTrack.Play();
            }
            wasEnemyInRange = enemyInRange;
        }
        if (jupiterPullCast)
        {
            Debug.Log("JupiterPullCast detected");
            if (!srcAttacksAbilities.isPlaying)
            {
                srcAttacksAbilities.PlayOneShot(jup_pull);
            }
           
        }
        if (isJumping)
        {
            if (!srcMovement.isPlaying)
            {
                srcMovement.PlayOneShot(jumpAir);
            }
        }
        else
        {
            Debug.Log("No JupiterPullCast");
        }
    }


}
