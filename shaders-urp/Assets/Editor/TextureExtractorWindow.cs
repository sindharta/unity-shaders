using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class TextureExtractorWindow : EditorWindow {

    [MenuItem("UnityShadersTools/Texture Extractor")]
    static void ShowTextureExtractorWindow() {
        EditorWindow wnd = GetWindow<TextureExtractorWindow>();
        wnd.titleContent = new GUIContent("My Custom Editor");
        wnd.minSize = new Vector2(450, 200);
        wnd.maxSize = new Vector2(1920, 720);
    }

    void CreateGUI() {
        
        
        FloatField floatField = UIToolkitEditorUtility.AddField<FloatField, float >(rootVisualElement, new GUIContent("Somethiung"), 0.0f, (evt) => {
            Debug.Log("Field 1 value changed: " + evt.newValue);
        });
        
        
        // UIElementsEditorUtility.AddField<Toggle, bool>(defaultSectionContainer, Contents.SHOW_GROUPS_IN_HIERARCHY, 
        //     projSettings.AreGroupsVisibleInHierarchy(),
        //     (e) => {
        //         projSettings.ShowGroupsInHierarchy(e.newValue);
        //         projSettings.SaveInEditor();
        //         RefreshGroupHideFlagsInEditor();
        //     }
        // );
        
        rootVisualElement.Add(floatField);
        
        
      // // Get a list of all sprites in the project.
      // var allObjectGuids = AssetDatabase.FindAssets("t:Sprite");
      // var allObjects = new List<Sprite>();
      // foreach (var guid in allObjectGuids)
      // {
      //   allObjects.Add(AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(guid)));
      // }
      //
      // // Create a two-pane view with the left pane being fixed.
      // var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
      //
      // // Add the panel to the visual tree by adding it as a child to the root element.
      // rootVisualElement.Add(splitView);
      //
      // // A TwoPaneSplitView always needs two child elements.
      // var leftPane = new ListView();
      // splitView.Add(leftPane);
      // m_RightPane = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
      // splitView.Add(m_RightPane);
      //
      // // Initialize the list view with all sprites' names.
      // leftPane.makeItem = () => new Label();
      // leftPane.bindItem = (item, index) => { (item as Label).text = allObjects[index].name; };
      // leftPane.itemsSource = allObjects;
      //
      // // React to the user's selection.
      // leftPane.selectionChanged += OnSpriteSelectionChange;
      //
      // // Restore the selection index from before the hot reload.
      // leftPane.selectedIndex = m_SelectedIndex;
      //
      // // Store the selection index when the selection changes.
      // leftPane.selectionChanged += (items) => { m_SelectedIndex = leftPane.selectedIndex; };
    }

  private void OnSpriteSelectionChange(IEnumerable<object> selectedItems)
  {
      // // Clear all previous content from the pane.
      // m_RightPane.Clear();
      //
      // var enumerator = selectedItems.GetEnumerator();
      // if (enumerator.MoveNext())
      // {
      //     var selectedSprite = enumerator.Current as Sprite;
      //     if (selectedSprite != null)
      //     {
      //         // Add a new Image control and display the sprite.
      //         var spriteImage = new Image();
      //         spriteImage.scaleMode = ScaleMode.ScaleToFit;
      //         spriteImage.sprite = selectedSprite;
      //
      //         // Add the Image control to the right-hand pane.
      //         m_RightPane.Add(spriteImage);
      //     }
      // }
  }
  
  [SerializeField] private int m_SelectedIndex = -1;
  private VisualElement m_RightPane;
  
}