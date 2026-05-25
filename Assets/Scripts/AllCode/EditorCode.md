using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StatsComponent))]
public class StatsComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StatsComponent stats = (StatsComponent)target;

        GUILayout.Space(10);

        GUI.enabled = Application.isPlaying;

        if (GUILayout.Button("Random Speed"))
        {
            stats.RandomSpeed();
        }

        if (GUILayout.Button("Take 10 Raw HP Damage"))
        {
            stats.TakeHPDamage(10);
        }

        if (GUILayout.Button("Take 10 Raw SP Damage"))
        {
            stats.TakeSPDamage(10);
        }

        if (GUILayout.Button("Calculate 10 Slashing Physical Damage"))
        {
            stats.CalculateDamage(
                10,
                DamageType.Slashing,
                DamageAffinity.Physical
            );
        }

        if (GUILayout.Button("Calculate 10 Fire Damage"))
        {
            stats.CalculateDamage(
                10,
                DamageType.Slashing,
                DamageAffinity.Fire
            );
        }

        if (GUILayout.Button("Calculate 10 Mind Damage"))
        {
            stats.CalculateDamage(
                10,
                DamageType.Bludgening,
                DamageAffinity.Mind
            );
        }

        GUI.enabled = true;
    }
}

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HealthComponent))]
public class HealthComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HealthComponent health = (HealthComponent)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Set To Max"))
        {
            health.SetToMax();
        }

        if (GUILayout.Button("Take 10 Damage"))
        {
            health.DecreaseHP(10);
        }

        if (GUILayout.Button("Heal 10"))
        {
            health.IncreaseHP(10);
        }

        if (GUILayout.Button("Death"))
        {
            health.Death();
        }

        if (GUILayout.Button("Revive 10 HP"))
        {
            health.Revive(10);
        }
    }
}

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SanityComponent))]
public class SanityComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SanityComponent sanity = (SanityComponent)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Set To Max"))
        {
            sanity.SetToMax();
        }

        if (GUILayout.Button("Take 10 SP Damage"))
        {
            sanity.DecreaseSanity(10);
        }

        if (GUILayout.Button("Restore 10 SP"))
        {
            sanity.IncreaseSanity(10);
        }

        if (GUILayout.Button("Madness"))
        {
            sanity.Madness();
        }

        if (GUILayout.Button("Recover 10 SP"))
        {
            sanity.Recover(10);
        }
    }
}

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShieldComponent))]
public class ShieldComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ShieldComponent shield = (ShieldComponent)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Set To Starting"))
        {
            shield.SetToStarting();
        }

        if (GUILayout.Button("Add 10 Shield"))
        {
            shield.IncreaseShield(10);
        }

        if (GUILayout.Button("Remove 10 Shield"))
        {
            shield.DecreaseShield(10);
        }
    }
}

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StaggerComponent))]
public class StaggerComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StaggerComponent stagger = (StaggerComponent)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Stagger"))
        {
            stagger.Stagger();
        }

        if (GUILayout.Button("Unstagger"))
        {
            stagger.Unstagger();
        }

        if (GUILayout.Button("Increase Threshold"))
        {
            stagger.IncreaseStaggerThreshold(10);
        }

        if (GUILayout.Button("Decrease Threshold"))
        {
            stagger.DecreaseStaggerThreshold(10);
        }
    }
}

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ResistanceComponent))]
public class ResistanceComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ResistanceComponent resistance = (ResistanceComponent)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Set Standard Resistances"))
        {
            resistance.SetResistanceToStandard();
        }

        if (GUILayout.Button("Set All Type Resistances To Fatal"))
        {
            resistance.SetDamageTypeResistancesToMax();
        }

        if (GUILayout.Button("Set All Affinity Resistances To Fatal"))
        {
            resistance.SetDamageAffinityResistancesToMax();
        }

        if (GUILayout.Button("Increase All Type Resistances"))
        {
            resistance.IncreaseAllDamageTypeResistances();
        }

        if (GUILayout.Button("Decrease All Type Resistances"))
        {
            resistance.DecreaseAllDamageTypeResistances();
        }

        if (GUILayout.Button("Increase All Affinity Resistances"))
        {
            resistance.IncreaseAllDamageAffinityResistances();
        }

        if (GUILayout.Button("Decrease All Affinity Resistances"))
        {
            resistance.DecreaseAllDamageAffinityResistances();
        }
    }
}

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpeedComponent))]
public class SpeedComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SpeedComponent speed = (SpeedComponent)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Random Speed"))
        {
            speed.SetRandomSpeed();
        }

        if (GUILayout.Button("Increase Speed"))
        {
            speed.IncreaseSpeed(1);
        }

        if (GUILayout.Button("Decrease Speed"))
        {
            speed.DecreaseSpeed(1);
        }
    }
}

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HandComponent))]
public class HandComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HandComponent hand = (HandComponent)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Rebuild Deck"))
        {
            hand.RebuildDeck();
        }

        if (GUILayout.Button("Draw Skill"))
        {
            hand.DrawSkill();
        }
    }
}

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillComponent))]
public class SkillComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SkillComponent skill = (SkillComponent)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Set Clashing"))
        {
            skill.SetClashing();
        }

        if (GUILayout.Button("Set Not Clashing"))
        {
            skill.SetNotClashing();
        }

        if (GUILayout.Button("Clear Current Skill"))
        {
            skill.ClearCurrentSkill();
        }

        if (GUILayout.Button("Add Current Skill To Used"))
        {
            skill.AddCurrentSkillToUsed();
        }

        if (GUILayout.Button("Clear Used Skills"))
        {
            skill.ClearUsedSkills();
        }
    }
}

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TargetComponent))]
public class TargetComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TargetComponent targetComponent = (TargetComponent)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Clear Main Target"))
        {
            targetComponent.ClearMainTarget();
        }

        if (GUILayout.Button("Clear Sub Targets"))
        {
            targetComponent.ClearSubTargets();
        }

        if (GUILayout.Button("Clear All Targets"))
        {
            targetComponent.ClearTargets();
        }

        if (GUILayout.Button("Remove First Sub Target"))
        {
            targetComponent.RemoveFirstSubTarget();
        }
    }
}

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerController controller = (PlayerController)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Draw Hand"))
        {
            controller.DrawAllCards();
        }

        if (GUILayout.Button("Clear Hand"))
        {
            controller.ClearAllCards();
        }

        if (GUILayout.Button("Refresh Hand"))
        {
            controller.RefreshHandView();
        }

        if (GUILayout.Button("Fill Components"))
        {
            controller.FillComponents();
        }
    }
}

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CombatManager))]
public class CombatManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CombatManager combat = (CombatManager)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Clear Attacker"))
        {
            combat.ClearAttacker();
        }

        if (GUILayout.Button("Execute Combat"))
        {
            combat.ExecuteCombat();
        }
    }
}