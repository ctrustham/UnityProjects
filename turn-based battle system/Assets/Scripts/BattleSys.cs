// using https://www.youtube.com/watch?v=_1pz_ohupPs as a guide/tutorial

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYER_TURN, ENEMY_TURN, WON, LOST }

public class BattleSys : MonoBehaviour
{
    // these are all set using the unity GUI by drag-dropping
    public GameObject playerPrefab; // gameObject holds all info for an entity
    public GameObject enemyPrefab;

    public Transform playerBattleStation; // transform holds position information of an entity
    public Transform enemyBattleStation;

    public Text dialogueTxt;

    public BattleHud playerHud;
    public BattleHud enemyHud;

    public Button atkButton;


    Unit playerUnit; // information about the player's pokemon/fighter
    Unit enemyUnit;

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine( SetupBattle());
    }

    // turn SetupBattle() into co-routine to run parallel - allows for wating / time delay
    #region Callback info
     /* A coroutine is like a function that has the ability to pause execution 
     * and return control to Unity but then to continue where it left off on the 
     * following frame.
     * A 'yield return null;' line is the point at which execution will pause and be
     * resumed the following frame. *By default, a coroutine is resumed on the frame 
     * after it yields but it is also possible to introduce a time delay using WaitForSeconds(),
     * as in the case below
     * 
     * Note: You can stop a Coroutine with StopCoroutine and StopAllCoroutines. A coroutines also 
     * stops when the GameObject it is attached to is disabled with SetActive(false). Calling 
     * Destroy(example) (where example is a MonoBehaviour instance) immediately triggers OnDisable
     * and the coroutine is processed, effectively stopping it. Finally, OnDestroy is invoked at 
     * the end of the frame.
     * Coroutines are not stopped when disabling a MonoBehaviour by setting enabled to false on a 
     * MonoBehaviour instance. 
     * */
    #endregion
        /// <summary>
        /// Sets up the initial battle state conditions, and updates the UI
        /// </summary>
        /// <returns></returns>
    IEnumerator SetupBattle()
    {
        GameObject playerGObj = Instantiate(playerPrefab, playerBattleStation); // create a new prefab as a child of the given object
        playerUnit = playerGObj.GetComponent<Unit>(); // store the unit info from the created game object
        GameObject enemyGObj = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGObj.GetComponent<Unit>();

        atkButton.interactable= false;
        atkButton.GetComponentInChildren<Text>(true).color = Color.gray;
        playerHud.setHud(playerUnit); // set up the HUD info using the Unit data
        enemyHud.setHud(enemyUnit);

        dialogueTxt.text = "a wild " + enemyUnit.name + " appered!";

        yield return new WaitForSeconds(2f); // delay/wait for 2 seconds

        state = BattleState.PLAYER_TURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        dialogueTxt.text = "Choose action...";
        atkButton.interactable = true;
        atkButton.GetComponentInChildren<Text>(true).color = Color.black;
    }

    public void OnAttackButton()
    {
        if (state == BattleState.PLAYER_TURN)
        {
            StartCoroutine(PlayerAttack());
        }

        return;
    }

    IEnumerator PlayerAttack()
    {
 
        //damage enemy
        dialogueTxt.text = "Player attacked!";

        //this can go here, but was moved to another co-routine to test coroutine-in-a-coroutine
        //float delay = 1f / playerUnit.damage;
        //for (int i = 0; i < playerUnit.damage; i++)
        //{
        //    yield return new WaitForSeconds(delay);  //this causes the hp bar to drain over time
        //    enemyUnit.currHp--;
        //    enemyHud.setHP(enemyUnit.currHp);
        //}

        StartCoroutine(DealDamage(playerUnit.damage, enemyHud, enemyUnit.currHp)); // change hp slider over time
        enemyUnit.currHp -= playerUnit.damage; // actually apply damage
        yield return new WaitForSeconds(2f);
        dialogueTxt.text = "Dealt " + playerUnit.damage + " damage!";
        

        yield return new WaitForSeconds(2f);
        if (enemyUnit.currHp <= 0)
        {
            dialogueTxt.text = "enemy fainted!";
        }
        //change state to enemy turn
        dialogueTxt.text = "enemy turn!";
    }

    IEnumerator DealDamage(int dam, BattleHud hud, int hp)
    {
        
        float delay = 1f / dam;
        for (int i = 0; i < dam; i++)
        {
            yield return new WaitForSeconds(delay); //this causes the hp bar to drain over time
            hp--;
            enemyHud.setHP(hp);
        }

    }
}
