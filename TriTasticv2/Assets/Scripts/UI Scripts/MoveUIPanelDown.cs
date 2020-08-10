using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUIPanelDown : MonoBehaviour
{
    public Animator UIPanelAnim;

    public void MoveUIPanelDownOnStart()
    {
        UIPanelAnim.SetTrigger("GameStarts");
    }
}
