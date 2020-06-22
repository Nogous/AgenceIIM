using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePush : CubeMovable
{
    [SerializeField] private GameObject stain = null;
    private Vector3 stainScale = Vector3.one;
    private Color stainColor = Color.white;

    [SerializeField] private AnimationCurve fadeCurve = null;
    [SerializeField] private AnimationCurve shrinkCurve = null;

    [SerializeField] private ParticleSystem moveParticle = null;

    private float elapsedTime = 0;

    public bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.OnResetLevel += ResetCube;

        if (stain != null)
        {
            stainScale = stain.transform.localScale;
            stainColor = stain.GetComponent<Renderer>().material.color;
        }

        SetModeVoid();
        //TestTile();
    }

    // Update is called once per frame
    void Update()
    {
        DoAction();
    }

    public override void SetModeMove(Vector3 vector)
    {
        if (DoAction == DoActionFall) return;

        isMoving = true;

        RotationCheck();

        _elapsedTime = 0;

        direction = transform.position + orientation;
        previousPos = transform.position;

        // init move

        moveParticle.transform.eulerAngles = Quaternion.Euler(moveParticle.transform.rotation.eulerAngles) * -orientation;

        moveParticle.Play();

        DoAction = DoActionPush;
    }

    private void DoActionPush()
    {
        _elapsedTime += Time.deltaTime;

        float ratio = _elapsedTime / _moveTime;

        transform.position = Vector3.Lerp(previousPos, direction, ratio);

        if (_elapsedTime >= _moveTime)
        {
            // end move
            EndMoveBehavior();
            isMoving = false;
        }
    }

    public override bool TestWall()
    {
        Ray ray = new Ray(transform.position, orientation);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 0.5f))
        {

            if (hit.transform.gameObject.GetComponent<CubeWall>() || hit.transform.gameObject.GetComponent<Enemy>() || hit.transform.gameObject.GetComponent<CubePush>())
            {
                return true;
            }
        }

        return false;
    }

    public override void TestTile()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.gameObject.GetComponent<CubeDash>())
            {
                CubeDash tmpCube = hit.transform.gameObject.GetComponent<CubeDash>();

                if ((int)tmpCube.dashOrientation == 0)
                {
                    orientation = Vector3.forward;
                }
                else if ((int)tmpCube.dashOrientation == 1)
                {
                    orientation = Vector3.back;
                }
                else if ((int)tmpCube.dashOrientation == 2)
                {
                    orientation = Vector3.right;
                }
                else
                {
                    orientation = Vector3.left;
                }

                SetModeDash();
            }
            else if (hit.transform.gameObject.GetComponent<CubeDetonator>())
            {
                CubeDetonator tmpCube = hit.transform.gameObject.GetComponent<CubeDetonator>();

                tmpCube.ActivateTnt();
            }
            else if (hit.transform.gameObject.GetComponent<CubeTeleporter>())
            {
                CubeTeleporter tmpCube = hit.transform.gameObject.GetComponent<CubeTeleporter>();

                gameObject.transform.position = new Vector3(teleportDestination.transform.position.x, teleportDestination.transform.position.y + 1f, teleportDestination.transform.position.z);
            }
            else if (hit.transform.gameObject.GetComponent<CubeSlid>())
            {
                if (orientation != Vector3.zero)
                if (!TestWall()) SetModeSlid();
            }
        }
        else
        {
            DoAction = DoActionFall;
        }
    }

    public override void DoActionFall()
    {
        base.DoActionFall();

        if (transform.position.y <= initialPosition.y - 1) SetModeVoid();
    }

    public void ActivateStain(Color tint)
    {
        if (stain == null) return;
        
        stain.SetActive(true);
        stain.GetComponent<Renderer>().material.color = tint;

        stain.transform.localScale = stainScale;
        elapsedTime = 0;

        StopCoroutine(StainRemove());
        StartCoroutine(StainRemove());
        
    }

    private IEnumerator StainRemove()
    {
        Color colorFade = stain.GetComponent<Renderer>().material.color;
        Vector3 sizeShrink = stainScale;

        while (stain.GetComponent<Renderer>().material.color.a > 0)
        {           
            elapsedTime += Time.deltaTime;

            colorFade.a = fadeCurve.Evaluate(elapsedTime);
            sizeShrink.x = shrinkCurve.Evaluate(elapsedTime);
            sizeShrink.y = shrinkCurve.Evaluate(elapsedTime);

            stain.transform.localScale = sizeShrink;
            stain.GetComponent<Renderer>().material.color = colorFade;

            yield return null;
        }

        StainReset();
        
        yield return null;
    }

    private void StainReset()
    {
        StopCoroutine(StainRemove());

        stain.GetComponent<Renderer>().material.color = stainColor;
        stain.transform.localScale = stainScale;
        elapsedTime = 0;

        stain.SetActive(false);
        
    }
}
