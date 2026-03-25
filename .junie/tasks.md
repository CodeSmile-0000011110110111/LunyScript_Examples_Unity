# New Tasks

DONE:
- [X] refactored ScriptEventScheduler to generic event interface
- [X] Variables Window: add Global/Instance toggle

NEXT FOCUS:
- Window: Show script block execution hierarchy (consider unification of scheduler sequences)
  - show each event/block with the same name as API
  - show current block parameters
  - show result of conditions' Evaluate
  - allow execute of actions
- Sequence diagnostics
  - Coroutines: refactor runner, coroutines should be scheduled sequences in the respective lifecycle events for diagnostics => schedule a CoroutineSequenceBlock, which holds the event sequences
    - add the coroutine sequences to the tree with additional nestings
    - Input action sequences should then follow the same system
    - then refactor the Input Actions in the tree
  - common interface for blocks containing blocks => If, While, etc. don't show theirs
  - Windows: refactor lifecycle and events to base class and merge with variables
- API diagnostics: forward CallerMemberName to construct the API calls automatically


- marketing: write "docs" blog post, send patreon, discussions thread, ask for feedback
- doc: add "install" placeholder
- DOC fixes
  - [X] "advanced block uses" => for instance the ActionBlock[] arrays returned from the methods above
  - [X] runtime loops => repeated in control flow
  - [X] control flow, loop with variables => var enemyCount = Var.("enemyCount", 3);
  - [X] arithmetics, passing result to parameter => missing variable declaration
- DOC: 
  - add event usage examples
  - document Check and Run
- implement VarHandle<T>/Variable<T> changed events
- BUG: Debug.Log("msg" + variable) <== creates a variable block, doesn't log "msg"!

# Improvements
- EDITING
  - Inspector: directly assign LunyScript
  - Inspector assigned values and references passed into script
  - Script run options eg relax name matching
- DIAGNOSTICS
  - Editor Window that lists variables (runtime-enabled UI)
  - Editor Window that lists block execution trace
  - Ensure consistent naming/ToString of Block types
  - Log runtime-value of variable blocks
- CONSISTENCY
  - Scan API for missing/breaking uses of variables as block parameters
  - Scan builder APIs for inconsistencies eg repeated uses If().Then().Then() or wrong order If().ElseIf().Then() (missing Then)
- ERROR HANDLING
  - Verify that all "outside of runners" uses of blocks notify (use builder token everywhere, eg WhileBlock, Object.Create)
  - Make type/name uses of BuilderToken consistent for error messages
  - Make all exceptions/logs show script name + line number => eg double click opens user script, not Luny code
- RUNTIME
  - Implement the Create-Spawn<=>Despawn-Destroy lifecycle
  - Expose the For() loop's counter value
- EVENTS
  - add When.Quitting
  - Consider a refactor to a generic block runner where data is separated from execution (systems + component data)
      - (Likely overkill?) => refactor Scheduler instead to have a generic "Sequence" interface instead to avoid duplication per type
      - Execute/Evaluate could be an injected lambda respectively a reference to a predefined lambda representing the action
      - would this make systems easier to create over fixed-function classes?
      - https://share.google/aimode/O5xyoUW7LDUVEM9zz
- VARIABLE
  - Add more math operations (Sqrt, Cos, Abs, etc)
  - Integrate Variable<T> (needs design)


Build a minimal, playable Brotato clone in 3D

## Thoughts
- compare LunyScript also with its VisualScripting counterpart

## Next Steps (Sprint)
- 
- [ ] BUG: Coroutine.Every frames/heartbeats technically correct but semantically wrong: "every 8 frames" => 8 + 8 = 17! (verify)
- [ ] BUG: enabling game object does not start coroutines
- [ ] On.Collision: require filters first, blocks last
- [ ] Transform.Move* => move second parameter to Speed(3) method
- [ ] Rigidbody: add "stay upgright" block
- [ ] Event System: Event.Send("kick").To("ball") -- sets an "event variable", processed after each update (2-3x per frame) and then reset - allows 2-3 event chaining per frame (Heartbeat: A sends B, Post Heartbeat: B sends C, Post Update: C sends D, Post LateUpdate: D sends E, PostUpdate: E sends F; Next Frame PreUpdate: F sends ..)

- [ ] Input: merge old and new input handling
- [ ] Input: add missing When.Input.* events (direct input, not action maps)
- [ ] Input: what's a suitable "placeholder" for an incorrect action map name? (eg "Palyer")
- 
- 
- [ ] LunyScript: Object.Create().At() <== position or target name (w/o parenting)
- [ ] LunyScript Time API: create Time blocks returning TimeService values
- [ ] LunyScript: scripts provide settings which contain list of object names to run for (solves pattern matching issue)
- [ ] Luny: Scene unload => should run object destroy cleanup before engine unload event
- 

## Minor Improvements
- [ ] Object.Create: add Color() builder, and Object/Renderer.SetColor()
- [ ] Rigidbody.Move*/Rotate* analogous to Transform.* variants but using physics
- [ ] Component.Enable/Disable: builder with .All().InChildren()/.InParents()
- [ ] LunyScriptUnityAdapter/LunyScriptMonoBehaviourEventRelayInstaller => should be a UnityPhysicsService providing these callbacks
- [ ] CollisionBuilder: (maybe) decide 2D/3D based on whether Collider or Collider2D exists
- [ ] CollisionEventRelay: add components without the "Update" event
- [ ] API: consider For.Seconds(3).Do(blocks)
- [ ] API: For should perhaps use a variable block?
- [ ] API: Run => allow custom name, or generate name from callstack otherwise
- [ ] API: If().Then().Then() is allowed
- [ ] ObjectRef class => encapsulates string with object/asset to support passing both equally
- [ ] Coroutines (et al): can't use toggle references easily:
		var tic = Coroutine("Timer: tic-toc tic");
  		var toc = Coroutine("Timer: tic-toc toc");
  		var ticBlock = tic.In(1).Seconds().WhenStarted(Object.Enable("Tic")).WhenElapsed(Object.Disable("Tic"));
  		var tocBlock = toc.In(1).Seconds().WhenStarted(Object.Enable("Toc")).WhenElapsed(Object.Disable("Toc"));
  		// TODO: current necessity since we can't yet use "tocBlock" in "ticBlock" or vice versa
        Coroutine("TicHelper").Every(1).Seconds().WhenElapsed(
            If(tictoc == 0).Then(tictoc.Inc(), tocBlock.Start())
            .ElseIf(tictoc == 1).Then(tictoc.Dec(), ticBlock.Start()));

## Backlog
- ### Post-MVP Refactor
  - VariableBlock: refactor to IVariable return type - treat Variable and Variable<T> the same
  - Blocks Source Generator: most blocks are trivial and could be autogenerated
  - Object API: remove trailing .Do()
  - Scene Load/Unload events should probably not be part of LunyEngine directly
- ### Refactor
  - Consider: LunyEngine explicit Interface Implementations to "hide" Developer SDK methods from public API (beginner-level users) while allowing developers to utilize the SDK features without having to use InternalsVisibleTo. Alternatively: a DeveloperApi, similar to how LunyScript implements its fluent Api.
  - [ ] LunyEngine: consider the registries as service providers - don't pass their references around, instead pass the data, or maybe relay calls via LunyEngine
  - [ ] LunyObjectRegistry: GetByName should use Dictionary, not FirstOrDefault
  - [ ] LunyObjectLifecycle: refactor DestroyNativeNullObjects() to avoid list copy (runs on scene unload)
  - [ ] LunyScriptRunner: make nested class LunyScriptEngine.Observer, forward to LunyScriptBlockRunner + coroutines
  - [ ] LunyScriptRunner: keep or remove the VarHandles for frame/heartbeat counters and elapsed time?
  - [ ] 'MockAssetService' => should be an engine mock, and tested via LunyEngine
  - [ ] ScriptLifecycle/ObjectEventHandler: evaluate, perhaps events should be handled by the runtime context?
- ### Engine Mocks
  - [ ] ..
- ### LunyEngine
  - [ ] Table: allow nested Tables with path-based indexing (dot and/or slash, or multiple indexers: t["1st"]["2nd"])
  - [ ] Table: support array/list variables
  - [ ] Scene Service: implement depth-first enumeration with pre-order or post-order (as IEnumerable?)
  - [ ] Asset Service (Unity): support loading of non-Resource assets (via ScriptableObject DB in Resources)
  - [ ] Asset Service: services initialize placeholders in Dictionary<Type, object> and store them on the Luny side? Release: a single placeholder for any asset type?
  - [ ] Primitives: should have a "WithPhysics()" setting that properly sets up the thing to work physically (Unity: adds Rigidbody, Godot: do the 20 things to make it a working physics object)
  - [ ] Luny: Logger with filterable categories and/or compile flags (ie trace finalizer)
  - [ ] LunyObject: pull down IsNativeObjectValid tests?
  - [ ] Test scene (un-)(re-)load and hook up to scene service callbacks, verify against engine call order (get this first)
- ### LunyScript Design & Lifecycle
  - [ ] ensure deterministic LunyScript execution order (follow scene hierarchy order?)
  - [ ] Handle LunyObject parenting with hierarchy gaps
  - [ ] Review OnIntervalUpdate implementation in script Scheduler and object event handler
  - [ ] How to pass references to blocks via event parameters (eg Scene, "Other" from colliders, etc) => Design
  - [ ] Coroutines: add While condition blocks: While(condition).Do(blocks) (runs while true, replaces For() builder) (hearbeat vs update?)
  - [ ] Coroutines: refactor to step builder with an "options" DTO passed between them, using Interfaces and generics to avoid boxing
  - [ ] LunyScript: design how to edit values in Inspector and apply them to ScriptContext
      - Godot: enable multiple scripts per node!
  - [ ] For LoopCounter: find a better solution than the one I had
  - [ ] LunyScript: extensible API via Interface Mixins and Roslyn generated API property (see [2026-02-02-extending-lunyscript-api.md](/_posts/2026-02-02-extending-lunyscript-api.md))
- ### LunyScript Debug / Profile
  - [ ] Variable validation (log read access of non-existing variables)
  - [ ] Metadata for global variable read/write tracking
- ### LunyScript Blocks & API
  - [ ] LunyScript: Blocks(params blocks) method to create a "Sequence" of blocks. Test if block sequences are clone- and/or reusable: check for any side-effects.
  - [ ] LunyScript: bool IsSingleton property; autocreates object, makes script/object non-destroyable via native feature (DDOL, root node)
  - [ ] [[Event Handling Blocks]] foundation (Input, Collision, SendMessage)
  - [ ] Create Scene load blocks
  - [ ] Implement Random/Shuffle blocks and API
- ### LunyScript Coroutines
  - [ ] Coroutines: re-use existing coroutines for re-created objects/scripts
  - [ ] Variable coroutines, ie runs when variable changes or has a specific value => acts like an event system (though While conditions would cover this already)
  - [ ] add option to disable auto-start of coroutines: DontStart()?
  - [ ] Pause/Start blocks use reference, this introduces a 1-frame delay if ref-call is positioned after coroutine creation => use string based coroutine lookup to work around this?
  - [ ] CoroutineRunner: registry for the coroutine collections
- ### Unity Specific
  - AssetService: load actual prefab asset, current workaround create a "prefab" + "prefab instance" in scene
- ### Godot Specific
  - 
- ### LATER (minor)
  - [ ] LunyScript.Method.Run => could use overloads for When/Every BUT I don't want to make it "too easy" to inject lambdas
  - [ ] Consider: Case insensitive and partial name matching for object-script activator (controlled by LunyScript flags), configurable: starts with/contains/etc
  - [ ] Unity: Add `[IgnoredByDeepProfiler]` attribute to debug methods
- ### EPICS (needs design)
  - LunyScript running on global object (autoload, DDOL) - should still run after scene (re)load
  - [[Lua Integration]] of scripting API
  - **Repository Structure**: LunyEngine as separate package/addon from LunyScript?
  - **Distribution**: versioning of LunyEngine for multiple frameworks? Lightweight package manager?
  - **Hot Reload**: Support for both C# and Lua, manual triggers for testing
  - **Sandboxed Script Execution**: API filter (Lua: blacklist/whitelist, C#: custom sandbox service), object/asset filters
  - **On-Screen Diagnostics**: Display design and required services
  - **Developer SDK Documentation**: Observer patterns, Service API extension, engine support guide, behavioral contracts
  - **Ingame Console**: Run blocks/runnables at runtime via console
  - (maybe) create separate Unity package / Godot addon for in-engine ContractTest verification => figure out how to approach this
  - [ ] Roslyn Generator to inject extension interface properties (base interface: ILunyScriptApi, property: )
      - Should be a incremental generator, using [LunyScriptExtension] and [LunyScriptApi] attributes
      - separate project, compiled to DLL, DLL for both engines
      - Godot generator is installed by modifying .csproj (sticky, ie via plugin.gd)
      - Analyzer message if class isn't partial, but only if it implements a ILunyScriptApi interface
      - `public interface IExtension { ExtensionApi Extension => new(); } // user defined`
      - `public ExtensionApi Extension => ((IExtensionApi)this).Extension; // injected property`
- ### Ideas "Outside Project Scope" ...
  - **Cross-Engine Editor Tooling**: Custom importers, build scripts, editor windows/settings
  - **Portable Scene Format**: Design in Godot, import in Unity, and vice versa (portable asset types)
  - Luny could have a 1-click solution to setting up a git repository for source control. and instructions how to put it in the cloud (github, aws, .. whatever is easy/popular)
  - .. Improve execution speed of git pull/commit script (parallelize? rewrite to non-generic version for better control?)
    - .. auto-pull per solution, so i only need to switch autopull in the solution i don't work in (even automate the switch?? cautions?)
    - .. a proper C# tool instead of 'bush' script?
    - .. a way for me to very quickly get an overview of what's changed, and view the diffs, and perhaps even comment on them

## DONE

### CW13-2026 Mar
- [X] Window: Display Script Variables with live update, timestamps, value editing


### CW12-2026 Mar
- [X] Write first draft of manual:
  - Getting Started, Script Anatomy, Advanced Block Usage, Control Flow, Events, Variables
- [X] For (etc) => support passing script variables
- [X] Collision & Input event rename for consistency
- [X] website: remove timeline with milestones

### CW11-2026 Mar
- Add Local Multiplayer Input support, device pairing, Smoke Test
- Add DocFX manual/API workflow & deployment, initial pages

### CW10-2026 Mar
- Smoke Tests: Physics, Objects, Coroutines
- When.Input Events
- API Builder review / overhaul
- add VariableBlock<T>

### CW09-2026 Mar
- [X] BuilderToken: use MarkFinished to finalize "open" builders provided they are in a finalizable state, then remove trailing Do() pattern
- [X] Timer: has no When* event blocks
- [X] Counter: has no When* event blocks
- [X] BUG: Coroutine.For() never ends, Every().XXX.Do() ends after time -- mixup?
- [X] BUG: Timer() TimeScale is not respected
- [X] BUG: Timer Every ms does not run event blocks
- [X] BUG: IfBlockBuilder calls its own Execute every frame, building the block every time
- [X] ISSUE: Uninitialized variable + arithmetics fails because null != 0
- [X] Problem: VariableBlock wraps any type, we can't rule out nonsensical uses (ie Vector2 where Quaternion is expected)
- [X] Scheduler: should hold ISequenceBlock to be more flexible
- [X] Input: use event phases and pass this to service base, avoids clearing state
- [X] Input: add handling of started, performed, canceled events
- [X] Input: add When.InputAction API
- [X] CollisionBuilder: Begins/Ends/Update should not have Layered/Tagged/Named/etc
- [X] API: Every* adds little over Counter() => could merge "DelayBy" with Counter? At least it omits the "Counter()." part

### CW08-2026 Feb
- [X] Luny: add Variable<T> for all value types
- [X] Luny: create LunyVector2, LunyVector3, LunyQuaternion
- [X] Luny: Variable should understand primitive engine types: Vector2/3, Rect, etc  (no-boxing generic Variable<T>)
    - https://share.google/aimode/g8Apk6pzL1LzLHC2o
- [X] refactor VariableBlock: public API polluted with GetValue(context)
- [X] LunyScript: allow non-table variables => ComputedVariableBlock
- [X] LunyScript: create input event API
- [X] LunyScript: Remove LoopCounter since it polluted GetValue with ScriptRuntimeContext parameter
- [X] VariableBlocks: remove context from GetValue() - when do we need context, and why?
- [X] LunyScript: Transform => move and shift blocks

### CW07-2026 Feb
- [X] Luny: add Alarm & Stopwatch structs 
- [X] Luny: add Timer & Counter classes 
- [X] Luny: LunyAssetBase as common base class for assets
- [X] Luny: use Dispose() and GC.SuppressFinalize(this) with finalizer logging to catch memory leaks!
- [X] LunyScript: coroutines use Timer/Counter classes
- [X] LunyScriptDefinitionRegistry: fix "already registered" in tests (Assembly shadow copies)
- [X] LunyScript: pass "build context" to Build and return it for settings. Better than property overrides.
- [X] LunyScript: Create Object Lifecycle tests
  - [X] Create/Destroy cycle => should rebuild scripts
  - [X] Create/Destroy cycle => should rebuild coroutines
  - [X] Create scripted object later => should build script or use prebuilt script
  - [X] Create multiple objects with same name 
- [X] Coroutine Improvements:
    - [X] CoroutineBlock: separate Timer/CounterCoroutineBlock subclasses
    - [X] IScriptCoroutineBlock with sub-interfaces => use/return correct types everywhere
    - [X] TimeSlice: internalize time-slicing check, should run in heartbeat or frameupdate dep. on process mode
    - [X] Disallow negative values: In(-10).Frames() => use UInt or throw?
    - [X] Ensure "In(1).Frames" will run in the next frame, and "In(0).Frames" runs in current frame if started in Heartbeat or FrameUpdate
    - [X] Remove PerpetualStyleCounter ... leftover
    - [X] Dangling coroutine builders log and throw
    - [X] Rename ".Build()" to "Do()" in regular Coroutine
- [X] Coroutine Tests: fill gaps in test suite. Verify permutations (including nonsensical => should fail).


### CW06-2026 Feb
- [X] Comprehensive variable tests: split into arithmetic and comparison scripts with separate variables
- [X] Arithmetic tests: add inter-variable operations and complex calculations
- [X] Split variable tests into dedicated files (ArithmeticVariableTests.cs and ComparisonVariableTests.cs)
- [X] Fixed side-effect bug in arithmetic tests (variable overwriting)
- [X] Refactor flow tests to use the Variable API (Var/GVar) instead of GlobalVars/LocalVars directly
- [X] ILunyScriptEngine: hide GetScriptContext from the public interface
- [X] GDSharp: GDScript wrapper following C# guidelines => https://github.com/CodeSmile-0000011110110111/GDSharp
- [X] add LunyScript-Analyzers (Roslyn), always builds release, copies DLL to LunyScript/Analyzers, plugin.gd updates Godot .csproj, and tested working callbacks in Godot + Unity (stub only, no function)
- [X] Rename example engine project repos to "LunyScript_*"
- [X] LunyScript: refactor Api classes to their own files (namespace: LunyScript.Api.DebugApi)
- [X] table.NotifyVariableChanged => move into table ?
- [X] Refactor Table OnValueChanged => move responsibility into VarHandle
- [X] rename: ArithmaticOperationBlockBase (AddVariableBlock); ComparisonBlockBase (IsGreaterVariableBlock)
- [X] VariableConditionBlock: refactor to individual classes
- [X] Variable API: refactor from Var.Add("name", ..) => Var("name").Add/Is(..)
- [X] Variables: complex operations fail => `hp.Sub((hp + 10) / 2)`
- [X] Variables: support `Var("..") > 50` etc
- [X] Variables: support `++` and `--` operators
- [X] Variables: test ! operator
- [X] Variables: Refactor architecture (decoupling) and naming convention
- [X] Variables: Propagate TargetHandle in expressions (bi-directional)
- [X] Variables: Add Const() "readonly" table besides Var/GVar for named constant values (global)
- [X] replace LunyScript.GlobalVars/LocalVars by Var and GVar block-based APIs, and rename to "Variables"
- [X] Refactor When.* API to new proposed On.* API (see [LunyScript_On_vs_When_API_Refactor.md](/RFC/docs/2026-02/LunyScript_On_vs_When_API_Refactor.md))
- [X] [[Coroutine & Timer Blocks]] - see: [LunyScript_CoroutineAndTimer_Design.md](/RFC/docs/2026-02/LunyScript_CoroutineAndTimer_Design.md)
- [X] Coroutine: handle Enabled/Disabled and Destroy events
- [X] CoroutineControlBlock => split into separate classes, one per control action
- [X] CoroutineFinalBuilder => refactor to use fewer ctor params (DTO struct, reusable by coroutine?)
- [X] Repeating timers don't work yet?
- [X] Refactor after implementing timers:
    - System.Threading.Interlocked.Increment => we are single-threaded, remove this
    - [X] timeslice: perhaps defer to specialized subclasses?
    - [X] EveryBuilder: unique name generation with GUID! could be an incrementing static int
    - [X] rename engine event methods (fixedupdate etc) => LunyScriptObjectEventHandler, CoroutineRunner
    - [X] CoroutineBlock => move to Blocks folder
    - [X] TimerBuilder => _isRepeating - is it necessary?
    - [X] CoroutineBuilder: OnUpdate => OnFrameUpdate ?
    - [X] CoroutineInstance: _elapsedCount and _targetCount fields for count-based tracking could be merged with the double types elapsedTime and duration, just advancing in integer steps
        - [X] OR: separate CoroutineInstance types (time, count, time-sliced)
        - [X] AND: reduce logic duplication via template-method in CoroutineBase and progression structs (TimeProgress, CountProgress)
    - [X] CoroutineRunner: Even/Odd could be turned into +0/+1 Delay not carried into logic?
    - [X] LunyScript/Coroutines location: we have Execution and Event subfolders. Check if we should re-organize these types (both folder and naming)
    - [X] perform a complete refactoring pass
        - [X] EveryBuilder
        - [X] TimerBuilder
        - [X] DurationBuilder
        - [X] CoroutineBuilder
        - [X] Coroutine + Base + Config
        - [X] CoroutineRunner
        - [X] CoroutineBlock
- [X] Coroutine: implement OnEnable/OnDisable/OnDestroy behaviour
- [X] CoroutineRunner: use TimeService instead of manually counting update/step
- [X] CoroutineBlock: prevent use in regular sequences
- [X] CounterCoroutine: split into TimeSliceCoroutine for time-sliced variants?
- [X] Luny: refactor "OnObjectDestroyed" fired separately via Lifecycle.OnObjectDestroyed(this); => renamed
- [X] Luny: refactor how time is passed around

### CW05-2026 Jan
- [X] Godot: SceneTree should inherit from MainLoop, with GetMainLoop() returning MainLoop just like Godot
- [X] Godot: Node class' Notification const must be 'long' just like Godot
- [X] Godot: Name property must be of type 'StringName'
- [X] Godot: ensure classes that are require to be 'partial' in Godot have the 'partial' keyword. Add an "empty" partial stub just so the 'partial' keyword sticks when running "code cleanup" (which removes keywords and usings it determines to be superfluous)
- [X] Setup LunyLua and LunyLua-Test with Lua.dll reference
- [X] LunyScriptVariables: renamed to LunyTable
- [X] LunyScriptVariable implementation changes:
    - [X] Rename class to `LunyVariable`, add `ILunyVariable` interface
    - [X] Memory footprint optimization: `Name` returns "(N/A)" in non-debug builds
    - [X] Private constructor, `Create` static methods, updated signature (value first)
- [X] `LunyNumber` struct: wraps a `System.Double` type
    - [X] complete implicit conversion to any other type from byte to long and float and bool ("loss of precision" casts are acceptable)
    - [X] cast to bool: first cast to long, if result is 0 it's 'false', otherwise 'true'
    - [X] add arithmetic operators for bool and string that throw an exception (overrule implicit conversions)
- [X] Variable: refactor to variant type
- [X] Add remaining tests for Table class
- [X] LunyScript: refactor the s_Instance away by changing static subclasses to static struct and assign the instance in Initialize
- [X] [[Conditional Blocks]] (if/else, while, AND/OR/NOT)
- [X] [[Variable Blocks]]
- #### Asset Service ✓
    - [X] Implement Asset/Resource loading by name/path ✓
    - [X] Addressing: LunyUrl/LunyPath: handles resource path conversion & cleanup ie remove "res://") ✓
    - [X] we also need an `AssetID` for LunyEngine internal addressing (indexing) of assets/resources, and mapping to engine-native addressing (ID, GUID, Path) ✓
    - [X] Return valid "error" objects from Asset service ✓
    - [X] Implement loading of "prefabs" (Godot: packedscene, or just any scene?) ✓

### CW04-2026 Jan
- [X] Contract Tests now contain LunyScript tests which should not be there => separate contract from implementation tests
- [X] Move LunyEngine tests from Luny-ContractTest to Luny-Test
- [X] *BridgeTest: should they inherit from ContractTestBase? (YES)
- [X] Remove LunyTestableAttribute (unused, we now have tests in separate projects)
- [X] Check if we can make the native engine projects build by providing Mocks for native APIs (enables engine-wide refactoring!)
- [X] LunyServiceRegistry: refactor engine check, remove adapter and replace with global "engine name" property provided by adapter
- [X] refactor existing *-Test tests:
    - [X] make tests use contract test infrastructure
    - [X] make all tests run once with each engine adapter, without duplicating the test code
    - [X] where it makes sense, move tests to Luny-ContractTest
- [X] GetAllObjects(): change to only convert to LunyObject if scripted
- [X] LunyObject: return cached instance before creating new one
- [X] Implement FindByName (with proxy fallback) for Object.Create that should run a script (consider prebuilt scripts)
- [X] Implement Every.TimeInterval block
- [X] LunyScript.When.Self (updates) should not run while disabled
- [X] Create object via API and register (activate script)
- [X] Get and destroy registered object via API
