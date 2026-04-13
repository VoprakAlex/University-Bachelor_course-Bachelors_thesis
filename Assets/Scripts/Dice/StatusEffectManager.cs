using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;
using System.Linq;
/*
public class StatusEffectManager : MonoBehaviour
{

    [SerializeField] public Dictionary<StatusEffect,int> ApliedStatusEffects = new Dictionary<StatusEffect,int>();

    public List<GameObject> StatusVisuals = new List<GameObject>();

    public GameObject StatusPrefab;

    public GameObject StatusEffectPosition;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //this.GetComponent<Unit>().Stats;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHit()
    {
        Character target = this.GetComponent<Unit>().Stats;


        foreach (var effect in ApliedStatusEffects.Keys.ToList()) 
        {
            if (effect.InfllictOnHit)
            {
                effect.ApplyEffect(target, ApliedStatusEffects[effect]);
                if (ApliedStatusEffects[effect] > 0)
                {
                    ApliedStatusEffects[effect] = ApliedStatusEffects[effect] - 1;
                }
            }
        }
        UpdateVisuals();

    }

    public void OnTurnEnd()
    {
        Character target = this.GetComponent<Unit>().Stats;

        foreach (var effect in ApliedStatusEffects.Keys.ToList())
        {
            if (effect.InfllictOnTurnEnd)
            {
                effect.ApplyEffect(target, ApliedStatusEffects[effect]);
                if (ApliedStatusEffects[effect] > 0)
                {
                    ApliedStatusEffects[effect] = ApliedStatusEffects[effect] - 1;
                }
               
            }
        }
        UpdateVisuals();
    }

    public void AddStatusEffect(StatusEffect statusEffect,int Count)
    {
        if (ApliedStatusEffects.ContainsKey(statusEffect))
        {
            ApliedStatusEffects[statusEffect] += Count;
        }
        else
        {
            ApliedStatusEffects.Add(statusEffect, Count);
        }

        UpdateVisuals();

        //Instantiate(DicePrefab, HandTransform.position, Quaternion.identity, HandTransform);
    }

    public void UpdateVisuals()
    {
        foreach (var visuals in StatusVisuals)
        {
            Destroy(visuals);
        }
        StatusVisuals.Clear();

        foreach (var effect in ApliedStatusEffects.Keys.ToList())
        {
            if (effect.InfllictOnHit)
            {
                Debug.Log(effect.Name);
                Debug.Log(ApliedStatusEffects[effect]);
            }
        }


        foreach (var effect in ApliedStatusEffects)
        {
            if (effect.Value > 0)
            {
                GameObject NewVisual = Instantiate(StatusPrefab, StatusEffectPosition.transform.position, Quaternion.identity, StatusEffectPosition.transform);
                NewVisual.GetComponent<StatusDisplay>().StatusEffectData = effect.Key;
                NewVisual.GetComponent<StatusDisplay>().ApliedCount = effect.Value;
                StatusVisuals.Add(NewVisual);

                
            }
        }
    }
}
*/