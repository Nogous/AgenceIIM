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

    public Renderer[] faceColor = new Renderer[6];
    private Color[] initColors = new Color[6];
    public Color baseColor = Color.white;
    private MoveDir moveDir;

    private Color frontColor;

    [SerializeField] private TrailRenderer trail = null;

    [SerializeField] private ParticleSystem Splash = null;
    [SerializeField] private ParticleSystem Smoke = null;

    public static event Action<Vector3> OnMove;

    [Header("Options Axes Mobile")]
    [HideInInspector]
    public bool MobileAxeHorPos = false;
    [HideInInspector]
    public bool MobileAxeHorNeg = false;
    [HideInInspector]
    public bool MobileAxeVerPos = false;
    [HideInInspector]
    public bool MobileAxeVerNeg = false;
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

    [HideInInspector] public int nbMove = 0;

    public bool canReset = true;
    
    public override void OnAwake()
    {
        videoEnded = true;
        base.OnAwake();
        SwipeDetector.OnSwipe += ReciveSwipe;
        SetModeVoid();
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
    }

    public void resetOnMove()
    {
        if(OnMove != null)OnMove = null;
    }

    public void ResetPlayerMove()
    {
        //base.ResetCube();
        resetOnMove();

        nbMove = 0;
        GameManager.instance.txtNbCoups.text = "Coups : " + nbMove;

        StartPlayer();
        for (int i = 6; i-->0;)
        {
            faceColor[i].material.color = initColors[i];
        }
    }

    private bool spwanEnded = false;
    [HideInInspector]public bool videoEnded;

    public void SpawnLevelEnded()
    {
        spwanEnded = true;
        StartPlayer();
    }

    public void VideoTutoEnded()
    {
        videoEnded = true;
        StartPlayer();
    }

    public void StartPlayer()
    {
        if (spwanEnded && videoEnded)
        {
            SetModeWait();
        }
    }

    void OnDestroy()
    {
        SwipeDetector.OnSwipe -= ReciveSwipe;
    }

    void UpdateControlImage()
    {
        timeNoAction += Time.deltaTime;
        if (LUT_FadeControls.Evaluate(timeNoAction) >= 10)
        {
            crossUI.color = new Color(255, 255, 255, 1);
        }
        else
        {
            crossUI.color = new Color(255, 255, 255, LUT_FadeControls.Evaluate(timeNoAction));
        }
    }
    void Update()
    {
        DoAction();
        UpdateControlImage();
    }

    public override void StartMoveBehavior()
    {
        base.StartMoveBehavior();

        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.gameObject.GetComponent<CubeDestructible>())
            {
                CubeDestructible tmpCube = hit.transform.gameObject.GetComponent<CubeDestructible>();
                tmpCube.Crumble();
            }
        }

        TestEnemy();
    }

    void ReciveSwipe(SwipeData data)
    {
        if(data.Direction == SwipeDirection.Up)
        {
            Debug.Log("Swipe Up");
            ProcessMobileInputUp();
        }
        else if (data.Direction == SwipeDirection.Left)
        {
            Debug.Log("Swipe Left");
            ProcessMobileInputLeft();
        }
        else if (data.Direction == SwipeDirection.Right)
        {
            Debug.Log("Swipe Right");
            ProcessMobileInputRight();
        }
        else if (data.Direction == SwipeDirection.Down)
        {
            Debug.Log("Swipe Down");
            ProcessMobileInputDown();
        }
    }

    private void TestEnemy()
    {
        Ray ray = new Ray(transform.position, Vector3.forward);
        RaycastHit hit;

        frontColor = faceColor[4].GetComponent<Renderer>().material.color;

        // test si enemy sur le passage du joueur
        switch (moveDir)
        {
            case MoveDir.down:
                ray = new Ray(transform.position, Vector3.back);
                frontColor = faceColor[3].GetComponent<Renderer>().material.color;
                break;
            case MoveDir.right:
                ray = new Ray(transform.position, Vector3.right);
                frontColor = faceColor[2].GetComponent<Renderer>().material.color;
                break;
            case MoveDir.left:
                ray = new Ray(transform.position, Vector3.left);
                frontColor = faceColor[5].GetComponent<Renderer>().material.color;
                break;
        }

        if (Physics.Raycast(ray, out hit, 0.51f))
        {
            if (hit.transform.gameObject.GetComponent<Enemy>())
            {
                Enemy tmpCube = hit.transform.gameObject.GetComponent<Enemy>();

                if (frontColor == tmpCube.initColor)
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

    private void TestEnemyDash()
    {
        Ray ray = new Ray(transform.position, Vector3.forward);
        RaycastHit hit;

        frontColor = faceColor[4].GetComponent<Renderer>().material.color;

        // test si enemy sur le passage du joueur
        switch (moveDir)
        {
            case MoveDir.down:
                ray = new Ray(transform.position, Vector3.back);
                frontColor = faceColor[3].GetComponent<Renderer>().material.color;
                break;
            case MoveDir.right:
                ray = new Ray(transform.position, Vector3.right);
                frontColor = faceColor[2].GetComponent<Renderer>().material.color;
                break;
            case MoveDir.left:
                ray = new Ray(transform.position, Vector3.left);
                frontColor = faceColor[5].GetComponent<Renderer>().material.color;
                break;
        }

        if (Physics.Raycast(ray, out hit, 0.49f))
        {
            if (hit.transform.gameObject.GetComponent<Enemy>())
            {
                Enemy tmpCube = hit.transform.gameObject.GetComponent<Enemy>();

                if (frontColor == tmpCube.initColor)
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

    public override void EndMoveBehavior(bool slid = false)
    {
        transform.position = new Vector3((int)transform.position.x, initialPosition.y, (int)transform.position.z);

        canReset = true;

        if (trail != null)
        {
            trail.gameObject.SetActive(false);
        }

        transform.eulerAngles = Vector3.zero;
        if (!slid)
        {
            UpdateColor();
        }

        SplashPaint();

        TestTile();

        if (DoAction == DoActionDash || DoAction == DoActionFall || DoAction == DoActionSlid) return;
        else SetModeStretch();
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

        transform.localScale = Vector3.one;

        SetModeWait();
    }

    #region Actions

    #region Wait

    private void SetModeWait()
    {
        DoAction = DoActionWait;

        canReset = true;
    }

    private void DoActionWait()
    {
        #region normal
        if (!CameraHandler.instance.position)
        {
            if (GameManager.isQwerty)
            {
                if (replayer.GetAxis(RewiredConsts.Action.MoveVerticalQwerty) > 0.1f || MobileAxeVerPos)
                {
                    orientation = Vector3.forward;
                    moveDir = MoveDir.up;
                    timeNoAction = 0.0f;
                    ApplyStain();

                    if (!TestWall()) SetModeMove(Vector3.zero);

                    else return;
                }
                else if (replayer.GetAxis(RewiredConsts.Action.MoveVerticalQwerty) < -0.1f || MobileAxeVerNeg)
                {
                    orientation = Vector3.back;
                    moveDir = MoveDir.down;
                    timeNoAction = 0.0f;
                    ApplyStain();

                    if (!TestWall()) SetModeMove(Vector3.zero);

                    else return;
                }
                else if (replayer.GetAxis(RewiredConsts.Action.MoveHorizontalQwerty) > 0.1f || MobileAxeHorPos)
                {
                    orientation = Vector3.right;
                    moveDir = MoveDir.right;
                    timeNoAction = 0.0f;
                    ApplyStain();

                    if (!TestWall()) SetModeMove(Vector3.zero);

                    else return;
                }
                else if (replayer.GetAxis(RewiredConsts.Action.MoveHorizontalQwerty) < -0.1f || MobileAxeHorNeg)
                {
                    orientation = Vector3.left;
                    moveDir = MoveDir.left;
                    timeNoAction = 0.0f;
                    ApplyStain();

                    if (!TestWall()) SetModeMove(Vector3.zero);

                    else return;
                }
            }
            else
            {
                if (replayer.GetAxis(RewiredConsts.Action.MoveVert) > 0.1f || MobileAxeVerPos)
                {
                    orientation = Vector3.forward;
                    moveDir = MoveDir.up;
                    timeNoAction = 0.0f;
                    ApplyStain();

                    if (!TestWall()) SetModeMove(Vector3.zero);

                    else return;
                }
                else if (replayer.GetAxis(RewiredConsts.Action.MoveVert) < -0.1f || MobileAxeVerNeg)
                {
                    orientation = Vector3.back;
                    moveDir = MoveDir.down;
                    timeNoAction = 0.0f;
                    ApplyStain();

                    if (!TestWall()) SetModeMove(Vector3.zero);

                    else return;

                }
                else if (replayer.GetAxis(RewiredConsts.Action.MoveHor) > 0.1f || MobileAxeHorPos)
                {
                    orientation = Vector3.right;
                    moveDir = MoveDir.right;
                    timeNoAction = 0.0f;
                    ApplyStain();

                    if (!TestWall()) SetModeMove(Vector3.zero);

                    else return;
                }
                else if (replayer.GetAxis(RewiredConsts.Action.MoveHor) < -0.1f || MobileAxeHorNeg)
                {
                    orientation = Vector3.left;
                    moveDir = MoveDir.left;
                    timeNoAction = 0.0f;
                    ApplyStain();

                    if (!TestWall()) SetModeMove(Vector3.zero);

                    else return;
                }
            }
        }
        #endregion
        #region inverted
        else
        {
            if (GameManager.isQwerty)
            {
                if (replayer.GetAxis(RewiredConsts.Action.MoveVerticalQwerty) * -1 > 0.1f || MobileAxeVerNeg)
                {
                    orientation = Vector3.forward;
                    moveDir = MoveDir.up;
                    timeNoAction = 0.0f;
                    ApplyStain();

                    if (!TestWall()) SetModeMove(Vector3.zero);

                    else return;
                }
                else if (replayer.GetAxis(RewiredConsts.Action.MoveVerticalQwerty) * -1 < -0.1f || MobileAxeVerPos)
                {
                    orientation = Vector3.back;
                    moveDir = MoveDir.down;
                    timeNoAction = 0.0f;
                    ApplyStain();

                    if (!TestWall()) SetModeMove(Vector3.zero);

                    else return;
                }
                else if (replayer.GetAxis(RewiredConsts.Action.MoveHorizontalQwerty) * -1 > 0.1f || MobileAxeHorNeg)
                {
                    orientation = Vector3.right;
                    moveDir = MoveDir.right;
                    timeNoAction = 0.0f;
                    ApplyStain();

                    if (!TestWall()) SetModeMove(Vector3.zero);

                    else return;
                }
                else if (replayer.GetAxis(RewiredConsts.Action.MoveHorizontalQwerty) * -1 < -0.1f || MobileAxeHorPos)
                {
                    orientation = Vector3.left;
                    moveDir = MoveDir.left;
                    timeNoAction = 0.0f;
                    ApplyStain();

                    if (!TestWall()) SetModeMove(Vector3.zero);

                    else return;
                }
            }
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
                }
                else if (replayer.GetAxis(RewiredConsts.Action.MoveVert) * -1 < -0.1f || MobileAxeVerPos)
                {
                    orientation = Vector3.back;
                    moveDir = MoveDir.down;
                    timeNoAction = 0.0f;
                    ApplyStain();

                    if (!TestWall()) SetModeMove(Vector3.zero);

                    else return;
                }
                else if (replayer.GetAxis(RewiredConsts.Action.MoveHor) * -1 > 0.1f || MobileAxeHorNeg)
                {
                    orientation = Vector3.right;
                    moveDir = MoveDir.right;
                    timeNoAction = 0.0f;
                    ApplyStain();

                    if (!TestWall()) SetModeMove(Vector3.zero);

                    else return;
                }
                else if (replayer.GetAxis(RewiredConsts.Action.MoveHor) * -1 < -0.1f || MobileAxeHorPos)
                {
                    orientation = Vector3.left;
                    moveDir = MoveDir.left;
                    timeNoAction = 0.0f;
                    ApplyStain();

                    if (!TestWall()) SetModeMove(Vector3.zero);

                    else return;
                }
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

    public bool isDeaing = false;

    private IEnumerator Death(string deathInfo = null)
    {
        isDeaing = true;

        if (deathInfo == "fall")
        {
            yield return new WaitForSeconds(GameManager.instance.fallDuration);
            SetModeVoid();
        }

        SetModeVoid();

        CameraHandler.instance.StartCoroutine(CameraHandler.instance.Shake(TimeShakePlayer, MagnShakePlayer));
        
        AudioManager.instance.Play("Death");

        GameManager.instance.ResetParty(2f);
        gameObject.SetActive(false);

        isDeaing = false;
    }

    #region Move

    public override void SetModeMove(Vector3 vector)
    {
        RaycastHit hit;

        //Debug.DrawLine((transform.position + orientation), (transform.position + orientation) + Vector3.down, Color.blue, 10f);

        if (!Physics.Raycast((transform.position + orientation), Vector3.down, out hit, 1f))
        {
            return;
        }

        canReset = false;

        OnMove?.Invoke(orientation);

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

    protected override void DoActionMove()
    {
        base.DoActionMove();
    }

    protected override void DoActionDash()
    {
        base.DoActionDash();

        TestEnemyDash();
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
        if (trail != null)
        {
            trail.gameObject.SetActive(true);
        }
        canReset = false;

        RotationCheck();

        _elapsedTime = 0;

        direction = transform.position + orientation * 2;
        previousRot = transform.rotation;
        addedRotation = previousRot * Quaternion.AngleAxis(90f, axis);
        previousPos = transform.position;

        AudioManager.instance.Play("Dash");

        // init move
        StartMoveBehavior();

        DoAction = DoActionDash;
    }

    #endregion

    protected override void SetModeSlid()
    {
        canReset = false;

        base.SetModeSlid();
    }

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


    public override void TestTile(bool isTNT = false)
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if(faceColor[1].GetComponent<Renderer>().material.color == baseColor && !hit.transform.gameObject.GetComponent<CubePaint>())
            {
                Smoke.Play();
            }

            if (hit.transform.gameObject.GetComponent<CubePaint>())
            {
                CubePaint tmpCube = hit.transform.gameObject.GetComponent<CubePaint>();

                Color tmpColor = faceColor[1].GetComponent<Renderer>().material.color;

                if(tmpCube.colorPotencial > 0)
                {
                    if (tmpColor != baseColor)
                    {
                        if (faceColor[1].GetComponent<Renderer>().material.color != tmpCube.GetPaintColor())
                        {
                            SetModeVoid();
                            StartCoroutine(Death());
                            Explode();
                            return;
                        }
                    }
                    else
                    {
                        faceColor[1].GetComponent<Renderer>().material.color = tmpCube.GetPaintColor();
                        //reduce Color
                        /*tmpCube.colorPotencial -= 25;
                        tmpCube.GetComponent<Renderer>().material.color = Color.Lerp(tmpCube.GetComponent<Renderer>().material.color, Color.white, 1 - (float)tmpCube.colorPotencial / 100);*/
                        //Hide Color

                        tmpCube.colorPotencial -= tmpCube.colorLost;
                        tmpCube.mask.transform.localScale = Vector3.Lerp(tmpCube.initalScale, Vector3.zero, 1 - (float)tmpCube.colorPotencial / 100);
                    }
                }
                if(!isTNT)
                nbMove++;
                SetModeWait();
            }
            else if (hit.transform.gameObject.GetComponent<CubeCleaner>())
            {
                CubeCleaner tmpCleaner = hit.transform.gameObject.GetComponent<CubeCleaner>();
                tmpCleaner.Clean(this);

                if (!isTNT)
                    nbMove++;

                SetModeWait();
            }
            else if (hit.transform.gameObject.GetComponent<CubeDash>())
            {
                CubeDash tmpCube = hit.transform.gameObject.GetComponent<CubeDash>();
                AudioManager.instance.Play("Bloc Glissant");
                if ((int)tmpCube.dashOrientation == 0)
                {
                    orientation = Vector3.forward;
                    moveDir = MoveDir.up;
                }
                else if ((int)tmpCube.dashOrientation == 1)
                {
                    orientation = Vector3.back;
                    moveDir = MoveDir.down;
                }
                else if ((int)tmpCube.dashOrientation == 2)
                {
                    orientation = Vector3.right;
                    moveDir = MoveDir.right;
                }
                else
                {
                    orientation = Vector3.left;
                    moveDir = MoveDir.left;
                }

                SetModeDash();
            }
            else if (hit.transform.gameObject.GetComponent<CubeDetonator>())
            {
                CubeDetonator tmpDetonator = hit.transform.gameObject.GetComponent<CubeDetonator>();
                tmpDetonator.ActivateTnt();

                SetModeWait();
                if (!isTNT)
                    nbMove++;
            }
            else if (hit.transform.gameObject.GetComponent<CubeTeleporter>())
            {
                CubeTeleporter tmpTeleporter = hit.transform.gameObject.GetComponent<CubeTeleporter>();
                tmpTeleporter.TeleportPlayer(this);

                SetModeWait();
                if (!isTNT)
                    nbMove++;
            }
            else if (hit.transform.gameObject.GetComponent<CubeSlid>())
            {
                if (!TestWall()) SetModeSlid();
                else
                {
                    SetModeWait();
                    if (!isTNT)
                        nbMove++;
                }
            }
            else
            {
                SetModeWait();
                if (!isTNT)
                    nbMove++;
            }
        }
        else
        {
            DoAction = DoActionFall;

            StartCoroutine(Death("fall"));
        }

        // test tile d'arriver
        GameManager.instance.txtNbCoups.text = "Coups : " + nbMove;
    }

    public override bool TestWall()
    {
        Ray ray = new Ray(transform.position, orientation);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 0.51f))
        {
            if (hit.transform.gameObject.GetComponent<CubeStatic>())
            {
                return true;
            }
            else if (hit.transform.gameObject.GetComponent<CubePush>())
            {
                CubePush tmpCube = hit.transform.gameObject.GetComponent<CubePush>();

                if (!tmpCube.isMoving)
                {
                    tmpCube.orientation = orientation.normalized;

                    if (!tmpCube.TestWall())
                    {
                        tmpCube.SetModeMove(tmpCube.orientation);
                    }
                    else return true;
                }

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
    
    public void ProcessMobileInputLeft()
    {
        StartCoroutine(MobileLeftAxisBehaviour());
    }

    public void ProcessMobileInputRight()
    {
        StartCoroutine(MobileRightAxisBehaviour());
    }

    public void ProcessMobileInputDown()
    {
        StartCoroutine(MobileDownAxisBehaviour());
    }

    public void ProcessMobileInputUp()
    {
        StartCoroutine(MobileUpAxisBehaviour());
    }

    public IEnumerator MobileUpAxisBehaviour()
    {
        MobileAxeVerPos = true;
        yield return new WaitForSeconds(DuréeActivationAxe);
        MobileAxeVerPos = false;
    }

    public IEnumerator MobileDownAxisBehaviour()
    {
        MobileAxeVerNeg = true;
        yield return new WaitForSeconds(DuréeActivationAxe);
        MobileAxeVerNeg = false;
    }

    public IEnumerator MobileRightAxisBehaviour()
    {
        MobileAxeHorPos = true;
        yield return new WaitForSeconds(DuréeActivationAxe);
        MobileAxeHorPos = false;
    }

    public IEnumerator MobileLeftAxisBehaviour()
    {
        MobileAxeHorNeg = true;
        yield return new WaitForSeconds(DuréeActivationAxe);
        MobileAxeHorNeg = false;
    }
}
