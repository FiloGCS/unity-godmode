using UnityEditor;
using UnityEngine;
public class MenuTest : MonoBehaviour {
    



    // Add a menu item to create custom GameObjects.
    // Priority 1 ensures it is grouped with the other menu items of the same kind
    // and propagated to the hierarchy dropdown and hierarchy context menus.
    [MenuItem("GameObject/UI/GM Progress Bar", false, 10)]
    static void CreateProgressBar(MenuCommand menuCommand) {
        // Create a custom game object
        GameObject go = new GameObject("GM Progress Bar");
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
}