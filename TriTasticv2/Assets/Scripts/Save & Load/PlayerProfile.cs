using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProfile
{
    public PlayerProfile()
    {
        skinPurchased = new Dictionary<Skin, bool>();
    
        // Enum.GetValues returns list of all possible values
        foreach(Skin s in Enum.GetValues(typeof(Skin)))
        {
            skinPurchased[s] = false;
        }
        skinPurchased[Skin.Triangle] = true;
        
    }
    public int Tricoins;

    public int HighScoreArcade;
    public int HighScoreRings;
    public int HighScoreShoot;

    //------Arcade-------
    public int PowerUpsCollected;
    public int ArcadeMatchesPlayed;
    public int ArcadeBlocksAvoided;
    //------Rings---------
    public int RingsMatchesPlayed;
    public int RingsCatched;
    //------Shoot---------
    public int ShootMatchesPlayed;
    public int ShootKills;

    public int Quality;
    public int GameMode;
    public Skin savedCurrentSkin;

    public Background currentBg;
    //public Skin currentSkin; // Check: z.B. if (currentSkin == Skin.Triangle)
    public Dictionary<Skin, bool> skinPurchased; // z.B. skinPurchased[Skin.Batman] = true
}

public enum Skin : int
{
    Triangle = 0,
    Jet = 1,
    Batman = 2,
    Shuriken = 3,
    Easter = 4,
    Halloween = 5,
    Christmas = 6,
    NewYear = 7,
}

public enum Background : int
{
    SolidRed = 0,
    SolidGreen = 1,
    SolidBlue = 2,
    SolidGrey = 3,
}

