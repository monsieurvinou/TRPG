using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BoardCreator))]
public class BoardCreatorInspector : Editor
{
    public BoardCreator current
    {
        get
        {
            return (BoardCreator)target;
        }
    }

    /// <summary>
    /// Override of the inspector GUI. This define buttons and their functions
    /// </summary>
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Clear"))       current.Clear();
        if (GUILayout.Button("Grow"))        current.Grow();
        if (GUILayout.Button("Shrink"))      current.Shrink();
        if (GUILayout.Button("Grow Area"))   current.GrowArea();
        if (GUILayout.Button("Shrink Area")) current.ShrinkArea();
        if (GUILayout.Button("Grow All"))    current.GrowAll();
        if (GUILayout.Button("Shrink All"))  current.ShrinkAll();
        if (GUILayout.Button("Save"))        current.Save();
        if (GUILayout.Button("Load"))        current.Load();

        if (GUI.changed) current.UpdateMarker();
    }

    /// <summary>
    /// Override of the OnSceneGUI to allow the selection of tiles to grow / shrink
    /// </summary>
    private void OnSceneGUI()
    {
        if (ActiveEditorTracker.sharedTracker.isLocked)
        {
            Object[] objects = Selection.objects;

            if (objects.Length > 0)
            {
                Point[] positions = new Point[objects.Length];

                for (int i = 0; i < objects.Length; i++)
                {
                    if (objects[i].GetType() == typeof(GameObject))
                    {
                        GameObject selectedObject = (GameObject) objects[i];
                        Vector3 objectPosition = selectedObject.transform.position;
                        positions[i] = new Point((int)objectPosition.x, (int)objectPosition.z);
                    }
                }

                current.setPositions(positions);
                current.UpdateMarker();
            }
        }
    }
}
