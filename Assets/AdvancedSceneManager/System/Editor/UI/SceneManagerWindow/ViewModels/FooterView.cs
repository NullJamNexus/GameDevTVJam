using AdvancedSceneManager.Models;
using UnityEditor;
using UnityEngine.UIElements;

namespace AdvancedSceneManager.Editor.UI
{

    partial class SceneManagerWindow
    {

        class FooterView : ViewModel
        {

            public override void OnCreateGUI(VisualElement element)
            {
                SetupPlayButton(element);
                SetupProfile(element);
                SetupCollectionButton(element);
                SetupSceneHelper(element);
            }

            void SetupPlayButton(VisualElement element) =>
                element.Q<Button>("button-play").BindEnabled(SceneManager.settings.user, nameof(SceneManager.settings.user.activeProfile));

            void SetupProfile(VisualElement element)
            {

                var profileButton = element.Q<Button>("button-profile");

                profileButton.clicked += window.popups.Open<ProfilePopup>;

                Profile.onProfileChanged += OnProfileChanged;
                profileButton.RegisterCallback<DetachFromPanelEvent>(e => Profile.onProfileChanged -= OnProfileChanged);

                OnProfileChanged();
                void OnProfileChanged() => profileButton.BindText(Profile.current, nameof(Profile.name), "create");

            }

            void SetupCollectionButton(VisualElement element)
            {

                var button = element.Q("split-button-add-collection");
                window.BindEnabledToProfile(button);

                button.Q<Button>("button-add-collection-menu").clicked += window.popups.Open<ExtraCollectionPopup>;
                button.Q<Button>("button-add-collection").clicked += () =>
                {
                    Profile.current.CreateCollection();
                    window.collections.Reload();
                };

            }

            void SetupSceneHelper(VisualElement element)
            {

                var button = element.Q<Button>("button-scene-helper");


#if UNITY_2021 || UNITY_2022
                button.RegisterCallback<MouseDownEvent>(e =>
#else
                button.RegisterCallback<PointerDownEvent>(e =>
#endif
                {

                    e.PreventDefault();
                    e.StopPropagation();
                    e.StopImmediatePropagation();

                    DragAndDrop.PrepareStartDrag();
                    DragAndDrop.objectReferences = new[] { ASMSceneHelper.instance };
                    DragAndDrop.StartDrag("Drag: Scene helper");

                }, TrickleDown.TrickleDown);

            }

        }

    }

}
