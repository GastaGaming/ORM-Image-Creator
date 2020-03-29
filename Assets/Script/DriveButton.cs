using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gasta;
public class DriveButton : MonoBehaviour
{
    private FileBrowser fb = null;
    private void Start()
    {
        fb = FileBrowser.Instance;
    }
    public void DriveButtonPressed()
    {
        string name = this.gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
        fb.currentPath = "";
        fb.SetPath(name);
    }
}
