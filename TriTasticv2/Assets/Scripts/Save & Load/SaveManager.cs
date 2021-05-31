using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour
{
    private static PlayerProfile playerProfile  = null;

    public static void Save()
    {
        if (playerProfile == null)
        {
            Debug.LogWarning("PlayerProfile was not loaded, will not be saved");
            return;
        }
        try
        {
            string path = Application.persistentDataPath + "/profile.dt";
            if(File.Exists(path))
            {
                File.Delete(path);
            }
            FileStream file = File.Create(path);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, playerProfile);
            file.Close();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    public static void ResetData()
    {
        if(playerProfile != null)
        {
            //General
            playerProfile.Tricoins = 0;
            playerProfile.Quality = 1;
            playerProfile.GameMode = 0;
            playerProfile.savedCurrentSkin = Skin.Triangle;
            playerProfile.currentBg = Background.SolidRed;

            //------Arcade-------
            playerProfile.HighScoreArcade = 0;
            playerProfile.PowerUpsCollected = 0;
            playerProfile.ArcadeMatchesPlayed = 0;
            playerProfile.ArcadeBlocksAvoided = 0;

            //------Rings---------
            playerProfile.HighScoreRings = 0;
            playerProfile.RingsMatchesPlayed = 0;
            playerProfile.RingsCatched = 0;

            //------Shoot---------
            playerProfile.HighScoreShoot = 0;
            playerProfile.ShootMatchesPlayed = 0;
            playerProfile.ShootKills = 0;

            //------Upgrades-------
            playerProfile.ShootUpgrade = 0;
            playerProfile.BoostUpgrade = 0;
            playerProfile.TricoinsUpgrade = 0;

            playerProfile.ShootUpgradeCount = 0;
            playerProfile.BoostUpgradeCount = 0;
            playerProfile.TricoinsUpgradeCount = 0;

            //------Skins bought-----
            playerProfile.skinPurchased[Skin.Triangle] = true;

            playerProfile.skinPurchased[Skin.Jet] = false;
            playerProfile.skinPurchased[Skin.Helicopter] = false;
            playerProfile.skinPurchased[Skin.Motorrad] = false;
            playerProfile.skinPurchased[Skin.Shuriken] = false;
            playerProfile.skinPurchased[Skin.Easter] = false;
            playerProfile.skinPurchased[Skin.Halloween] = false;
            playerProfile.skinPurchased[Skin.Christmas] = false;
            playerProfile.skinPurchased[Skin.NewYear] = false;
        }
    }

    public static PlayerProfile Load()
    {
        if (playerProfile != null)
        {
            return playerProfile;
        }
        try
        {
            string path = Application.persistentDataPath + "/profile.dt";
            if (File.Exists(path))
            {
                FileStream file = File.Open(path, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                playerProfile = (PlayerProfile)bf.Deserialize(file);
                file.Close();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            playerProfile = null;
        }

        if (playerProfile == null)
            playerProfile = new PlayerProfile();

        return playerProfile;
    }
}
