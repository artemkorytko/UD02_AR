using System;
using System.Linq;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] private Cell[] cells;

    public event Action<CellType> OnCompleted;
    
    private void Awake()
    {
        cells = GetComponentsInChildren<Cell>();
    }

    public void Initialize()
    {
        for (var i = 0; i < cells.Length; i++)
        {
            cells[i].Initialize(i);
        }
    }

    public void SetState(bool state)
    {
        for (var i = 0; i < cells.Length; i++)
        {
            cells[i].IsActive = state;
        }
    }
    
    public void Refresh()
    {
        for (var i = 0; i < cells.Length; i++)
        {
            cells[i].Refresh();
        }
    }

    public void SetSell(int index, CellType type)
    {
        if (index < 0 || index >= cells.Length) return;

        switch (type)
        {
            case CellType.Cross:
                cells[index].SelectCross();
                break;
            case CellType.Zero:
                cells[index].SelectZero();
                break;
        }

        BoardCheck();
    }

    private void BoardCheck()
    {
        if (CheckLines(out CellType winType))
        {
            OnCompleted?.Invoke(winType);
            return;
        }

        if (!CheckCount())
        {
            OnCompleted?.Invoke(CellType.None);
        }
    }
    
    private bool CheckLines(out CellType winType)
    {
        winType = CellType.None;

        CellType cellType = CellType.None;
        
        // Check rows
        for (int i = 0; i < cells.Length; i+=3)
        {
            cellType = cells[i].Type;
            bool result = true;
            
            for (int j = i + 1; j < i + 3; j++)
            {
                if (cellType != cells[j].Type)
                {
                    result = false;
                    break;
                }
            }

            if (!result && cellType != CellType.None)
            {
                winType = cellType;
                return result;
            }
        }
        
        // Check columns
        for (int i = 0; i < 3; i++)
        {
            cellType = cells[i].Type;
            bool result = true;
            
            for (int j = i + 3; j < cells.Length; j+=3)
            {
                if (cellType != cells[j].Type)
                {
                    result = false;
                    break;
                }
            }

            if (result && cellType != CellType.None)
            {
                winType = cellType;
                return result;
            }
        }
        
        // Check diagonal 1
        {
            cellType = cells[0].Type;
            bool result = true;
            
            for (int j = 4; j < cells.Length; j+=4)
            {
                if (cellType != cells[j].Type)
                {
                    result = false;
                    break;
                }
            }

            if (result && cellType != CellType.None)
            {
                winType = cellType;
                return result;
            }
        }
        
        // Check diagonal 2
        {
            cellType = cells[2].Type;
            bool result = true;
            
            for (int j = 4; j < cells.Length - 1; j+=2)
            {
                if (cellType != cells[j].Type)
                {
                    result = false;
                    break;
                }
            }

            if (result && cellType != CellType.None)
            {
                winType = cellType;
                return result;
            }
        }

        return false;
    }
    
    private bool CheckCount()
    {
        return cells.Count(x => x.Type == CellType.None) != 0;
    }

    public CellType[] FieldToArray()
    {
        CellType[] intField = new CellType[9];

        for (int i = 0; i < cells.Length; i++)
        {
            intField[i] = cells[i].Type;
        }

        return intField;
    }
}
