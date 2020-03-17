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

public class SpriteSheetRenderer : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref Translation t) =>
       {
           Graphics.DrawMesh(SSSAnim_GameHandler.GetInstance().quadMesh, // the reference to the mesh
               t.Value, // the translation value (location of the object)
               Quaternion.identity, // no rotation
               SSSAnim_GameHandler.GetInstance().spriteMaterial,
               0); // the layer to draw on
       });
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
}

