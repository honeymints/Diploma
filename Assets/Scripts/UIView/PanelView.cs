using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelView : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text panelText;

    public void ShowPanel(string text)
    {
        panel.SetActive(true);
        panelText.text = text;
    }

    public void HidePanel()
    {
        panel.SetActive(false);
    }
}
