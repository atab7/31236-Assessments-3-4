using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostControllerLatency : MonoBehaviour
{
    int[,] levelMap =
    {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,2,5,0,0,0,4,0,0,0},
    };
    Vector3[,] grid;
    GhostMovement ghost0, ghost1, ghost2, ghost3;
    int ghostTurn = 3;
    GhostMovement currentGhost;
    InGameUIController uiController;
    PacStuLifeController lifeController;
    

    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("HUD").GetComponent<InGameUIController>();
        lifeController = GameObject.FindGameObjectWithTag("PacStudent").GetComponent<PacStuLifeController>();
        ghost0 = new GhostMovement(1);
        ghost1 = new GhostMovement(2);
        ghost2 = new GhostMovement(3);
        ghost3 = new GhostMovement(4);
        CompleteMap();
    }

    // Update is called once per frame
    void Update()
    {
        if (!uiController.go || lifeController.isGameOver)
            return;

        ghostTurn = (ghostTurn + 1) % 4;
        SetGhost();

        if (currentGhost.isLerping)
        {
            currentGhost.MoveGhost();
            return;
        }
        else if (currentGhost.exitSeq >= 11)
        {
            CoordinateExit();
            currentGhost.isLerping = true;
        }
        else
        {
            SetTarget();
            
        }
        //Debug.Log(currentGhost.ghostObj.transform.position + " " + grid[11, 13]);
    }

    private bool isWall(int i)
    {
        if ((i >= 1) && (i <= 4) || i == 7)
            return true;
        return false;
    }

    private bool IsForbiddenTile(int row, int col)
    {
        if (row == 12 && col == 13)
            return true;
        if (row == 12 && col == 14)
            return true;
        if (row == 17 && col == 13)
            return true;
        if (row == 17 && col == 14)
            return true;

        return false;
    }

    private List<Vector3> GetMovableDirections(int currRow, int currCol)
    {
        List<Vector3> movableDirections = new List<Vector3>();

        int rows = levelMap.GetLength(0);
        int cols = levelMap.GetLength(1);

        if ((currRow + 1) < rows)
        {
            if (!isWall(levelMap[currRow + 1, currCol]) && !IsForbiddenTile(currRow + 1, currCol))
                movableDirections.Add(Vector3.down);
        }

        if ((currCol + 1) < cols)
        {
            if (!isWall(levelMap[currRow, currCol + 1]) && !IsForbiddenTile(currRow, currCol + 1))
                movableDirections.Add(Vector3.right);
        }

        if ((currRow - 1) >= 0)
        {
            if (!isWall(levelMap[currRow - 1, currCol]) && !IsForbiddenTile(currRow - 1, currCol))
                movableDirections.Add(Vector3.up);
        }

        if ((currCol - 1) >= 0)
        {
            if (!isWall(levelMap[currRow, currCol - 1]) && !IsForbiddenTile(currRow, currCol - 1))
                movableDirections.Add(Vector3.left);
        }
        return movableDirections;
    }

    private void SetGhost()
    {
        if (ghostTurn == 0)
            this.currentGhost = ghost0;
        else if (ghostTurn == 1)
            this.currentGhost = ghost1;
        else if (ghostTurn == 2)
            this.currentGhost = ghost2;
        else if (ghostTurn == 3)
            this.currentGhost = ghost3;
    }


    public void CoordinateExit()
    {
        if (currentGhost.exitSeq >= 13)
        {
            SetGhostMovement(grid[13, 13], 13, 13);
            currentGhost.exitSeq--;
        }
        else if (currentGhost.exitSeq >= 11)
        {
            SetGhostMovement(grid[currentGhost.exitSeq, 13], currentGhost.exitSeq, 13);
            currentGhost.exitSeq--;
        }
    }

    private void SetGhostMovement(Vector3 targetPosition, int targetRow, int targetCol)
    {
        currentGhost.SetStartPosition();
        currentGhost.targetPosition = targetPosition;
        currentGhost.currRow = targetRow;
        currentGhost.currCol = targetCol;
    }


    private CoordinatePair GetNextTile()
    {
        if (currentGhost.direction == Vector3.right)
            return new CoordinatePair(currentGhost.currRow, currentGhost.currCol + 1, Vector3.right);

        if (currentGhost.direction == Vector3.left)
            return new CoordinatePair(currentGhost.currRow, currentGhost.currCol - 1, Vector3.left);

        if (currentGhost.direction == Vector3.up)
            return new CoordinatePair(currentGhost.currRow - 1, currentGhost.currCol , Vector3.up);

        if (currentGhost.direction == Vector3.down)
            return new CoordinatePair(currentGhost.currRow + 1, currentGhost.currCol, Vector3.down);

        return null;
    }

    private Vector3 PickGhost3StyleDirection()
    {
        List<Vector3> movableDirections = GetMovableDirections(currentGhost.currRow, currentGhost.currRow);
        Vector3 direction = movableDirections[Random.Range(0, movableDirections.Count)];
        return direction;
    }

    private void SetTarget()
    {
        if (ghostTurn == 0)
        {

        }
        else if (ghostTurn == 1)
        {

        }
        else if (ghostTurn == 2)
        {
            if (WallExists(currentGhost.direction))
            {
                Vector3 directon = PickGhost3StyleDirection();
                currentGhost.direction = directon;
            }
            CoordinatePair nextTile = GetNextTile();
            SetGhostMovement(grid[nextTile.row, nextTile.column], nextTile.row, nextTile.column);

        }
        else if (ghostTurn == 3)
        {

        }
    }

    bool WallExists(Vector3 direction)
    {
        int rows = levelMap.GetLength(0);
        int cols = levelMap.GetLength(1);

        if (direction == Vector3.right)
        {
            if (currentGhost.currCol + 1 < cols)
            {
                if (isWall(levelMap[currentGhost.currRow, currentGhost.currCol + 1]) || IsForbiddenTile(currentGhost.currRow, currentGhost.currCol + 1))
                    return true;
            }
        }
        else if (direction == Vector3.left)
        {
            if (currentGhost.currCol - 1 >= 0)
            {
                if (isWall(levelMap[currentGhost.currRow, currentGhost.currCol - 1]) || IsForbiddenTile(currentGhost.currRow, currentGhost.currCol - 1))
                    return true;
            }
        }
        else if (direction == Vector3.up)
        {
            if (currentGhost.currRow - 1 >= 0)
            {
                if (isWall(levelMap[currentGhost.currRow - 1, currentGhost.currCol]) || IsForbiddenTile(currentGhost.currRow - 1, currentGhost.currCol))
                    return true;
            }
        }
        else if (direction == Vector3.down)
        {
            if (currentGhost.currRow + 1 < rows)
            {
                if (isWall(levelMap[currentGhost.currRow + 1, currentGhost.currCol]) || IsForbiddenTile(currentGhost.currRow + 1, currentGhost.currCol))
                    return true;
            }
        }

        return false;
    }

    void CompleteMap()
    {
        int rows = levelMap.GetLength(0);
        int cols = levelMap.GetLength(1);

        int[,] completeLevelMap = new int[rows * 2, cols * 2];

        for (int i = 0; i < rows; i++)
        {
            for (int s = 0; s < cols; s++)
            {
                completeLevelMap[i, s] = levelMap[i, s];
                completeLevelMap[i, cols + s] = levelMap[i, cols - s - 1];
            }
        }

        int compRows = completeLevelMap.GetLength(0);
        int compCols = completeLevelMap.GetLength(1);


        for (int i = 0; i < compCols; i++)
        {
            for (int s = 0; s < rows; s++)
            {
                completeLevelMap[compRows - s - 1, i] = completeLevelMap[s, i];
            }
        }
        levelMap = completeLevelMap;


        grid = new Vector3[compRows, compCols];
        Vector3 initPos = GameObject.FindGameObjectWithTag("gridAnchor").transform.position;
        Vector3 rightIncrement = Vector3.right * 11.45f;
        Vector3 downIncrement = Vector3.down * 11.45f;

        for (int i = 0; i < compRows; i++)
        {
            for (int s = 0; s < compCols; s++)
            {
                if (isWall(levelMap[i, s]))
                {
                    grid[i, s] = Vector3.zero;
                }
                else
                {
                    grid[i, s] = initPos + (i * downIncrement) + (s * rightIncrement);
                }
            }
        }
        ghost0.InitGhost(grid[13, 12], 13, 12);
        ghost1.InitGhost(grid[13, 13], 13, 13);
        ghost2.InitGhost(grid[13, 14], 13, 14);
        ghost3.InitGhost(grid[13, 15], 13, 15);
    }
}


public class CoordinatePair
{
    public int row { get; private set; }
    public int column { get; private set; }

    public CoordinatePair(int row, int column, Vector3 direction)
    {
        this.row = row;
        this.column = column;
    }
}


public class GhostMovement
{
    public GameObject ghostObj;
    public Vector3 startPosition;
    public Vector3 targetPosition;
    public Vector3 direction = Vector3.up;
    public int currRow, currCol;
    float elapsedTime = 0f;
    private const float duration = 0.05f;
    public bool isLerping = false;
    public int exitSeq = 13;

    public GhostMovement(int ghostID)
    {
        ghostObj = GameObject.FindGameObjectWithTag("Ghost" + ghostID);
    }


    public void InitGhost(Vector3 initialPosition, int row, int col)
    {
        ghostObj.transform.position = initialPosition;
        startPosition = initialPosition;
        targetPosition = initialPosition;
        currRow = row;
        currCol = col;
    }

    public void SetStartPosition()
    {
        startPosition = ghostObj.transform.position;
    }

    private void LerpGhost()
    {
        elapsedTime += Time.deltaTime;
        float ratio = elapsedTime / duration;
        ghostObj.transform.position = Vector3.Lerp(startPosition, targetPosition, ratio);
    }

    public void MoveGhost()
    {
        if (Vector3.Distance(ghostObj.transform.position, targetPosition) > 0.1f)
        {
            LerpGhost();
            isLerping = true;
        }
        else
        {
            isLerping = false;
            elapsedTime = 0f;
        }
    }

}