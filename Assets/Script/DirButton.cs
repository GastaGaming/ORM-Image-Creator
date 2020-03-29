using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gasta;
public class DirButton : MonoBehaviour
{
    private FileBrowser fb = null;
    private void Start()
    {
        fb = FileBrowser.Instance;
    }
    public void DirButtonPressed()
    {
        string name = this.gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
        fb.SetPath(fb.currentPath + "\\" + name);
    }
}
