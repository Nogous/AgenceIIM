using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTravel : MonoBehaviour
{
    public float speedMove =1;
    public Transform CamObj;

    public Transform[] camPositions = new Transform[2];
    
    private Vector3 pointAngle;

    private int posTab = -1;
    private int posTabPlus = 0;
    private float lerpCount = 1;

    Vector3 _posO;
    Vector3 _tmpVectOA;
    Vector3 _tmpC;

    private bool start = false;
    private bool moveEnd = true;

    private void Awake()
    {
        CamObj.transform.position = camPositions[0].position;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //MoveToNextPoint();
        }

        MoveUpdate();
    }

    public bool MoveToNextPoint()
    {
        if (!moveEnd) return false;
        moveEnd = false;
        posTab++;

        if (posTab >= camPositions.Length)
        {
            posTab = 0;
        }
        posTabPlus = posTab + 1;
        if (posTabPlus >= camPositions.Length)
        {
            posTabPlus = 0;
        }

        lerpCount = 0;
        
        _posO = camPositions[posTab].position + (camPositions[posTabPlus].position - camPositions[posTab].position) / 2;
        _tmpVectOA = camPositions[posTab].position - _posO;
        _tmpC = _posO + new Vector3(_tmpVectOA.z, _tmpVectOA.y, -_tmpVectOA.x);

        start = true;
        return true;
    }


    public void MoveUpdate()
    {
        if (!start) return;
        lerpCount += Time.deltaTime * speedMove;
        if (lerpCount >= 1)
        {
            moveEnd = true;
            start = false;
        }

        Vector3 tmpPos = Vector3.Lerp(
                    Vector3.Lerp(camPositions[posTab].position, _tmpC, lerpCount),
                    Vector3.Lerp(_tmpC, camPositions[posTabPlus].position, lerpCount),
                    lerpCount);

        CamObj.transform.position = tmpPos;
    }

    private void OnDrawGizmos()
    {
        Vector3 posO;
        Vector3 tmpVectOA;
        Vector3 tmpC;

        int smoother = 10;
        float l;

        Vector3 tmpPos0;
        Vector3 tmpPos1;

        for (int j = 0; j+1 < camPositions.Length; j++)
        {
            posO = camPositions[j].position + (camPositions[j+1].position - camPositions[j].position) / 2;
            tmpVectOA = camPositions[j].position - posO;
            tmpC = posO + new Vector3(tmpVectOA.z, tmpVectOA.y, -tmpVectOA.x);

            l = 0f;
            for (int i = 0; i < smoother; i++)
            {
                tmpPos0 = Vector3.Lerp(
                    Vector3.Lerp(camPositions[j].position, tmpC, l),
                    Vector3.Lerp(tmpC, camPositions[j+1].position, l),
                    l);

                l += 1 / (float)smoother;

                tmpPos1 = Vector3.Lerp(
                    Vector3.Lerp(camPositions[j].position, tmpC, l),
                    Vector3.Lerp(tmpC, camPositions[j+1].position, l),
                    l);

                Gizmos.DrawLine(tmpPos0, tmpPos1);
            }
        }
        posO = camPositions[camPositions.Length-1].position + (camPositions[0].position - camPositions[camPositions.Length - 1].position) / 2;
        tmpVectOA = camPositions[camPositions.Length - 1].position - posO;
        tmpC = posO + new Vector3(tmpVectOA.z, tmpVectOA.y, -tmpVectOA.x);

        l = 0f;
        for (int i = 0; i < smoother; i++)
        {
            tmpPos0 = Vector3.Lerp(
                Vector3.Lerp(camPositions[camPositions.Length - 1].position, tmpC, l),
                Vector3.Lerp(tmpC, camPositions[0].position, l),
                l);

            l += 1 / (float)smoother;

            tmpPos1 = Vector3.Lerp(
                Vector3.Lerp(camPositions[camPositions.Length - 1].position, tmpC, l),
                Vector3.Lerp(tmpC, camPositions[0].position, l),
                l);

            Gizmos.DrawLine(tmpPos0, tmpPos1);
        }
    }
}
