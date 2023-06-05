using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{

    private enum Mode
    {
        LookAt,
        LookAtInverted,
    }

    [SerializeField] private Mode mode = Mode.LookAt;

    // Update is called once per frame
    private void LateUpdate()
    {

        switch (mode)
        {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                Vector3 directionFromCamera = transform.position - Camera.main.transform.position;

                transform.LookAt(transform.position + directionFromCamera);
                break;
        }
    }
}
