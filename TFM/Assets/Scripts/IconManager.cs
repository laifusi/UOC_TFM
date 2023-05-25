using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconManager : MonoBehaviour
{
    [SerializeField] StatSprite[] statSprites;

    public static IconManager Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public Sprite GetStatSprite(StatType stat)
    {
        foreach(StatSprite statSprite in statSprites)
        {
            if(statSprite.stat == stat)
            {
                return statSprite.sprite;
            }
        }

        return null;
    }

    public Sprite GetPowerSprite(Power power)
    {
        foreach (StatSprite statSprite in statSprites)
        {
            if (statSprite.stat == StatType.Power && statSprite.power == power)
            {
                return statSprite.sprite;
            }
        }

        return null;
    }
}

[Serializable]
public struct StatSprite
{
    public StatType stat;
    public Sprite sprite;
    public Power power;
}
