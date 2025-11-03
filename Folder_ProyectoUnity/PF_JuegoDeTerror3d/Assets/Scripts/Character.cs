using System;
using UnityEngine;

public enum CharacterType
{
    None,
    Normal,
    Fast,
    Tanky
}
[Serializable]
public class Statistics
{
    public int Attack;
    public float Speed;
    public int Life;
}

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Objects/Character")]
public class Character : ScriptableObject
{
    public CharacterType CharacterType;
    public string Name;
    public Sprite Icon;
    public GameObject prefab;
    public Statistics statistics;

}
