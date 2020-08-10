using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAbilityPanelUp : MonoBehaviour
{
    public Animator AbilityButtonAnim;

     public void MoveAbilityPanelUpMethod()
    {
        AbilityButtonAnim.SetTrigger("GameStarted");
    }
}
