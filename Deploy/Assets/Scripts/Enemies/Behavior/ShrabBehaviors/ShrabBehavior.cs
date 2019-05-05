using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrabBehavior : MonoBehaviour
{
    public float shurikenRange;
    public List<Shrab> shrabsInRange;
    public float pincerMovementRange;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        this.shurikenRange = 15;
        this.shrabsInRange = new List<Shrab>();
        this.pincerMovementRange = 10;
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Shrab s in Shrab.shrabList)
        {
            if(Vector3.Distance(s.referenceObject.transform.position, transform.position) < pincerMovementRange)
            {
                shrabsInRange.Add(s);
            }
        }
        int numShrabs = 0;
        foreach(Shrab s in shrabsInRange)
        {
            numShrabs += s.getNumShrabs();
        }
        if(numShrabs >= 12)
        {
            pincerMovement();
        }
    }

    public void pincerMovement()
    {
        foreach (Shrab s in shrabsInRange)
        {
            s.referenceObject.AddComponent<SphericalMovement>();
        }
    }

    public void waterShurikenAttack()
    {

    }
}
