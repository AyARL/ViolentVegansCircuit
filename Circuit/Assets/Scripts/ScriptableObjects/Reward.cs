using UnityEngine;
using System.Collections;

public class Reward : ScriptableObject
{
    [SerializeField]
    private int starsRequired = 0;
    public int StarsRequired { get { return starsRequired; } }

    [SerializeField]
    private Sprite badgeImage = null;
    public Sprite BadgeImage { get { return badgeImage; } }
}
