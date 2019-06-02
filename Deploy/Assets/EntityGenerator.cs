using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityGenerator : MonoBehaviour
{
    public Mesh entityMesh;
    public Vector3 EulerRotationDisplacement;
    public Vector3 PositionDisplacement;

    public Vector3 scaleMultiplier;

    public List<EntitySpawner> spawners;

    public List<Vector3> positions;
    public List<Quaternion> rotations;
    public List<Vector3> scales;

    public CollisionHandler collisionHandler;


    public void CreateEntity(Vector3 position, Quaternion rotation, Vector3 scale, int closestPoint){
        positions.Add(position);
        rotations.Add(rotation);
        scales.Add(scale);

        Quaternion newRot = Quaternion.Euler(EulerRotationDisplacement.x, EulerRotationDisplacement.y, EulerRotationDisplacement.z);


        Quaternion trueRot = rotation * newRot;
        Vector3 truePos = position + trueRot * PositionDisplacement;
        Vector3 trueScale = Vector3.Scale(scale, scaleMultiplier);

        foreach(EntitySpawner e in spawners){
            e.CreateArchetype(truePos, trueRot, trueScale);
        }
        collisionHandler.AddObject(this, closestPoint, truePos, trueRot, trueScale);
    }
}
