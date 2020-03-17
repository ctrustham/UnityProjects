// following https://www.youtube.com/watch?v=tvi44I_SK3w

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Collections;

public class SSSAnim_GameHandler : MonoBehaviour
{
    private static SSSAnim_GameHandler instance;


    public static SSSAnim_GameHandler GetInstance()
    {
        return instance;
    }

    // here for testing, should be moved to more appropriate place?
    public Mesh quadMesh;
    public Material spriteMaterial;

    private void Awake()
    {
        instance = this; // allows accessing mesh and material fields -> just for testing; not a good way to manage references

        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityArchetype myArchetype = entityManager.CreateArchetype(
            typeof(Translation)
            );

        Entity myEentity = entityManager.CreateEntity(myArchetype);
    }
}
