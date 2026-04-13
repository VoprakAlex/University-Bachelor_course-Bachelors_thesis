using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
/*
public class DeckManager : MonoBehaviour
{

    public List<Dice> AllDice = new List<Dice>();

    public void DrawDice(HandManager manager)
    {
        if (AllDice.Count == 0 || manager.DicesInHand.Count > 7)
        {
            return;
        }

        int x = Random.Range(0, AllDice.Count);
        Dice NextDice = AllDice[x];
        manager.AddCardToHand(NextDice);

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HandManager Hand  = FindFirstObjectByType<HandManager>();
        for (int i = 0; i < 3; i++)
        {
            DrawDice(Hand);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/