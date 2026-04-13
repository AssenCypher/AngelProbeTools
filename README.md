# Angel Probe Tools

Angel Probe Tools is a free Editor-first probe workflow utility for VRChat world creators.

## Current scope

- Reflection Probe generation
- LightProbeGroup generation
- Bounds expansion
- Zone split / scaffold generation
- Optional Box Trigger scaffold generation

## Product boundary

Angel Probe Tools keeps the base workflow focused on probe and scaffold creation.

The following areas are intentionally not created directly inside APT at this stage:

- Runtime trigger logic
- Voice isolation logic

## Usage notes

- When AngelPanel Core is present, APT can open inside AngelPanel.
- When AngelPanel Core is absent, APT works as a standalone window.
- Custom root names can be used to separate floors, rooms, or areas in the same scene.
- Use **Use Selection Name** to quickly derive a root name from the current selection.

## Requirements

- Unity 2022.3
- VRChat Worlds SDK 3.7.6 or newer

## Notes

- Optional extension status can be viewed in the panel.
