using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : Entity
{
    private const float STEP_DELAY = 0.5f;
    
    public override void GetStep(params CellType[] field)
    {
        StartCoroutine(ChooseStep(field));
    }

    private IEnumerator ChooseStep(CellType[] field)
    {
        yield return new WaitForSeconds(STEP_DELAY);
        
        var freeCells = new List<int>();

        for (var i = 0; i < field.Length; i++)
        {
            if (field[i] == CellType.None)
            {
                freeCells.Add(i);
            }
        }
        
        OnStep?.Invoke(freeCells[Random.Range(0,freeCells.Count)], playWith);
    }
}
