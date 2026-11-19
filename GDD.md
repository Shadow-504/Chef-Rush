# Chef Rush

**Game design document · v0.2 · Prototype proposal**

Chef Rush is a cooperative downhill cooking game for 2–4 friends. Everyone rides down a mountain. One rider wears a torso-mounted pan and cooks while steering. The other riders split off to pick up ingredients and pass them to the chef at agreed meeting points. The crew serves roadside customers on the same descent. Use downhill bikes for the prototype.

**Pitch:** “I have the eggs. Take the lower bridge. Please tell me you took the lower bridge.”

## 1. Product overview

| Item | Working decision |
| --- | --- |
| Genre | Online co-op party game with arcade downhill riding and light order management |
| Players | 2–4 friends in private rooms |
| Session | One 6–9 minute descent, plus lobby and results |
| Platform | PC first, with keyboard and gamepad support |
| View | Third-person chase camera for every rider, with different HUDs for chef and couriers |
| Tone | Playful roadside food festival with expressive riders, wobbly cargo, and recoverable mishaps |
| Format | Replayable authored routes with seeded orders and supply variations |
| Distribution | Premium PC game as a planning assumption. Price and storefront strategy are open. |

### Design pillars

1. **Cook on the descent.** Steer, brake, load the pan, and take food off the heat while moving. A dish can become ready halfway through a turn, so choosing where to start cooking is part of riding well.
2. **The recipe creates the rendezvous.** The chef knows what is missing. The couriers know what they can reach. The team has to agree where the food will change hands.
3. **A mistake makes a story and leaves a next move.** Missing an ingredient can cost a meal. The crew should still have a reason to regroup and serve the next customer.
4. **Players can read the road and their job together.** Show the next task close to the riding view. Keep interaction windows generous enough that both jobs are learnable.

## 2. Players and controls

### Roles

| Role | Responsibilities | Arcade tradeoff |
| --- | --- | --- |
| Chef, exactly one | Steer a bike while operating a pan strapped to the torso. Pin orders, receive ingredients, cook, and serve from a wearable dish pouch | Cooking and steering compete for attention. The chef can brake to create more time before a customer. |
| Courier, one to three | Spot supply lanes, pick up ingredients, mark a meeting point, match the chef rider, and offload cargo | Couriers have a higher boost cap for catching up after supply detours. Each has two cargo slots. |

Players choose roles in the lobby and can swap between runs. In a two-player run the chef still drives and cooks, and the other player is the only courier. Larger crews can split coverage at forks, with more parallel orders to give everyone work. A crew may send multiple riders down the same route.

### Input

| Action | Keyboard default | Gamepad default |
| --- | --- | --- |
| Steer | A / D | Left stick |
| Boost | W | Right trigger |
| Brake | S | Left trigger |
| Context action | Space | South face button |
| Cycle order or cargo slot | Q / E | Shoulder buttons |
| Open ping wheel and confirm | Hold Tab, point, release | Hold D-pad up, point, release |

Gravity provides a baseline forward speed. Brake allows controlled matching and recovery, and boost lets riders catch up. The chef retains steering and braking during cooking. Use a tap-to-load and tap-to-plate interaction on the context button so either input device can steer and cook at once. Context prioritizes serving over pan actions when both are available. Couriers initiate transfers independently. Couriers see handoff prompts only inside a marked merge window. Camera look and menu inputs are separate from steering. Rebinding and hold/toggle alternatives are part of the control design.

## 3. Run structure

1. **Briefing.** Pick the route and chef. Show the menu, major forks, and customer stops. The chef starts with one common ingredient to make the first order forgiving.
2. **Order revealed.** An upcoming customer appears on the timeline with a dish, required ingredients, and service location. The chef pins an order to put its missing ingredients on every rider’s HUD.
3. **Split and collect.** Couriers choose supply lanes by reading the fork signs and team pings. Ride through a clearly marked pickup arc to fill a cargo slot if one is available.
4. **Meet and transfer.** Mark an upcoming merge, approach the chef, match its speed, and hand off the selected ingredient. Riders can agree to use a later merge if timing slips.
5. **Cook while moving.** Load the torso pan and take the dish off the heat when ready. The road continues while the heat timer runs.
6. **Serve or recover.** Enter the customer lane and use the serve prompt. If the crew misses the stop, the ticket expires. Another customer can still be served further down the route.
7. **Finish.** Crossing the finish line closes service. Results show delivered orders, score, grade, route time, and a short feed of memorable saves and mishaps. The party can rotate chef and retry the seed.

**Example.** The chef has dough for a Picnic Flatbread. Linh takes the orchard lane for tomato and pins Mill Bridge. Sam takes the dairy lane for cheese. Linh hands off at the bridge and Sam catches the chef at the next straight. The chef finishes the flatbread on the way to the picnic stand. If Sam misses both windows, the crew can use the tomato and dough for a Tomato Pocket at the next compatible stop.

## 4. Downhill riding and routes

- Use a downhill track with authored forks and reconvergence, readable lane markings, and a local spline frame for progress and camera alignment. Players steer continuously across the road. Route signs show supply icons and the next shared meeting point before commitment.
- Start tuning at 12 m/s cruise and 6 m/s minimum on typical slopes. The chef boosts to 17 m/s and couriers to 21 m/s. Adjust per route segment to keep the lead-in, handoff, and service timings readable.
- Use one downhill direction. A fork locks once the rider passes its splitter. Avoid layouts where a wrong turn traps a rider or prevents all remaining service.
- Hazards include potholes, loose crates, and mud. Telegraph their effect with shape, placement, and sound. Rider contact is soft. Players can bump and wobble without repeatedly locking out a teammate's controls.
- A major spill briefly slows the rider, drops at most one loose ingredient, and returns the rider to a safe lane on the same branch. An unreachable drop is lost. Couriers who fall far behind get temporary catch-up boost outside active handoff zones.
- Reserve straight, wide sections for transfers and service. Tight corners, appealing shortcuts, and supply density create route choices. Early routes leave spare windows and later routes make the crew choose which orders to pursue.

### First route — Festival Pass

| Section | Route and decision | What it teaches |
| --- | --- | --- |
| Summit launch | Shared road with a tomato pickup, first merge, and lookout customer | Read a ticket and complete one handoff |
| Orchard and dairy fork | Parallel lanes with distinct ingredients, rejoining at Mill Bridge | Call a route before the split |
| Switchbacks | Mud on the inside line and a wide outside recovery lane | Brake and cook through a turn |
| Riverside | Short optional supply spur, shared transfer lane, picnic stand | Decide whether the pickup is worth a later rendezvous |
| Village finish | Two spaced customer stops with a last common pickup | Triage late tickets and finish the descent |

Build the first route with fixed placements. Seeded variations can reorder compatible tickets and rotate pickups within authored sockets. A generator must prove each required ingredient can reach the chef before its service cutoff, accounting for player count, travel time, handoff time, pan capacity, and cook time. A fallback authored layout covers any rejected seed.

## 5. Ingredients, rendezvous, and cooking

### Inventory

Ingredients are discrete units with a type and unique ID. A courier carries two units and can select which to transfer. The chef wears an ingredient pouch with six loose slots, a single torso-mounted pan, and a dish pouch with two slots. Loading the pan consumes the selected recipe's ingredients in one transaction. A full slot shows a clear prompt. Players can discard selected loose cargo or a dish through an explicit hold action with its own prompt away from pickup and service zones.

### Handoffs

1. A courier pings an authored meeting point. The crew sees its name, branch, and distance.
2. The handoff prompt opens when the courier is beside or just behind the chef in the merge zone, within a starting tolerance of 3 meters and 3 m/s relative speed.
3. The courier taps context. The chef automatically receives the ingredient in a free pouch slot. Both players get an ingredient icon, audio cue, and a confirmation. The chef can keep steering.
4. On a full ingredient pouch, the selected unit stays with the courier. On a missed window, the HUD points to the next reachable merge.

Target 4–6 seconds of usable transfer time at cruise speed. Test these dimensions with network delay and different input devices. Loose food flying between riders is presentation. Authoritative inventory changes only after a confirmed transaction.

### Wearable cooking

1. **Load the pan.** With a recipe pinned, sufficient ingredients on the belt, and an empty pan, tap context to load and start cooking. The chef continues to steer and brake.
2. **Watch the heat while riding.** Cooking advances in real time through turns, merges, and spills. A near-reticle heat indicator, animation, and audio cue tell the chef when to take the dish off. Frame the camera so the road and the rider's cooking motion are readable together.
3. **Plate.** Tap context during the ready window to move the dish into the wearable dish pouch. With a full pouch the dish remains on the heat. Overdone dishes score less. A burnt dish must be cleared.
4. **Serve.** Ride into the matching customer's lane and tap context. The oldest eligible dish is taken from the pouch and the ticket is completed.

Use a single pan and a single action to load or plate for the prototype. Try an optional timed flip only after playtesting the basic steering and heat interaction. Pan tilt and food wobble can provide visual feedback while the host controls cooking and inventory state.

| Recipe | Inputs | Cook | Ready window | Use |
| --- | --- | ---: | ---: | --- |
| Tomato Pocket | Dough + tomato | 4 s | 4 s | First delivery and easy fallback when ticketed |
| Orchard Toast | Bread + apple | 3 s | 4 s | Fast turnaround |
| Picnic Flatbread | Dough + tomato + cheese | 6 s | 4 s | Multi-courier coordination or two pickups by one courier |

After the ready window, allow 3 seconds of overdone time before burning. Initial tuning assumes one dish cooking at a time. The challenge comes from steering, timing, and planning the handoff.

## 6. Customers, scoring, and recovery

- **Ticket lifecycle:** announced → active → served or expired. Each ticket specifies one recipe and one customer stop. Announce first-pass tickets about 25–45 seconds before service as route geometry permits. Use route progress for customer deadlines and elapsed time for cooking.
- **Service windows:** target 5–7 seconds at chef cruise speed. Mark the correct lane early. A wrong dish stays in the pouch and the prompt shows the required dish. A valid plated dish can be used at a later compatible stop if its original ticket expired.
- **Score:** provisional values are 100 for a normal delivery and 60 for overdone. Add up to 20 for serving in the first half of the customer zone and 15 per consecutive completed ticket after the first, capped at 45. An expired ticket breaks the streak. Spills affect scoring through time and cargo loss.
- **Grade:** divide earned score by the maximum for the current seed and group size. Start with S at 90%, A at 75%, B at 55%, and C below 55%. Display the number of delivered orders alongside the grade.
- **Finish condition:** the chef crosses the finish or reaches the run time cap, provisionally 12 minutes. A bad run still reaches results. Use a shared score and highlight each player's contributions without a leaderboard within the crew.
- **Scaling:** start with 4, 5, and 6 tickets for 2, 3, and 4 players respectively. Two-player seeds limit simultaneous demand and ensure a single courier can collect everything required. Larger seeds add split supply choices and overlapping tickets while keeping pan throughput feasible.

## 7. Communication, UI, and accessibility

- **Shared HUD:** next named junction, persistent rendezvous marker, relevant teammate arrows, and the pinned ticket with available and missing ingredients.
- **Chef HUD:** ingredients grouped by type, pan heat and dish state, plated-dish slots, and a route timeline of visible customer stops. Make ready and burning states visually distinct with icons and sound.
- **Courier HUD:** cargo slots, next pickups, chef-relative direction, and a proximity or speed-match cue during handoffs.
- **Pings:** “Need ingredient,” “Taking this lane,” “Meet here,” and “Missed it.” Pings show the sender and relevant target. Rate-limit repeated signals. Every required action can be coordinated with the ping system. External voice is optional.
- **Teaching:** a short first-run briefing, contextual prompts during the protected opening stretch, and an optional starter route replay. Teach one decision at a time and let players learn from the results feed.
- **Accessibility:** scalable HUD and text, high-contrast symbols paired with colors, captions for meaningful audio, adjustable camera shake and field of view, reduced motion effects, and remappable inputs. An easier lobby preset widens timing windows and reduces hazards. Grade against that preset's own targets.

## 8. Art and audio

Use stylized 3D, chunky vehicle silhouettes, fabric awnings, bright painted road signs, and high-contrast ingredient shapes. Give the chef a distinct silhouette with a chest harness and front-mounted pan. Use a slightly offset chase camera and clear arm, steam, and heat cues to show cooking without obscuring the road. Distinguish routes with landmarks visible above the track so teammates can call them by name. Use original riders, roadside businesses, and festivals. Focus comedy on reactions, wobble, and cargo physics while keeping collisions and interactive objects visually reliable.

Layer light percussion and acoustic instruments under the descent. Use separate sound signatures for route warnings, successful handoffs, ready food, and service. Let the cooking cue cut through the mix without burying nearby road hazards. Give music, effects, and UI sound separate volume controls.

## 9. Multiplayer and implementation

- **Architecture:** private invite or room-code lobbies with a listen host for the prototype. The host owns pickups, inventories, pan timers, tickets, spills, and the score. Clients predict their own steering and interpolate remote riders. Use a fixed simulation step, starting at 30 Hz for authoritative state.
- **Transactions:** validate route segment, proximity, speed difference, item ownership, and capacity on the host. Give interaction requests unique IDs so retries cannot duplicate ingredients or deliveries. Send reliable state changes for transfers and serving. Vehicle snapshots can be lossy.
- **Run state:** lobby → countdown → descent → finish → results. Disable mid-run joins for the first release scope. Give a disconnected courier a short reclaim window. After it expires, future tickets use the smaller crew's schedule where feasible. If the chef disconnects and cannot reclaim the role after a short grace period, end the run and show the results accumulated so far. Host departure returns the group to its lobby flow. Host migration is deferred.
- **Data:** define recipes, ingredient types, routes, pickup sockets, merge and service zones, ticket schedules, and tuning presets as editable assets. Log seed, role counts, ticket outcomes, and failed handoff reasons in development builds without recording chat.
- **Engine selection:** open. Compare engine candidates against multiplayer iteration, vehicle prediction, spline authoring, input remapping, and PC build workflow. Prove the transfer online before committing to a production content pipeline.
- **Performance target:** 60 fps at 1080p on a representative midrange PC, with a stable simulation through four-player spills and transfers. Define specific reference hardware during the prototype.

## 10. Progression and replayability

**Recommended progression:** Finish a run → earn a crew medal → unlock another slope or a cosmetic → pick a new route or replay for a better score. Use the existing run grade to award medals. Start with B for Bronze, A for Silver, and S for Gold. A C run still records the score and lets the crew retry. These thresholds and rewards are playtest proposals.

| Milestone | Reward | New challenge |
| --- | --- | --- |
| Start | Festival Pass and the full recipe set | Learn the wearable pan, a simple fork, and a generous service window |
| Silver on Festival Pass | Second slope | Pick between supply lanes and plan a later merge |
| Silver on the second slope | Third slope | Coordinate tighter service windows and heat timing through harder terrain |
| Gold on a route | One cosmetic tied to that route, such as a pan finish, apron, or bike paint | Pursue a near-complete service run and replay the same seed |

Save best scores and medals per route and difficulty preset on each player's profile, with comparable records kept by party size. Everyone who completes the run receives the crew result on their own profile, regardless of role. For mixed-progress groups, let the host select routes they have unlocked and let guests join and keep medals earned there. For personal route unlocks, check the prerequisite route medal directly. Credit qualifying medals on both standard and easier presets so groups can progress with their preferred accessibility settings. Rewards change appearance only.

For the vertical slice, build the results and same-seed replay loop around Festival Pass. Test whether players understand why they missed a medal and choose to try again. Add saved unlocks and cosmetics when additional routes are playable.

## 11. Scope and milestones

| Milestone | Deliverable | Exit criteria |
| --- | --- | --- |
| Ride, exchange, and cook prototype | Graybox fork, two riders, a torso pan, one pickup, one merge, one recipe, and one customer | Online riders can split, transfer an ingredient, and cook and serve without stopping under simulated network delay |
| Vertical slice | Festival Pass, three recipes, both roles, 2–4 players, tutorial prompts, one complete results loop | A new crew can finish a descent and understand any missed order |
| First-release candidate | Three distinct routes, six recipes, private room flow, score grades, accessibility settings, disconnect handling, performance pass | Every route passes online validation at each supported party size |
| After release, subject to playtests | More routes, cosmetics, rotating seeds, alternate wearable cooking equipment | Each addition earns its place through playtest demand and production capacity |

Public matchmaking, dedicated servers, split-screen, solo bots, built-in voice, and a route editor are outside the initial release scope. Voice-free coordination and two-player viability are in scope from the vertical slice.

## 12. Playtest and acceptance plan

| Check | Prototype target |
| --- | --- |
| Ride and cook | The chef loads and plates a dish while steering through a turn. Verify both input devices retain steering and brake control. |
| First delivery | At least 80% of first-time crews complete one order on their first descent without live coaching |
| Transfer readability | At least 80% of deliberate transfers succeed when the riders reach the chosen merge together. Record the dominant causes of failures. |
| Role engagement | A two-player team can complete every ticket on the authored baseline seed. In four-player tests, each courier has useful route decisions and transfers. |
| Recovery | Force a spill and a missed merge early. Verify the crew can return to a common route and complete a later order. |
| Voice-free coordination | Complete a descent using only pings and verify that the crew can communicate the missing ingredient and the correct meeting point |
| Network correctness | Test at 100–150 ms round-trip delay and 1–2% packet loss. Verify retries, full inventories, simultaneous transfers, and repeated serve inputs preserve item and score counts. |
| Session behavior | Exercise lobby start, replay, role change, client disconnect, and host exit. Check that every remaining player has a clear next action. |

Prioritize fixing missed information before increasing difficulty. Compare chef workload against courier downtime using observation, interaction timing, and a short role preference survey after each run. If parallel tickets overload the chef, simplify pan input or expand the spacing between stops. If couriers spend too long pacing the chef, add meaningful supply fork choices before adding pickup volume.

## 13. Open decisions

- Which camera offset makes the torso pan readable while leaving room to see the next bend?
- How soon can a new crew read and act on a second simultaneous ticket?
- Which merge tolerances feel intentional under latency without requiring precise steering?
- Should a disconnected chef be replaceable by a courier at a designated pullout in a later version?
- Which visual theme, engine, and release scope fit the available team and budget?

Treat player counts, tuning values, platform, business model, and production scope as working assumptions. Revise them against the vertical-slice playtests.
