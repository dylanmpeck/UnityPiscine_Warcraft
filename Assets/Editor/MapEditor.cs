using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
public class MapEditor : EditorWindow
{
	public static GameObject	LastTile;
	public const int			SIZE_X = 2;
	public const int			SIZE_Y = 2;
	
	[SerializeField]
	static bool					isActive = false;

	static MapEditor()
	{
		SceneView.onSceneGUIDelegate += OnSceneInteract;
	}
	
	static void OnSceneInteract(SceneView sceneView)
	{
		Event e = Event.current;
		
		if (isActive)
		{
			Vector2 mousePos = Event.current.mousePosition;
			mousePos.y = sceneView.camera.pixelHeight - mousePos.y;
			Vector3 position = sceneView.camera.ScreenPointToRay(mousePos).origin;
			Vector3 roundedPosition = new Vector3(Mathf.Round((position.x / SIZE_X) * SIZE_X), Mathf.Round((position.y / SIZE_Y) * SIZE_Y), 0);
			if (e.type == EventType.MouseDown && e.button == 0)
			{
				if (Selection.activeGameObject && Selection.activeGameObject.tag == "Tile")
					LastTile = Selection.activeGameObject;
				if (LastTile)
				{	
					GameObject tile = Instantiate(LastTile, roundedPosition, Quaternion.identity) as GameObject;
					tile.name = LastTile.name;
				}
			}
			else if (e.type == EventType.MouseDown && e.button == 1)
			{
				GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
				foreach (GameObject tile in tiles)
				{
					if (tile.transform.position == roundedPosition)
						DestroyImmediate(tile);
				}
			}
		}
	}
	
	[MenuItem("Tools/Map Editor")]	
	public static void	ShowWindow()
	{
		EditorWindow.GetWindow(typeof(MapEditor));
	}
	
	void OnGUI()
	{
		GUILayout.Label("Selected tile: " + ((LastTile) ? LastTile.name : "None"));
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Active Map Editor");
		isActive = GUILayout.Toggle(isActive, "");
		EditorGUILayout.EndHorizontal();
		Repaint();
	}

	void OnInspectorUpdate()
	{
		Repaint ();
	}
}
