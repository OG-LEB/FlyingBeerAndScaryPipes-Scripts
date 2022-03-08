using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FLOOR_MOVEMENT : MonoBehaviour
{
    private const float Movement_speed = 15;
    private const float MOVE_START_POS_X = 1.418f;
    private const float MOVE_END_POS_X = -0.76f;
    private Vector3 POSITION_VECTOR = new Vector3(0,-18,0);

    private LEVEL lvl;

    private void Start()
    {
        lvl = LEVEL.GetInstance();
        RestartPosition();
    }

    private void Update()
    {
        if (lvl._isPlaying())
        {
            transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * Movement_speed);
            if (transform.position.x <= MOVE_END_POS_X)
            {
                RestartPosition();
            }
        }
    }

    private void RestartPosition() 
    {
        transform.position = new Vector3(MOVE_START_POS_X, POSITION_VECTOR.y, POSITION_VECTOR.z);
    }
}
