using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WheelItem))]
public class WheelItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
    
    public override bool HasPreviewGUI()
    {
        return true;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        var wheelItem = (WheelItem)target;
        if (wheelItem.ItemSprite != null)
        {
            GUI.DrawTexture(r, wheelItem.ItemSprite.texture, ScaleMode.ScaleToFit);
        }
    }
}