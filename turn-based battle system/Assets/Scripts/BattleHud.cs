// using https://www.youtube.com/watch?v=_1pz_ohupPs as a guide/tutorial

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Holds references to all the data for each fighter
/// </summary>
public class BattleHud : MonoBehaviour
{
    //  these are set using the unity GUI by drag-dropping
    public Text NameTxt;
    public Slider hpSlider;

    public Text hpVal;

    /// <summary>
    /// Changes HUD elements to relfect Unit's values
    /// </summary>
    /// <param name="unit">The pokemon/fighter the HUD is reflecting</param>
    public void setHud(Unit unit)
    {
        NameTxt.text = unit.name;
        hpSlider.maxValue = unit.MaxHp; // set the maximum value the HP slider can represent
        hpSlider.value = unit.currHp; // set the current value the HP slider is representing
        hpVal.text = "" + unit.currHp;
    }

    public void setHP(int hp)
    {
        hpSlider.value = hp;
        hpVal.text = "" + hp;
    }


}
