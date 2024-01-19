using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    TILEVALUE whoseTurn = TILEVALUE.X;
    private GridManager grid;

    public void SetGridManager(GridManager _gm)
    {
        grid = _gm;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

            if (rayHit.collider.gameObject.TryGetComponent<InteractiveTile>(out InteractiveTile tile))
            {
                (Coordinate, TILEVALUE) item = tile.TileWasClicked(whoseTurn);
                if (item.Item2 != TILEVALUE.NONE)
                {
                    grid.UpdateTile(item.Item1, item.Item2);
                    whoseTurn = TurnOver();
                }
            }
        }
    }

    private TILEVALUE TurnOver()
    {
        return whoseTurn == TILEVALUE.X ? TILEVALUE.O : TILEVALUE.X;
    }

}
