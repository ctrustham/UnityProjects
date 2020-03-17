// following https://www.youtube.com/watch?v=6eV9NR3Vb9U&list=PLzDRvYVwl53s40yP5RQXitbT--IRcHqba&index=8&t=88s

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Collections;

public class ECS_SpriteMaker_GameHandler : MonoBehaviour
{
    //[SerializeField] private Mesh mesh;
    [SerializeField] private Material material;

    private void Awake()
    {
        EntityManager myEntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        NativeArray<Entity> entityArr = new NativeArray<Entity>(10, Allocator.Temp);

        /// replaced with array of entities
        //Entity entity = entMngr.CreateEntity( //The types of components to add to the new entity:
        //    typeof(RenderMesh), // the renderer
        //    typeof(WorldRenderBounds), // needed by the renderer
        //    typeof(RenderBounds), // needed by the renderer
        //    typeof(LocalToWorld), // calculates a matrix for how the mesh should be displayed - needed to make the rendermesh visable
        //    typeof(Translation), // calculates local-to-world value. Not required but saves from having to set the world vals by hand
        //    typeof(Rotation), // the rotation of the object
        //    //typeof(Scale) // scale in all directions the same
        //     typeof(NonUniformScale) // scale in each direction
        //    );

        EntityArchetype myEntity = myEntityManager.CreateArchetype( //The types of components to add to the new entity:
            typeof(RenderMesh), // the renderer
            typeof(WorldRenderBounds), // needed by the renderer
            typeof(RenderBounds), // needed by the renderer
            typeof(LocalToWorld), // calculates a matrix for how the mesh should be displayed - needed to make the rendermesh visable
            typeof(Translation), // calculates local-to-world value. Not required but saves from having to set the world vals by hand
            typeof(Rotation) // the rotation of the object
          //typeof(Scale) // scale in all directions the same
          //typeof(NonUniformScale) // scale in each direction
        );

        myEntityManager.CreateEntity(myEntity, entityArr);

        /// moved to SpriteSheetRenderer
        //foreach (Entity e in entityArr)
        //{
        //    entMngr.SetSharedComponentData(e, new RenderMesh
        //    {
        //        mesh = CreateMesh(1f, 1f), // create a mesh (sprite) for the object
        //        material = material,
        //    });


        //    //entMngr.SetComponentData(e, new NonUniformScale
        //    //{
        //    //    Value = Vector3.one // new vector3(1f,3f,1f)
        //    //});

        //    entMngr.SetComponentData(e, new Translation
        //    {
        //        Value = new float3 (UnityEngine.Random.Range(-10f,10f), UnityEngine.Random.Range(-10f, 10f), 0f)
        //    });
        //}
        entityArr.Dispose();
    }

    private Mesh CreateMesh(float width = 1, float height = 1)
    {
        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        vertices[0] = new Vector3(-halfWidth, -halfHeight); // bottom left
        vertices[1] = new Vector3(-halfWidth, +halfHeight); // top left
        vertices[2] = new Vector3(+halfWidth, +halfHeight); // top right
        vertices[3] = new Vector3(+halfWidth, -halfHeight); // bottom right

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);

        //must be defined clockwise or the back will be shown
        triangles[0] = 0; // bottom left
        triangles[1] = 1; // top left
        triangles[2] = 3; // bottom right
        triangles[3] = 1; // top left
        triangles[4] = 2; // top right
        triangles[5] = 3; // bottom right

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        return mesh;
    }


    /// replaced/not needed with animations
    //public class MoveSystem : ComponentSystem
    //{
    //    protected override void OnUpdate()
    //    {
    //        Entities.ForEach((ref Translation trans) => // for each entity with a translation component
    //        {
    //            float moveSpeed = 1f;
    //            //trans.Value.x += moveSpeed * Time.DeltaTime;
    //        });
    //    }
    //}

    /// replaced/not needed with animations
    //public class RotateSystem : ComponentSystem
    //{
    //    protected override void OnUpdate()
    //    {
    //        Entities.ForEach((ref Rotation rot) => // for each entity with a rotation component
    //        {
    //            float rotSpeed = 1f;
    //            //rot.Value = quaternion.Euler(0,0, rotSpeed * math.PI * (float)Time.ElapsedTime);

    //            rot.Value = quaternion.RotateZ(rotSpeed * math.PI * (float)Time.ElapsedTime); // rotates around z axis by rotSpeed (counter-clockwise)
    //        });
    //    }
    //}

    /// replaced with creating custom material/sprite size
    //public class ScaleSystem : ComponentSystem
    //{
    //    protected override void OnUpdate()
    //    {
    //        Entities.ForEach((ref Scale scale) => // for each entity with a Scale component
    //        {
    //            float amount = 1f; // * (float)Time.ElapsedTime;
    //            scale.Value = amount;
    //        });
    //    }
    //}
}
