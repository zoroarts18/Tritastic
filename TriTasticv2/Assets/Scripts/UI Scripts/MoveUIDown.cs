using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUIDown : MonoBehaviour
{
    public Animator PanelAnim;


    public void MoveTheUIDown()
    {
        PanelAnim.SetTrigger("GameIsPlayed2");
    }
}
