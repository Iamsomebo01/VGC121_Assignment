
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public float minXPos;
    public float maxXPos;

    public float minYPos;
    public float maxYPos;

    public Transform target;
    public float cameraSpeed = 5;

    void Start()
    {
        if (target == null)
        {
            GameObject Player = GameObject.FindGameObjectWithTag("Player");

            if (Player == null)
            {
                Debug.Log("No Player found!");
                return;
            }

            target = Player.transform;
        }
    }

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 currentPos = transform.position;

       
        currentPos.x = Mathf.Clamp(target.position.x, minXPos, maxXPos);
        
        currentPos.y = Mathf.Clamp(target.position.y, minYPos, maxYPos);

        transform.position = Vector3.MoveTowards(transform.position,currentPos,cameraSpeed * Time.deltaTime);
    }
}
