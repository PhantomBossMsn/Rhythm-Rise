# Rhythm-Rise 🎵🏃

[![itch.io](https://img.shields.io/badge/Play%20on-itch.io-FA5C5C?style=for-the-badge&logo=itchdotio&logoColor=white)](https://sft-dev.itch.io/ry)
[![GitHub](https://img.shields.io/badge/Repository-Rhythm--Rise-181717?style=for-the-badge&logo=github&logoColor=white)](https://github.com/PhantomBossMsn/Rhythm-Rise)
[![Unity](https://img.shields.io/badge/Engine-Unity%202022%2F2023-black?style=for-the-badge&logo=unity&logoColor=white)](https://unity.com/)

**Rhythm-Rise** is an adrenaline-pumping, first-person hybrid experience that combines **high-speed 3D parkour movement** with **sample-accurate timed rhythm gameplay**.

Navigate neon-lit corridors, sprint along vertical surfaces with fluid wall-running mechanics, leap over death hazards, and match beats in high-energy rhythm challenges with real-time audio synchronization and visual shader feedback.

🎮 **Play the Live Windows Build on itch.io**: [https://sft-dev.itch.io/ry](https://sft-dev.itch.io/ry)  
👤 **Developer / Creator**: [@PhantomBossMsn](https://github.com/PhantomBossMsn)  
📁 **Repository**: [https://github.com/PhantomBossMsn/Rhythm-Rise](https://github.com/PhantomBossMsn/Rhythm-Rise)

---

## 🕹️ Game Overview & Flow

The game blends parkour platforming and timed musical challenges into a seamless narrative campaign:

1. **Starting Parkour Routine (`Scene1(parkore)`)**
   - The game launches with an intense first-person parkour section.
   - Master wall-running, bunny-hopping, sliding across slopes, and dodging bottomless pits and hazard zones before entering the rhythm arenas.
2. **First Wave Rhythm Challenges (`Game1(ftMAN)` & `Game2(ftNIGHT)`)**
   - Enter musical sequence tracks. Hit incoming beat targets precisely on time (`E` key).
   - Real-time physics trigger spheres determine hit accuracy (**Perfect**, **Alright**, or **Miss**).
   - Dynamic camera angles lerp cinematically between beats and narrative character focal points.
3. **Mid-Game Parkour Routine (`2Scene1(parkore)`)**
   - In the middle of the game, players are thrust back into an expanded parkour routine.
   - Tests traversal mastery under tighter movement constraints, vertical wall-jump sequences, and interactive triggers before the climactic stages.
4. **Climax Rhythm Stages (`Game3(ftBot)`, `Game4(ftCar)`, `Game5(ftMOTEL)`)**
   - Advanced multi-track rhythm patterns, dual-lane beat rhythms, cutscenes, and boss-level beat density.
5. **Score Grading & Rank Transmission (`TransmisionRANK`)**
   - Evaluates performance accuracy percentage into stylized ranks (**S, A, B, C, D, E, F**).

---

## ⌨️ Controls & Keybindings

### Parkour Routine Controls
| Action | Key / Input |
|---|---|
| **Move Forward / Backward / Strafe** | `W` / `A` / `S` / `D` |
| **Look Around / Aim** | `Mouse Movement` |
| **Jump** | `Spacebar` |
| **Sprint** | `Left Shift` (Hold) |
| **Wall Run** | Run against a wall while airborne |
| **Wall Jump** | `Spacebar` while wall-running |
| **Interact / Open Doors / Teleport** | `E` |

### Rhythm Mode Controls
| Action | Key / Input |
|---|---|
| **Hit Beat Target** | `E` |
| **Restart Scene** | UI Button / Mouse Click |
| **Continue / Next Scene** | UI Button / Mouse Click |

---

## 📜 Detailed Script-by-Script Breakdown

Every script in the codebase is documented below, grouped by domain responsibility.

```
Assets/
├── BeatDeath.cs
├── Event1.cs
├── Event2.cs
├── lookAT.cs
├── QuitGame.cs
├── ShaderSET.cs
├── TransmisionRANK.cs
├── Musics/
│   ├── BeatManager.cs
│   ├── ONERytBeatManager.cs
│   ├── TWORytBeatManager.cs
│   ├── PulseToTheBeat.cs
│   └── TypeWriterEffect.cs
├── WALLRUNER/
│   ├── PlayerMovement.cs
│   ├── WallRun.cs
│   ├── PlayerLook.cs
│   └── MoveCamera.cs
└── TutorialInfo/Scripts/
    ├── Readme.cs
    └── Editor/ReadmeEditor.cs
```

---

### 1. Rhythm & Music Gameplay Scripts (`Assets/Musics/`)

#### 🎼 `BeatManager.cs`
- **Purpose**: Low-level audio sample tracker and BPM interval generator.
- **How it Works**:
  - Samples the current track position using `_audioSource.timeSamples` against `_audioSource.clip.frequency`.
  - Calculates beat length with `60f / (bpm * _steps)`.
  - Fires serialized `UnityEvent` triggers whenever the playback clock enters a new interval step, allowing musical event dispatching without frame-rate drift.

#### 🎯 `ONERytBeatManager.cs`
- **Purpose**: Controller for single-beat rhythm levels.
- **How it Works**:
  - Orchestrates note spawning via a coroutine `ScheduleInvokes(TimeInterval, Repeat, TimeDelay, SpawnBeat)`.
  - Fires synchronized narrative triggers to `Event1` using `EventCaller(delay, id)`.
  - On player pressing `E`:
    - Checks overlap spheres against `triggerPERFECT` and `triggerALRIGHT` layers.
    - **Perfect Hit**: Adds `+1` to `AccuracyState`, plays clap audio, triggers visual burst (`Beateffect`), switches headphone UI sprite to `UIP` (Perfect), and notifies `Event1(-1)`.
    - **Alright Hit**: Adds `+0.5` to `AccuracyState`, switches headphone UI to `UIA` (Alright), and notifies `Event1(-2)`.
    - **Miss**: Subtracts `-0.5` from `AccuracyState`, triggers `UIS` (Shit/Miss) sprite.
  - Automatically resets headphone sprite to idle (`UII`) after 0.5s.

#### 🔀 `TWORytBeatManager.cs`
- **Purpose**: Controller for dual-lane / alternating beat rhythm levels.
- **How it Works**:
  - Manages two distinct beat animators and target prefabs (`Beat1` and `Beat2`).
  - Spawns notes according to boolean flags `IsB1[index]` to switch between alternating sides.
  - Automatically kills notes with configurable lifespans (`Killb1`, `Killb2`) if missed.
  - When the player hits `E`, checks overlap for whichever beat is active, updates accuracy score (`+1f`, `+0.5f`, `-0.5f`), and invokes camera choreography IDs (`10` / `-10`) on `Event1`.

#### 💓 `PulseToTheBeat.cs`
- **Purpose**: Visual beat-pumping effect for reactive 3D scene props and UI elements.
- **How it Works**:
  - On receiving `Pulse()`, instantaneously enlarges the object's transform to `_startSize * _pulseSize` (e.g. 115%).
  - In `Update()`, smoothly interpolates back down to baseline scale using `Vector3.Lerp` at `_returnSpeed`.
  - Includes a test coroutine (`TestBeat`) for tuning scale and response without starting an audio track.

#### 💬 `TypeWriterEffect.cs`
- **Purpose**: Atmospheric narrative dialogue system using TextMeshPro.
- **How it Works**:
  - Reveals text character-by-character based on `typingSpeed` after a configurable `startDelay`.
  - Plays an optional keystroke sound effect (`typingAudioSource.PlayOneShot(typingSound)`) with each character.
  - Enables a follow-up GameObject (`objectToActivate`) once typing begins, allowing sequential UI/cinematic progression.

---

### 2. Parkour & Movement Engine (`Assets/WALLRUNER/`)

#### 🏃‍♂️ `PlayerMovement.cs`
- **Purpose**: Rigidbody-based first-person movement controller tuned for fast parkour.
- **Key Features**:
  - **Ground & Slope Detection**: Uses sphere checks (`groundCheck`, `groundDistance`, `groundMask`) and surface normal raycasts (`OnSlope()`) to project movement along slopes (`Vector3.ProjectOnPlane`).
  - **Physics Drag Tuning**: Applies high drag (`groundDrag = 6`) when grounded for crisp stopping, and low drag (`airDrag = 2`) in mid-air to preserve jump momentum.
  - **Sprinting & Acceleration**: Smoothly lerps between `walkSpeed` and `sprintSpeed` upon holding `LeftShift`.
  - **Jumping**: Resets vertical velocity and applies an instant upward impulse force (`jumpForce`).
  - **Animation Hook**: Updates the character animator integer `"states"` (0 for Idle, 1 for Running).

#### 🧗 `WallRun.cs`
- **Purpose**: Titanfall-style wall-running and wall-jumping mechanics.
- **How it Works**:
  - Uses lateral raycasts (`-orientation.right` and `orientation.right`) to detect walls when airborne.
  - Disables standard gravity and applies a custom gentle downward force (`wallRunGravity`).
  - Smoothly expands the camera field-of-view (`wallRunfov`) and rolls the camera (`tilt`) left or right.
  - Sets character animation states (State `2` for left wall run, State `3` for right wall run, State `4` for airborne fall).
  - Pressing `Spacebar` while wall-running calculates a launch vector combining upward impulse and the wall's surface normal (`transform.up + wallHit.normal`), kicking the player off the wall.

#### 👁️ `PlayerLook.cs`
- **Purpose**: First-person mouse look and camera rotation manager.
- **Key Features**:
  - Locks and hides the cursor on start (`CursorLockMode.Locked`).
  - Handles mouse delta input with configurable sensitivities (`sensX`, `sensY`).
  - Clamps vertical pitch between `-90°` and `+90°`.
  - Integrates dynamic camera tilt from `WallRun.tilt` to deliver realistic camera roll during wall runs.
  - Rotates player orientation yaw independently from camera pitch.

#### 🎥 `MoveCamera.cs`
- **Purpose**: Decoupled camera mount.
- **How it Works**:
  - Snaps the main camera transform position to a designated head/anchor transform (`cameraPosition`) in `Update()`.
  - Prevents physics-induced camera jitter caused by Rigidbody interpolation.

---

### 3. Level Logic, Cinematics & Events (`Assets/`)

#### 💀 `BeatDeath.cs`
- **Purpose**: Note despawn and auto-recycle manager.
- **How it Works**:
  - Supports timer-based deactivation (`Invoke("off", time)`).
  - Supports collision-based deactivation (`KillbyTrig`) using `Physics.CheckSphere` with `triggerSHIT`.
  - Deactivates the GameObject and resets position to `Vector3.zero` (unless `NonVectReset` is active).

#### 🎬 `Event1.cs`
- **Purpose**: Master level sequence dispatcher and camera choreography manager for rhythm levels.
- **Handled Events (`EventHandler(id)`)**:
  - `id == -1`: Beat hit flash effect (sets `shader2` to color preset).
  - `id == -2`: Beat miss/off-beat flash effect.
  - `id == 1`: Starts camera smooth-lerp towards focus target (`ManPos`).
  - `id == 2`: Lerps camera back to `defualtPos`.
  - `id == 3`: Disables rhythm manager and initiates fade-out transition.
  - `id == 4` / `5`: **Level 2 (NightBus)** – Activates bus sequence and triggers end transition.
  - `id == 6`: **Level 3 (Roby)** – Triggers robot animation state.
  - `id == 7`: Plays cutscene video player (`VidPlayer.Play()`) and activates focal actor.
  - `id == 8`: Reverts camera focus.
  - `id == 9`: Level end sequence after 5 seconds.
  - `id == 10` / `-10`: **Level 4 (Car)** – Directional camera shift on alternating beats.
  - `id == 11`: **Level 5 (Hotel)** – Final transition to next scene.

#### 🚪 `Event2.cs`
- **Purpose**: Interaction and trigger management for the parkour and exploration sequences.
- **Features**:
  - Continuously samples surroundings using `Physics.CheckSphere`:
    - `doorLAYER`: Triggers automatic sliding doors (`DoorAnimator`).
    - `triggerLAYER` / `CARLAYER`: Displays interaction prompt (`Eshocase`) and handles `E` key teleports / transitions.
    - `triggerLAYER2`: Launches dialogue UI (`UITALKER`) and transitions scene after 13s.
    - `DeathLayer`: Instant hazard detection — reloads the parkour stage (`SceneNum`) when the player falls into void/spikes.
    - `ForcedTrigger`: Locks player movement, plays narrative cinematic (`wsgAnime`), and advances to the next scene.
    - `ForcedTrigger2` / `ForcedTrigger3`: Disables kill zones and executes smooth scene transitions.

#### 🏆 `TransmisionRANK.cs`
- **Purpose**: End-of-level performance ranking and score screen.
- **How it Works**:
  - Pulls `AccuracyState` from either `ONERytBeatManager` or `TWORytBeatManager`.
  - Calculates accuracy percentage (`currentAccuracy * 100f`, capped at 100%).
  - Awards stylized ranks:
    - **S**: $\ge 95\%$
    - **A**: $\ge 90\%$
    - **B**: $\ge 80\%$
    - **C**: $\ge 70\%$
    - **D**: $\ge 60\%$
    - **E**: $\ge 50\%$
    - **F**: $< 50\%$
  - Renders formatted statistics in TextMeshPro (`Accuracy: XX.X% | Grade: X`).
  - Provides button callbacks: `RestartScene()` and `LoadScene()`.

#### 🎨 `ShaderSET.cs`
- **Purpose**: Procedural material tint initializer.
- **How it Works**:
  - Sets the base color of an assigned `Material` to `(r, g, b)` upon `Start()`.

#### 🎯 `lookAT.cs`
- **Purpose**: Billboarding and directional tracking.
- **How it Works**:
  - Aligns the GameObject's forward vector directly toward a `target` Transform every frame via `transform.LookAt(target)`.

#### 🚪 `QuitGame.cs`
- **Purpose**: Graceful desktop exit helper.
- **How it Works**:
  - Public method `QUIT()` invoking `Application.Quit()` for main menu buttons.

---

### 4. Project Documentation Helpers (`Assets/TutorialInfo/`)

#### 📖 `Readme.cs` & `ReadmeEditor.cs`
- **Purpose**: Unity Editor Inspector metadata presenter.
- Custom inspector script rendering rich formatting, icon headings, and external documentation buttons within the Unity Inspector.

---

## 🗺️ Scene Architecture

The game is organized as a sequential build pipeline:

```
[Menu] ──► [Scene1(parkore)] ──► [cutscene1] ──► [Game1(ftMAN)] ──► [cutscene2]
             (Starting Parkour)                     (Rhythm Level 1)
                                                           │
┌──────────────────────────────────────────────────────────┘
▼
[Game2(ftNIGHT)] ──► [cutscene3] ──► [2Scene1(parkore)] ──► [cutscene4]
 (Rhythm Level 2)                      (Mid-Game Parkour)
                                               │
┌──────────────────────────────────────────────┘
▼
[Game3(ftBot)] ──► [Game4(ftCar)] ──► [Game5(ftMOTEL)] ──► [Last]
 (Rhythm Level 3)   (Rhythm Level 4)   (Rhythm Level 5)     (Ending)
```

---

## 🛠️ How to Build and Run

### Running the Live Build
1. Visit the itch.io release page: **[https://sft-dev.itch.io/ry](https://sft-dev.itch.io/ry)**
2. Download the Windows zip package.
3. Extract and launch `RythemGame.exe`.

### Opening in Unity Editor
1. Ensure **Unity 2022.3 LTS** (or compatible Unity 2022/2023 version) is installed via Unity Hub.
2. Clone this repository:
   ```bash
   git clone https://github.com/PhantomBossMsn/Rhythm-Rise.git
   ```
3. Open Unity Hub, click **Add**, and select the cloned `Rhythm-Rise` project folder.
4. Open `Assets/Menu.unity` or `Assets/Scene1(parkore).unity`.
5. Press the **Play** button in the Unity Editor toolbar.

---

## 📄 License & Credits

- **Game Design & Programming**: [@PhantomBossMsn](https://github.com/PhantomBossMsn)
- **Live Build**: [itch.io/ry](https://sft-dev.itch.io/ry)
- Created for game jam and rhythm-action enthusiasts.

