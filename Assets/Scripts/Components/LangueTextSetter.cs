using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LangueTextSetter : MonoBehaviour
{
    private Text _text;
    private TMP_Text _yMPText;
    public int TextIndex;
    public bool IsUpdateOnLangueChange;

    public void Start() 
    {
        OnEnable();
        if (IsUpdateOnLangueChange)
        {
            LangueManager.OnChangeLangue.AddListener(OnEnable);
        }
    }

    public void OnEnable()
    {
        if (GetComponent<Text>() != null) _text = GetComponent<Text>();
        if (GetComponent<TMP_Text>()!=null) _yMPText = GetComponent<TMP_Text>();
        if(_text!=null)_text.text = LangueManager.GetSringByID(TextIndex);
        if (_yMPText != null) _yMPText.text = LangueManager.GetSringByID(TextIndex);
    }
    [ContextMenu("Donner le text de l'index")]
    public void TestIndex()
    {
        Debug.Log(LangueManager.GetSringByID(TextIndex));
    }
}
