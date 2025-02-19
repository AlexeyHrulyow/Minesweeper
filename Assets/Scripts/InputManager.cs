using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private MineField currentMineField;
    private bool isActive = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!isActive) return;

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            InteractWithCell();
        }
    }

    private void InteractWithCell()
    {
        RaycastHit2D hit = Utils.GetRaycastHit2DFromMousePosition();
        if (!hit) return;

        Vector3Int cellCoords = new Vector3Int((int)hit.transform.position.x, (int)hit.transform.position.y, 0);

        if (currentMineField != null)
        {
            if (Input.GetMouseButton(0))
            {
                currentMineField.OpenCellByCoords(cellCoords);
            }
            else
            {
                currentMineField.SetBombFlag(cellCoords);
            }
        }
    }

    public void SetMineField(MineField mineField)
    {
        currentMineField = mineField;
    }

    public void EnableInput(bool enable)
    {
        isActive = enable;
    }
}