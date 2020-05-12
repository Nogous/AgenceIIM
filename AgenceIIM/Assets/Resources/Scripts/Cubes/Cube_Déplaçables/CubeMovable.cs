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

public class CubeMovable : Cube
{
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
    virtual public void EndMoveBehavior(){

    }
    
    
}


