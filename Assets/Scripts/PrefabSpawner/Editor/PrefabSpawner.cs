using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



public class PrefabSpawner : EditorWindow {

    int selectedAction;
    GameObject cubeA;
    GameObject cubeB;
    GameObject cubeC;


    [MenuItem("CustomTools/PrefabSpawner")]
    private static void ShowWindow() {
        var window = GetWindow<PrefabSpawner>();
        window.titleContent = new GUIContent("PrefabSpawner");
        window.Show();
    }

    private void OnGUI() {
        string[] actionLabels = new string[] {"Cube A", "Cube B", "Cube C"};
        selectedAction = GUILayout.SelectionGrid(selectedAction, actionLabels, 3, GUILayout.Width(position.width - 5), GUILayout.Height(30));
        cubeA  = (GameObject)EditorGUILayout.ObjectField("Cube A",cubeA, typeof(GameObject), false);

    }

    private void OnSceneGUI(SceneView sceneView) {
        
        Event e = Event.current;

        if(e.type == EventType.MouseUp)
        {
            Debug.Log("So far so good");
            RaycastHit hitInfo;
            Ray r = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x,Camera.current.pixelHeight - e.mousePosition.y));
            bool rayCollided = Physics.Raycast(r,out hitInfo);
            if(rayCollided)
                if(selectedAction == 0)
                    Instantiate(cubeA, hitInfo.point, Quaternion.identity, null);
            
        }
    }

    private void OnEnable() {
        SceneView.duringSceneGui-= OnSceneGUI;
        SceneView.duringSceneGui+= OnSceneGUI; 
    }
        
}

