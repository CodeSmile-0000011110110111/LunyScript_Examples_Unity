# Execution Plan

Goal: Create a UI Toolkit window with a tree view that shows the selected object's events, their sequences, and their blocks for diagnostic purposes.

## Template

We already built a similar but simpler listview for variables:

[ScriptVariablesEditorWindow.cs](../Packages/de.codesmile.lunyscript/LunyScript.Unity/Editor/Diagnostics/ScriptVariablesEditorWindow.cs)
[ScriptVariablesController.cs](../Packages/de.codesmile.lunyscript/LunyScript.Unity/Editor/Diagnostics/ScriptVariablesController.cs)
[ScriptVariablesWindow.uss](../Packages/de.codesmile.lunyscript/LunyScript.Unity/UI/ScriptVariablesWindow.uss)
[ScriptVariablesWindow.uxml](../Packages/de.codesmile.lunyscript/LunyScript.Unity/UI/ScriptVariablesWindow.uxml)
[ScriptVariableState.cs](../Packages/de.codesmile.lunyscript/LunyScript/Diagnostics/ScriptVariableState.cs)

We should follow the same patterns, specifically window/controller split and acting only on the selected object.
And the "empty UI" fallback outside playmode.

## Requirements

- we would need to update the display strings of each block, as their internal values (variables) may change
  - for now it suffices to update the view of block elements every frame, BUT
    - first, check if frameCount changed, since editor update may run multiple times per frame, and even during playmode pause
    - only update (block.ToString) expanded items, don't update collapsed items (use IsItemExpanded)
    - only enumerate visibleItemIndices to skip culled items
- variables have ScriptVariableState to record FrameStamp of last execution, we should have a ScriptBlockState for similar purpose
  - use the State's methods/properties for instance to get the block's string, rather than using the block instance directly

Same as Variables window:
- only show tree in playmode
- use selected gameobject to get script runtime context
- if: no selection or unscripted object selected or outside playmode => empty tree, show different notes with reason

Different than variables window:
- no global/instance toggle
- filter filters by block name
  - auto-collapse events/sequences that are empty due to filter
  - auto-expand events/sequences no longer empty when updating filter string
  - use a "grayed out" text color for events/sequences and their block which are collapsed due to filtering (and revert to default color when no longer "empty")
  - user may expand events collapsed due to filtering (expanded state is reset whenever filter changes)
  - event/sequence block counts need not be updated due to filtering

## Opportunities

- ScriptEventScheduler: it will likely need (internal) accessors to get the registered enum types, and to enumerate the available sequences
- for UI code and CSS, where possible and meaningful: extract common functionality either in static helpers, or abstract base class (there may be more windows in the future), or utility classes respectively overarching USS
- use VisualElement.userData property if helpful

## Notes

- no need for observing changes to sequences/blocks: the runtime events/sequences are fixed after building a script
- a scheduled sequence will never be empty, it is either null or contains blocks => null checks suffice
 
## UI document

Top: filter button(s)
    - we need at least a toggle "Show Empty" which is off by default (hides empty event categories and empty event methods)
TreeView:
- event categories (names of category enums eg LunyObjectEvent, LunyCollisionEvent)
  - event methods (names of respective enum members eg OnEnabled, OnCollisionStarted)
    - list of ISequenceBlock instances (if not null)
      - list of blocks in sequence (using instance.ToString())

default behaviour:
- hide "empty" events/sequences
- expand all events
- event methods should list number of blocks across sequences in their name, eg "OnEnabled [12 block(s)]"
- sequences should list number of blocks in their name eg "SequenceBlock [7 block(s)]"

## Questions (please answer)
