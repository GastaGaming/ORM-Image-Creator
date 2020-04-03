using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Data;
using System.Linq;
using UnityEngine.Networking;
namespace Gasta
{
    public class OrmImageCreator : MonoBehaviour
    {
        public static OrmImageCreator Instance { get; private set; }
        private string inputPath = "";
        private FileBrowser fb;
        private List<string> inputFilePaths = new List<string>();
        private List<Texture2D> inputTextures = new List<Texture2D>();
        public GameObject previewContent;
        private List<GameObject> previewGImage = new List<GameObject>();
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            FileBrowser fb = Gasta.FileBrowser.Instance;
            fb.gameObject.SetActive(false);

            //Setting deffault window size;
            Screen.SetResolution(1024, 768, false);
        }
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                AplicationQuit();
            }
        }
        public void SelectSource()
        {
            //string path = EditorUtility.OpenFilePanel("Select Source Path", "", "");
            //OpenInFileBrowser.Open(UnityEngine.Application.dataPath);
            //System.Diagnostics.Process p = new System.Diagnostics.Process();
            //p.StartInfo = new System.Diagnostics.ProcessStartInfo("explorer.exe");
            //p.Start();
            FileBrowser fb = Gasta.FileBrowser.Instance;
            fb.gameObject.SetActive(true);
        }
        public void SetInputPath(string path)
        {
            inputPath = path;
        }
        public void AplicationQuit()
        {
            Application.Quit();
        }
        public void CheckInput()
        {
            bool inputOk = false;
            if (System.IO.Directory.Exists(inputPath))
            {
                inputOk = true;
            }
            else
            {
                //ErrorOut
            }
            if (inputOk)
            {
                inputPath = inputPath.Replace("\\", "/");
                DirectoryInfo inputDir = new DirectoryInfo(inputPath);
                //Lets go trough all the files and if match image format lets load it
                foreach (var file in inputDir.GetFilesByExtensions(".jpg", ".png", ".tif"))
                {
                    inputFilePaths.Add(file.Name);
                    //http://gyanendushekhar.com/2017/07/08/load-image-runtime-unity/
                    byte[] byteArray = File.ReadAllBytes(inputPath + "\\"+ file.Name); //Read image and store in a byte array
                    Texture2D tex = new Texture2D(2, 2); // Texture size does not matter
                    bool isLoaded = tex.LoadImage(byteArray); //Size of texture will be replased
                    inputTextures.Add(tex); //Add image to loaded files
                }
                foreach(Transform child in previewContent.transform)
                {
                    previewGImage.Add(child.gameObject);
                }
                int count = 0;
                foreach (GameObject g in previewGImage)
                {
                    //g.GetComponentInChildren<UnityEngine.UI.RawImage>().texture = inputTextures[count];
                    TMPro.TMP_Dropdown dropDown = g.GetComponentInChildren<TMPro.TMP_Dropdown>();
                    dropDown.options.Clear();
                    for (int i = 0; i < inputFilePaths.Count; i++)
                    {
                        TMPro.TMP_Dropdown.OptionData temp = new TMPro.TMP_Dropdown.OptionData();
                        temp.text = inputFilePaths[i];
                        dropDown.options.Add(temp);

                        if (temp.text.Contains("dif") || temp.text.Contains("basecolor") || temp.text.Contains("albedo") && count == 0)
                        {
                            g.GetComponentInChildren<UnityEngine.UI.RawImage>().texture = inputTextures[i];
                            dropDown.value = i;
                        }
                        if (temp.text.Contains("occlusion") || temp.text.Contains("ao") && count == 1)
                        {
                            //g.GetComponentInChildren<UnityEngine.UI.RawImage>().texture = inputTextures[count];
                            dropDown.value = i;
                        }
                        if (temp.text.Contains("roughness") || temp.text.Contains("rough") && count == 2)
                        {
                            g.GetComponentInChildren<UnityEngine.UI.RawImage>().texture = inputTextures[i];
                            dropDown.value = i;
                        }
                        if (temp.text.Contains("metal") || temp.text.Contains("metallic") && count == 3)
                        {
                            g.GetComponentInChildren<UnityEngine.UI.RawImage>().texture = inputTextures[i];
                            dropDown.value = i;
                        }
                        if (inputFilePaths.Contains("nor") || inputFilePaths.Contains("normal") && count == 4)
                        {
                            g.GetComponentInChildren<UnityEngine.UI.RawImage>().texture = inputTextures[i];
                            dropDown.value = i;
                        }
                        if (inputFilePaths.Contains("emit") || inputFilePaths.Contains("emission") && count == 5)
                        {
                            g.GetComponentInChildren<UnityEngine.UI.RawImage>().texture = inputTextures[i];
                            dropDown.value = i;
                        }
                    }
                    count++;
                }
                //Then lets make them visible in the ui
            }
        }
    }
}
static class Tools
{
    public static IEnumerable<FileInfo> GetFilesByExtensions(this DirectoryInfo dir, params string[] extensions)
    {
        if (extensions == null)
            throw new ArgumentNullException("extensions");
        IEnumerable<FileInfo> files = dir.EnumerateFiles();
        return files.Where(f => extensions.Contains(f.Extension));
    }
}