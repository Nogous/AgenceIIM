using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : CubeMovable
{
    public enum MoveDir
    {
        up,
        down,
        right,
        left,
    }

    private Quaternion newCubeRot;

    [Header("Color Settings")]

    [SerializeField] private Renderer[] faceColor = new Renderer[6];
    private Color[] initColors = new Color[6];
    [SerializeField] private Color baseColor = Color.white;
    private MoveDir moveDir;

    [SerializeField] private TrailRenderer trail = null;

    [SerializeField] private ParticleSystem Splash = null;

    public static event Action<Vector3> OnMove;

    [Header("Options Axes Mobile")]
    bool MobileAxeHorPos = false;
    bool MobileAxeHorNeg = false;
    bool MobileAxeVerPos = false;
    bool MobileAxeVerNeg = false;
    public float DuréeActivationAxe = 0.01f;
    [Header("Image des controles")]
    public Image crossUI;
    public float timeNoAction;
    public AnimationCurve LUT_FadeControls;
    [Header("Variables ScreenShake")]
    public float TimeShakeEnnemy;
    public float MagnShakeEnnemy;
    public float TimeShakePlayer;
    public float MagnShakePlayer;

    public override void Awake()
    {
        SetModeVoid();

        initialPosition = transform.position;
        initialRotation = transform.rotation;

        vectors.Add(Vector3.forward);
        vectors.Add(Vector3.back);
        vectors.Add(Vector3.left);
        vectors.Add(Vector3.right);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.OnResetLevel += ResetCube;

        if (GameManager.instance.player == null)
        {
            GameManager.instance.player = this;
        }

        // initialisation depart
        for (int i = faceColor.Length; i-- > 0;)
        {
            initColors[i] = faceColor[i].material.color;
        }

        crossUI = GameObject.Find("ControlIcon").GetComponent<Image>();
        SwipeDetector.OnSwipe += ProcessMobileInput;
    }

    public void StartPlayer()
    {
        SetModeWait();
    }

    void OnDestroy()
    {
        SwipeDetector.OnSwipe -= ProcessMobileInput;
    }

    void Update()
    {
        DoAction();
        timeNoAction += Time.deltaTime;
        /*
        if (LUT_FadeControls.Evaluate(timeNoAction) >= 10)
        {
            crossUI.color = new Color(255, 255, 255, 1);
        }
        else
        {
            crossUI.color = new Color(255, 255, 255, LUT_FadeControls.Evaluate(timeNoAction));
        }
        */
    }

    public override void StartMoveBehavior()
    {
        base.StartMoveBehavior();

        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        Color tmpColor;

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.gameObject.GetComponent<CubeDestructible>())
            {
                CubeDestructible tmpCube = hit.transform.gameObject.GetComponent<CubeDestructible>();
                tmpCube.Crumble();

                /*tmpColor = tmpCube.GetColor();
                if (tmpColor != Color.white)
                {

                    if (baseColor == faceColor[1].GetComponent<Renderer>().material.color)
                    {
                        faceColor[1].GetComponent<Renderer>().material.color = tmpColor;
                    }
                }*/
            }
        }

        // test si enemy sur le passage du joueur
        ray = new Ray(transform.position, Vector3.forward);
        tmpColor = faceColor[4].GetComponent<Renderer>().material.color;

        switch (moveDir)
        {
            case MoveDir.down:
                ray = new Ray(transform.position, Vector3.back);
                tmpColor = faceColor[3].GetComponent<Renderer>().material.color;
                break;
            case MoveDir.right:
                ray = new Ray(transform.position, Vector3.right);
                tmpColor = faceColor[2].GetComponent<Renderer>().material.color;
                break;
            case MoveDir.left:
                ray = new Ray(transform.position, Vector3.left);
                tmpColor = faceColor[5].GetComponent<Renderer>().material.color;
                break;
        }

        //Debug.DrawRay(ray.origin, ray.direction, Color.black, 1f);
        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.gameObject.GetComponent<Enemy>())
            {
                Enemy tmpCube = hit.transform.gameObject.GetComponent<Enemy>();

                if (tmpColor == tmpCube.enemyColor)
                {
                    CameraHandler.instance.StartCoroutine(CameraHandler.instance.Shake(TimeShakeEnnemy, MagnShakeEnnemy));
                    tmpCube.Explode();
                }
                else
                {
                    SetModeVoid();
                    StartCoroutine(Death());
                    Explode();
                }
            }
        }
    }

    public override void EndMoveBehavior()
    {

        SetModeWait();

        transform.eulerAngles = Vector3.zero;
        UpdateColor();

        SplashPaint();

        TestTile();
    }

    private void ProcessMobileInput(SwipeData data)
    {
        if (data.Direction == SwipeDirection.Up)
        {
            StartCoroutine(MobileUpAxisBehaviour());
        }
        else if (data.Direction == SwipeDirection.Down)
        {
            StartCoroutine(MobileDownAxisBehaviour());
        }
        else if (data.Direction == SwipeDirection.Right)
        {
            StartCoroutine(MobileRightAxisBehaviour());
        }
        else if (data.Direction == SwipeDirection.Left)
        {
            StartCoroutine(MobileLeftAxisBehaviour());
        }
    }

    public void ResetPlayer()
    {
        trail.gameObject.SetActive(false);

        for (int i = faceColor.Length; i-- > 0;)
        {
            faceColor[i].gameObject.SetActive(true);
            faceColor[i].material.color = initColors[i];
        }
        gameObject.SetActive(true);

        SetModeWait();
    }

    #region Actions

    #region Wait

    private void SetModeWait()
    {
        DoAction = DoActionWait;
        if (trail != null)
        {
            trail.gameObject.SetActive(true);
        }
    }

    private void DoActionWait()
    {
        #region normal
        if (!CameraHandler.instance.position)
        {

            if (replayer.GetAxis(RewiredConsts.Action.MoveVert) > 0.1f || MobileAxeVerPos)
            {
                orientation = Vector3.forward;
                moveDir = MoveDir.up;
                timeNoAction = 0.0f;
                ApplyStain();

                if (!TestWall()) SetModeMove(Vector3.zero);

                else return;

                OnMove?.Invoke(orientation);
            }
            else if (replayer.GetAxis(RewiredConsts.Action.MoveVert) < -0.1f || MobileAxeVerNeg)
            {
                orientation = Vector3.back;
                moveDir = MoveDir.down;
                timeNoAction = 0.0f;
                ApplyStain();

                if (!TestWall()) SetModeMove(Vector3.zero);

                else return;

                OnMove?.Invoke(orientation);
            }
            else if (replayer.GetAxis(RewiredConsts.Action.MoveHor) > 0.1f || MobileAxeHorPos)
            {
                orientation = Vector3.right;
                moveDir = MoveDir.right;
                timeNoAction = 0.0f;
                ApplyStain();

                if (!TestWall()) SetModeMove(Vector3.zero);

                else return;

                OnMove?.Invoke(orientation);
            }
            else if (replayer.GetAxis(RewiredConsts.Action.MoveHor) < -0.1f || MobileAxeHorNeg)
            {
                orientation = Vector3.left;
                moveDir = MoveDir.left;
                timeNoAction = 0.0f;
                ApplyStain();

                if (!TestWall()) SetModeMove(Vector3.zero);

                else return;

                OnMove?.Invoke(orientation);
            }
        }
        #endregion
        #region inverted
        else
        {

            if (replayer.GetAxis(RewiredConsts.Action.MoveVert) * -1 > 0.1f || MobileAxeVerNeg)
            {
                orientation = Vector3.forward;
                moveDir = MoveDir.up;
                timeNoAction = 0.0f;
                ApplyStain();

                if (!TestWall()) SetModeMove(Vector3.zero);

                else return;

                OnMove?.Invoke(orientation);
            }
            else if (replayer.GetAxis(RewiredConsts.Action.MoveVert) * -1 < -0.1f || MobileAxeVerPos)
            {
                orientation = Vector3.back;
                moveDir = MoveDir.down;
                timeNoAction = 0.0f;
                ApplyStain();

                if (!TestWall()) SetModeMove(Vector3.zero);

                else return;

                OnMove?.Invoke(orientation);
            }
            else if (replayer.GetAxis(RewiredConsts.Action.MoveHor) * -1 > 0.1f || MobileAxeHorNeg)
            {
                orientation = Vector3.right;
                moveDir = MoveDir.right;
                timeNoAction = 0.0f;
                ApplyStain();

                if (!TestWall()) SetModeMove(Vector3.zero);

                else return;

                OnMove?.Invoke(orientation);
            }
            else if (replayer.GetAxis(RewiredConsts.Action.MoveHor) * -1 < -0.1f || MobileAxeHorPos)
            {
                orientation = Vector3.left;
                moveDir = MoveDir.left;
                timeNoAction = 0.0f;
                ApplyStain();

                if (!TestWall()) SetModeMove(Vector3.zero);

                else return;

                OnMove?.Invoke(orientation);
            }
        }
        #endregion
    }

    #endregion

    public void SetDeath()
    {
        SetModeVoid();
        StartCoroutine(Death());
        Explode();
    }

    public override void Explode()
    {
        Color color = Color.white;

        List<Color> colors = new List<Color>();

        for (int i = 0; i < faceColor.Length; i++)
        {
            if (faceColor[i].material.color != Color.white)
            {
                colors.Add(faceColor[i].material.color);
            }
        }

        for (int i = 0; i < colors.Count; i++)
        {
            color = colors[(int)UnityEngine.Random.value * colors.Count];
        }

        ParticleSystem particles = Instantiate(particleDeath, transform.position, Quaternion.identity);

        ParticleSystem.MainModule mainMod = particles.main;

        mainMod.startColor = color;

        particles.Play();
    }

    private IEnumerator Death(string deathInfo = null)
    {
        if (deathInfo == "fall")
        {
            yield return new WaitForSeconds(GameManager.instance.fallDuration);
            SetModeVoid();
        }

        SetModeVoid();

        gameObject.SetActive(false);

        CameraHandler.instance.StartCoroutine(CameraHandler.instance.Shake(TimeShakePlayer, MagnShakePlayer));
        
        AudioManager.instance.Play("Death");
        yield return new WaitForSeconds(2f);

        GameManager.instance.ResetParty();
    }

    #region Move

    public override void SetModeMove(Vector3 vector)
    {
        RotationCheck();

        _elapsedTime = 0;

        direction = transform.position + orientation;
        previousRot = transform.rotation;
        addedRotation = previousRot * Quaternion.AngleAxis(90f, axis);
        previousPos = transform.position;

        // init move
        StartMoveBehavior();

        DoAction = DoActionMove;
    }


    public AnimationCurve stretchCube;
    public float stretchSpeed = 1f;
    private float stretchLerp = 0;
    private Vector3 stretchInitPos;

    public void SetModeStretch()
    {
        stretchInitPos = transform.localPosition;
        stretchLerp = 0;
        DoAction = DoStretchCube;

    }

    public void DoStretchCube()
    {
        stretchLerp += Time.deltaTime * stretchSpeed;

        CubeStretch(stretchCube.Evaluate(stretchLerp), stretchInitPos);

        if (stretchLerp >= 1f)
        {
            SetModeWait();
        }
    }

    public void CubeStretch(float y, Vector3 _initPos)
    {
        float x, z;
        z = x = Mathf.Sqrt(1 / y);

        transform.localScale = new Vector3(x, y, z);
        transform.localPosition = _initPos + (((y - 1) / 2) * Vector3.up);
    }

    #endregion

    #region Dash

    public override void SetModeDash()
    {
        RotationCheck();

        _elapsedTime = 0;

        direction = transform.position + orientation * 2;
        previousRot = transform.rotation;
        addedRotation = previousRot * Quaternion.AngleAxis(90f, axis);
        previousPos = transform.position;

        // init move
        StartMoveBehavior();

        DoAction = DoActionMove;
    }

    #endregion

    #endregion

    private void UpdateColor()
    {
        Color tmpColor = faceColor[0].material.color;

        switch (moveDir)
        {
            case MoveDir.down:
                faceColor[0].material.color = faceColor[4].material.color;
                faceColor[4].material.color = faceColor[1].material.color;
                faceColor[1].material.color = faceColor[3].material.color;
                faceColor[3].material.color = tmpColor;
                break;
            case MoveDir.up:
                faceColor[0].material.color = faceColor[3].material.color;
                faceColor[3].material.color = faceColor[1].material.color;
                faceColor[1].material.color = faceColor[4].material.color;
                faceColor[4].material.color = tmpColor;
                break;
            case MoveDir.left:
                faceColor[0].material.color = faceColor[2].material.color;
                faceColor[2].material.color = faceColor[1].material.color;
                faceColor[1].material.color = faceColor[5].material.color;
                faceColor[5].material.color = tmpColor;
                break;
            case MoveDir.right:
                faceColor[0].material.color = faceColor[5].material.color;
                faceColor[5].material.color = faceColor[1].material.color;
                faceColor[1].material.color = faceColor[2].material.color;
                faceColor[2].material.color = tmpColor;
                break;
        }
    }


    public override void TestTile()
    {
        // test tile d'arriver

        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.gameObject.GetComponent<CubePaint>())
            {
                CubePaint tmpCube = hit.transform.gameObject.GetComponent<CubePaint>();

                Color tmpColor = faceColor[1].GetComponent<Renderer>().material.color;

                if (tmpColor != baseColor)
                {
                    if (faceColor[1].GetComponent<Renderer>().material.color != tmpCube.GetPaintColor())
                    {
                        SetModeVoid();
                        StartCoroutine(Death());
                        Explode();
                    }
                }
                else
                {
                    faceColor[1].GetComponent<Renderer>().material.color = tmpCube.GetPaintColor();
                }
            }
            else if (hit.transform.gameObject.GetComponent<CubeCleaner>())
            {
                faceColor[1].GetComponent<Renderer>().material.color = baseColor;
            }
            else if (hit.transform.gameObject.GetComponent<CubeDash>())
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
                //transform.position = new Vector3(tmpCube.teleportDestination.transform.position.x, tmpCube.teleportDestination.transform.position.y + 1f, tmpCube.teleportDestination.transform.position.z);
            }

        }
        else
        {
            DoAction = DoActionFall;

            StartCoroutine(Death("fall"));
        }
    }

    public override bool TestWall()
    {
        Ray ray = new Ray(transform.position, orientation);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.gameObject.GetComponent<CubeWall>())
            {
                return true;
            }
            else if (hit.transform.gameObject.GetComponent<CubePush>())
            {
                CubePush tmpCube = hit.transform.gameObject.GetComponent<CubePush>();

                tmpCube.orientation = orientation;
                if (!tmpCube.TestWall())
                {
                    tmpCube.SetModeMove(tmpCube.orientation);
                }
                else return true;
            }
            
        }

        return false;
    }

    private void SplashPaint()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f))
        {

            if (faceColor[1].GetComponent<Renderer>().material.color != Color.white)
            {
                ParticleSystem.MainModule main = Splash.main;
                main.startColor = faceColor[1].GetComponent<Renderer>().material.color;

                AudioManager.instance.Play("TouchSolPaint");
                Splash.Play();

            }
            else
            {
                AudioManager.instance.Play("TouchSolNeutre");
            }
        }

    }

    private void ApplyStain()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.GetComponent<CubeStatic>() != null && hit.transform.GetComponent<Renderer>().material.color == Color.white && faceColor[1].GetComponent<Renderer>().material.color != Color.white)
            {
                hit.transform.GetComponent<CubeStatic>().ActivateStain(faceColor[1].GetComponent<Renderer>().material.color);

            }
            else if(hit.transform.GetComponent<CubePush>() != null && hit.transform.GetComponent<Renderer>().material.color == Color.white && faceColor[1].GetComponent<Renderer>().material.color != Color.white)
            {
                hit.transform.GetComponent<CubePush>().ActivateStain(faceColor[1].GetComponent<Renderer>().material.color);
            }
        }
    }

    public override void DoActionFall()
    {
        base.DoActionFall();

        if (transform.position.y <= initialPosition.y - 2) SetDeath();
    }

    private IEnumerator MobileUpAxisBehaviour()
    {
        MobileAxeVerPos = true;
        yield return new WaitForSeconds(DuréeActivationAxe);
        MobileAxeVerPos = false;
    }

    private IEnumerator MobileDownAxisBehaviour()
    {
        MobileAxeVerNeg = true;
        yield return new WaitForSeconds(DuréeActivationAxe);
        MobileAxeVerNeg = false;
    }

    private IEnumerator MobileRightAxisBehaviour()
    {
        MobileAxeHorPos = true;
        yield return new WaitForSeconds(DuréeActivationAxe);
        MobileAxeHorPos = false;
    }

    private IEnumerator MobileLeftAxisBehaviour()
    {
        MobileAxeHorNeg = true;
        yield return new WaitForSeconds(DuréeActivationAxe);
        MobileAxeHorNeg = false;
    }
}
