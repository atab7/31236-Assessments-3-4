using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GhostController : MonoBehaviour
{

    private Animator animator;
    private Vector3 currentInput = Vector3.up;
    private Vector3 lastInput;
    private Vector3 targetPosition;
    private Vector3 startPosition;
    private float elapsedTime;
    private const float duration = 0.1f;
    private int currRow;
    private int currCol;
    private int startMovingRow = 11;
    private int startMovingCol = 13;
    public int resetRow;
    public int resetColumn;
    Vector3[,] grid;
    private InGameUIController uiController;
    private PacStuLifeController lifeController;
    private GameObject PacStudent;
    private float inputTimer = 1.5f;
    private int exitSeq = 13;
    public bool isDead = false;

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

    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("HUD").GetComponent<InGameUIController>();
        CompleteMap();
        animator = gameObject.GetComponent<Animator>();
        PacStudent = GameObject.FindGameObjectWithTag("PacStudent");
        lifeController = PacStudent.GetComponent<PacStuLifeController>();
        currRow = resetRow;
        currCol = resetColumn;
    }

    // Update is called once per frame
    void Update()
    {
        if (!uiController.go || lifeController.isGameOver)
            return;
        
        if (!isDead) 
            GetInput();
            MoveGhost();
    }
    void MoveGhost()
    {
        if (lastInput == null || isDead)
            return;

        if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            LerpGhost();
            return;
        }
        elapsedTime = 0;


        if (exitSeq >= 10)
        {
            PlayExit();
            return;
        }

        if (!WallExists(lastInput))
        {
            UpdateMapPosition(lastInput);
            startPosition = transform.position;
            targetPosition = grid[currRow, currCol];
            currentInput = lastInput;
            SetAnimatorParam(lastInput);
            RotateChildren(lastInput);
        }
        else
        {
            if (!WallExists(currentInput))
            {
                UpdateMapPosition(currentInput);
                startPosition = transform.position;
                targetPosition = grid[currRow, currCol];
            }
        }

    }

    public void ResetGhost()
    {
        lastInput = Vector3.zero;
        currentInput = Vector3.zero;
        exitSeq = 13;
        currCol = startMovingCol;
        currRow = startMovingRow;
        startPosition = grid[resetRow, resetColumn];
        targetPosition = grid[resetRow, resetColumn];
        transform.position = grid[resetRow, resetColumn];
        animator.SetInteger("Direction", 0);
    }

    void RotateChildren(Vector3 direction)
    {
        if (direction == Vector3.right)
        {
            gameObject.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction == Vector3.left)
        {
            gameObject.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (direction == Vector3.up)
        {
            gameObject.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (direction == Vector3.down)
        {
            gameObject.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 270);
        }
    }

    void SetAnimatorParam(Vector3 direction)
    {
        if (direction == Vector3.right)
        {
            animator.SetInteger("Direction", 3);
        }
        else if (direction == Vector3.left)
        {
            animator.SetInteger("Direction", 9);
        }
        else if (direction == Vector3.up)
        {
            animator.SetInteger("Direction", 12);
        }
        else if (direction == Vector3.down)
        {
            animator.SetInteger("Direction", 6);
        }
    }


    void GetInput()
    {
        if (inputTimer >= 1f)
        {
            List<Vector3> movableDirections = GetMovableDirections(currentInput);
            if (movableDirections.Count > 0)
            {
                System.Random rnd = new System.Random();
                int randIndex = rnd.Next(movableDirections.Count);
                Vector3 random = movableDirections[randIndex];
                lastInput = random;
            }
            else
                lastInput = GetRandomDirection();
  

        }
        else if (inputTimer <= 0)
        {
            inputTimer = 1f;
        }
        else
        {
            inputTimer -= Time.deltaTime;
        }
    }


    private Vector3 GetRandomDirection()
    {
        List<Vector3> directions = new List<Vector3> { Vector3.up, Vector3.left, Vector3.right, Vector3.down };
        System.Random rnd = new System.Random();
        int randIndex = rnd.Next(directions.Count);
        return directions[randIndex];
    }

    private List<Vector3> GetMovableDirections(Vector3 currentDirection)
    {
        if (gameObject.CompareTag("Ghost1"))
        {
            List<Vector3> movable = new List<Vector3>();
            Vector3 vectorToPacStu = PacStudent.transform.position - gameObject.transform.position;
            if (vectorToPacStu.x > 0)
                movable.Add(Vector3.right);
            else if (vectorToPacStu.x < 0)
                movable.Add(Vector3.left);

            if (vectorToPacStu.y > 0)
                movable.Add(Vector3.up);
            else if (vectorToPacStu.y < 0)
                movable.Add(Vector3.down);

            for (int i = 0; i < movable.Count; i++)
            {
                if (WallExists(movable[i]))
                    movable[i] *= -1;
            }

            return movable;
        }
        else if (gameObject.CompareTag("Ghost2"))
        {
            List<Vector3> movable = new List<Vector3>();
            Vector3 vectorToPacStu = PacStudent.transform.position - gameObject.transform.position;
            if (vectorToPacStu.x > 0)
                movable.Add(Vector3.left);
            else if (vectorToPacStu.x < 0)
                movable.Add(Vector3.right);

            if (vectorToPacStu.y > 0)
                movable.Add(Vector3.down);
            else if (vectorToPacStu.y < 0)
                movable.Add(Vector3.up);

            for (int i = 0; i < movable.Count; i++)
            {
                if (WallExists(movable[i]))
                    movable[i] *= -1;
            }

            return movable;
        }
        else if (gameObject.CompareTag("Ghost3") || gameObject.CompareTag("Ghost4"))
        {
            if (currentDirection == Vector3.up)
                return new List<Vector3> { Vector3.up, Vector3.left, Vector3.right };

            if (currentDirection == Vector3.down)
                return new List<Vector3> { Vector3.down, Vector3.left, Vector3.right };

            if (currentDirection == Vector3.left)
                return new List<Vector3> { Vector3.up, Vector3.down, Vector3.left };

            if (currentDirection == Vector3.right)
                return new List<Vector3> { Vector3.up, Vector3.down, Vector3.right };
        }

        return new List<Vector3>();
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

    void LerpGhost()
    {
        elapsedTime += Time.deltaTime;
        float ratio = elapsedTime / duration;
        transform.position = Vector3.Lerp(startPosition, targetPosition, ratio);
    }


    bool WallExists(Vector3 direction)
    {
        if (direction == Vector3.right)
        {
            if (isWall(levelMap[currRow, currCol + 1]) || IsForbiddenTile(currRow, currCol + 1))
                return true;
        }
        else if (direction == Vector3.left)
        {
            if (isWall(levelMap[currRow, currCol - 1]) || IsForbiddenTile(currRow, currCol - 1))
                return true;
        }
        else if (direction == Vector3.up)
        {
            if (isWall(levelMap[currRow - 1, currCol]) || IsForbiddenTile(currRow - 1, currCol))
                return true;
        }
        else if (direction == Vector3.down)
        {
            if (isWall(levelMap[currRow + 1, currCol]) || IsForbiddenTile(currRow + 1, currCol))
                return true;
        }

        return false;
    }

    private void PlayExit()
    {
        if (exitSeq >= 13)
        {
            startPosition = transform.position;
            targetPosition = grid[13, 13];
            exitSeq--;
        }
        else if (exitSeq >= 11)
        {
            startPosition = transform.position;
            targetPosition = grid[exitSeq, 13];
            exitSeq--;
        }
        else
        {
            currRow = startMovingRow;
            currCol = startMovingCol;
            transform.position = grid[currRow, currCol];
            startPosition = transform.position;
            targetPosition = transform.position;
            exitSeq--;
        }
    }

    private bool isWall(int i)
    {
        if ((i >= 1) && (i <= 4) || i == 7)
            return true;
        return false;
    }


    private void UpdateMapPosition(Vector3 direction)
    {
        if (direction == Vector3.right)
        {
            currCol += 1;
        }
        else if (direction == Vector3.left)
        {
            currCol -= 1;
        }
        else if (direction == Vector3.up)
        {
            currRow -= 1;
        }
        else if (direction == Vector3.down)
        {
            currRow += 1;
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
        transform.position = grid[resetRow, resetColumn];

    }
}
