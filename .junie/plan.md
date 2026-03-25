# Execution Plan

## Goal

the diagnostics window currently doesn't show blocks "contained within"
i feel blocks which have in themselves one or more collections of blocks (arrays) should have further nested levels in the tree

## Template

## Requirements

i want to find a solution how we could extract that information from blocks to use it in the blocks diagnostic window
the nesting has no limits, just like a nested If or For loop

specifically:
- does it have condition[]/action[] ? (implements interface)
- get number of condition[]/action[] collections
- get the blocks in a specific collection or enumerate them in order
- what is the name of each collection (eg If, Then, ElseIf, While)

if we use interfaces or base class: they should provide default implementations (not throwing, returning valid strings)
note: we already have ISequenceBlock which indicates the block has a list of actions

## Opportunities

## Notes

I identified these blocks to have block collections:
IfBlock (most complex)
WhileBlock (only one array each for conditions and actions)
ForBlock (only actions)
AndBlock, OrBlock => contain 2-n conditions
InputEventSequenceBlock => only actions
PhysicsEventSequenceBlock (+2 subclasses) => only actions

To be refactored later:
CoroutineBlock and subclasses => contain blocks array for each Coroutine.Events entry
    NOTE: Coroutines should match the interface but return bogus data. I will later refactor coroutines to make them work similar to IfBlock.  Currently coroutines don't store their sequences in ScriptEventScheduler.

Special cases:
NotBlock => contains 1 condition (not a container, this should not increase nesting but would change item name from "c" to "NOT(c)")
RunBlock => action that runs a Action lambda, should appear in diagnostics but not provide details other than "RunBlock(λ)"
CheckBlock => condition that evaluates a Func lambda, should appear in diagnostics but not provide details "CheckBlock(λ) => false"


For getting IfBlock branch names, I imagine we could use something like:
    GetNameForConditionSequence(int index)
    GetNameForActionSequence(int index)
The IfBlock would then return "If" for condition index 0, otherwise "ElseIf"
And for actions it would return "Then" except the last index, which would return "Else"

## Planned Features (important for context but outside scope)

Conditions: we will evaluate them every frame and, if their truth state changes, we update the string by adding a emoji prefix (checkmark, X)

## UI document

Example how an IfBlock would look like in treeview (without comments)

IfBlock(..)	 // block.ToString() shows a summary
	If	  // <-- name obtained via new interface
		condition1 // 1-n conditions
		OR() // example of nested condition (logical operator)
			condition2 // nested in OR
			condition3 // nested in OR
	Then // <-- name obtained via new interface
		action1
		action2
	ElseIf // same as If
		..
	Then
		..
	Else // same as If but only actions (less nesting)
		action1
		action2
