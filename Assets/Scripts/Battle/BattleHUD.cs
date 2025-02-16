// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class BattleHUD : MonoBehaviour
// {
//     public TMP_Text enemyNameText;
//     public TMP_Text phaseText;
//     public HealthBar enemyHPBar;
//     public HealthBar playerHPBar;
//     public GameObject attackButtons;
//     public TMP_Text potionText;

//     public GameObject enemyStatus;
//     public GameObject playerStatus;

//     public void SetEnemyHUD(Unit _unit)
//     {
//         enemyNameText.text = _unit.unitName;
//         enemyHPBar.SetMaxHealth(_unit.maxHealth);
//     }

//     public void SetPlayerHUD(BattlePlayer _player)
//     {
//         playerHPBar.SetMaxHealth(_player.maxHealth);
//     }

//     public void UpdateEnemyHP(int _health)
//     {
//         enemyHPBar.SetHealth(_health);
//     }

//     public void UpdatePlayerHP(int _health)
//     {
//         playerHPBar.SetHealth(_health);
//     }

//     public void UpdatePotionCounter()
//     {
//         potionText.text = GameManager.Instance.potionCount.ToString();
//     }

//     public IEnumerator UpdatePhase(BattleState _state)
//     {
//         Animator _optionAnim;
//         Animator _phaseAnim;

//         _optionAnim = attackButtons.GetComponent<Animator>();
//         _phaseAnim = phaseText.GetComponentInParent<Animator>();

//         if (_state == BattleState.ATTACK)
//         {
//             phaseText.text = "ATTACK PHASE";
//             _phaseAnim.Play("Phase Change");
//             yield return new WaitForSeconds(2f);
//             _optionAnim.Play("Slide Up");
//         }
//         else if (_state == BattleState.DEFENSE)
//         {
//             phaseText.text = "DEFENSE PHASE";
//             _phaseAnim.Play("Phase Change");
//         }
//         else
//         {
//             phaseText.text = "WAITING...";
//         }
//     }
// }
