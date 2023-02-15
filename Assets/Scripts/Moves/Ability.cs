using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementyType
{
    None, 
    Earth,
    Fire,
    Wind,
    Water,
    Heart
}

[System.Serializable]
public class Ability
{
    public string abilityName = "";
    public float damage = 0;
    public ElementyType element = ElementyType.None;
}
