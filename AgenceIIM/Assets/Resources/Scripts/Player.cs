using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveDir
{
    up,
    down,
    right,
    left,
}

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]

    [SerializeField] private float _moveTime = 0.2f;

    private float _elapsedTime = 0;
    public Rewired.Player replayer;
    private Vector3 direction;
    public Vector3 orientation = Vector3.forward;
    private Vector3 axis = Vector3.right;

    private Quaternion addedRotation;
    private Vector3 previousPos;
    private Vector3 previousOr;
    private Quaternion previousRot;

    private Quaternion zeroRot = new Quaternion(0, 0, 0, 0);

    private static float diagonal = 1 * Mathf.Sqrt(2);
    private float offset = (diagonal - 1) / 2;
    public float speed = 5;

    private Action DoAction;
    [SerializeField] private GameObject Cube = null;

    private Quaternion newCubeRot;

    [Header("Color Settings")]

    [SerializeField] private Renderer[] faceColor = new Renderer[6];
    private Color[] initColors = new Color[6];
    [SerializeField] private Color baseColor = Color.white;
    private MoveDir moveDir;

    [SerializeField] private TrailRenderer trail = null;

    [SerializeField] private ParticleSystem Splash = null;

    public static event Action<Vector3> OnMove;

    [SerializeField] private ParticleSystem particleDeath = null;

    [Header("Options Axes Mobile")]
    bool MobileAxeHorPos = false;
    bool MobileAxeHorNeg = false;
    bool MobileAxeVerPos = false;
    bool MobileAxeVerNeg = false;
    public float DuréeActivationAxe = 0.01f;

    [Header("Variables ScreenShake")]
    public float TimeShakeEnnemy;
    public float MagnShakeEnnemy;
    public float TimeShakePlayer;
    public float MagnShakePlayer;

    void Awake()
    {
        
        SwipeDetector.OnSwipe += ProcessMobileInput;     
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.player == null)
        {
        GameManager.instance.player = this;
        }

        // initialisation depart
        for (int i = faceColor.Length; i-- > 0;)
        {
            initColors[i] = faceColor[i].material.color;
        }

        // start move
        SetModeWait();
    }

    void Update()
    {
        DoAction();

       
    }

    private void ProcessMobileInput(SwipeData data) {
        if (data.Direction == SwipeDirection.Up)
        {
            StartCoroutine(MobileUpAxisBehaviour());
        } else if (data.Direction == SwipeDirection.Down)
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
        Cube.SetActive(true);

        SetModeWait();
    }

    #region Actions

    private void DoActionNull()
    {

    }

    public void SetModeNull()
    {
        DoAction = DoActionNull;
    }

    #region Wait

    private void SetModeWait()
    {
        DoAction = DoActionWait;
        trail.gameObject.SetActive(true);
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

                ApplyStain();

                if (!TestWall()) SetModeMove();

                else return;

                OnMove?.Invoke(orientation);
            }
            else if (replayer.GetAxis(RewiredConsts.Action.MoveVert) < -0.1f || MobileAxeVerNeg)
            {
                orientation = Vector3.back;
                moveDir = MoveDir.down;

                ApplyStain();

                if (!TestWall()) SetModeMove();

                else return;

                OnMove?.Invoke(orientation);
            }
            else if (replayer.GetAxis(RewiredConsts.Action.MoveHor) > 0.1f || MobileAxeHorPos)
            {
                orientation = Vector3.right;
                moveDir = MoveDir.right;

                ApplyStain();

                if (!TestWall()) SetModeMove();

                else return;

                OnMove?.Invoke(orientation);
            }
            else if (replayer.GetAxis(RewiredConsts.Action.MoveHor) < -0.1f || MobileAxeHorNeg)
            {
                orientation = Vector3.left;
                moveDir = MoveDir.left;

                ApplyStain();

                if (!TestWall()) SetModeMove();

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

                ApplyStain();

                if(!TestWall())SetModeMove();

                else return;

                OnMove?.Invoke(orientation);
            }
            else if (replayer.GetAxis(RewiredConsts.Action.MoveVert) * -1 < -0.1f || MobileAxeVerPos)
            {
                orientation = Vector3.back;
                moveDir = MoveDir.down;

                ApplyStain();

                if (!TestWall()) SetModeMove();

                else return;

                OnMove?.Invoke(orientation);
            }
            else if (replayer.GetAxis(RewiredConsts.Action.MoveHor) * -1 > 0.1f || MobileAxeHorNeg)
            {
                orientation = Vector3.right;
                moveDir = MoveDir.right;

                ApplyStain();

                if (!TestWall()) SetModeMove();

                else return;

                OnMove?.Invoke(orientation);
            }
            else if (replayer.GetAxis(RewiredConsts.Action.MoveHor) * -1 < -0.1f || MobileAxeHorPos)
            {
                orientation = Vector3.left;
                moveDir = MoveDir.left;

                ApplyStain();

                if (!TestWall()) SetModeMove();

                else return;

                OnMove?.Invoke(orientation);
            }
        }
        #endregion
    }

    #endregion

    #region Fall

    private void DoActionFall()
    {
        transform.position += Vector3.down * Time.deltaTime * GameManager.instance.fallSpeed;
    }

    #endregion

    public void SetDeath()
    {  
        SetModeNull();
        StartCoroutine(Death());
        DeathSplash();
    }

    private void DeathSplash()
    {
        Color color = Color.white;

        List<Color> colors = new List<Color>();

        for (int i = 0; i < faceColor.Length; i++)
        {
            if(faceColor[i].material.color != Color.white)
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
        }

        SetModeNull();

        Cube.SetActive(false);

        if (gameObject.GetComponent<Cube>())
        {
            CameraHandler.instance.StartCoroutine(CameraHandler.instance.Shake(TimeShakePlayer, MagnShakePlayer));
            gameObject.GetComponent<Cube>().Explode(true);

            yield return new WaitForSeconds(2f);
        }

        GameManager.instance.ResetParty();
    }

    #region Move

    private void SetModeMove()
    {
        RotationCheck();

        _elapsedTime = 0;

        direction = transform.position + orientation;
        previousRot = Cube.transform.rotation;
        addedRotation = previousRot * Quaternion.AngleAxis(90f, axis);
        previousPos = transform.position;

        // init move
        LeaveTile();

        DoAction = DoActionMove;
    }

    private void DoActionMove()
    {
        _elapsedTime += Time.deltaTime;

        float ratio = _elapsedTime / _moveTime;

        transform.position = Vector3.Lerp(previousPos, direction, ratio);

        Cube.transform.rotation = Quaternion.Lerp(previousRot, addedRotation, ratio);

        Cube.transform.position = new Vector3(Cube.transform.position.x, previousPos.y + Mathf.Clamp(Mathf.Sin(ratio * Mathf.PI) * offset, 0, 1), Cube.transform.position.z);

        if (_elapsedTime >= _moveTime)
        {
            // end move
            //SetModeWait();

            SetModeStretch();

            Cube.transform.eulerAngles = Vector3.zero;
            UpdateColor();

            SplashPaint();

            TestTile();
        }
    }

    public AnimationCurve stretchCube;
    public float stretchSpeed = 1f;
    private float stretchLerp = 0;
    private Vector3 stretchInitPos;

    public void SetModeStretch()
    {
        stretchInitPos = Cube.transform.localPosition;
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

        Cube.transform.localScale = new Vector3(x, y, z);
        Cube.transform.localPosition = _initPos + (((y - 1) / 2) * Vector3.up);
    }

    private void RotationCheck()
    {
        if (orientation == Vector3.forward) axis = Vector3.right;
        else if (orientation == Vector3.right) axis = Vector3.back;
        else if (orientation == Vector3.back) axis = Vector3.left;
        else axis = Vector3.forward;
    }

    #endregion

    #region Dash

    private void SetModeDash()
    {
        RotationCheck();

        _elapsedTime = 0;

        direction = transform.position + orientation*2;
        previousRot = Cube.transform.rotation;
        addedRotation = previousRot * Quaternion.AngleAxis(90f, axis);
        previousPos = transform.position;

        // init move
        LeaveTile();

        DoAction = DoActionDash;
    }

    private void DoActionDash()
    {
        _elapsedTime += Time.deltaTime;

        float ratio = _elapsedTime / _moveTime;

        transform.position = Vector3.Lerp(previousPos, direction, ratio);

        Cube.transform.rotation = Quaternion.Lerp(previousRot, addedRotation, ratio);

        Cube.transform.position = new Vector3(Cube.transform.position.x, previousPos.y + Mathf.Clamp(Mathf.Sin(ratio * Mathf.PI) * offset, 0, 1), Cube.transform.position.z);

        if (_elapsedTime >= _moveTime)
        {
            // end move
            SetModeWait();

            Cube.transform.eulerAngles = Vector3.zero;
            UpdateColor();

            SplashPaint();

            TestTile();
        }
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

    private void LeaveTile()
    {
        // test destruction de la tile quiter par le joueur
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        Color tmpColor;

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.gameObject.GetComponent<Cube>())
            {
                Cube tmpCube = hit.transform.gameObject.GetComponent<Cube>();
                tmpCube.Explode();

                tmpColor = tmpCube.GetColor();
                if (tmpColor != Color.white)
                {

                    if (baseColor == faceColor[1].GetComponent<Renderer>().material.color)
                    {
                        faceColor[1].GetComponent<Renderer>().material.color = tmpColor;
                    }
                }
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
            if (hit.transform.gameObject.GetComponent<Cube>())
            {
                Cube tmpCube = hit.transform.gameObject.GetComponent<Cube>();
                if (tmpCube.isEnemy)
                {
                    if (tmpColor == tmpCube.enemyColor)
                    {
                       CameraHandler.instance.StartCoroutine(CameraHandler.instance.Shake(TimeShakeEnnemy, MagnShakeEnnemy));
                        tmpCube.Explode();
                    }
                    else
                    {
                        SetModeNull();
                        StartCoroutine(Death());
                        DeathSplash();
                    }
                }
            }
        }
    }

    private void TestTile()
    {
        // test tile d'arriver

        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.gameObject.GetComponent<Cube>())
            {
                Cube tmpCube = hit.transform.gameObject.GetComponent<Cube>();

                if (tmpCube.colorPotencial > 0)
                {

                    Debug.Log("Color block");

                    Color tmpColor = faceColor[1].GetComponent<Renderer>().material.color;

                    if (tmpColor != baseColor)
                    {
                        if (faceColor[1].GetComponent<Renderer>().material.color != hit.transform.gameObject.GetComponent<Renderer>().material.color)
                        {
                            SetModeNull();
                            StartCoroutine(Death());
                            DeathSplash();
                        }
                    }
                    else
                    {
                        faceColor[1].GetComponent<Renderer>().material.color = hit.transform.gameObject.GetComponent<Renderer>().material.color;
                    }
                }
                else if (tmpCube.isCleaningBox)
                {
                    faceColor[1].GetComponent<Renderer>().material.color = baseColor;
                }

                else if (tmpCube.isDashBox)
                {

                    if((int)tmpCube.dashOrientation == 0)
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
                else if (tmpCube.isTrigger)
                {
                    tmpCube.ActivateTnt();
                }

            }

        }
        else
        {
            DoAction = DoActionFall;

            StartCoroutine(Death("fall"));
        }
    }

    private bool TestWall()
    {
        Ray ray = new Ray(transform.position, orientation);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.gameObject.GetComponent<Cube>())
            {
                Cube tmpCube = hit.transform.gameObject.GetComponent<Cube>();

                if (tmpCube.isWall) return true;

                else if (tmpCube.isPushBlock)
                {
                    tmpCube.orientation = orientation;
                    if (!tmpCube.TestWall())
                    {
                        tmpCube.SetModePush(tmpCube.orientation);
                    }
                    else return true;
                }
            }
        }

        return false;
    }

    private void SplashPaint()
    {
        Ray ray = new Ray(Cube.transform.position, Vector3.down);
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
        Ray ray = new Ray(Cube.transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.GetComponent<Cube>() != null && hit.transform.GetComponent<Renderer>().material.color == Color.white && faceColor[1].GetComponent<Renderer>().material.color != Color.white)
            {
                hit.transform.GetComponent<Cube>().ActivateStain(faceColor[1].GetComponent<Renderer>().material.color);

            }
        }
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
