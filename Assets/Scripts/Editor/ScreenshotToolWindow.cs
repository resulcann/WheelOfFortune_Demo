using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class ScreenshotToolWindow : EditorWindow
{
    private List<ResolutionSetting> _resolutions = new List<ResolutionSetting>();
    private Vector2 _scrollPosition;

    [MenuItem("Tools/Screenshot Tool")]
    public static void ShowWindow()
    {
        GetWindow<ScreenshotToolWindow>("Screenshot Tool");
    }

    private void OnGUI()
    {
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        for (int i = 0; i < _resolutions.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            _resolutions[i].resolution = EditorGUILayout.Vector2Field("", _resolutions[i].resolution);
            _resolutions[i].path = EditorGUILayout.TextField("Path", _resolutions[i].path);
            
            if (GUILayout.Button("Browse"))
            {
                string folderPath = EditorUtility.OpenFolderPanel("Select Save Folder", "", "");
                if (!string.IsNullOrEmpty(folderPath))
                {
                    _resolutions[i].path = folderPath;
                }
            }
    
            _resolutions[i].filename = EditorGUILayout.TextField("Filename", _resolutions[i].filename);

            if (GUILayout.Button("Remove"))
            {
                _resolutions.RemoveAt(i);
                i--;
            }

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Resolution"))
        {
            _resolutions.Add(new ResolutionSetting());
        }

        if (GUILayout.Button("Take Screenshots"))
        {
            foreach (var setting in _resolutions)
            {
                TakeScreenshot((int)setting.resolution.x, (int)setting.resolution.y, setting.path, setting.filename);
            }
        }

        EditorGUILayout.EndScrollView();
    }

    private void TakeScreenshot(int width, int height, string path, string filename)
    {
        Camera camera = Camera.main;
        RenderTexture rt = new RenderTexture(width, height, 24);
        camera.targetTexture = rt;

        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
        camera.Render();
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);

        camera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt);

        byte[] bytes = screenshot.EncodeToPNG();
        string fullPath = System.IO.Path.Combine(path, filename + ".png");
        System.IO.File.WriteAllBytes(fullPath, bytes);
        Debug.Log("Screenshot saved to: " + fullPath);
    }

    private class ResolutionSetting
    {
        public Vector2 resolution;
        public string path;
        public string filename;
    }
}
