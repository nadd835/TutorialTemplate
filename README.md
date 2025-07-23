# VR Tutorial Template

Unity tutorial template for VR project that allows developers to quickly add, remove, or customize interactive tutorials like teleport, grab object, click, etc.

---

## Features

- Multi-sequence tutorial flow via `TutorialController`
- Ready-to-use tutorial steps (Teleport, Grab, Click, etc.) via `SequenceController`
- Customizable step conditions with UnityEvents
- Voice-over support with RT-Voice (optional)
- Default delay between steps and optional per-step delay timers

---

## Project Structure

```
Assets/
├── TutorialTemplate/
│   ├── Scripts/
│   │   ├── Controllers/
│   │   │   ├── TutorialController.cs        <- Manages multiple sequences
│   │   │   ├── SequenceController.cs        <- Manages individual steps in a sequence
│   │   │   └── SequenceStep.cs              <- Trigger next step logic + voice
│   │   ├── Actions/
│   │   │   ├── TeleportController.cs        
│   │   │   ├── ObjectGrabCounter.cs     
│   │   │   ├── OutlineSelection.cs         
│   │   │   └── AlwaysFaceCamera.cs          
│   │   ├── Audio/
│   │   │   └── AudioTrigger.cs           
│   ├── Prefabs/
│   │   ├── Tutorials/
│   │   │   ├── Click Button Tutorial.prefab
│   │   │   ├── End Tutorial.prefab
│   │   │   ├── Grab & Drop.prefab
│   │   │   ├── Highlight Object Tutorial.prefab
│   │   │   ├── Show Menu.prefab
│   │   │   ├── Start Tutorial.prefab
│   │   │   └── Teleport Tutorial.prefab
│   │   ├── UI/
│   │   │   ├── Panel.prefab
│   │   │   ├── Tutorial Canvas.prefab
|   |   |   └── Welldone Panel.prefab
│   │   ├── AudioController.prefab
│   │   ├── Environment.prefab
│   │   ├── Inventory.prefab
│   │   └── XR Rig Advanced.prefab
│   └── Examples/
│       └── DemoTutorial.unity  
```

---

## Getting Started

### 1. Import the package 

Import the package into your Unity project.

### 2. Add the TutorialController
- Drag in the `TutorialManager` prefab from the `Prefabs` folder
- It comes pre-attached with the `TutorialController` script
- In the Inspector, customize the tutorial `sequences` list
- Each `SequenceController` handles a list of steps that will be played in order from 0 to end

You can create additional tutorials by duplicating the prefab, adjusting the steps and events accordingly. You may also add new tutorial sequences from existing prefabs and edit the `TutorialController` list to control the playback order.


### 3. Configure Voice Over (Optional)
- Use the `Use Voice Over` checkbox in `TutorialController` to enable or disable voice playback during tutorial steps.
- When enabled, each step will use the text in `voiceText[]` to auto-generate speech with RT-Voice.


### 4. Triggering Step and Sequence Progression
- To trigger the **next step** in a sequence, call `SequenceStep.NextStep()` (e.g., from a UI button).
- To trigger the **next sequence**, call `SequenceController.CloseSequence()` on the final step (via UnityEvent).

---

## Sequence Flow

- `TutorialController` runs sequences in order
- Each `SequenceController` runs steps one-by-one
- When a sequence finishes, `TutorialController` auto-starts the next one
- Events like `onSequenceFinished` let you continue to the next sequence

---

## Example: Multi-Step, Multi-Sequence Tutorial

This structure is demonstrated in the included `DemoTutorial` scene.

### Setup

1. Add `TutorialController` to a manager GameObject
2. Create child GameObjects for each tutorial sequence
3. Attach `SequenceController` to each sequence
4. Add `SequenceStep` to each step objects inside `SequenceController`
5. Drag and drop in the `sequences` list in `TutorialController`

### Example Flow

```
TutorialCanvas
├── Start Tutorial
│   └── Start Panel, Overview Controller Panel
├── Click Button Tutorial
│   └── Click Button Panel, Click Here Button, Welldone Click Button
├── Teleport Tutorial
│   └── Teleport Panel, Welldone Teleport Panel, Teleport Destinations
├── Highlight Object Tutorial
│   └── Highlight Object Panel, Welldone Teleport Panel
├── Grab & Drop Object
│   └── Grab Object Panel, Move to Tray Panel, Move to Basket Panel, Welldone Teleport Panel
├── Show Menu
│   └── Show Menu Panel, Welldone Teleport Panel
├── End Tutorial
│   └── Tutorial Complete Panel
```
---

## Adding Custom Steps

You can define your own tutorial interaction logic by extending `SequenceStep.cs`. To add a custom tutorial:
- Create a new GameObject and attach your custom step script
- Add it to a `SequenceController` in the step list
- Implement your logic to trigger interaction-specific actions
- Call `NextStep()` when the interaction has finished to continue the flow

You can also add tutorials from existing prefabs inside the `Tutorials` folder, then adjust the flow by modifying the sequence list in the `TutorialController`. The sequences will be played automatically in order from start to finish.

---

## Required Third-Party Packages

This template uses the following Unity packages:

- **VR Interaction Framework (VRIF)** 
- **RT-Voice** 
- **Proto UI** 
- **Quick Outline** 
- **Future UI Sound Library** 
