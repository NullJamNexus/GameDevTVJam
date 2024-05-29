using System.Linq;
using AdvancedSceneManager.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace AdvancedSceneManager.Editor.UI
{

    partial class SceneManagerWindow
    {

        [SerializeField] private string m_dynamicCollectionPopup_collection;

        class DynamicCollectionPopup : ViewModel, IPopup
        {

            public void OnOpen(VisualElement element, object parameter)
            {

                parameter ??= Profile.current.dynamicCollections.FirstOrDefault(c => c.id == window.m_dynamicCollectionPopup_collection);

                if (parameter is not DynamicCollection collection)
                {
                    _ = window.popups.Close();
                    return;
                }

                window.m_dynamicCollectionPopup_collection = collection.id;

                element.Q<TextField>("text-title").BindTwoWay(collection, nameof(collection.title));
                element.Q<TextField>("text-path").BindTwoWay(collection, nameof(collection.path));

            }

            void IPopup.OnClose(VisualElement element) =>
                window.m_dynamicCollectionPopup_collection = null;

        }

    }

}
