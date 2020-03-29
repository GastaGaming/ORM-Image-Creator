using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
public class OrmImageCreator : MonoBehaviour
{
    public void SelectSource()
    {
        //string path = EditorUtility.OpenFilePanel("Select Source Path", "", "");
        //OpenInFileBrowser.Open(UnityEngine.Application.dataPath);
        System.Diagnostics.Process p = new System.Diagnostics.Process();
        p.StartInfo = new System.Diagnostics.ProcessStartInfo("explorer.exe");
        p.Start();
    }
}
