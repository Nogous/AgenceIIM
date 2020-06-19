using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveDirection
{
    up,
    down,
    right,
    left,
}

public enum moveEnum
{
    forward,
    backward,
    right,
    left,
    pause,
};

public class CubeMovable : Cube
{ 
    [SerializeField] protected float _moveTime = 0.2f;

    protected float _elapsedTime = 0;
    public Rewired.Player replayer;
    protected Vector3 direction;
    public Vector3 orientation = Vector3.forward;
    protected Vector3 axis = Vector3.right;

    protected Quaternion addedRotation;
    protected Vector3 previousPos;
    protected Vector3 previousOr;
    protected Quaternion previousRot;

    protected Quaternion zeroRot = new Quaternion(0, 0, 0, 0);

    protected static float diagonal = 1 * Mathf.Sqrt(2);
    protected float offset = (diagonal - 1) / 2;
    public float speed = 5;

    protected List<Vector3> vectors = new List<Vector3>();

    public GameObject teleportDestination = null;

    protected Action DoAction;

    [SerializeField] protected ParticleSystem particleDeath = null;
    public Color color = Color.white;

    protected bool isSliding;

    public override void OnAwake()
    {
        base.OnAwake();

        vectors.Add(Vector3.forward);
        vectors.Add(Vector3.back);
        vectors.Add(Vector3.left);
        vectors.Add(Vector3.right);
    }

    //Mouvement du cube
    public void MoveCube(){

    }
    /* Tests au début du déplacement
        Paramètres : 
    */
    public void StartMoveCheckTile(){
        
        
    }
    /* Tests à la fin du déplacement
        Paramètres : 
    */
    public void EndMoveCheckTile(){

    }
    /* Comportement suite au test de tuile de StartMoveCheckTile()
        Paramètres : cubeLanding (enum CubeType, cube sur lequel le bloc mouvant atteri) 
    */
    virtual public void StartMoveBehavior(){
        
    }
    /* Comportement suite au test de tuile de StartMoveCheckTile()
        Paramètres : cubeLanding (enum CubeType, cube sur lequel le bloc mouvant atteri) 
    */
    virtual public void EndMoveBehavior(bool slide = false){
        SetModeVoid();
        
        transform.eulerAngles = Vector3.zero;

        transform.position = new Vector3((int)transform.position.x, initialPosition.y, (int)transform.position.z);

        TestTile();
    }

    public virtual void SetModeVoid()
    {
        DoAction = DoActionVoid;
    }

    protected virtual void DoActionVoid()
    {
        isDashing = false;
        isSliding = false;
    }

    virtual public void SetModeMove(Vector3 vector)
    {
        if (isSliding) return;
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

    protected virtual void SetModeSlid()
    {
        isSliding = true;
        _elapsedTime = 0;
        direction = transform.position + orientation;

        previousPos = transform.position;

        StartMoveBehavior();

        DoAction = DoActionSlid;
    }
    
    public virtual void DoActionSlid()
    {
        _elapsedTime += Time.deltaTime;
        float ratio = _elapsedTime / _moveTime;
        transform.position = Vector3.Lerp(previousPos, direction, ratio);
        transform.position = new Vector3(transform.position.x, previousPos.y + Mathf.Clamp(Mathf.Sin(ratio * Mathf.PI) * offset, 0, 1), transform.position.z);

        if (_elapsedTime >= _moveTime)
        {
            EndMoveBehavior(true);
        }
    }

    virtual protected void DoActionMove()
    {
        _elapsedTime += Time.deltaTime;

        float ratio = _elapsedTime / _moveTime;

        transform.position = Vector3.Lerp(previousPos, direction, ratio);

        transform.rotation = Quaternion.Lerp(previousRot, addedRotation, ratio);

        transform.position = new Vector3(transform.position.x, previousPos.y + Mathf.Clamp(Mathf.Sin(ratio * Mathf.PI) * offset, 0, 1), transform.position.z);

        if (_elapsedTime >= _moveTime)
        {
            EndMoveBehavior();
        }
    }

    protected bool isDashing;

    virtual protected void DoActionDash()
    {
        DoActionMove();

        if (TestWall()) EndMoveBehavior();
    }

    protected void RotationCheck()
    {
        if (orientation == Vector3.forward) axis = Vector3.right;
        else if (orientation == Vector3.right) axis = Vector3.back;
        else if (orientation == Vector3.back) axis = Vector3.left;
        else axis = Vector3.forward;
    }

    virtual public void SetModeDash()
    {
        isDashing = true;
        RotationCheck();

        _elapsedTime = 0;

        direction = transform.position + orientation * 2;
        previousRot = transform.rotation;
        addedRotation = previousRot * Quaternion.AngleAxis(90f, axis);
        previousPos = transform.position;

        AudioManager.instance.Play("Dash");

        DoAction = DoActionDash;
    }

    virtual public bool TestWall()
    {
        return false;
    }

    virtual public void TestTile()
    {

    }

    virtual public void DoActionFall()
    {
        transform.position += Vector3.down * Time.deltaTime * GameManager.instance.fallSpeed;
    }

    virtual public void Explode()
    {
        ParticleSystem particles = Instantiate(particleDeath, transform.position, Quaternion.identity);

        ParticleSystem.MainModule mainMod = particles.main;

        mainMod.startColor = color;

        particles.Play();

        gameObject.SetActive(false);
        AudioManager.instance.Play("Splash");
        AudioManager.instance.Play("ExplosionCube");
    }
}


