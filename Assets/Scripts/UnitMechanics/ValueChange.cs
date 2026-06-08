using UnityEngine;
using TMPro;

public class ValueChange : MonoBehaviour
{
    [SerializeField] public TMP_Text Value;

    public void ClearText()
    {
        Value.text = "";
    }

    public void SetText(string text)
    {
        Value.text = text;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ClearText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
