using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum ElineType
{
    Horizontal,
    Vertical,
    DiagonalDownRight,
    DiagonalUpRight,
}

public class TicTacToeManager : MonoBehaviour
{
    [SerializeField] private GameGrid gameGrid;

    [Header("Turn Settings")]
    public bool isPlayerXStart = true;
    public bool isPlayerXTurn = true;

    [Header("Score")]
    [ReadOnly] public int playerXScore = 0;
    [ReadOnly] public int playerOScore = 0;

    [Header("Score")]
    [ReadOnly] public bool playerXWin = false;
    public UnityEvent onWinGame;

    [Header("Reference")]
    [SerializeField] private TextMeshProUGUI player1TestSlot;
    [SerializeField] private Image player1Image;
    [SerializeField] private TextMeshProUGUI player2TestSlot;
    [SerializeField] private Image player2Image;

    [SerializeField] private RectTransform line;
    [SerializeField] private TextMeshProUGUI endGameTextSlot;

    void Start()
    {
        gameGrid.OnCellButtonSelected.AddListener(OnCellPressed);
        StartGame();
    }

    public void StartGame()
    {
        isPlayerXTurn = isPlayerXStart;
        player1Image.enabled = isPlayerXTurn;
        player2Image.enabled = !isPlayerXTurn;

        line.gameObject.SetActive(false);
        gameGrid.ResetGrid();
    }

    public void CheckForLine()
    {
        ECellState currentLookedCellState;
        bool isLineValid;

        //Check for vertical Line
        for (int row = 0; row < GameGrid.GRID_SIZE; row++)
        {
            currentLookedCellState = gameGrid.GetCellAt(0, row).cellState;

            if (currentLookedCellState == ECellState.Empty) continue;

            isLineValid = true;
            for (int col = 0; col < GameGrid.GRID_SIZE; col++)
            {
                isLineValid &= currentLookedCellState == gameGrid.GetCellAt(col, row).cellState;
            }

            if (isLineValid)
            {
                OnLineFind(ElineType.Vertical, new Vector2Int(0, row));
            }
        }

        //Check for Vertical Line
        for (int col = 0; col < GameGrid.GRID_SIZE; col++)
        {
            currentLookedCellState = gameGrid.GetCellAt(col, 0).cellState;

            if (currentLookedCellState == ECellState.Empty) continue;

            isLineValid = true;
            for (int row = 0; row < GameGrid.GRID_SIZE; row++)
            {
                isLineValid &= currentLookedCellState == gameGrid.GetCellAt(col, row).cellState;
            }

            if (isLineValid)
            {
                OnLineFind(ElineType.Horizontal, new Vector2Int(col, 0));
            }
        }

        //Check for diagonal up right
        currentLookedCellState = gameGrid.GetCellAt(0, 0).cellState;
        if (currentLookedCellState != ECellState.Empty)
        {
            isLineValid = true;
            for (int i = 0; i < GameGrid.GRID_SIZE; i++)
            {
                isLineValid &= currentLookedCellState == gameGrid.GetCellAt(i, i).cellState;

            }
            if (isLineValid)
            {
                OnLineFind(ElineType.DiagonalDownRight, new Vector2Int(0, 0));
            }
        }

        //Check for diagonal up right
        currentLookedCellState = gameGrid.GetCellAt(0, GameGrid.GRID_SIZE - 1).cellState;
        if (currentLookedCellState != ECellState.Empty)
        {
            isLineValid = true;
            for (int i = 0; i < GameGrid.GRID_SIZE; i++)
            {
                isLineValid &= currentLookedCellState == gameGrid.GetCellAt(i, (GameGrid.GRID_SIZE - 1) - i).cellState;
                isLineValid &= currentLookedCellState != ECellState.Empty;

            }
            if (isLineValid)
            {
                OnLineFind(ElineType.DiagonalUpRight, new Vector2Int(0, GameGrid.GRID_SIZE - 1));
            }
        }

    }

    private void OnLineFind(ElineType lineType, Vector2Int startPos)
    {
        playerXWin = isPlayerXTurn;
        line.gameObject.SetActive(true);

        CellButton startingCell = gameGrid.GetCellAt(startPos);
        CellButton endingCell;

        switch (lineType)
        {
            case ElineType.Horizontal:
                line.localRotation = Quaternion.Euler(0, 0, 0);
                endingCell = gameGrid.GetCellAt(GameGrid.GRID_SIZE - 1, startPos.y);
                break;
            case ElineType.Vertical:
                line.localRotation = Quaternion.Euler(0, 0, 90);
                endingCell = gameGrid.GetCellAt(startPos.x, GameGrid.GRID_SIZE - 1);
                break;
            case ElineType.DiagonalDownRight:
                line.localRotation = Quaternion.Euler(0, 0, -45);
                endingCell = gameGrid.GetCellAt(GameGrid.GRID_SIZE - 1, GameGrid.GRID_SIZE - 1);
                break;
            case ElineType.DiagonalUpRight:
                line.localRotation = Quaternion.Euler(0, 0, 45);
                endingCell = gameGrid.GetCellAt(GameGrid.GRID_SIZE - 1, 0);
                break;
            default:
                endingCell = gameGrid.GetCellAt(GameGrid.GRID_SIZE - 1, GameGrid.GRID_SIZE - 1);
                break;
        }

        line.position = startingCell.Rect.position;
        line.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Vector3.Distance(startingCell.Rect.anchoredPosition, endingCell.Rect.anchoredPosition));

        Debug.Log($"Line Type {lineType} at {startPos}");

        foreach (var cell in gameGrid.cells)
        {
            cell.Desactivate();
        }

        Invoke("OnGameEnd", 2.5f);
    }

    private void OnCellPressed(CellButton cellButton)
    {
        if (isPlayerXTurn)
        {
            cellButton.FillCellWith(ECellState.Player1);
        }
        else
        {
            cellButton.FillCellWith(ECellState.Player2);
        }

        CheckForLine();

        //Change Player Turn
        isPlayerXTurn = !isPlayerXTurn;
        player1Image.enabled = isPlayerXTurn;
        player2Image.enabled = !isPlayerXTurn;
    }

    private void OnGameEnd()
    {
        if (playerXWin)
        {
            playerXScore++;
            player1TestSlot.text = playerXScore.ToString();

            endGameTextSlot.text = "Player 1 win !";
        }
        else
        {
            playerOScore++;
            player1TestSlot.text = playerXScore.ToString();

            endGameTextSlot.text = "Player 2 win !";
        }

        onWinGame?.Invoke();
    }
}
