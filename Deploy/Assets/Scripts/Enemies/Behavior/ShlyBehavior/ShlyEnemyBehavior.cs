using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShlyEnemyBehavior : MonoBehaviour
{
    public GameObject player;
    public ShlyEnemy shly;
    private Rigidbody r;
    private float speed;

    public float searchRadius;
    public float sprainEffectRadius;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        speed = GetComponent<StatManager>().kamikazeMovementSpeed;
        r = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        shly = GetComponent<StatManager>().shly;
        anim = GetComponent<Animator>();

        /*foreach (ShlyEnemy e in Enemy.enemyList) {
            ShlyEnemy.shlyList.Add(e); //adds all shlies to shlylist
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform); //Ensures they're always looking at the player
        Vector3 playerPos = player.transform.position;
        Vector3 enemyPos = this.gameObject.transform.position;
        shly.pelletDrop(searchRadius);
        shly.bullCharge(searchRadius);
        shly.sprain(sprainEffectRadius);
    }  
}
