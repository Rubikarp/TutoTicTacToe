using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

public enum ECellState
{
    Empty,
    Player1,
    Player2
}

[RequireComponent (typeof(Button))]
public class CellButton : MonoBehaviour
{
    public RectTransform Rect => (RectTransform)transform;

    [Header("Components")]
    [SerializeField] private Image stateImage;
    [SerializeField] private Button button;

    [Header("Visual")]
    public Sprite spriteX;
    public Color colorX = Color.blue;
    [Space]
    public Sprite spriteO;
    public Color colorO = Color.red;

    [Header("Cell Info")]
    public Vector2Int GridPosition;
    public ECellState cellState = ECellState.Empty;
    public UnityEvent<CellButton> OnCellClicked;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }
    private void OnValidate()
    {
        RefreshVisual();
    }

    public void Init(Vector2Int posInGrid)
    {
        GridPosition = posInGrid;
        button.interactable = true;
        SetState(ECellState.Empty);
    }

    public void Desactivate()
    {
        button.interactable = false;
    }
    public void FillCellWith(ECellState state)
    {
        SetState(state);
        if(state != ECellState.Empty)
        {
            button.interactable = false;
        }
    }
    private void SetState(ECellState state)
    {
        cellState = state;
        RefreshVisual();
    }
    private void RefreshVisual()
    {
        stateImage.color = Color.white;
        switch (cellState)
        {
            case ECellState.Player1:
                stateImage.sprite = spriteX;
                stateImage.color = colorX;
                break;
            case ECellState.Player2:
                stateImage.sprite = spriteO;
                stateImage.color = colorO;
                break;
            case ECellState.Empty:
            default:
                stateImage.sprite = null;
                stateImage.color = Color.clear;
                break;
        }
    }

    private void OnClick()
    {
        OnCellClicked?.Invoke(this);
    }
}
