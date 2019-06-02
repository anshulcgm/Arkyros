using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Rendering;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class EntitySpawner : MonoBehaviour
{
    private static EntityManager Manager;
    private RenderMesh Renderer;
    private EntityArchetype Archetype;

    // Start is called before the first frame update
    public void Awake()
    {
        Manager = World.Active.GetOrCreateManager<EntityManager>();

        Archetype = Manager.CreateArchetype(typeof(LocalToWorld));
    }

    public void Start(){
        Renderer = GetComponent<RenderMeshProxy>().Value;
    }

    public void CreateArchetype(Vector3 position, Quaternion rotation, Vector3 scale){
        Entity entity = Manager.CreateEntity(Archetype);
        Matrix4x4 m = Matrix4x4.TRS(position, rotation, scale);
        Manager.SetComponentData(entity, new LocalToWorld {
            Value = new float4x4(
                m[0,0], m[0,1], m[0,2], m[0,3],
                m[1,0], m[1,1], m[1,2], m[1,3],
                m[2,0], m[2,1], m[2,2], m[2,3],
                m[3,0], m[3,1], m[3,2], m[3,3]
            )});
        Manager.AddSharedComponentData(entity, Renderer);
    }   
}
