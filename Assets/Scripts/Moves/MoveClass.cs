using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MoveClass : ScriptableObject
{
    public string moveName = "";
    public List<MoveClass> beats;
    // [Separator(2, 12)]
    public Sprite img;
    public float damage;
    [Range(0, 100)]
    public float wear;
    public bool doesCombat;

    public List<Ability> moveAbilities;

    public float GetElo()
    {
        return NormalizeValue(0, damage, damage * (wear/100));
    }
    public float NormalizeValue(float min, float max, float num)
    {
        float normalizeTop = (num-min);
        float normalizeBottom = (max-min);
        return (normalizeTop / normalizeBottom);
    }
}
