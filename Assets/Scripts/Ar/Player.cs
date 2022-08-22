using System.Collections;
using UnityEngine;

namespace Ar
{
    public class Player : Entity
    {
        [SerializeField] private Camera arCamera;

        public override void GetStep(params CellType[] field)
        {
            StartCoroutine(ChooseStep());
        }

        private IEnumerator ChooseStep()
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 touchPosition = Input.mousePosition;
                    Ray ray = arCamera.ScreenPointToRay(touchPosition);

                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        Cell cell = hit.collider.gameObject.GetComponent<Cell>();

                        if (cell && cell.IsActive)
                        {
                            OnStep?.Invoke(cell.Index, playWith);
                            break;
                        }
                    }
                }

                yield return null;
            }
        }
    }
}