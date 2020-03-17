// following https://www.youtube.com/watch?v=g5ED-d-RadQ&list=PLBIb_auVtBwBq9S1R-j4oL0HnlDh_rpLW&index=3

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum colours {"Red", "Blue", "Green"}

public class ChangeColour : MonoBehaviour
{
    public GameObject panel;
    public SpriteRenderer head;

    public Color red, blue, green;

    public int currColour = 1;

    private void Awake()
    {
        panel.SetActive(false);
        colourPick(-1);
    }

    public void OpenPanel()
    {
        panel.SetActive(true);
    }
    public void ClosePanel()
    {
        panel.SetActive(false);
    }

    public void colourPick(int c)
    {
        switch (c)
        {
            case 0:
                head.color = Color.red;
                
                break;
            case 1:
                head.color = Color.blue;
                break;
            case 2:
                head.color = Color.green;
                break;
            default:
                head.color = Color.white;
                break;
        }
    }
}
