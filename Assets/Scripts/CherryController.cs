using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    [SerializeField] private GameObject cherryPrefab;
    private GameObject cherryObj;
    private float timeSinceLastSpawn;
    private float elapsedTime;
    const float duration = 8.0f;
    private Vector3 startPos;
    private Vector3 endPos;


    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0;
        timeSinceLastSpawn = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= 10.0f)
        {
            Spawn();
            timeSinceLastSpawn = 0;
        }
        else
        {
            if (cherryObj != null)
            {
                if (Vector3.Distance(cherryObj.transform.position, endPos) > 0.1f)
                    LerpCherry(startPos, endPos);
                else
                {
                    Destroy(cherryObj);
                    elapsedTime = 0;
                }
            }
        }
    }

    void LerpCherry(Vector3 source, Vector3 target)
    {
        if (source != null && target != null)
        {
            elapsedTime += Time.deltaTime;
            float ratio = elapsedTime / duration;
            cherryObj.transform.position = Vector3.Lerp(source, target, ratio);
        }
        
    }

    void Spawn()
    {

        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.nearClipPlane));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

        float cameraYTop = topRight.y;
        float cameraYBottom = bottomRight.y;
        float cameraYMiddle = (cameraYTop - cameraYBottom) / 2 + cameraYBottom;

        float cameraXLeftmost = bottomLeft.x;
        float cameraXRightmost = bottomRight.x;

        float startXPos = Random.Range(0.0f, 1.0f) > 0.5 ? cameraXLeftmost : cameraXRightmost;
        float startYPos = Random.Range(cameraYTop, cameraYBottom);
        startPos = new Vector3(startXPos, startYPos, 0);

        float endXPos = startXPos == cameraXLeftmost ? cameraXRightmost : cameraXLeftmost;
        float endYPos = startYPos > cameraYMiddle ? (cameraYTop - startYPos) + cameraYBottom : cameraYTop - (startYPos - cameraYBottom);
        endPos = new Vector3(endXPos, endYPos, 0);

        cherryObj = Instantiate(cherryPrefab, startPos, Quaternion.identity);
    }
}
