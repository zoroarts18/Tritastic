using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLogoUp : MonoBehaviour
{
    public Animator LogoAnim;

    public void MoveTheLogoUp()
    {
        //versetzung des Logos nach oben ( wenn start gedrückt wird )

        LogoAnim.SetTrigger("GameIsPlayed");
    }

    
}
