# Angel Probe Tools

Free probe scaffolding toolkit for VRChat world creation under 2d Angel.

![Version](https://img.shields.io/badge/version-0.12.2-8a2be2)
![Unity](https://img.shields.io/badge/Unity-2022.3-black)
![VRChat Worlds](https://img.shields.io/badge/VRChat%20Worlds-%3E%3D3.7.6-1f6feb)
![Package](https://img.shields.io/badge/package-com.2dangel.angelprobetools.core-2ea44f)

**Author:** E-Mommy  
**Brand:** 2d Angel  
**Booth Store:** 2d Angel VRC

---

## Language / 言語 / 언어 / 语言

- [English](#english)
- [日本語](#日本語)
- [한국어](#한국어)
- [简体中文](#简体中文)
- [繁體中文](#繁體中文)

---

## Quick Links

- [Public GitHub Repository](https://github.com/AssenCypher/AngelProbeTools)
- [VPM Listing Repository](https://github.com/AssenCypher/2dAngel)
- [VPM Releases](https://github.com/AssenCypher/AngelProbeTools/releases)
- [Changelog](CHANGELOG.md)
- [License](LICENSE.md)

---

## Screenshots

> Replace these paths after you upload final screenshots.

```md
![APT Main Window](docs/images/apt-main-window.png)
![APT in AngelPanel](docs/images/apt-inside-ap.png)
![APT Generation Settings](docs/images/apt-generation-settings.png)
![APT Result Handling](docs/images/apt-result-handling.png)
```

---

# English

## Overview

Angel Probe Tools is a free editor-side probe utility built for VRChat world production under the 2d Angel ecosystem.

This package focuses on one thing: making probe-related setup faster, cleaner, and less annoying. Instead of manually rebuilding the same Reflection Probe, LightProbeGroup, bounds scaffold, and trigger scaffold structure again and again, APT lets you generate organized results from the current selection with a compact workflow.

APT is designed to work in two ways:

- as a standalone editor tool
- or as an AP-compatible tool page when AngelPanel Core is installed

APT is intentionally kept focused. It handles probe scaffolding and generation workflow. It does **not** try to turn itself into a bloated all-in-one lighting suite.

## What is included in Angel Probe Tools 0.12.2

- Reflection Probe generation
- LightProbeGroup generation
- Bounds expansion controls
- Zone scaffold / split workflow
- Optional Box Trigger scaffold generation
- Custom root naming for multi-area workflows
- Result handling modes for repeated generation
- Per-generator handling override
- Standalone window and AngelPanel-hosted page support
- Lightweight package detection for AP / VRChat workflow context

## Core Idea

APT is built around a practical problem.

When building VRChat worlds, probe work is repetitive. You often need to create organized probe setups for different floors, rooms, wings, or isolated areas. Doing this by hand is possible, but doing it repeatedly is slow, messy, and easy to lose track of.

APT turns that into a faster editor workflow:

1. Select the target objects.
2. Define how bounds should be interpreted.
3. Choose how zones should be created.
4. Choose which generators should run.
5. Generate organized results under a named root.

That is the core of the tool.

## Menu Path

After installation, open Angel Probe Tools from the Unity top menu:

`2dAngel -> Angel Probe Tools -> Open`

If AngelPanel Core is installed, APT can also appear inside AngelPanel under the Tools area.

## Installation

### Option A — VCC / Community Repository

1. Add the 2d Angel VPM repository to VCC / ALCOM.
2. Open your VRChat world project.
3. Add **Angel Probe Tools** to the project.
4. Wait for Unity import and script compilation to finish.
5. Open the tool from `2dAngel -> Angel Probe Tools`.

### Option B — Local package / development package

1. Open your Unity / VCC project.
2. Add the package that contains this `package.json`.
3. Import and resolve packages.
4. Open the tool from `2dAngel -> Angel Probe Tools`.

## Supported Baseline

- Unity: `2022.3`
- VRChat Worlds SDK: `>= 3.7.6`
- Package Name: `com.2dangel.angelprobetools.core`

## Feature Breakdown

### 1. Reflection Probe Generation

APT can generate Reflection Probes from the current selection bounds.

This is meant to speed up repetitive reflection setup for rooms, floors, sections, and other production zones. Instead of manually sizing and placing each probe from scratch, you can use the selected objects as the spatial source and let APT scaffold the result.

Useful for:

- room-by-room setup
- floor separation
- fast first-pass reflection coverage
- reorganizing probe layout during iteration

### 2. LightProbeGroup Generation

APT can generate a LightProbeGroup that attempts to cover the selected range more evenly.

The generation flow is built around practical editor use:

- it works from current selection bounds
- it can use expanded bounds
- it supports zone-based generation
- it is designed for repeated iteration rather than one-shot throwaway creation

This makes it much easier to rough in probe coverage for different sections of a world without rebuilding the same structure by hand every time.

### 3. Bounds Expansion

Bounds are often slightly too tight for probe workflow if they are taken straight from the current renderer set.

APT includes a bounds expansion control so you can push the working volume outward before generation. This is useful when you want:

- a little more breathing room around a room shell
- safer probe coverage near edges
- a broader scaffold for first-pass setup

### 4. Zone Modes

APT supports multiple ways to interpret the selected space.

Current workflow supports:

- combined bounds style generation
- per-selected-root style generation
- split / scaffold-driven generation flow

This matters because one selection does not always equal one logical probe area. Sometimes you want one root to control one room. Sometimes you want many selected roots to be combined into one shared area. Sometimes you want to break an area into multiple managed zones.

### 5. Trigger Scaffold

APT can optionally create a simple Box Trigger scaffold.

This is intentionally lightweight. It is a scaffold object for organization and spatial setup, not a full gameplay or runtime trigger system.

### 6. Custom Root Naming

One of the most useful quality-of-life features in APT is custom root naming.

You are not forced into a single hardcoded root for every generation pass. You can name your output root based on your actual production structure, for example:

- `1F_Auto Probes`
- `2F_Auto Probes`
- `Lobby Probes`
- `Room_A_Probes`

This makes APT much more comfortable for real-world iteration because different floors and rooms can remain clearly separated in the hierarchy.

### 7. Repeated Generation Without Hierarchy Chaos

Probe setup is rarely done once.

You generate, inspect, adjust bounds, change spacing, rerun, compare, refine, and rerun again. If the tool always creates a fresh mess every time, it becomes annoying very fast.

APT includes result handling modes to reduce that pain:

- always create new
- replace matching names
- update matching names

There is also a per-generator override path for more detailed control when needed.

### 8. Standalone + AngelPanel Integration

APT is not hard-dependent on AngelPanel Core.

If AP is not installed, APT works as a standalone window.  
If AP is installed, APT can appear inside AngelPanel as a hosted tool page while still keeping the option to open a separate standalone window.

### 9. Lightweight Expansion-Aware Design

APT is intentionally narrow in scope.

It is not trying to become a full lighting suite. Features that belong more naturally to a broader lighting product line are intentionally left outside the main APT workflow. That keeps the tool cleaner, easier to understand, and more comfortable for frequent use.

## Typical Use Cases

APT is especially useful for workflows like:

- roughing in initial probe coverage for a new room
- creating separate probe roots for multiple floors
- reorganizing probe hierarchy in an existing scene
- quickly iterating LPG layouts for different areas
- building cleaner editor-side scaffolding for production teams

## Why Angel Probe Tools Exists

Some tools try to solve everything and end up becoming cluttered.

APT does the opposite. It focuses on a smaller, recurring pain point in world production and turns that into a faster workflow with cleaner hierarchy output.

---

# 日本語

## 概要

Angel Probe Tools は、2d Angel エコシステム向けに作られた、VRChat ワールド制作用の無料エディター側 Probe 補助ツールです。

このパッケージは、Reflection Probe、LightProbeGroup、Bounds Scaffold、Trigger Scaffold などの反復作業を、より速く、整理された形で進めるために設計されています。毎回手作業で同じ構成を組み直すのではなく、現在の選択オブジェクトを基準に、まとまった生成フローで結果を作成できます。

APT は次の 2 つの形で使えます。

- 単体のエディターツールとして使用
- AngelPanel Core が導入されている場合は AngelPanel 内のツールページとして使用

APT は意図的に役割を絞っています。Probe の生成補助と Scaffold ワークフローに集中し、Lighting 全体を 1 ページに詰め込む設計にはしていません。

## Angel Probe Tools 0.12.2 の内容

- Reflection Probe 生成
- LightProbeGroup 生成
- Bounds 拡張
- Zone 分割 / Scaffold ワークフロー
- 任意の Box Trigger Scaffold 生成
- 複数エリア向けのカスタム Root 名設定
- 再生成向けの Result Handling モード
- Generator ごとの個別処理設定
- 単体ウィンドウ / AngelPanel 内表示の両対応
- AP / VRChat ワークフロー向けの軽量パッケージ検出

## 基本コンセプト

APT は、VRChat ワールド制作で繰り返し発生する Probe 設定作業を楽にするためのツールです。

たとえば、階ごと、部屋ごと、エリアごとに Reflection Probe や LightProbeGroup を整理して作りたい場面では、手動作業は毎回少しずつ面倒で、階層も散らかりやすくなります。

APT はその流れを次のようにまとめます。

1. 対象オブジェクトを選択する
2. Bounds の扱いを決める
3. Zone の作り方を選ぶ
4. 実行する Generator を選ぶ
5. 指定した Root の下に整理された結果を生成する

## メニューパス

インストール後、Unity 上部メニューから開けます。

`2dAngel -> Angel Probe Tools -> Open`

AngelPanel Core が入っている場合は、AngelPanel の Tools エリアにも表示できます。

## インストール

### Option A — VCC / Community Repository

1. 2d Angel の VPM リポジトリを VCC / ALCOM に追加します。
2. VRChat ワールドプロジェクトを開きます。
3. **Angel Probe Tools** を追加します。
4. Unity のインポートとスクリプトコンパイル完了まで待ちます。
5. `2dAngel -> Angel Probe Tools` から開きます。

### Option B — Local package / development package

1. Unity / VCC プロジェクトを開きます。
2. この `package.json` を含むパッケージを追加します。
3. パッケージを解決 / インポートします。
4. `2dAngel -> Angel Probe Tools` から開きます。

## 対応ベースライン

- Unity: `2022.3`
- VRChat Worlds SDK: `>= 3.7.6`
- Package Name: `com.2dangel.angelprobetools.core`

## 機能詳細

### 1. Reflection Probe 生成

現在の選択 Bounds を基準に Reflection Probe を生成できます。

部屋単位、階単位、区画単位などで反射カバレッジを素早く整えるための補助機能です。毎回サイズ調整と配置を一から行う代わりに、選択範囲を基準として Scaffold を作成できます。

### 2. LightProbeGroup 生成

現在の選択範囲を基に、より扱いやすい LightProbeGroup を生成できます。

この生成フローは、実際の反復作業を前提にしています。

- 現在の選択 Bounds を使用
- Expanded Bounds に対応
- Zone ベース生成に対応
- 使い捨てではなく、繰り返しの調整に向いた設計

### 3. Bounds 拡張

選択したオブジェクトの Renderer だけを基準にすると、Probe 用の作業範囲としては少し狭すぎることがあります。

APT では Bounds 拡張を行えるため、生成前に作業ボリュームを外側へ広げられます。

### 4. Zone モード

APT では選択空間の扱い方を複数選べます。

- 統合 Bounds ベース
- 選択 Root ごとの扱い
- 分割 / Scaffold ベースの生成フロー

1 つの選択が必ずしも 1 つの Probe エリアとは限らないため、この柔軟性は実用上かなり重要です。

### 5. Trigger Scaffold

必要に応じて、軽量な Box Trigger Scaffold を生成できます。

これはあくまで空間整理用の Scaffold であり、フル機能の runtime Trigger システムではありません。

### 6. カスタム Root 名

APT では出力 Root 名を自由に決められます。

たとえば：

- `1F_Auto Probes`
- `2F_Auto Probes`
- `Lobby Probes`
- `Room_A_Probes`

のように、実際の制作構造に沿った整理ができます。これにより、階層の見通しがかなり良くなります。

### 7. 再生成時の煩雑さを軽減

Probe 作業は一度で終わるものではありません。

生成して、確認して、Bounds や間隔を調整して、また生成する。その繰り返しです。APT はその反復時に Hierarchy が汚れにくいよう、Result Handling を用意しています。

- always create new
- replace matching names
- update matching names

必要なら Generator ごとの個別設定もできます。

### 8. Standalone + AngelPanel 連携

APT は AngelPanel Core に依存しません。

- AP が無い場合は単体ウィンドウとして動作
- AP がある場合は AngelPanel 内のツールページとして表示可能
- それでも必要なら単体ウィンドウも開ける

### 9. 拡張を見越した軽量設計

APT は役割を意図的に絞っています。

より大きな Lighting 系ツールに属するべき機能は、APT の中心ワークフローに無理に詰め込んでいません。これにより、日常的に使いやすい軽さを保っています。

---

# 한국어

## 개요

Angel Probe Tools는 2d Angel 생태계를 위한 VRChat 월드 제작용 무료 에디터 사이드 Probe 보조 도구입니다.

이 패키지는 Reflection Probe, LightProbeGroup, Bounds Scaffold, Trigger Scaffold 같은 반복적인 작업을 더 빠르고 정리된 방식으로 처리하기 위해 만들어졌습니다. 매번 손으로 같은 구조를 다시 만드는 대신, 현재 선택을 기준으로 정리된 생성 흐름을 사용할 수 있습니다.

APT는 두 가지 방식으로 사용할 수 있습니다.

- 독립적인 에디터 도구
- AngelPanel Core 설치 시 AngelPanel 내부 도구 페이지

APT는 의도적으로 역할을 좁게 유지합니다. Probe 생성과 Scaffold 워크플로에 집중하며, 모든 Lighting 기능을 한 페이지에 억지로 넣지 않습니다.

## Angel Probe Tools 0.12.2 포함 내용

- Reflection Probe 생성
- LightProbeGroup 생성
- Bounds 확장
- Zone 분할 / Scaffold 워크플로
- 선택적 Box Trigger Scaffold 생성
- 멀티 구역 작업을 위한 사용자 지정 Root 이름
- 반복 생성용 Result Handling 모드
- Generator별 개별 처리 모드
- 독립 창 / AngelPanel 내장 페이지 지원
- AP / VRChat 워크플로용 경량 패키지 감지

## 핵심 컨셉

VRChat 월드 제작에서는 Probe 관련 작업이 자주 반복됩니다.

층별, 방별, 구역별로 Probe를 정리해서 생성해야 할 때가 많고, 손으로 계속 반복하면 느리고 지저분해지기 쉽습니다.

APT는 그 흐름을 다음처럼 정리합니다.

1. 대상 오브젝트 선택
2. Bounds 처리 방식 결정
3. Zone 생성 방식 선택
4. 실행할 Generator 선택
5. 지정한 Root 아래에 정리된 결과 생성

## 메뉴 경로

설치 후 Unity 상단 메뉴에서 열 수 있습니다.

`2dAngel -> Angel Probe Tools -> Open`

AngelPanel Core가 설치된 경우 AngelPanel의 Tools 영역에도 표시될 수 있습니다.

## 설치

### Option A — VCC / Community Repository

1. 2d Angel VPM 저장소를 VCC / ALCOM에 추가합니다.
2. VRChat 월드 프로젝트를 엽니다.
3. **Angel Probe Tools**를 추가합니다.
4. Unity import 및 스크립트 컴파일이 끝날 때까지 기다립니다.
5. `2dAngel -> Angel Probe Tools`에서 엽니다.

### Option B — Local package / development package

1. Unity / VCC 프로젝트를 엽니다.
2. 이 `package.json`이 포함된 패키지를 추가합니다.
3. 패키지를 import / resolve 합니다.
4. `2dAngel -> Angel Probe Tools`에서 엽니다.

## 지원 기준

- Unity: `2022.3`
- VRChat Worlds SDK: `>= 3.7.6`
- Package Name: `com.2dangel.angelprobetools.core`

## 기능 구성

### 1. Reflection Probe 생성

현재 선택 Bounds를 기준으로 Reflection Probe를 생성할 수 있습니다.

방, 층, 구역 단위로 반사 커버리지를 빠르게 잡아야 할 때 반복 작업을 줄여 줍니다.

### 2. LightProbeGroup 생성

현재 선택 범위를 기반으로 더 다루기 쉬운 LightProbeGroup을 생성할 수 있습니다.

이 생성 흐름은 실제 반복 작업을 전제로 만들어졌습니다.

- 현재 선택 Bounds 사용
- Bounds 확장 지원
- Zone 기반 생성 지원
- 일회성보다 반복 조정에 적합한 흐름

### 3. Bounds 확장

선택한 Renderer 범위만 그대로 사용할 경우 Probe 작업 범위로는 조금 타이트할 수 있습니다.

APT는 Bounds 확장 기능을 제공하므로 생성 전에 작업 볼륨을 바깥으로 넓힐 수 있습니다.

### 4. Zone 모드

APT는 선택 공간을 여러 방식으로 해석할 수 있습니다.

- 통합 Bounds 기반
- 선택 Root별 처리
- 분할 / Scaffold 기반 생성 흐름

현실적인 월드 제작에서는 하나의 선택이 하나의 Probe 영역과 정확히 일치하지 않는 경우가 많기 때문에 이 유연성이 중요합니다.

### 5. Trigger Scaffold

원하면 가벼운 Box Trigger Scaffold를 생성할 수 있습니다.

이것은 runtime 게임플레이 트리거 시스템이 아니라, 공간 정리와 시각화 보조용 Scaffold입니다.

### 6. 사용자 지정 Root 이름

APT에서는 출력 Root 이름을 직접 지정할 수 있습니다.

예시:

- `1F_Auto Probes`
- `2F_Auto Probes`
- `Lobby Probes`
- `Room_A_Probes`

실제 제작 구조에 맞춰 계층을 정리할 수 있어서 반복 작업 시 훨씬 편합니다.

### 7. 반복 생성 시 계층 정리

Probe 작업은 보통 한 번으로 끝나지 않습니다.

생성하고, 확인하고, Bounds나 간격을 조정하고, 다시 생성합니다. APT는 이 반복 과정에서 Hierarchy가 과하게 더러워지지 않도록 Result Handling을 제공합니다.

- always create new
- replace matching names
- update matching names

필요하면 Generator별 개별 처리도 가능합니다.

### 8. Standalone + AngelPanel 연동

APT는 AngelPanel Core가 없어도 동작합니다.

- AP가 없으면 독립 창으로 사용
- AP가 있으면 AngelPanel 내부 페이지로 사용 가능
- 필요하면 별도 창도 계속 열 수 있음

### 9. 확장을 고려한 경량 설계

APT는 의도적으로 범위를 좁게 유지합니다.

더 넓은 Lighting 제품군에 어울리는 기능은 APT 중심 흐름에 억지로 넣지 않았습니다. 그래서 더 가볍고 자주 쓰기 편한 도구로 남습니다.

---

# 简体中文

## 概述

Angel Probe Tools 是 2d Angel 生态下，面向 VRChat 世界制作的免费编辑器侧 Probe 辅助工具。

这个包的目标很明确：把 Reflection Probe、LightProbeGroup、Bounds Scaffold、Trigger Scaffold 这类反复出现的工作，变得更快、更整洁、更不烦。你不需要每次都手动重新摆一遍相同的结构，而是可以基于当前选择对象，快速生成一套更有组织的结果。

APT 有两种使用方式：

- 作为独立编辑器工具单独使用
- 在安装 AngelPanel Core 时，作为 AngelPanel 内的工具页使用

APT 是刻意做得更聚焦的。它专注于 Probe 生成与 Scaffold 工作流，不会把所有 Lighting 或运行时功能都硬塞进同一页里。

## Angel Probe Tools 0.12.2 包含内容

- Reflection Probe 生成
- LightProbeGroup 生成
- Bounds 外扩
- Zone 拆分 / Scaffold 工作流
- 可选 Box Trigger Scaffold 生成
- 面向多区域工作流的自定义根名称
- 面向重复生成的 Result Handling 模式
- 按生成器细分的单独处理模式
- 独立窗口 / AngelPanel 内嵌页双形态支持
- 面向 AP / VRChat 工作流的轻量环境检测

## 核心思路

APT 解决的是一个很实际的问题。

在 VRChat 世界制作里，Probe 相关工作经常重复出现。你会不断地按楼层、房间、区域去搭 Reflection Probe 或 LightProbeGroup。手动做不是做不了，但做多了很慢、很乱，也很容易在层级里丢失结构感。

APT 把这套流程收成了更直接的一条线：

1. 选择目标对象
2. 决定 Bounds 的解释方式
3. 选择 Zone 的生成方式
4. 选择要执行哪些生成器
5. 在指定的根节点下生成整理好的结果

## 菜单路径

安装完成后，可以从 Unity 顶部菜单打开：

`2dAngel -> Angel Probe Tools -> Open`

如果项目里安装了 AngelPanel Core，APT 也可以作为 AngelPanel 的 Tools 页出现。

## 安装方式

### 方式 A — VCC / Community Repository

1. 在 VCC / ALCOM 中添加 2d Angel 的 VPM 仓库。
2. 打开你的 VRChat 世界项目。
3. 将 **Angel Probe Tools** 添加到项目中。
4. 等待 Unity 导入和脚本编译完成。
5. 从 `2dAngel -> Angel Probe Tools` 打开工具。

### 方式 B — 本地包 / 开发包

1. 打开 Unity / VCC 项目。
2. 添加包含这个 `package.json` 的包。
3. 完成包解析与导入。
4. 从 `2dAngel -> Angel Probe Tools` 打开工具。

## 支持基线

- Unity: `2022.3`
- VRChat Worlds SDK: `>= 3.7.6`
- Package Name: `com.2dangel.angelprobetools.core`

## 功能拆解

### 1. Reflection Probe 生成

APT 可以基于当前选择范围的 Bounds 生成 Reflection Probe。

它适合用来加快那种一遍又一遍重复出现的反射覆盖搭建工作，比如按房间、按楼层、按区域快速铺出第一轮 Reflection Probe 结构，而不是每次都从零开始调位置和大小。

### 2. LightProbeGroup 生成

APT 可以基于当前选择范围生成更易于使用的 LightProbeGroup。

这个生成逻辑是围绕“反复迭代”来设计的：

- 基于当前选择 Bounds
- 支持 Bounds 外扩
- 支持按 Zone 生成
- 适合一轮轮调整，而不是只用一次的临时生成

### 3. Bounds 外扩

如果完全照当前 Renderer 的范围去做 Bounds，很多时候对 Probe 工作来说会显得有点太紧。

APT 提供 Bounds 外扩，用来在生成前把工作体积往外推开一些。这样在做房间边缘、外壳或更宽一点的首轮覆盖时会更舒服。

### 4. Zone 模式

APT 支持多种空间解释方式。

当前工作流支持：

- 合并 Bounds 方式
- 按选中 Root 分开处理
- 拆分 / Scaffold 风格生成

因为一个选择不一定就等于一个逻辑上的 Probe 区域，所以这个灵活度在真实项目里非常重要。

### 5. Trigger Scaffold

APT 可以按需生成轻量级 Box Trigger Scaffold。

它本质上是一个空间整理和可视化 scaffold，而不是完整的运行时玩法 Trigger 系统。

### 6. 自定义根名称

APT 里有一个很实用的体验点，就是自定义输出根名称。

你不必所有区域都强行堆在一个固定根下面。你可以按照真实项目结构去命名，比如：

- `1F_Auto Probes`
- `2F_Auto Probes`
- `Lobby Probes`
- `Room_A_Probes`

这样在多楼层、多房间、多区块迭代时，层级会清楚很多。

### 7. 重复生成时不再越来越乱

Probe 工作几乎不会只做一次。

你通常会经历：

- 生成
- 查看
- 调 Bounds
- 调 Grid Step
- 再生成
- 再比较

如果工具每次都只是无脑继续堆新对象，那很快就会变得很烦。

APT 提供了 Result Handling 模式来缓解这个问题：

- always create new
- replace matching names
- update matching names

同时也支持按生成器分别处理，让你在不同对象上采用不同策略。

### 8. 独立工具 + AngelPanel 集成

APT 不依赖 AngelPanel Core 才能工作。

- 没有 AP 时，它就是一个独立窗口
- 有 AP 时，它可以作为 AngelPanel 的内嵌工具页出现
- 同时仍然可以保留单独窗口入口

### 9. 面向扩展的轻量化设计

APT 的边界是刻意收窄的。

它不是想变成一套把所有 Lighting 功能都吞进去的重工具。更适合放进大 Lighting 产品线里的功能，被有意留在外部，这样 APT 本体才能保持干净、直观、适合高频使用。

## 典型使用场景

APT 尤其适合这些场景：

- 给新房间快速铺第一轮 Probe 覆盖
- 为不同楼层创建独立 Probe 根
- 清理并重构旧场景里的 Probe 层级
- 快速迭代不同区域的 LPG 布局
- 给团队项目建立更干净的编辑器侧 Probe Scaffold

## 为什么会有 Angel Probe Tools

有些工具试图什么都做，最后就会越来越杂。

APT 反过来只做一件更小、但非常常见的事：把 Probe 搭建这块反复出现的工作，做得更快、更整洁、更适合长期反复使用。

---

# 繁體中文

## 概述

Angel Probe Tools 是 2d Angel 生態下，面向 VRChat 世界製作的免費編輯器側 Probe 輔助工具。

這個套件的目標很明確：把 Reflection Probe、LightProbeGroup、Bounds Scaffold、Trigger Scaffold 這類反覆出現的工作，變得更快、更整潔、更不煩。你不需要每次都手動重建一遍相同結構，而是可以基於目前的選取物件，快速生成一套更有組織的結果。

APT 有兩種使用方式：

- 作為獨立編輯器工具單獨使用
- 在安裝 AngelPanel Core 時，作為 AngelPanel 內的工具頁使用

APT 是刻意保持聚焦的。它專注於 Probe 生成與 Scaffold 工作流，不會把所有 Lighting 或執行時功能都硬塞進同一頁。

## Angel Probe Tools 0.12.2 包含內容

- Reflection Probe 生成
- LightProbeGroup 生成
- Bounds 外擴
- Zone 拆分 / Scaffold 工作流
- 可選 Box Trigger Scaffold 生成
- 面向多區域工作流的自訂根名稱
- 面向重複生成的 Result Handling 模式
- 依生成器細分的單獨處理模式
- 獨立視窗 / AngelPanel 內嵌頁雙形態支援
- 面向 AP / VRChat 工作流的輕量環境檢測

## 核心思路

APT 解決的是一個非常實際的問題。

在 VRChat 世界製作中，Probe 相關工作會反覆出現。你會不斷地依樓層、房間、區域去建立 Reflection Probe 或 LightProbeGroup。手動做不是做不到，但做多了很慢、很亂，也很容易在層級裡失去結構感。

APT 把這條流程整理成更直接的一套：

1. 選取目標物件
2. 決定 Bounds 的解讀方式
3. 選擇 Zone 的建立方式
4. 選擇要執行的生成器
5. 在指定根節點下生成整理好的結果

## 菜單路徑

安裝完成後，可以從 Unity 頂部選單開啟：

`2dAngel -> Angel Probe Tools -> Open`

如果專案裡安裝了 AngelPanel Core，APT 也可以作為 AngelPanel 的 Tools 頁出現。

## 安裝方式

### 方式 A — VCC / Community Repository

1. 在 VCC / ALCOM 中加入 2d Angel 的 VPM 倉庫。
2. 開啟你的 VRChat 世界專案。
3. 將 **Angel Probe Tools** 加入專案。
4. 等待 Unity 匯入與腳本編譯完成。
5. 從 `2dAngel -> Angel Probe Tools` 開啟工具。

### 方式 B — 本地套件 / 開發套件

1. 開啟 Unity / VCC 專案。
2. 加入包含這個 `package.json` 的套件。
3. 完成套件解析與匯入。
4. 從 `2dAngel -> Angel Probe Tools` 開啟工具。

## 支援基線

- Unity: `2022.3`
- VRChat Worlds SDK: `>= 3.7.6`
- Package Name: `com.2dangel.angelprobetools.core`

## 功能拆解

### 1. Reflection Probe 生成

APT 可以根據目前選取範圍的 Bounds 生成 Reflection Probe。

它適合用來加速那種一再出現的反射覆蓋建立工作，例如依房間、樓層、區域快速鋪出第一輪 Reflection Probe 結構，而不必每次都從零開始調整大小與位置。

### 2. LightProbeGroup 生成

APT 可以根據目前選取範圍生成更容易使用的 LightProbeGroup。

這套生成邏輯是圍繞「反覆迭代」設計的：

- 基於目前選取 Bounds
- 支援 Bounds 外擴
- 支援依 Zone 生成
- 適合一輪輪調整，而不是只用一次的臨時生成

### 3. Bounds 外擴

如果完全依照目前 Renderer 的範圍去取 Bounds，很多時候對 Probe 工作來說會顯得有點太緊。

APT 提供 Bounds 外擴，讓你在生成前先把工作體積往外推開一些。這對房間邊界、外殼或更寬鬆的首輪覆蓋很有幫助。

### 4. Zone 模式

APT 支援多種空間解讀方式。

目前工作流支援：

- 合併 Bounds 方式
- 依選取 Root 分開處理
- 拆分 / Scaffold 風格生成

因為一個選取不一定就等於一個邏輯上的 Probe 區域，所以這種彈性在真實專案裡非常重要。

### 5. Trigger Scaffold

APT 可以按需生成輕量級 Box Trigger Scaffold。

它本質上是空間整理與視覺化用的 scaffold，而不是完整的執行時玩法 Trigger 系統。

### 6. 自訂根名稱

APT 有一個非常實用的體驗點，就是自訂輸出根名稱。

你不必把所有區域都堆在同一個固定根底下。你可以按照真實專案結構命名，例如：

- `1F_Auto Probes`
- `2F_Auto Probes`
- `Lobby Probes`
- `Room_A_Probes`

這會讓多樓層、多房間、多區塊的迭代過程更清楚。

### 7. 重複生成時不再越來越亂

Probe 工作幾乎不會只做一次。

你通常會經歷：

- 生成
- 檢查
- 調整 Bounds
- 調整 Grid Step
- 再生成
- 再比較

如果工具每次都只是繼續堆新物件，很快就會變得很煩。

APT 提供了 Result Handling 模式來緩解這件事：

- always create new
- replace matching names
- update matching names

同時也支援依生成器分別處理，讓你能對不同輸出採用不同策略。

### 8. 獨立工具 + AngelPanel 整合

APT 不依賴 AngelPanel Core 才能運作。

- 沒有 AP 時，它就是一個獨立視窗
- 有 AP 時，它可以作為 AngelPanel 的內嵌工具頁出現
- 同時仍保留獨立視窗入口

### 9. 面向擴展的輕量化設計

APT 的邊界是刻意收窄的。

它不是要變成一套把所有 Lighting 功能都吞進去的重工具。更適合放進大型 Lighting 產品線的功能，被有意留在外部，這樣 APT 本體才能保持乾淨、直觀、適合高頻使用。

---

## License

This project is distributed under the terms described in [LICENSE.md](LICENSE.md).

## Notes

- Angel Probe Tools is editor-focused.
- Runtime gameplay systems are outside the scope of this package.
- AngelPanel Core is optional, but supported.
