## 0.12.2

- Cleaned optional extension text in the UI to remove leftover dev wording.
- Prepared refreshed release package for VPM upload.

# Changelog

## 0.12.1

- Release prep cleanup for the first public VPM-ready build.
- Unified the package version and package manifest metadata.
- Updated the package ID to `com.2dangel.angelprobetools.core`.
- Fixed the result-mode option method regression.
- Kept the VRCSDK Worlds detector compatible with both naming variants used during the recent refactor.

## 0.12.0

- Added per-generator result handling so Reflection, LPG, and Trigger can each use different create/replace/update behavior.
- Kept the global result mode as the default path so the main panel stays simple.

## 0.11.0

- Added result handling modes for cleaner repeated workflows: create new, replace matching names, or update matching names.
- Added generation plan preview with estimated LPG probe counts and heavy-plan warning.
- Added last-run summary feedback so repeated passes feel more predictable.
- Improved root focusing behavior so the tool no longer creates a new shared root just from a focus action.

## 0.10.0

- Fixed the Optional Features compile error by restoring a compatibility path for earlier UI calls.
- Standardized generated object names so Reflection Probe, Light Probe Group, and Trigger Scaffold results are easier to read in the hierarchy.
- Improved generated Light Probe Group folder naming for cleaner grouping.

## 0.9.0

- Added a quick root naming flow with Use Selection Name and Reset actions.
- Added a compact selection summary in the Actions section.
- Reduced UI noise by converting several always-visible note boxes into lighter inline notes.
- Improved per-run root naming to use cleaner numbered suffixes.

## 0.8.0

- Simplified the main panel so generation and action areas stay dominant.
- Added a custom root name field so creators can separate probes by floor, room, or area.
- Made overview, extensions, and environment sections collapsed by default.
- Removed package-layout assumptions and kept the product aligned to the Assets-root workflow.
