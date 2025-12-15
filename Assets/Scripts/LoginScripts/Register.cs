using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class Register : MonoBehaviour, IPointerClickHandler
{
    private GameObject Auth;
    public TMP_Text text;
    AuthManager authManager;
    void Start()
    {
        Auth = GameObject.FindGameObjectWithTag("Auth");
        authManager = Auth.GetComponent<AuthManager>();
    }
    void Reset()
    {
        if (text == null) text = GetComponent<TMP_Text>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, null);
        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = text.textInfo.linkInfo[linkIndex];
            string id = linkInfo.GetLinkID();
            if (id == "register")
            {
                authManager.ShowSig();
            }
            if (id == "login")
            {
                authManager.ShowLogin();
            }
        }
    }
}
