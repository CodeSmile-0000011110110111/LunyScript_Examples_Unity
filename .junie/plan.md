# Execution Plan

Goal: Create a UI Toolkit window which lists all variables stored in a Luny-scripted object.

Requirements:
- Target: Unity 6.0 but opportunistically can also use 6.3 features
- The UI Document should in principle also work at runtime, though current focus is on editor.
- in principle, the document should also be embeddable as part of a component's Inspector 
- UI Document lives in LunyScript.Unity (package assembly) under UI/ folder.

- Selection in Hierarchy window determines what the variables window shows
  - selected object is looked up via LunyScript.ScriptEngine.GetScriptContext() using the selected object's GetEntityId()
  - multi-selection is not supported
  - there are two kinds of variables: global and local/instance (object bound)
     - get global variables from LunyScript.ScriptEngine.GlobalVariables
- the current references (selected object, table reference) should be expected to change while the window is open
  - it probably makes sense to have a separate controller script
  - the ui script should not be concerned with how to get the table reference or where it came from
  - ui script should handle null tables (don't show anything)

- Script variables always use the VarHandleBase and VarHandle types, which has an additional IsConstant property
  - we use this only to change the row's or just the value fields visualization eg different background or text color

- Tables has OnVariableChanged event which should be used to monitor for changes and update changed variables
  - this records

UI:
- should be a list view with multiple columns and header
- columns: name, value, timestamp (framecount of last change)
  - name: textfield 
  - value : variables have a type (number, bool, string) and the type may change (duck typing) which should change the value element:
    - number: number field (double type)
    - string: text field
    - bool: checkbox
  - timestamp: int field 

- sorting by clicking on the header should work (toggles between ascending/descending)
- the value field is editable and changes the variable's value. All other column elements are not editable.
- above the listview headers there should be a textfield for filtering (for now: non-empty filter filters list by case & culture invariant substring matching of variable name)


Caution:
- Variable and Table class are in engine-agnostic Luny project. They cannot reference Unity engine types. We'll likely need a wrapper eg ViewModel.
- The window will remain open when transitioning between edit and play modes. It should "deactivate" outside playmode.
- Table API is currently two-fold, given direct access to Variable and using VarHandleBase
  - the OnValueChangedEvent could be changed to return VarHandle or VarHandleBase
  - the indexer should be avoided, use GetHandle to get variable handles instead
- enter/exit playmode: this is where startup/shutdown timing and reference invalidation will matter
  - it would make sense to have a ILunyEngineObserver implementation (autoloaded by reflection via type lookup by interface) to have the engine lifecycle events 
  - this implementation should be general purpose, optional, runtime-enabled (not just editor): ScriptDiagnosticsObserver
  - we can assume it to be a singleton, since we are in the editor, we likely need to know when the singleton ref is valid
    - for that, if singleton is null, one subscribes to a static OnDiagnosticsStartup<ScriptDiagnosticsObserver> event which other scripts can subscribe to, to learn when the engine / diagnostics observer is ready (and the singleton reference is valid)
    - we need to null that static field upon exiting playmode => this should be a ResetStaticFields() method called by LunyEngine.ForceReset_UnityEditorAndUnitTestsOnly()
  - OnDiagnosticsShutdown fires when the engine shuts down, diagnostics singleton is still valid 

Note:
- we currently don't have any (meaningful) variables to show outside playmode but it should work in edit mode in principle
- whereever possible

Questions:
- data binding or manual binding? pros and cons? (only consider runtime-enabled bindings)
- if data binding: is changing variable type (changes value field element) going to cause issues?
- binding or not: what wrapper types do we need?
- any other aspects to consider or design?
