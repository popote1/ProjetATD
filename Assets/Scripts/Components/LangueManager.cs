using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;


public class LangueManager : MonoBehaviour
{
    public int IndexLangue;
    public List<SOLangue> SoLangues;
    public bool IsUsingSOLangue;

    public static bool IsUsingLangueS;
    public static int IndexLangueS;
    public static List<SOLangue> SoLanguesS;
    public static List<string> LangueToChose= new List<string>();
    [Header("JSONed Data")]
    [TextArea] public string JSONedData;
    public LangueData SelectedData;
    public static LangueData SelectTextData = new LangueData();
    public static UnityEvent OnChangeLangue = new UnityEvent();

    private void Awake()
    {
        IsUsingLangueS = IsUsingSOLangue;
        SoLanguesS = SoLangues;
        IndexLangueS = IndexLangue;
        if (!IsUsingSOLangue)
        {
            LangueToChose = CheckFileInLangueFolder();
            if (LangueToChose.Count > 0)
            {
                if (IndexLangue >= 0 && IndexLangue < LangueToChose.Count)
                {
                    DeSerilatzData(LangueToChose[IndexLangue]);
                    Debug.Log(" Injection de la data " + SelectedData.Tests[IndexLangue]);
                }
                else
                {
                    DeSerilatzData(LangueToChose[0]);
                    Debug.Log(" Injection de la data " + SelectedData.Tests[0]);
                }
            }
        }
    }

    public void Start()
    {
        if (IsUsingLangueS)
        {
            ChangeLangue(IndexLangueS);
        }
    }

    [ContextMenu("JSONation de la séléctedData")]
    public void SerilatizeData() {
        JSONedData = JsonUtility.ToJson(SelectedData);
       //string path =Application.dataPath+"/Langue/" + SelectedData.Tests[0]+".Lag";
       string path =Application.streamingAssetsPath +"/"+ SelectedData.Tests[0]+".Lag";// Application.persistentDataPath +"/" + SelectedData.Tests[0];
       Debug.Log(path);
       File.WriteAllText(path , JSONedData);
    }
    public void DeSerilatzData(string loadName) {
        string path = Application.streamingAssetsPath +"/"+ loadName+".Lag";
        //string path = Application.dataPath +"/Langue/" + loadName+".Lag";
        string data = File.ReadAllText(path);
        JSONedData = data;
        SelectedData =JsonUtility.FromJson<LangueData>(data);
        SelectTextData = SelectedData;
    }
    public static void DeSerilatzData(int loadIndex) {
        string path = Application.streamingAssetsPath+"/"+ LangueToChose[loadIndex]+".Lag";
        //string path = Application.dataPath +"/Langue/" + LangueToChose[loadIndex]+".Lag";
        string data = File.ReadAllText(path);
        SelectTextData  =JsonUtility.FromJson<LangueData>(data);
    }
    [ContextMenu("Met a jour Les Langues")]
    public static List<string> CheckFileInLangueFolder()
    {
        String[] files = Directory.GetFiles(Application.streamingAssetsPath);
        // String[] files = Directory.GetFiles(Application.dataPath + "/Langue/");
        List<string> stringToReturn = new List<string>();
        foreach (var file in files) {
            if (Path.GetExtension(file) == ".Lag") {
                stringToReturn.Add(Path.GetFileNameWithoutExtension(file));
                Debug.Log(Path.GetFileNameWithoutExtension(file));
            }
        }
        Debug.Log(stringToReturn.Count+" langue dans la liste");
        return stringToReturn;
    }
    [ContextMenu("Changement de Langue")]
    public void  ChangeLangue()
    {
        if (IsUsingLangueS)
        {
            SelectTextData.Tests= SoLanguesS[IndexLangue].LangueSave;
            OnChangeLangue.Invoke();
        }
        else
        {
            if (IndexLangue >= 0 && IndexLangue < LangueToChose.Count)
            {
                DeSerilatzData(LangueToChose[IndexLangue]);
                OnChangeLangue.Invoke();
            }
            else
            {
                Debug.LogError("Mauvais Index de Langue");
                IndexLangue = 0;
            }
        }
    }

    public static void ChangeLangue(int langueIndex)
    {
        if (IsUsingLangueS)
        {
           SelectTextData.Tests= SoLanguesS[langueIndex].LangueSave;
           OnChangeLangue.Invoke();
        }
        else
        {
            if (langueIndex >= 0 && langueIndex < LangueToChose.Count)
            {
                DeSerilatzData(langueIndex);
                OnChangeLangue.Invoke();
            }
            else
            {
                Debug.LogError("Mauvais Index de Langue");
            }
        }
    }

    public static List<string> GetLangues()
    {
        return LangueToChose;
    }

    public static string GetSringByID(int index) { 
        return  SelectTextData.Tests[index];
    }
}

[Serializable]
public class LangueData
{
    [TextArea]public string[] Tests = new string[300];
}
