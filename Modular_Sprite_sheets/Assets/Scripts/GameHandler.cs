using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private Texture2D headTexture; // the sprite with all the heads
    [SerializeField] private Texture2D bodyTexture; // the sprite with all the bodies

    [SerializeField] private Material dudeMaterial;

    private void Awake()
    {
        Texture2D newTexture = new Texture2D(32,64, TextureFormat.RGBA32, true); // has all colours + alpha chanel; mipChain = true

        Color[] spritesheetHeadPixels = headTexture.GetPixels(0,0,32,32); // returns an array with the colour of each pixel
        newTexture.SetPixels(0,32,32,32,spritesheetHeadPixels);

        Color[] spritesheetBodyPixels = bodyTexture.GetPixels(0, 0, 32, 32); // returns an array with the colour of each pixel
        newTexture.SetPixels(0, 0, 32, 32, spritesheetBodyPixels);

        newTexture.Apply(); // NEED to call this for changes to be applied

        dudeMaterial.mainTexture = newTexture;
        newTexture = (Texture2D)dudeMaterial.mainTexture;
        
    }
}
