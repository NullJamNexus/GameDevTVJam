#if UNITY_2022_1_OR_NEWER
using System.Linq;
using AdvancedSceneManager.Utility;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;

namespace AdvancedSceneManager.Editor.Utility
{

    [InitializeInEditor]
    [Overlay(typeof(SceneView), "Collections", defaultDisplay = true, defaultDockPosition = DockPosition.Bottom, defaultDockZone = DockZone.LeftColumn)]
    public class CollectionOverlay : IMGUIOverlay, ITransientOverlay
    {

        static CollectionOverlay()
        {
            SceneManager.runtime.sceneOpened += _ => Refresh();
            SceneManager.runtime.sceneClosed += _ => Refresh();
            SceneManager.runtime.collectionOpened += _ => Refresh();
            SceneManager.runtime.collectionClosed += _ => Refresh();
        }

        private static void Refresh()
        {
            var collections = SceneManager.openScenes.Select(s => s.FindCollection()).Distinct().NonNull();
            isVisible = collections.Any();
        }

        public bool visible => isVisible;
        static bool isVisible;

        public override void OnGUI()
        {

            var collections = SceneManager.openScenes.Select(s => s.FindCollection()).Distinct().NonNull();
            GUILayout.Space(12);
            foreach (var collection in collections)
            {

                GUILayout.BeginHorizontal();
                GUILayout.Space(12);

                GUILayout.Label(collection.title, GUILayout.ExpandHeight(true));

                GUILayout.Space(16);
                if (GUILayout.Button(collection.isOpen ? "close" : "open", GUILayout.Width(64), GUILayout.Height(20)))
                    if (collection.isOpen)
                        collection.Close();
                    else
                        collection.OpenAdditive();

                GUILayout.Space(12);
                GUILayout.EndHorizontal();
                GUILayout.Space(2);

            }
            GUILayout.Space(12);

        }

    }

}
#endif
