# Execution Plan

## Goal

ScriptCoroutineRunner is a storage and runner for sequences.
ScriptEventScheduler is our main storage for sequences, with LunyScriptRunner running them.

I was thinking that the coroutine sequences should be stored in ScriptEventScheduler, to make the coroutine blocks accessible through the same interface. This would allow the block diagnostic window to show coroutine sequences.

### Overarching Goal

But then I realized ...

Since the coroutine runner processes heartbeat/frameupdate called via ScriptObjectEventHandler,
and the handler already runs heartbeat/frameupdate sequences, 
it seems to me as if we could integrate the coroutine runner itself in the heartbeat/frameupdate sequences.

### The Idea
We could make the coroutine runner a SequenceBlock that implements IBlockContainer,
scheduled on either heartbeat or frameupdate as is currently done in the coroutine runner's Register() method.

It would internalize the sequences it runs, not storing them in the ScriptEventScheduler, while the IBlockContainer 
interface enables the required diagnostics inspection.

## Requirements

## Opportunities

## Notes

## UI
