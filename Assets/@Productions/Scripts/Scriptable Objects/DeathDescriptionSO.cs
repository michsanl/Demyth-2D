using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Death Description SO/New Death Description")]
public class DeathDescriptionSO : ScriptableObject
{
    [Header("EN")]
    [Header("Level 2")]
    public string ENMariaDialogueA;
    public string ENMariaDialogueB;
    [Header("Level 4")]
    public string ENPetraFight;
    [Header("Level 7")]
    public string ENSriDialogueA;
    public string ENSriDialogueB;
    public string ENSriFight;
    [Space]
    [Header("ID")]
    [Header("Level 2")]
    public string IDMariaDialogueA;
    public string IDMariaDialogueB;
    [Header("Level 4")]
    public string IDPetraFight;
    [Header("Level 7")]
    public string IDSriDialogueA;
    public string IDSriDialogueB;
    public string IDSriFight;
}
