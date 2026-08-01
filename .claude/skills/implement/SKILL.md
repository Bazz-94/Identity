---
name: implement
description: Does the technical planning and implementation of a story.
---

Implements a story task by task, with the user reviewing each task.

## Instructions
1. Run the `/caveman lite` skill unless caveman mode is already on.
2. Create a plan with the `/create-implementation-plan` skill. If a plan already exists in `artifacts/implementation-plans/`, resume from its task statuses.
3. Implement all tasks according to `artifacts/standards.md`.
4. Iterate over the tasks with the process below.
  - If the user gives feedback, update the implementation, the plan, or the story as needed.
  - Build and run related unit tests after each code change.
5. After all tasks complete, build and run all unit tests to verify the story.
6. Create a sub agent to review the overall implementation using the `standards.md` rule. Use the sub agent to review the implementation and provide feedback. 
7. Implement the feedback. Then build and run all related unit tests again.
8. Verify the acceptance criteria are met and ask the user to review the story. On confirmation, set Status to Completed in the plan file; otherwise apply feedback and repeat from step 4.

## Implementation Process
1. Pick the next uncompleted task and set its Status to In Progress in the plan file.
2. Create unit tests for the task if possible (test driven development).
3. Implement the task.
4. Verify the acceptance criteria are met.
5. Build and run related unit tests.
6. Create a sub agent to review using the `standards.md` rule. Use the sub agent to review the implementation and provide feedback. 
7. Then implement the feedback. After confirming step 5 again, Set Status to Completed in the plan file.
