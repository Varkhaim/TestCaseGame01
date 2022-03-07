using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KillMission : Mission
{
    public List<Monster> MonstersToKill;
    
    private int amountToKill;

    private void Awake()
    {
        amountToKill = MonstersToKill.Count;
        MonstersToKill.ForEach(x => x.OnDeath.AddListener(AddKilled));
    }

    private void AddKilled()
    {
        amountToKill -= 1;
        if (amountToKill <= 0)
        {
            OnCompletion.Invoke();
        }
    }
}
