using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Stores relevant information for the current pokemon </summary>
public class Unit : MonoBehaviour
{
    //  these are all set using the unity GUI
    public int MaxHp, currHp, damage;
    public string name;

    public bool takeDAmage(int dmg)
    {
        bool dead = false;
        currHp -= dmg;
        if (currHp <= 0)
        {
            currHp = 0;
            dead = true;
        }
        return dead;
    }
}
