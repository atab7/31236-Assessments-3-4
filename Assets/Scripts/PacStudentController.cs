using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PacStudentController : MonoBehaviour
{

    private Animator animator;
    [SerializeField] private AudioClip walkingSound;
    private GameObject dustEffectObj;
    private ParticleSystem dustEffect;
    public AudioClip eatingSound;
    public AudioSource audioSource;
    private Vector3 currentInput;
    private Vector3 lastInput;
    private Vector3 targetPosition;
    private Vector3 startPosition;
    private float elapsedTime;
    private const float duration = 0.08f;
    private int currRow = 1;
    private int currCol = 1;
    Vector3[,] grid;
    private GameObject pacStuCollider;
    private bool teleportNow = false;
    private InGameUIController uiController;
    private PacStuLifeController lifeController;

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
        pacStuCollider = GameObject.FindGameObjectWithTag("PacStuCollider");
        uiController = GameObject.FindGameObjectWithTag("HUD").GetComponent<InGameUIController>();
        dustEffectObj = GameObject.FindGameObjectWithTag("PacStuDustEffect");
        dustEffect = dustEffectObj.GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        CompleteMap();
        animator = gameObject.GetComponent<Animator>();
        lifeController = GetComponent<PacStuLifeController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!uiController.go || lifeController.isGameOver)
            return;

        GetInput();
        MovePacStu();
    }
    void MovePacStu()
    {
        if (lastInput == null)
            return;

        if (teleportNow)
        {
            TeleportPacStu();
            teleportNow = false;
            return;
        }

        if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            PlayDustEffect();
            PlayAudio();
            LerpPacStu();
            return;
        }
        else
        {
            SwapPellet();
        }
        elapsedTime = 0;

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

    bool IsPellet()
    {
        if (levelMap[currRow, currCol] == 5 || levelMap[currRow, currCol] == 6)
            return true;
        return false;
    }

    public void SwapPellet()
    {
        if (IsPellet())
            levelMap[currRow, currCol] = 0;
    }

    private void TeleportPacStu()
    {
        int col = 0;
        int row = 0;

        if (currRow == 14 && currCol == 1)
        {
            row = 14;
            col = 25;

            transform.position = grid[row, col];
            currRow = row;
            currCol = col;
            startPosition = grid[row, col]; ;
            targetPosition = grid[row, col];
        }
        else if (currRow == 15 && currCol == 1)
        {
            row = 15;
            col = 25;

            transform.position = grid[row, col];
            currRow = row;
            currCol = col;
            startPosition = grid[row, col]; ;
            targetPosition = grid[row, col];
        }
        else if (currRow == 14 && currCol == 26)
        {
            row = 14;
            col = 1;

            transform.position = grid[row, col];
            currRow = row;
            currCol = col;
            startPosition = grid[row, col]; ;
            targetPosition = grid[row, col];
        }
        else if (currRow == 15 && currCol == 26)
        {
            row = 15;
            col = 1;

            transform.position = grid[row, col];
            currRow = row;
            currCol = col;
            startPosition = grid[row, col]; ;
            targetPosition = grid[row, col];
        }
              
    }

    public void setTeleportNow(bool val)
    {
        teleportNow = val;
    }

    public void ResetPacStu()
    {
        lastInput = Vector3.zero;
        currentInput = Vector3.zero;

        currCol = 1;
        currRow = 1;
        startPosition = grid[currRow, currCol];
        targetPosition = grid[currRow, currCol];
        transform.position = grid[currRow, currCol];
        animator.SetInteger("Direction", 0);
    }

    void RotateChildren(Vector3 direction)
    {
        if (direction == Vector3.right)
        {
            pacStuCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
            dustEffectObj.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction == Vector3.left)
        {
            pacStuCollider.transform.rotation = Quaternion.Euler(0, 0, 180);
            dustEffectObj.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (direction == Vector3.up)
        {
            pacStuCollider.transform.rotation = Quaternion.Euler(0, 0, 90);
            dustEffectObj.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (direction == Vector3.down)
        {
            pacStuCollider.transform.rotation = Quaternion.Euler(0, 0, 270);
            dustEffectObj.transform.rotation = Quaternion.Euler(0, 0, 270);
        }
    }

    void PlayAudio()
    {
        if (!audioSource.isPlaying)
        {
            if (IsPellet())
            {
                audioSource.PlayOneShot(eatingSound, 0.3f);
            }
            else
                audioSource.PlayOneShot(walkingSound, 0.3f);
            
        }
    }

    void PlayDustEffect()
    {
        var main = dustEffect.main;
        main.simulationSpeed = 3f;
        dustEffect.Play();
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
        Vector3 rightIncrement = Vector3.right * 11.45f;
        Vector3 downIncrement = Vector3.down * 11.45f;

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
        targetPosition = transform.position;
        startPosition = transform.position;

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
