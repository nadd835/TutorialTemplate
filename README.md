# VR Tutorial Template

Unity tutorial template for VR project that allows developers to quickly add, remove, or customize interactive tutorials like teleport, grab object, click, etc.

---

## Features

- Multi-sequence tutorial flow via `TutorialController`
- Ready-to-use tutorial steps (Teleport, Grab, Click, etc.) via `SequenceController`
- Customizable step conditions with UnityEvents
- Voice-over support with RT-Voice (optional)
- Default delay between steps and optional per-step delay timers
- Open/Close notification
  
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
│   │   │   └── NotificationManager.cs   
│   │   ├── Actions/
│   │   │   ├── TeleportController.cs        
│   │   │   ├── ObjectGrabCounter.cs     
│   │   │   ├── OutlineSelection.cs         
│   │   │   └── AlwaysFaceCamera.cs          
│   │   ├── Audio/
│   │   │   └── AudioTrigger.cs
│   │   └── MainMenu.cs
│   │   └── OpenCloseNObj.cs     
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
│   │   └── XR Rig Advanced No Main Menu.prefab
│   │   └── XR Rig Advanced with Main Menu.prefab
│   └── Examples/
│       └── DemoTutorial.unity  
```

---

## Getting Started

### 1. Import the package 

Import the package into your Unity project.

### 2. Add and adjust Prefabs
- Drag in the RTVoice, Audio Controller, XR Rig advance prefabs
- Drag in the `TutorialComplete` prefab from the `Prefabs > Tutorials` folder
- Drag the AudioController into the scene and set AudioTrigger.Play("ButtonClick") on each button’s OnClick() event.
- In the Inspector of the TutorialCanvas, customize the sequences list in the TutorialController by adjusting the order, adding new sequences, or removing any you don't need.
- 
You can create additional tutorials by duplicating the prefab, adjusting the steps and events accordingly. 

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

1. Add `TutorialController` to a canvas GameObject
2. Create child GameObjects for each tutorial sequence
3. Attach `SequenceController` to each sequence
4. Add `SequenceStep` to each step objects inside `SequenceController`
5. Drag and drop in the `sequences` list in `TutorialController`

### Example Flow

```
TutorialCanvas
├── Start Tutorial (Sequence 1) 
│   └── Start Panel (Step 1), Overview Controller Panel (Step 2)
├── Click Button Tutorial (Sequence 2) 
│   └── Click Button Panel (Step 1), Click Here Button (Step 2), Welldone Click Button (Step 3)
├── Teleport Tutorial (Sequence 3) 
│   └── Teleport Panel (Step 1), Welldone Teleport Panel (Step 2), Teleport Destinations (Step 3)
├── Highlight and Grab Object Tutorial (Sequence 4) 
│   └── Highlight Object Panel (Step 1), Welldone Teleport Panel (Step 2), Grab Object Panel (Step 3), Move to Tray Panel (Step 4), Move to Basket Panel (Step 5), Welldone Teleport Panel (Step 6)
├── Show Menu (Sequence 5) 
│   └── Show Menu Panel (Step 1), Welldone Teleport Panel (Step 2)
├── End Tutorial (Sequence 6) 
│   └── Tutorial Complete Panel (Step 1)
```
---

## Required Third-Party Packages

This template uses the following Unity packages:

- **VR Interaction Framework (VRIF)** 
- **RT-Voice** 
- **Proto UI** 
- **Quick Outline** 
- **Future UI Sound Library** 
