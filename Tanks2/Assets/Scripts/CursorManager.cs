using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{

    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt)|| Input.GetKey(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;
        else
        {
            if(Cursor.lockState==CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}

