using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
namespace Gasta
{
    public class FileBrowser : MonoBehaviour
    {
        public static FileBrowser Instance { get; private set; }

        // Start is called before the first frame update
        public GameObject goDirPath;
        public GameObject goScrollAreaContent;
        public GameObject goScrollAreaDrives;
        public GameObject goImageIconPreFab;
        public GameObject goDirIconPreFab;
        public GameObject goDriveIconPreFab;
        public string currentPath = null;
        private List<string> pathHistory = new List<string>();
        private int historyLocation = 0;
        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (Transform child in goScrollAreaDrives.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (DriveInfo d in allDrives)
            {
                var folderIcon = Instantiate(goDriveIconPreFab, goScrollAreaDrives.transform);
                folderIcon.GetComponentInChildren<TMPro.TMP_Text>().text = "";
                folderIcon.GetComponentInChildren<TMPro.TMP_Text>().text = d.VolumeLabel;
                //folderIcon.GetComponentInChildren<TMPro.TMP_Text>().text = d.DriveType + " : " + d.VolumeLabel;
            }
        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        
        }
        public void SetPath(string path)
        {
            currentPath = path;
            goDirPath.GetComponent<TMPro.TMP_InputField>().text = currentPath;
            FillScrollContent();
        }
        public void FillScrollContent()
        {
            ClearScrollContent();
            string path = goDirPath.GetComponent<TMPro.TMP_InputField>().text;
            currentPath = path;
            AddPathToHistory(path);
            DirectoryInfo d = new DirectoryInfo(path);
            List<DirectoryInfo> dirList = new List<DirectoryInfo>();
            dirList.AddRange(d.GetDirectories());
            List<FileInfo> Files = new List<FileInfo>();
            Files.AddRange(d.GetFiles("*.png"));
            Files.AddRange(d.GetFiles("*.jpg"));
            foreach (DirectoryInfo dir in dirList)
            {
                string name = dir.Name;
                var folderIcon = Instantiate(goDirIconPreFab, goScrollAreaContent.transform);
                folderIcon.GetComponentInChildren<TMPro.TMP_Text>().text = "";
                folderIcon.GetComponentInChildren<TMPro.TMP_Text>().text = name;
            }
            foreach (FileInfo file in Files)
            {
                string name = file.Name;
                var imageIcon = Instantiate(goImageIconPreFab, goScrollAreaContent.transform);
                imageIcon.GetComponentInChildren<TMPro.TMP_Text>().text = "";
                imageIcon.GetComponentInChildren<TMPro.TMP_Text>().text = name;
            }
        }
        public void ClearScrollContent()
        {
            foreach (Transform child in goScrollAreaContent.transform)
            {
                Destroy(child.gameObject);
            }
        }
        public void GoBack()
        {
            if (currentPath == pathHistory[pathHistory.Count - 1])
            {
                historyLocation--;
                SetPath(pathHistory[historyLocation]);
                //EnnableForward
            }
            else if (currentPath != pathHistory[pathHistory.Count - 1] && historyLocation >= 1)
            {
                historyLocation--;
                SetPath(pathHistory[historyLocation]);
            }
        }
        public void GoForward()
        {
            if (currentPath != pathHistory[pathHistory.Count - 1])
            {
                historyLocation++;
                SetPath(pathHistory[historyLocation]);
            }
        }
        public void AddPathToHistory(string path)
        {
            if (pathHistory.Contains(path) == false)
            {
                pathHistory.Add(path);
                historyLocation++;
            }
        }
    }
};
