using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectMission : Mission
{
    public List<KeyItem> KeysToCollect;

    private int amountToCollect;

    private void Awake()
    {
        amountToCollect = KeysToCollect.Count;
        KeysToCollect.ForEach(x => x.OnCollected.AddListener(AddKilled));
    }

    private void AddKilled()
    {
        amountToCollect -= 1;
        if (amountToCollect <= 0)
        {
            OnCompletion.Invoke();
        }
    }
}
