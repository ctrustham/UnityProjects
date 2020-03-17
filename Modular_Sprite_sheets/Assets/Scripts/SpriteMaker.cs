// using https://www.youtube.com/watch?v=cIIaKdlZ4Cw

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))] // automatically attach a sprite renderer on the object this script is attached to
public class SpriteMaker : MonoBehaviour
{
    SpriteRenderer rend;
    public Texture2D src;
    
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>(); // reference to the attached spriteRenderer
        
        //create a texture
        Texture2D t = new Texture2D(80,80);
        Color[] colourArray = new Color[t.width * t.height]; // the array of colours for pixels that make up the texture
        Color[] srcArray = src.GetPixels(); // copy the pixels in the source image

        //fill t with colour
        for (int x = 0; x < t.width; x++)
        {
            for (int y = 0; y < t.height; y++)
            {
                int index = x + (y * t.width);
                //Color srcPixel = srcArray[index];

                // create gradient from black on bottom to white on top
                 colourArray[index] = Color.Lerp(Color.black, Color.white, (float)y / (float)t.width);

                // copy each pixel from one array to the next
                //colourArray[index] = srcPixel;
            }
        }

        t.SetPixels(0, 0, t.width, t.height, colourArray);
        t.Apply(); // apply changes

        t.wrapMode = TextureWrapMode.Clamp; // sets the edges to be sharp -> for an individual / stand-alone sprite; not intended for tiling
        t.filterMode = FilterMode.Bilinear; // sets the blending of pixels

        //create a sprite from the texture
        Sprite newSprite = Sprite.Create(t, new Rect(0, 0, 80, 80), Vector2.one * 0.5f); // make sprite from 0,0 to 8,8 and have a pivot point in the middle (pivot is vector2 from 0-1 => 0.5 is in the middle)

        // assign procedural sprite to rend.sprite
        rend.sprite = newSprite;
        rend.name = "test";
    }
}
