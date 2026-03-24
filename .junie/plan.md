# Execution Plan

Goal: Create a UI Toolkit window with a tree view that shows the selected object's events, their sequences, and their blocks for diagnostic purposes.

## Template

We already built a similar but simpler listview for variables:

[ScriptVariablesEditorWindow.cs](../Packages/de.codesmile.lunyscript/LunyScript.Unity/Editor/Diagnostics/ScriptVariablesEditorWindow.cs)
[ScriptVariablesController.cs](../Packages/de.codesmile.lunyscript/LunyScript.Unity/Editor/Diagnostics/ScriptVariablesController.cs)
[ScriptVariablesWindow.uss](../Packages/de.codesmile.lunyscript/LunyScript.Unity/UI/ScriptVariablesWindow.uss)
[ScriptVariablesWindow.uxml](../Packages/de.codesmile.lunyscript/LunyScript.Unity/UI/ScriptVariablesWindow.uxml)
[ScriptVariableState.cs](../Packages/de.codesmile.lunyscript/LunyScript/Diagnostics/ScriptVariableState.cs)

We should follow the same pattern, specifically window/controller split and acting only on the selected object.
And the "empty UI" fallback outside playmode.

## Requirements

- no need for observing tree changes, runtime events/sequences are fixed after building a script
- we may need to update the display strings of blocks, as their internal values may change
  - for now it suffices to update the view of elements every frame, BUT
    - first, check if frameCount changed, since editor update may run multiple times per frame, and even during playmode pause
    - only update (block.ToString) expanded items, don't update collapsed items (use IsItemExpanded)
    - only enumerate visibleItemIndices to skip culled items
- variables have ScriptVariableState to record FrameStamp of last execution, we should have a ScriptBlockState for similar purpose
 
## UI document

Top: filter button(s)
    - we need at least a toggle "Show Empty" which is off by default (hides empty event categories and empty event methods)
TreeView:
- event categories (category eg LunyObjectEvent, LunyCollisionEvent)
  - event methods (names of enum members eg OnEnabled, OnCollisionStarted)
    - list of ISequenceBlock instances (if not null)
      - list of blocks in sequence (using instance.ToString())

## Questions (please answer)
