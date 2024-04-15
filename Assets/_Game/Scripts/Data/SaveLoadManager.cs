using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using UnityEngine;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    private UserData userData;

    private string saveFileName = Constant.SAVE_FILE_NAME;
    private string saveKey = Constant.SAVE_KEY;
    private string saveFilePath; 

    [SerializeField] private bool loadOnStart = true;
    private BinaryFormatter formatter;

    public UserData UserData { get => userData; set => userData = value; }

    public void OnInit()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, saveFileName);
        formatter = new BinaryFormatter();
        if (loadOnStart)
        {
            Load();
        }
    }

    public void Load1()
    {

        try
        {
            FileStream file = new FileStream(saveFilePath, FileMode.Open, FileAccess.Read);
            try
            {
                userData = (UserData)formatter.Deserialize(file);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                file.Close();
                Save();
            }
            file.Close();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Save();
        }
    }

    public void Save1()
    {
        if (userData == null)
        {
            userData = new UserData();
        }
        try
        {
            FileStream file = new FileStream(saveFilePath, FileMode.OpenOrCreate, FileAccess.Write);
            formatter.Serialize(file, userData);
            file.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void Save()
    {
        if (userData == null)
        {
            userData = new UserData();
        }
        try
        {
            string key = Generate256BitKey();
            string keyfilepatch = Path.Combine(Application.persistentDataPath, saveKey);
            string filedatapatch = Path.Combine(Application.persistentDataPath, saveFileName);

            string json = JsonUtility.ToJson(userData);
            
            File.WriteAllText(keyfilepatch, key);
            File.WriteAllBytes(filedatapatch, EncryptStringToBytes_Aes(json, key));
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void Load()
    {

        try
        {
            string keyfilepatch = Path.Combine(Application.persistentDataPath, saveKey);
            string key = File.ReadAllText(keyfilepatch);

            string datafilepatch = Path.Combine(Application.persistentDataPath, saveFileName);
            byte[] data = File.ReadAllBytes(datafilepatch);
                
            string jsonfromfile = DecryptStringFromBytes_Aes(data,key);
            userData = JsonUtility.FromJson<UserData>(jsonfromfile);
        }
        catch
        {
            Save();
        }
    }
    private static string Generate256BitKey()
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.KeySize = 256; // Set the key size to 256 bits
            aesAlg.GenerateKey(); // Generate a random 256-bit key
            return Convert.ToBase64String(aesAlg.Key);
        }
    }

    private static byte[] EncryptStringToBytes_Aes(string plainText, string key)
    {
        byte[] encrypted;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Convert.FromBase64String(key);
            aesAlg.IV = new byte[aesAlg.BlockSize / 8]; // IV should be the same size as the block size
            aesAlg.Mode = CipherMode.CBC; // Set the mode to CBC
            aesAlg.Padding = PaddingMode.PKCS7; // Use PKCS7 padding

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }
                encrypted = msEncrypt.ToArray();
            }
        }

        return encrypted;
    }

    private static string DecryptStringFromBytes_Aes(byte[] cipherText, string key)
    {
        string plaintext = null;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Convert.FromBase64String(key);
            aesAlg.IV = new byte[aesAlg.BlockSize / 8]; // IV should be the same size as the block size
            aesAlg.Mode = CipherMode.CBC; // Set the mode to CBC
            aesAlg.Padding = PaddingMode.PKCS7; // Use PKCS7 padding

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
            {
                plaintext = srDecrypt.ReadToEnd();
            }
        }
        return plaintext;
    }
}
