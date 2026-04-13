using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
/*
public class BattleSystem : MonoBehaviour
{

    public string OnWinLoad;

    public EnemyAI EnemyAi;

    public HandManager Hand;

    public DeckManager PlayerDeck;

    public GameObject BattleButton;

    public BattleHud PlayerBattleHud;

    public BattleHud EnemyBattleHUd;

    public GameObject Player;

    public GameObject Enemy;

    public GameObject PlayerDiceText;
    public GameObject EnemyDiceText;

    public Dice PlayerDice;
    public Dice EnemyDice;

    public int PlayerScore;
    public int EnemyScore;

    public bool Battleinprogress = false;

    public GameObject LoseScreen;
    public GameObject WinScreen;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player.GetComponent<Unit>().Stats.StartBattle();
        Enemy.GetComponent<Unit>().Stats.StartBattle();
        StartCoroutine(TurnStart());
    }

    // Update is called once per frame
    void Update()
    {
        if (Hand.BattleReady)
        {
            BattleButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            BattleButton.GetComponent<Button>().interactable = false;
        }
        
        PlayerBattleHud.SetHud(Player.GetComponent<Unit>().Stats);
        EnemyBattleHUd.SetHud(Enemy.GetComponent<Unit>().Stats);

    }

    IEnumerator TurnStart()
    {
        Battleinprogress = true;
        BattleButton.GetComponent<Button>().interactable = false;
        

        yield return new WaitForSeconds(0.5f);

        PlayerDeck.DrawDice(Hand);
        EnemyAi.DrawDice();

        if (Enemy.GetComponent<Unit>().Stats.LoseNextTurn != 2 )
        {
            EnemyAi.ChooseDice();
        }

       

        if (Player.GetComponent<Unit>().Stats.LoseNextTurn == 2)
        {
            Battle();
        }
        else
        {
            Battleinprogress = false;
        }
    }

    public void Battle()
    {
        Battleinprogress = true;
        Hand.BattleReady = false;

        if (Player.GetComponent<Unit>().Stats.LoseNextTurn != 2)
        {
            PlayerDice = Hand.BattleDiceObject.GetComponent<DiceDisplay>().DiceData;
            
        }

        if (Enemy.GetComponent<Unit>().Stats.LoseNextTurn != 2)
        {
            EnemyDice = EnemyAi.BattleDice;
            
        }

        if (Hand.BattleDiceObject != null)
        {
            Hand.DicesInHand.Remove(Hand.BattleDiceObject);
            Destroy(Hand.BattleDiceObject);
        }
        if (EnemyAi.BattleDiceObject != null)
        {
            Destroy(EnemyAi.BattleDiceObject);
        }


        if (Enemy.GetComponent<Unit>().Stats.LoseNextTurn == 2 && Player.GetComponent<Unit>().Stats.LoseNextTurn == 2)
        {
            Enemy.GetComponent<Unit>().Stats.AfterLostTurn();
            Player.GetComponent<Unit>().Stats.AfterLostTurn();
            StartCoroutine(TurnStart());
        }
        else
        {
            StartCoroutine(Battling());
        }


        
    }

    IEnumerator Battling()
    {
        bool Playerclash, Enemyclash = false; ;
        
        if (Player.GetComponent<Unit>().Stats.LoseNextTurn == 2)
        {
            PlayerScore = 0;
            Playerclash = false;
        }
        else
        {
            PlayerScore = PlayerDice.ThrowDice();

            if (PlayerDice.Type == DiceType.SlashDamage || PlayerDice.Type == DiceType.PierceDamage || PlayerDice.Type == DiceType.BluntDamage)
            {
                Playerclash = true;
            }
            else
            {
                Playerclash = false;
            }
        }

        if (Enemy.GetComponent<Unit>().Stats.LoseNextTurn == 2)
        {
            EnemyScore = 0;
            Enemyclash = false;
        }
        else
        {
            EnemyScore = EnemyDice.ThrowDice();

            if (EnemyDice.Type == DiceType.SlashDamage || EnemyDice.Type == DiceType.PierceDamage || EnemyDice.Type == DiceType.BluntDamage)
            {
                Enemyclash = true;
            }
            else
            {
                Enemyclash = false;
            }
        }

        yield return new WaitForSeconds(0.5f);

        PlayerDiceText.GetComponent<TMP_Text>().text = PlayerScore.ToString();
        EnemyDiceText.GetComponent<TMP_Text>().text = EnemyScore.ToString(); 

        PlayerDiceText.SetActive(true);
        EnemyDiceText.SetActive(true);

        Player.GetComponent<Unit>().Attack();
        Enemy.GetComponent<Unit>().Attack();

        yield return new WaitForSeconds(0.5f);

        PlayerDiceText.SetActive(false);
        EnemyDiceText.SetActive(false);



        int Difference = PlayerScore - EnemyScore;

        if (Difference == 0 && (Playerclash || Enemyclash)) 
        {

            StartCoroutine(Battling());
        }
        else
        {
            yield return new WaitForSeconds(0.5f);

            Clash();

            yield return new WaitForSeconds(1);

            Player.GetComponent<StatusEffectManager>().OnTurnEnd();
            Enemy.GetComponent<StatusEffectManager>().OnTurnEnd();


            Player.GetComponent<Unit>().Stats.AfterLostTurn();
            Enemy.GetComponent<Unit>().Stats.AfterLostTurn();

            StartCoroutine(TurnStart());
        }
    }

    public void Clash()
    {

        if (Player.GetComponent<Unit>().Stats.LoseNextTurn != 2 && PlayerDice.Type == DiceType.Healing)
        {
            Player.GetComponent<Unit>().Stats.Heal((int)(PlayerScore), 0);
        }

        if (Enemy.GetComponent<Unit>().Stats.LoseNextTurn != 2 && EnemyDice.Type == DiceType.Healing)
        {
            Enemy.GetComponent<Unit>().Stats.Heal((int)(EnemyScore), 0);
        }
        

        if (PlayerScore > EnemyScore)
        {
            

            Player.GetComponent<Unit>().Attack();
            Enemy.GetComponent<Unit>().Hit();

            Enemy.GetComponent<StatusEffectManager>().OnHit();

            int defence;

            if (Enemy.GetComponent<Unit>().Stats.LoseNextTurn == 2 || DiceType.Defence != EnemyDice.Type)
            {
                defence = 0;
            }
            else
            {
                defence = (int)(EnemyScore);
            }

            int damage = (int)(PlayerScore) - defence;

            if (PlayerDice.Type == DiceType.SlashDamage)
            {
                if (Enemy.GetComponent<Unit>().Stats.TakeDamageSlach(damage))
                {
                    StartCoroutine(Win());
                    return;
                }
                Enemy.GetComponent<Unit>().Stats.TakeDamageSp(damage);
            }

            if (PlayerDice.Type == DiceType.PierceDamage)
            {
                if (Enemy.GetComponent<Unit>().Stats.TakeDamagePierce(damage))
                {
                    StartCoroutine(Win());
                    return;
                }
                Enemy.GetComponent<Unit>().Stats.TakeDamageSp(damage);
            }

            if (PlayerDice.Type == DiceType.BluntDamage)
            {
                if (Enemy.GetComponent<Unit>().Stats.TakeDamageBlunt(damage))
                {
                    StartCoroutine(Win());
                    return;
                }
                Enemy.GetComponent<Unit>().Stats.TakeDamageSp(damage);
            }


                
            

            

            if (PlayerDice.ApliedStatusEffect != null)
            {
                
               
                Enemy.GetComponent<StatusEffectManager>().AddStatusEffect(PlayerDice.ApliedStatusEffect, PlayerDice.CountAplied);
            }


        }
        else if (PlayerScore < EnemyScore)
        {
            

            Enemy.GetComponent<Unit>().Attack();
            Player.GetComponent<Unit>().Hit();

            Player.GetComponent<StatusEffectManager>().OnHit();


            int defence;

            if (Player.GetComponent<Unit>().Stats.LoseNextTurn == 2 || DiceType.Defence != PlayerDice.Type)
            {
                defence = 0;
            }
            else
            {
                defence = (int)(PlayerScore);
            }

            int damage = (int)(EnemyScore) - defence;

            if (EnemyDice.Type == DiceType.SlashDamage)
            {
                if (Player.GetComponent<Unit>().Stats.TakeDamageSlach(damage))
                {
                    StartCoroutine(Lose());
                    return;
                }
                Player.GetComponent<Unit>().Stats.TakeDamageSp(damage);
            }

            if (EnemyDice.Type == DiceType.PierceDamage)
            {
                if (Player.GetComponent<Unit>().Stats.TakeDamagePierce(damage))
                {
                    StartCoroutine(Lose());
                    return;
                }
                Player.GetComponent<Unit>().Stats.TakeDamageSp(damage);
            }

            if (EnemyDice.Type == DiceType.BluntDamage)
            {
                if (Player.GetComponent<Unit>().Stats.TakeDamageBlunt(damage))
                {
                    StartCoroutine(Lose());
                    return;
                }
                Player.GetComponent<Unit>().Stats.TakeDamageSp(damage);
            }

            

            if (EnemyDice.ApliedStatusEffect != null)
            {
                Player.GetComponent<StatusEffectManager>().AddStatusEffect(EnemyDice.ApliedStatusEffect, EnemyDice.CountAplied);
            }


        }

        
        

        //StartCoroutine(TurnStart());
    }

    IEnumerator Lose()
    {
        LoseScreen.SetActive(true);

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator Win()
    {
        WinScreen.SetActive(true);

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(OnWinLoad);
    }

}
*/