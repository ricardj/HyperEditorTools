using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(CharacterCustomizer))]
public class CharacterCustomizerEditor: Editor
{
    protected virtual void OnSceneGUI()
    {
        CharacterCustomizer characterCustomizer = (CharacterCustomizer)target;
        Vector3 characterPosition = characterCustomizer.transform.position;
        float size = 2f;
        float pickSize = size * 2f;
        if(Handles.Button(characterPosition, Quaternion.identity, size, size, Handles.ArrowHandleCap))
        {
            characterCustomizer.gameObject.GetComponentInChildren<MeshRenderer>().material = characterCustomizer.possibleMaterials[Random.Range(0, characterCustomizer.possibleMaterials.Length)];
        }
    }
}
