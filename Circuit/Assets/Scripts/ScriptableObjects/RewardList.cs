using UnityEngine;
using System.Collections;

public class RewardList : ScriptableObject
{
    [SerializeField]
    private Reward[] rewards = null;
    public Reward[] Rewards { get { return rewards; } } 
}
