using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using System;
using NaughtyAttributes;

public class GameGrid : MonoBehaviour
{
    public const int GRID_SIZE = 3;

    public GridLayoutGroup gridLayoutGroup;
    public RectTransform gridRecTransform;

    [Header("Cell Settings")]
    public CellButton cellPrefab;
    [Range(0, 1)] public float spacingRatio = 0.1f; // 10% of cell size
    public CellButton[] cells = new CellButton[GRID_SIZE * GRID_SIZE];

    public UnityEvent<CellButton> OnCellButtonSelected;

    [Button]
    public void ResetGrid()
    {
        transform.DeleteChildren();
        cells = new CellButton[GRID_SIZE * GRID_SIZE];
        for (int col = 0; col < GRID_SIZE; col++)
        {
            for (int row = 0; row < GRID_SIZE; row++)
            {
                CellButton cell = Instantiate(cellPrefab, transform);
                cell.gameObject.name = $"Cell [{row}-{col}]";
                cell.Init(new Vector2Int(row, col));
                cell.OnCellClicked.AddListener(OnButtonPress);
                cells[row + (col * GRID_SIZE)] = cell;
            }
        }

        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = GRID_SIZE;

        gridRecTransform = gridLayoutGroup.GetComponent<RectTransform>();
        ComputeGridSize();
    }
    public CellButton GetCellAt(int x, int y) => GetCellAt(new Vector2Int(x, y));
    public CellButton GetCellAt(Vector2Int pos)
    {
        if (pos.x < 0 || pos.y < 0 || pos.x >= GRID_SIZE || pos.y >= GRID_SIZE) return null;
        return cells[pos.x + (GRID_SIZE * pos.y)];
    }

    private void ComputeGridSize()
    {
        float gridwith = gridRecTransform.rect.width;
        float totalSpacing = gridwith / (GRID_SIZE - 1) * spacingRatio;
        float totalCellWidth = gridwith - totalSpacing;

        float cellSize = totalCellWidth / GRID_SIZE;
        float spacing = totalSpacing / (GRID_SIZE - 1);

        gridLayoutGroup.cellSize = Vector2.one * cellSize;
        gridLayoutGroup.spacing = Vector2.one * spacing;
    }
    private void OnButtonPress(CellButton cellButton)
    {
        OnCellButtonSelected?.Invoke(cellButton);
    }
}
