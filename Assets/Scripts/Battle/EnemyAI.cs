using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
/*
public class EnemyAI : MonoBehaviour
{

    public List<Dice> AllDice = new List<Dice>();
    
    public List<Dice> DicesInHand = new List<Dice>();

    public int Test = 1;

    public int TestCount = 0;

    public GameObject DicePrefab;
    
    public Dice BattleDice;

    public GameObject BattleDiceObject;

    public Transform BattleTransform;

    public GameObject Player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            DrawDice();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseDice()
    {
     
            
        Dictionary<int, Dice> DiceScore = new Dictionary<int, Dice>();
        List<int> Score = new List<int>();

        foreach (var dice in DicesInHand)
        {
            if (DiceScore.ContainsKey(EvaluateDice(dice)))
            {
                DiceScore[EvaluateDice(dice)] = dice;
            }
            else
            {
                DiceScore.Add(EvaluateDice(dice), dice);
                Score.Add(EvaluateDice(dice));
            }
        }

        BattleDice = DiceScore[Score.Max()];

        DicesInHand.Remove(DiceScore[Score.Max()]);

        DicePrefab.GetComponent<DiceDisplay>().DiceData = BattleDice;
        BattleDiceObject = Instantiate(DicePrefab, BattleTransform.position, Quaternion.identity, BattleTransform);

        return;

    }

    public void DrawDice()
    {
        int x = Random.Range(0, AllDice.Count);
        Dice NextDice = AllDice[x];
        DicesInHand.Add(NextDice);
    }

    public int EvaluateDice(Dice dice)
    {
        int MyHP = this.GetComponent<Unit>().Stats.CurrentHP;
        int MyMaxHP = this.GetComponent<Unit>().Stats.MaxHP;

        int MyHealthProblem = (int)(( (1 - MyHP / MyMaxHP) * 100));

        int PlayerHP = Player.GetComponent<Unit>().Stats.CurrentHP;
        int PlayerMaxHP = Player.GetComponent<Unit>().Stats.MaxHP;

        int PlayerHealthProblem = (int) (((1 - PlayerHP / PlayerMaxHP) * 100) + 1);

        int score = 0;

        
        
        if (dice.Type == DiceType.PierceDamage || dice.Type == DiceType.SlashDamage || dice.Type == DiceType.BluntDamage)
        {
            if (dice.ApliedStatusEffect != null )
            {
                int ApliedCount;
                if (Player.GetComponent<StatusEffectManager>().ApliedStatusEffects.ContainsKey(dice.ApliedStatusEffect) )
                {
                    ApliedCount = Player.GetComponent<StatusEffectManager>().ApliedStatusEffects[dice.ApliedStatusEffect];
                }
                else
                {
                    ApliedCount = 0;
                }

                score += (int)( (ApliedCount + dice.CountAplied)*10 + PlayerHealthProblem * dice.MaxValue * 1.2) ;
            }
            else
            {
                score += (int)(PlayerHealthProblem * dice.MaxValue * 1.2);
            }
            
        }

        if ( dice.Type == DiceType.Defence || dice.Type == DiceType.Evasion)
        {
            score += (int)( dice.MinValue *  MyHealthProblem);
        }
        return score;
    }
}
*/