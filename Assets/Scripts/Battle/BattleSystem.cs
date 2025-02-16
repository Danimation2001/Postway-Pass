// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public enum BattleState { START, ATTACK, DEFENSE, WON, LOST }

// public class BattleSystem : MonoBehaviour
// {
//     public GameObject enemyPrefab;
//     public BeatSystem beatSystem;
//     public Transform spawnLocation;
//     public BattleState state;
//     private Unit _enemyUnit;
//     public bool enemyDead;
//     public bool playerDead;
//     public BattlePlayer player;
//     public BattleHUD hud;
//     public float entryTime;
//     public bool beatOnScreen;
//     public GameObject currentEnemy;
//     public Animator gameOverAnim;

//     public GameObject attackButton;
//     public GameObject buffButton;
//     public GameObject debuffButton;
//     public GameObject potionButton;

//     // Start is called before the first frame update
//     void Start()
//     {
//         state = BattleState.START;
//         StartCoroutine(hud.UpdatePhase(state));
//         GameManager.Instance.needsReposition = true;
//         StartCoroutine(SetupBattle());
//     }

//     IEnumerator SetupBattle()
//     {
//         enemyPrefab = GameManager.Instance.encounteredEnemyCombatPrefab;
//         currentEnemy = Instantiate(enemyPrefab, spawnLocation); // Spawn the enemy
//         _enemyUnit = currentEnemy.GetComponent<Unit>(); // Get the unit script
//         // Set up HUDs
//         hud.SetEnemyHUD(_enemyUnit);
//         hud.SetPlayerHUD(player);
//         hud.UpdatePotionCounter();

//         yield return new WaitForSeconds(entryTime); // Wait for the animation to play

//         state = BattleState.ATTACK;
//         StartCoroutine(hud.UpdatePhase(state));
//         PlayerTurn();
//     }

//     void PlayerTurn()
//     {
//         // reactivate buttons
//         attackButton.GetComponent<UnityEngine.UI.Button>().interactable = true;

//         if (player.damageBuffed || (player.currentHealth - player.buffCost) <= 0)
//         {
//             buffButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
//         }
//         else
//         {
//             buffButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
//         }

//         if (player.enemyWeak || (player.currentHealth - player.debuffCost) <= 0)
//         {
//             debuffButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
//         }
//         else
//         {
//             debuffButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
//         }

//         if (GameManager.Instance.potionCount > 0 && player.currentHealth < player.maxHealth)
//         {
//             potionButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
//         }
//         else
//         {
//             potionButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
//         }

//         hud.enemyStatus.SetActive(player.enemyWeak);
//         hud.playerStatus.SetActive(player.damageBuffed);
//     }

//     IEnumerator EnemyTurn()
//     {
//         yield return new WaitForSeconds(1f);
//         beatSystem.GetComponent<Animator>().Play("Slide Down");
//         beatSystem.StartPattern(state);

//         while (beatOnScreen && !playerDead) // wait until beats are finished
//         {
//             yield return null;
//         }
//         if (!playerDead)
//         {
//             yield return new WaitForSeconds(1f);
//         }

//         beatSystem.GetComponent<Animator>().Play("Slide Up");

//         if (player.enemyWeak) // if the enemy was weakened this turn, turn off the debuff at the end of the turn
//         {
//             player.enemyWeak = false;
//         }

//         //check if player died
//         if (playerDead)
//         {
//             //end battle
//             Destroy(beatSystem.currentPattern);
//             state = BattleState.LOST;
//             EndBattle();
//         }
//         else
//         {
//             //player's turn
//             yield return new WaitForSeconds(1f);
//             state = BattleState.ATTACK;
//             StartCoroutine(hud.UpdatePhase(state));
//             yield return new WaitForSeconds(1f);
//             PlayerTurn();
//         }
//     }

//     public void OnAttackButton() //when pressing the attack button
//     {
//         if (state != BattleState.ATTACK)
//         {
//             return;
//         }
//         StartCoroutine(PlayerAttack());
//     }

//     IEnumerator PlayerAttack() //the player attack
//     {
//         hud.attackButtons.GetComponent<Animator>().Play("Slide Down");
//         beatSystem.GetComponent<Animator>().Play("Slide Down");
//         beatSystem.StartPattern(state);
//         player.CastAttack();

//         if (player.currentHealth <= 0) // if casting the attack lowered the player's health to 0, set their health to 1
//         {
//             player.currentHealth = 1;
//         }

//         hud.UpdatePlayerHP(player.currentHealth);

//         while (beatOnScreen && !enemyDead && !playerDead) // wait until beats are finished
//         {
//             yield return null;
//         }

//         if (!enemyDead && !playerDead)
//         {
//             yield return new WaitForSeconds(1f);
//         }

//         beatSystem.GetComponent<Animator>().Play("Slide Up");

//         if (player.damageBuffed) // if the player was buffed this turn, turn off the buff at the end of the turn
//         {
//             player.damageBuffed = false;
//         }

//         //check if enemy died
//         if (enemyDead)
//         {
//             //end battle
//             Destroy(beatSystem.currentPattern);
//             state = BattleState.WON;
//             EndBattle();
//         }
//         //check if player died
//         else if (playerDead)
//         {
//             //end battle
//             Destroy(beatSystem.currentPattern);
//             state = BattleState.LOST;
//             EndBattle();
//         }
//         else
//         {
//             //enemy turn
//             yield return new WaitForSeconds(1f);
//             state = BattleState.DEFENSE;
//             StartCoroutine(hud.UpdatePhase(state));
//             yield return new WaitForSeconds(2f);
//             StartCoroutine(EnemyTurn());
//         }
//     }

//     public void OnBuffButton() // when pressing the buff button
//     {
//         if (state != BattleState.ATTACK)
//         {
//             return;
//         }
//         StartCoroutine(PlayerBuff());
//     }

//     IEnumerator PlayerBuff() //the buff effect
//     {
//         player.CastBuff(); // pay the cost
//         hud.UpdatePlayerHP(player.currentHealth); //update the hud
//         player.playerAnim.Play("Buff", -1, 0f);

//         hud.attackButtons.GetComponent<Animator>().Play("Slide Down");

//         //enemy turn
//         yield return new WaitForSeconds(1f);
//         state = BattleState.DEFENSE;
//         StartCoroutine(hud.UpdatePhase(state));
//         yield return new WaitForSeconds(2f);
//         StartCoroutine(EnemyTurn());
//     }

//     public void OnDebuffButton() // when pressing the debuff button
//     {
//         if (state != BattleState.ATTACK)
//         {
//             return;
//         }
//         StartCoroutine(PlayerDebuff());
//     }

//     IEnumerator PlayerDebuff() //the debuff effect
//     {
//         player.CastDebuff(); // pay the cost
//         hud.UpdatePlayerHP(player.currentHealth); //update the hud
//         player.playerAnim.Play("Debuff", -1, 0f);

//         hud.attackButtons.GetComponent<Animator>().Play("Slide Down");

//         //enemy turn
//         yield return new WaitForSeconds(1f);
//         state = BattleState.DEFENSE;
//         StartCoroutine(hud.UpdatePhase(state));
//         yield return new WaitForSeconds(2f);
//         StartCoroutine(EnemyTurn());
//     }

//     public void OnPotionButton() // when pressing the potion button
//     {
//         if (state != BattleState.ATTACK)
//         {
//             return;
//         }

//         if (GameManager.Instance.potionCount > 0)
//         {
//             StartCoroutine(PlayerPotion());
//         }
//     }

//     IEnumerator PlayerPotion() //the potion effect
//     {
//         GameManager.Instance.potionCount--; // take away a potion
//         player.currentHealth += player.potionStrength; // restore health
//         if (player.currentHealth > player.maxHealth)
//         {
//             player.currentHealth = player.maxHealth;
//         }
//         hud.UpdatePlayerHP(player.currentHealth); // update huds
//         hud.UpdatePotionCounter();

//         hud.attackButtons.GetComponent<Animator>().Play("Slide Down");

//         //enemy turn
//         yield return new WaitForSeconds(1f);
//         state = BattleState.DEFENSE;
//         StartCoroutine(hud.UpdatePhase(state));
//         yield return new WaitForSeconds(2f);
//         StartCoroutine(EnemyTurn());
//     }

//     void EndBattle()
//     {
//         if (state == BattleState.WON)
//         {
//             Debug.Log("You Won!");
//             StartCoroutine(YouWin());
//         }
//         else if (state == BattleState.LOST)
//         {
//             Debug.Log("You Lost!");
//             StartCoroutine(GameOver());
//         }
//     }

//     IEnumerator YouWin()
//     {
//         GameManager.Instance.defeatedEnemies.Add(GameManager.Instance.encounteredEnemy);
//         currentEnemy.GetComponentInChildren<Animator>().Play("Die");
//         currentEnemy.GetComponentInChildren<Animator>().SetBool("isDead", true);
//         yield return new WaitForSeconds(1.5f);
//         player.playerAnim.Play("Buff");
//         yield return new WaitForSeconds(2f);
//         SceneLoader.Instance.LoadOverworldScene(GameManager.Instance.overworldSceneID);
//     }

//     IEnumerator GameOver()
//     {
//         GameManager.Instance.ResetAll();
//         player.playerAnim.Play("Die");
//         player.playerAnim.SetBool("isDead", true);
//         yield return new WaitForSeconds(2f);

//         gameOverAnim.Play("Fade In");
//     }

//     public void OnRetryButton()
//     {
//         SceneLoader.Instance.LoadOverworldScene(GameManager.Instance.overworldSceneID);
//     }
// }
