using UnityEngine;

namespace Ar
{
    public abstract class Entity : MonoBehaviour
    {
        [SerializeField] protected CellType playWith = CellType.None;
        public System.Action<int, CellType> OnStep;

        public abstract void GetStep(params CellType[] field);
    }
}