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
            //Debug.LogError("COuldnt access save");
            Debug.LogError(e);
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
