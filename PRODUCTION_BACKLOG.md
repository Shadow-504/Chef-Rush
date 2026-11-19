# Chef Rush production backlog

**Team size:** 3 developers · **Status:** Proposed · **Design reference:** [Chef Rush GDD](GDD.md)

Start with two online riders, one fork, and one ingredient exchange. The vertical slice expands this into Festival Pass, where 2–4 players can cook, deliver, and replay a full descent. The release plan expands to three routes and six recipes after the slice passes playtesting.

## Team ownership

| Person | Primary ownership | Regular handoff |
| --- | --- | --- |
| **A — Gameplay and networking** | Engine and builds, movement, multiplayer, inventory and cooking systems, saves and platform integration | Gives B editable route, recipe, and ticket assets. Gives C gameplay events and view state for UI, sound, and effects. |
| **B — Game and level design** | Game rules, route layouts, timing, order schedules, progression, playtest coordination | Gives A system rules and acceptance cases. Gives C route landmarks, sign needs, content lists, and feedback priorities. |
| **C — UI and audiovisual** | UX, HUD, menus, accessibility presentation, art direction, asset integration, SFX and VFX | Gives A interaction flows and cue requirements. Gives B reusable environment pieces and visual signage. |

These are proposed assignments. Map A, B, and C to teammates according to their strengths. Everyone reviews playable builds and joins multiplayer playtests. For four-player checks, recruit a fourth tester. B owns build acceptance, with a second person verifying fixes that affect gameplay.

### How to use this list

- Each task has a stable ID. Mark table tasks complete by replacing ☐ with ☑. Copy it into an issue with one owner and the acceptance condition. Split oversized items into subtasks as needed.
- **After** identifies the task that needs to land before final integration. Design sketches, sample assets, and test cases can begin earlier.
- Keep one main task active per person. Start with the first milestone, then re-estimate based on actual capacity and playtest findings.
- Use engine primitives, placeholder sounds, and simple icons through the first handoff test. Engine, hosting provider, and production schedule are open decisions.

## M0 — Online downhill handoff prototype

| ID | Owner | After | Task and acceptance |
| --- | --- | --- | --- |
| P01 | A | — | ☐ Choose an engine after checking PC builds, input, route splines, and network simulation. Set project structure, version control rules for binary assets, and a repeatable build command. Two teammates launch the same build on separate machines. |
| P02 | B | — | ☐ Specify chef and courier controls, cart and bike speed ranges, zone sizes, and the first handoff rules. Sketch a short route with two lanes and a shared merge. Define success, full-cart, and missed-window examples. |
| P03 | C | P02 | ☐ Add temporary vehicle silhouettes, follow camera framing, ingredient icons, merge marker, and handoff feedback. A player can tell which rider is the chef and which lane leads to the merge. |
| P04 | A | P01, P02 | ☐ Implement gravity-driven forward motion, steering, brake, boost, branch selection, and safe reset. Both vehicles can reach the merge from either branch. |
| P05 | B | P02, P04 | ☐ Graybox the playable fork with one ingredient pickup and one merge. Record travel times from both branches and adjust their lengths and sign placement so two riders can converge at cruise speed. |
| P06 | A | P01, P04 | ☐ Implement two-player listen-host join and role selection. Replicate rider motion with local prediction and remote interpolation. Produce a build that two machines can run with latency and loss simulation. |
| P07 | A | P05, P06 | ☐ Implement one host-authoritative pickup and a handoff with branch, range, speed, and capacity checks. Duplicate requests, rejected attempts, and reconnects preserve item ownership. |
| P08 | C | P03, P07 | ☐ Wire placeholder success and rejection feedback to actual game state. Confirm both riders see the accepted ingredient and understand when they are out of range or moving too fast. |
| P09 | B | P07, P08 | ☐ Run repeated two-player exchanges, including a deliberate miss and retries at 100–150 ms RTT with 1–2% loss. Record successful, rejected, and duplicated transfers separately and file fixes against observed failures. |

**Gate:** Two players on separate machines can take different branches, recognize the same rendezvous, and consistently pass exactly one ingredient to the cart. Use the GDD's 80% deliberate handoff target to judge the first playtest.

## M1 — Festival Pass vertical slice

### Gameplay, multiplayer, and data

- [ ] **S01 · A · After P09** — Define editable data for ingredient types, recipes, routes, zones, pickups, and ticket schedules. Validate missing IDs, bad references, and invalid zone bounds when loading content.
- [ ] **S02 · A · After S01** — Implement two-slot courier cargo, cart pantry, prep and plate capacity, cargo selection, discard, and simultaneous transfers. Test full destinations and two couriers attempting to transfer at once.
- [ ] **S03 · A · After S02** — Build the chef station loop with prep reservations, pause and resume, cooking, ready/overdone/burnt states, and plate storage. Reproduce each outcome while steering the moving cart.
- [ ] **S04 · A · After S03** — Implement ticket announcement, timed customer lanes, serving, expiration, and finish conditions. Test wrong dishes, later compatible customers, and duplicate serve attempts.
- [ ] **S05 · A · After S04** — Implement shared score, early service bonus, streaks, grades, and results data. Validate representative score cases against the GDD, including zero deliveries and time-cap completion.
- [ ] **S06 · A · After P09** — Build private rooms, 2–4-player role assignment with exactly one chef, countdown, results, replay, and return to lobby. Verify role changes between runs.
- [ ] **S07 · A · After P09** — Add spills, cargo drop and recovery, cart catch-up cues, and courier boost recovery. A spill costs at most one loose ingredient and returns the rider to the same branch.
- [ ] **S08 · A · After S01, S06** — Replicate ticket pins and team pings with sender, target, duration, and rate limits. Expose view state and confirmed events for HUD, audio, and effects.
- [ ] **S09 · A · After S02–S08** — Add development logs for time, seed, tick, branch, order outcomes, and rejection reasons without capturing chat. Publish a versioned slice build and basic connection instructions for testers.

### Game design and map design

- [ ] **S10 · B · After P09** — Block out Festival Pass from summit to village. Mark branches, shortcut, merge windows, service lanes, hazards, and reset points. Calculate arrival times at minimum, cruise, and boost speeds, and verify them in-game.
- [ ] **S11 · B · After S01, S10** — Author Tomato Pocket, Orchard Toast, and Picnic Flatbread. Place fixed ingredient supplies and customer orders for 2, 3, and 4 players. Show at least one achievable pickup-to-serve path per ticket, including travel, handoff, prep, and cook time.
- [ ] **S12 · B · After S10, S11** — Stage the opening lesson around one pickup, one handoff, and one service. Introduce split supply lanes and hazards later. Place signs where riders can react before commitment and confirm the route has a recovery path after a missed merge or spill.
- [ ] **S13 · B · After S05, S11** — Tune order lead time, heat windows, scoring, and grade thresholds. Record reference runs for each team size and verify that each courier has a meaningful assignment.
- [ ] **S14 · B · After S09, S12, S20** — Run first-time and experienced groups on Festival Pass, including voice-free trials. Track first delivery, handoff success, missed-order reasons, chef workload, and courier idle time. Compare first-order completion to the GDD's 80% target.

### UI, art, SFX, and VFX

- [ ] **S15 · C · After P09** — Wireframe join, role select, briefing, in-run role HUDs, and results. Test reading the next order and meeting point while looking at the road. Specify controller focus and all interaction prompt states.
- [ ] **S16 · C · After S06, S15** — Build menus, room codes or invite flow, role selection, countdown, briefing, and results. Show replay, role swap, and exit with clear controller navigation.
- [ ] **S17 · C · After S05, S08, S15** — Build the chef's pantry, station timers, plate slots, and ticket timeline. Build the courier's cargo selection, supply signs, and speed-match cues. Implement shared pings, teammate markers, and success and failure prompts.
- [ ] **S18 · C · After S10** — Build a small modular art kit for road pieces, terrain dressing, signs, customer stalls, props, cart, bikes, riders, and ingredients. Implement rider lean, wobble, and cart station animations. Check branch and customer silhouettes at riding speed.
- [ ] **S19 · C · After S08, S17** — Source or produce SFX for road surface, brake, boost, spill, pickup, handoff, cooking, warning, service, and UI. Implement camera-relative or positional playback as appropriate. Test that ready and burn cues are audible over riding and music.
- [ ] **S20 · C · After S08, S17** — Implement VFX for merge marking, pickup and handoff trails, dust, spills, steam, smoke, and delivery. Distinguish successful actions by host confirmation. Verify that effects leave road signs and prompts legible.
- [ ] **S21 · C · After S16, S17, S19, S20** — Add UI scaling, high-contrast symbol and color pairs, cue captions, reduced shake and motion options, field-of-view and volume controls. Review the handoff and serve flows with color and audio cues removed separately.

**Gate:** A new crew can enter a room, complete and understand a descent, swap roles, and replay. Test with 2, 3, and 4 players. Include one successful voice-free run, one missed merge with a later recovery, and a network test with simultaneous transfers.

## M2 — Content, progression, and feature completion

| ID | Owner | After | Task and acceptance |
| --- | --- | --- | --- |
| R01 | B | Slice gate | ☐ Pitch, graybox, and tune two additional routes with distinct terrain, supply choices, hazards, and named meeting points. Demonstrate a finish and a viable handoff path at every party size. |
| R02 | B | R01 | ☐ Add three recipes for six total and author ticket schedules for each route. Define difficulty presets and score thresholds. Document a feasible completion plan for every fixed baseline schedule. |
| R03 | B | S14 | ☐ Design medal thresholds, route unlocks, and cosmetic rewards. Define when medals are granted, what new players can access, and how a mixed-progress party picks routes. Keep all core recipes available from the start. |
| R04 | A | R03 | ☐ Implement versioned local profiles for personal bests, medals, route unlocks, cosmetics, and settings. Test first boot, restart persistence, repeated rewards, and recovery from a corrupt save. |
| R05 | C | R03, R04 | ☐ Implement route selection with availability and medal targets, result rewards, cosmetic selection, and applied cosmetics. Verify that players understand how to unlock a route and can see their selection in the next run. |
| R06 | A | S11, R01, R02 | ☐ Implement seeded variation within authored sockets, same-seed replay, and new-seed selection. Validate legal pickup, merge, cook, and service paths per crew size. Reject invalid schedules and use a tested authored fallback. |
| R07 | A | S06 | ☐ Implement disconnect grace and reclaim, courier schedule fallback, chef departure results, and host-exit recovery. Verify state, scores, and UI messages for each case. |
| R08 | A | S21 | ☐ Implement persistent input rebinding, keyboard and gamepad prompts, and hold/toggle alternatives. Include safe defaults and a recovery path for conflicting or missing bindings. |
| R09 | C | R01, R02 | ☐ Dress both new routes with variations of the modular kit, readable junctions, route landmarks, customer silhouettes, and recipe icons. Pass a fast-descent read-through with B before final dressing. |
| R10 | C | R09 | ☐ Finish audio mix, surface and route ambience, music loop and transitions, hazard cues, and restrained VFX variants. Maintain an asset list with source, license, attribution, and in-game use. |
| R11 | B | R02, R06, R07 | ☐ Balance route difficulty, crew sizes, recipes, and seed variety with fresh sessions. Freeze placements after fixing impossible tickets and unreadable lane choices. |
| R12 | A | R07, R09, R10 | ☐ Profile host and client during a four-player descent with spills and transfers. Pick reference PC hardware, check the 60 fps at 1080p goal, and address measured CPU, GPU, and network bottlenecks. |
| R13 | C | R05, R08, R10 | ☐ Review every menu, tutorial, error message, subtitle, accessibility control, and controller focus path. Verify supported resolutions, legible prompts, saved preferences, and cue clarity on each route. |

**Gate:** All three routes and six recipes work in the supported party sizes. Progress survives restart, replay preserves the intended seed, and the team can reproduce and diagnose connection and performance issues.

## M3 — Release validation and delivery

- [ ] **L01 · B · After M2 gate** — Own the test matrix. Run all three routes at 2, 3, and 4 players on baseline schedules, then sample seeded variants and both difficulty presets. Track expected outcome, build, seed, role, actual outcome, and repro steps.
- [ ] **L02 · A · After M2 gate** — Verify network correctness under simulated latency and loss. Cover repeated pickup/serve requests, simultaneous transfers, full cart, host exit, client loss/reclaim, late joins, and replay cleanup.
- [ ] **L03 · C · After M2 gate** — Verify full keyboard and controller flows, voice-free play, captions, reduced motion, readable icons, and volume settings on the packaged build. Check audio and VFX briefly for each of the six recipes.
- [ ] **L04 · A · After L01–L03** — Produce a clean release candidate and test install, update, uninstall behavior, and launch on a fresh PC. Set build/version labels, retain a previous known-good build, and document packaging and rollback.
- [ ] **L05 · C · After R10, L03** — Prepare representative screenshots, trailer captures if needed, credits, and store assets for the chosen storefront. Check that released assets are licensed and credited.
- [ ] **L06 · B · After L01–L05** — Coordinate final regression, known issues, release notes, support reporting, and the launch checklist. Confirm the release build completes a private-room run with fresh accounts or profiles.

**Gate:** The release candidate has no unresolved blockers for join, whole-run completion, item integrity, score integrity, or save integrity. Gather results from the packaged build before release.

## Workload and scope decisions

| Workstream | First commitment | If the three-person team runs short on capacity |
| --- | --- | --- |
| Networking | Reliable downhill movement, pickup and handoff, then private-room run flow | Keep private rooms and deterministic recovery. Defer optional platform integrations. |
| Map and gameplay | Finish Festival Pass and balance it at all party sizes | Ship fewer authored routes before committing to more procedural variation, subject to team approval. |
| UI and audiovisual | Make orders, branches, handoffs, and station state readable | Reuse modular art, shared animations, and cue variants. Defer unique cosmetics, music tracks, and decorative effects. |
| Progression | Local medals, personal bests, and clear route unlocks | Ship a smaller cosmetic set and postpone extra reward tracks. |
| Quality | Replay, save resilience, accessibility basics, and online validation | Reduce content before reducing these checks. |

Public matchmaking, dedicated servers, split-screen, solo bots, built-in voice, and a route editor remain outside the initial production scope in the GDD.

## First work cycle

1. **A:** Set up the engine project and share a launchable build. Implement controllable cart and bike movement.
2. **B:** Draw the fork and specify the pickup and merge windows. Graybox travel paths as soon as movement works.
3. **C:** Prototype the follow camera framing, vehicle readability, and handoff prompts. Use temporary icons and sounds to test the complete exchange.
4. **All:** Test one exchange on two machines. Log whether failures come from route reading, timing, interaction rules, or replication. Pick the next fixes before committing to more content.
