using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{

    private Vector3 currentInput;
    private Vector3 lastInput;
    private Vector3 targetPosition;
    private Vector3 startPosition;
    private float elapsedTime;
    private const float duration = 0.5f;
    private int currRow = 1;
    private int currCol = 1;
    Vector3[,] grid;


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
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    };

    // Start is called before the first frame update
    void Start()
    {
        CompleteMap();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        MovePacStu();
    }
    void MovePacStu()
    {
        if (lastInput == null || startPosition == null || targetPosition == null)
            return;

        //Vector3 nextPosition = this.transform.position;

        if (Vector3.Distance(startPosition, targetPosition) > 0.1f)
        {
            LerpPacStu();
            return;
        }
        elapsedTime = 0;

        if (!WallExists(lastInput))
        {
            //targetPosition = transform.position + lastInput * 16;
            //nextPosition += lastInput * 100.0f * Time.deltaTime;
            UpdateMapPosition(lastInput);
            targetPosition = grid[currRow, currCol];
            currentInput = lastInput;
        }
        else
        {
            if (!WallExists(currentInput))
            {
                //targetPosition = transform.position + lastInput * 14;
                //nextPosition += currentInput * 500.0f * Time.deltaTime;
                UpdateMapPosition(currentInput);
                targetPosition = grid[currRow, currCol];
            }
            else
                Debug.Log(currRow + ", " + currCol);
            
        }

        //this.transform.position = nextPosition;
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            lastInput = Vector3.right;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            lastInput = Vector3.left;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            lastInput = Vector3.up;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            lastInput = Vector3.down;
        }
    }

    void LerpPacStu()
    {
        elapsedTime += Time.deltaTime;
        float ratio = elapsedTime / duration;
        startPosition = transform.position;
        transform.position = Vector3.Lerp(startPosition, targetPosition, ratio);
    }


    bool WallExists(Vector3 direction)
    {
        if (direction == Vector3.right)
        {
            if (isWall(levelMap[currRow, currCol + 1]))
                return true;
        }
        else if (direction ==  Vector3.left)
        {
            if (isWall(levelMap[currRow, currCol - 1]))
                return true;
        }
        else if (direction == Vector3.up)
        {
            if (isWall(levelMap[currRow - 1, currCol]))
                return true;
        }
        else if (direction == Vector3.down)
        {
            if (isWall(levelMap[currRow + 1, currCol]))
                return true;
        }


        return false;
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

        int[,] completeLevelMap =  new int[rows * 2, cols * 2];

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
                completeLevelMap[compRows - s - 1 , i] = completeLevelMap[s, i];
            }
        }
        levelMap = completeLevelMap;


        grid = new Vector3[compRows, compCols];
        Vector3 initPos = GameObject.FindGameObjectWithTag("gridAnchor").transform.position;
        Vector3 rightIncrement = Vector3.right * 11.5f;
        Vector3 downIncrement = Vector3.down * 11.5f;

        for(int i = 0; i < compRows; i++)
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
        transform.position = grid[1, 1];

        /*
        string str = "";
        for (int i = 0; i < compRows; i++)
        {
            str += "[";
            for (int s = 0; s < compCols; s++)
            {
                str += grid[i, s] + ", ";
            }
            str += "]\n";
        }

        Debug.Log(str);
        */
    }

    
}
