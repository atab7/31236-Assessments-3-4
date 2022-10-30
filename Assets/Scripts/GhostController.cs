using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
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
            CloseBarnDoor();
            return;
        }
        else if (currentGhost.exitSeq >= 11)
        {
            OpenBarnDoor();
            CoordinateExit();
            currentGhost.isLerping = true;
        }
        //Debug.Log(currentGhost.ghostObj.transform.position + " " + grid[11, 13]);
    }

    private bool isWall(int i)
    {
        if ((i >= 1) && (i <= 4) || i == 7)
            return true;
        return false;
    }

    private void OpenBarnDoor()
    {
        levelMap[12, 13] = 0;
        levelMap[12, 14] = 0;
        levelMap[17, 13] = 0;
        levelMap[17, 14] = 0;
    }

    private void CloseBarnDoor()
    {
        levelMap[12, 13] = 4;
        levelMap[12, 14] = 4;
        levelMap[17, 13] = 4;
        levelMap[17, 14] = 4;
    }

    private List<CoordinatePair> getMovableTiles(int currRow, int currCol)
    {
        List<CoordinatePair> movableTiles = new List<CoordinatePair>();

        int rows = levelMap.GetLength(0);
        int cols = levelMap.GetLength(1);

        if ((currRow + 1) < rows &&  !isWall(levelMap[currRow + 1, currCol]))
            movableTiles.Add(new CoordinatePair(currRow + 1, currCol));

        if ((currCol + 1) < cols && !isWall(levelMap[currRow, currCol + 1]))
            movableTiles.Add(new CoordinatePair(currRow, currCol + 1));

        if ((currRow - 1) >= 0 && !isWall(levelMap[currRow - 1, currCol]))
            movableTiles.Add(new CoordinatePair(currRow - 1, currCol));

        if ((currCol - 1) >= 0  && !isWall(levelMap[currRow, currCol - 1]))
            movableTiles.Add(new CoordinatePair(currRow, currCol - 1 ));

        return movableTiles;
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

    private void PickTarget()
    {
        if (ghostTurn == 0)
        {

        }
        else if (ghostTurn == 1)
        {

        }
        else if (ghostTurn == 2)
        {
            //List<CoordinatePair> movableTiles = 
        }
        else if (ghostTurn == 3)
        {

        }
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

    public CoordinatePair(int row, int column)
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