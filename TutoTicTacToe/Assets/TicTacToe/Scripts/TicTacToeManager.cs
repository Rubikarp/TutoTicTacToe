using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum ElineType
{
    Horizontal,
    Vertical,
    Diagonal
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
        for (int col = 0; col < GameGrid.GRID_SIZE; col++)
        {
            currentLookedCellState = gameGrid.GetCellAt(0, col).cellState;

            if (currentLookedCellState == ECellState.Empty) continue;

            isLineValid = true;
            for (int row = 0; row < GameGrid.GRID_SIZE; row++)
            {
                isLineValid &= currentLookedCellState == gameGrid.GetCellAt(row, col).cellState;
            }

            if (isLineValid)
            {
                OnLineFind(ElineType.Vertical, col);
            }
        }

        //Check for Vertical Line
        for (int row = 0; row < GameGrid.GRID_SIZE; row++)
        {
            currentLookedCellState = gameGrid.GetCellAt(0, row).cellState;

            if (currentLookedCellState == ECellState.Empty) continue;

            isLineValid = true;
            for (int col = 0; col < GameGrid.GRID_SIZE; col++)
            {
                isLineValid &= currentLookedCellState == gameGrid.GetCellAt(row, col).cellState;
            }

            if (isLineValid)
            {
                OnLineFind(ElineType.Horizontal, row);
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
                OnLineFind(ElineType.Diagonal, 0, false);
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
                OnLineFind(ElineType.Diagonal, 0, true);
            }
        }

    }

    private void OnLineFind(ElineType lineType, int index, bool diagonalUpRight = false)
    {
        playerXWin = isPlayerXTurn;
        line.gameObject.SetActive(true);

        CellButton startingCell;
        CellButton endingCell;

        switch (lineType)
        {
            case ElineType.Horizontal:
                line.localRotation = Quaternion.Euler(0, 0, 0);
                startingCell = gameGrid.GetCellAt(0, index);
                endingCell = gameGrid.GetCellAt(GameGrid.GRID_SIZE - 1, index);
                break;
            case ElineType.Vertical:
                line.localRotation = Quaternion.Euler(0, 0, 90);
                startingCell = gameGrid.GetCellAt(index, 0);
                endingCell = gameGrid.GetCellAt(index, GameGrid.GRID_SIZE - 1);
                break;
            case ElineType.Diagonal:
                if (diagonalUpRight)
                {
                    line.localRotation = Quaternion.Euler(0, 0, 45);
                    startingCell = gameGrid.GetCellAt(0, GameGrid.GRID_SIZE - 1);
                    endingCell = gameGrid.GetCellAt(GameGrid.GRID_SIZE - 1, 0);
                }
                else
                {
                    line.localRotation = Quaternion.Euler(0, 0, -45);
                    startingCell = gameGrid.GetCellAt(0, 0);
                    endingCell = gameGrid.GetCellAt(GameGrid.GRID_SIZE - 1, GameGrid.GRID_SIZE - 1);
                }
                break;
            default:
                startingCell = gameGrid.GetCellAt(0, 0);
                endingCell = gameGrid.GetCellAt(GameGrid.GRID_SIZE - 1, GameGrid.GRID_SIZE - 1);
                break;
        }

        line.position = startingCell.Rect.position;
        line.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Vector3.Distance(startingCell.Rect.anchoredPosition, endingCell.Rect.anchoredPosition));

        Debug.Log($"Line Type {lineType} at {index}");
        Invoke("OnGameEnd", 2.5f);
    }

    private void OnCellPressed(CellButton cellButton)
    {
        if (isPlayerXTurn)
        {
            cellButton.FillCellWith(ECellState.X);
        }
        else
        {
            cellButton.FillCellWith(ECellState.O);
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
