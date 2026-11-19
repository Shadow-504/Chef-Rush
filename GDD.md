# Chef Rush

**Game design document · v0.1 · Prototype proposal**

Chef Rush is a cooperative downhill cooking game for 2–4 friends. One player runs a kitchen cart while the others ride ahead on smaller bikes, pick up ingredients, and rejoin the cart at places they agree on before the road forks. The crew cooks and serves roadside customers without bringing the kitchen to a halt.

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

1. **Gravity sets the pace.** Everyone can steer and brake, but the descent keeps going. A decision made at one fork shapes the next thirty seconds.
2. **The recipe creates the rendezvous.** The chef knows what is missing. The couriers know what they can reach. The team has to agree where the food will change hands.
3. **A mistake makes a story and leaves a next move.** Missing an ingredient can cost a meal. The crew should still have a reason to regroup and serve the next customer.
4. **Players can read the road and their job together.** Show the next task close to the riding view. Keep interaction windows generous enough that both jobs are learnable.

## 2. Players and controls

### Roles

| Role | Responsibilities | Arcade tradeoff |
| --- | --- | --- |
| Chef, exactly one | Steer the kitchen cart, pin an order, receive ingredients, work the cooking stations, and serve from the cart | The cart is wide and stable. Its momentum and busier HUD make late lane changes costly. |
| Courier, one to three | Spot supply lanes, pick up ingredients, mark a meeting point, match the cart, and offload cargo | Bikes accelerate and change lanes quickly. Each has two cargo slots and no cooking stations. |

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

Gravity provides a baseline forward speed. Brake allows controlled matching and recovery, and boost lets riders catch up. The chef can steer and brake while using stations. The context prompt names the action and gives precedence to serving, receiving, then station work when their zones overlap. Couriers see handoff prompts only inside a marked merge window. Camera look and menu inputs are separate from steering. Rebinding and hold/toggle alternatives are part of the control design.

## 3. Run structure

1. **Briefing.** Pick the route and chef. Show the menu, major forks, and customer stops. The cart starts with one common ingredient to make the first order forgiving.
2. **Order revealed.** An upcoming customer appears on the timeline with a dish, required ingredients, and service location. The chef pins an order to put its missing ingredients on every rider’s HUD.
3. **Split and collect.** Couriers choose supply lanes by reading the fork signs and team pings. Ride through a clearly marked pickup arc to fill a cargo slot if one is available.
4. **Meet and transfer.** Mark an upcoming merge, approach the cart, match its speed, and hand off the selected ingredient. Riders can agree to use a later merge if timing slips.
5. **Cook while moving.** Load ingredients at prep, start cooking, and plate. The road continues while the heat timer runs.
6. **Serve or recover.** Enter the customer lane and use the serve prompt. If the crew misses the stop, the ticket expires. Another customer can still be served further down the route.
7. **Finish.** Crossing the finish line closes service. Results show delivered orders, score, grade, route time, and a short feed of memorable saves and mishaps. The party can rotate chef and retry the seed.

**Example.** The cart has dough for a Picnic Flatbread. Linh takes the orchard lane for tomato and pins Mill Bridge. Sam takes the dairy lane for cheese. Linh hands off at the bridge and Sam catches the cart at the next straight. The chef finishes the flatbread on the way to the picnic stand. If Sam misses both windows, the crew can use the tomato and dough for a Tomato Pocket at the next compatible stop.

## 4. Downhill riding and routes

- Use a downhill track with authored forks and reconvergence, readable lane markings, and a local spline frame for progress and camera alignment. Players steer continuously across the road. Route signs show supply icons and the next shared meeting point before commitment.
- Start tuning at 12 m/s cruise and 6 m/s minimum on typical slopes. The cart boosts to 17 m/s and bikes to 21 m/s. Adjust per route segment to keep the lead-in, handoff, and service timings readable.
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

Build the first route with fixed placements. Seeded variations can reorder compatible tickets and rotate pickups within authored sockets. A generator must prove each required ingredient can reach the cart before its service cutoff, accounting for player count, travel time, handoff time, station capacity, and cook time. A fallback authored layout covers any rejected seed.

## 5. Ingredients, rendezvous, and cooking

### Inventory

Ingredients are discrete units with a type and unique ID. A courier carries two units and can select which to transfer. The chef's pantry holds six loose units. The cart has one prep board, one cooking surface, and two plated-dish slots. Prep reserves its inputs so two orders cannot consume the same unit. A full slot shows a clear prompt. Players can discard selected loose cargo or a dish through an explicit hold action with its own prompt away from pickup and service zones.

### Handoffs

1. A courier pings an authored meeting point. The crew sees its name, branch, and distance.
2. The handoff prompt opens when the courier is beside or just behind the cart in the merge zone, within a starting tolerance of 3 meters and 3 m/s relative speed.
3. The courier taps context. The cart accepts automatically into a free pantry slot. Both players get an ingredient icon, audio cue, and a confirmation. The chef can keep steering.
4. On a full pantry, the selected unit stays with the courier. On a missed window, the HUD points to the next reachable merge.

Target 4–6 seconds of usable transfer time at cruise speed. Test these dimensions with network delay and different input devices. Loose food flying between riders is presentation. Authoritative inventory changes only after a confirmed transaction.

### Stations

1. **Prep.** Pin a recipe and use context when the pantry contains its ingredients. Hold for 1.25 seconds to prep. The chef can release and resume while steering.
2. **Cook.** Use context to move the prepared food to the free cooking surface. The recipe timer runs as the cart moves. The prep board becomes available for the next recipe.
3. **Plate.** Use context during the ready window. A full plate rack leaves the dish on the heat, with the timer still running. An overdone dish can be plated for reduced score. A burnt dish must be cleared.
4. **Serve.** During the matching customer's zone, enter the marked side of the road and tap context. Service uses the oldest eligible plate and confirms the completed ticket.

| Recipe | Inputs | Cook | Ready window | Use |
| --- | --- | ---: | ---: | --- |
| Tomato Pocket | Dough + tomato | 4 s | 4 s | First delivery and easy fallback when ticketed |
| Orchard Toast | Bread + apple | 3 s | 4 s | Fast turnaround |
| Picnic Flatbread | Dough + tomato + cheese | 6 s | 4 s | Multi-courier coordination or two pickups by one courier |

After the ready window, allow 3 seconds of overdone time before burning. Tunes start with a single active cooking surface and no chopping minigame. The challenge comes from steering, timing, and planning the handoff.

## 6. Customers, scoring, and recovery

- **Ticket lifecycle:** announced → active → served or expired. Each ticket specifies one recipe and one customer stop. Announce first-pass tickets about 25–45 seconds before service as route geometry permits. Use route progress for customer deadlines and elapsed time for cooking.
- **Service windows:** target 5–7 seconds at cart cruise speed. Mark the correct lane early. A wrong dish stays on the rack and the prompt shows the required dish. A valid plated dish can be used at a later compatible stop if its original ticket expired.
- **Score:** provisional values are 100 for a normal delivery and 60 for overdone. Add up to 20 for serving in the first half of the customer zone and 15 per consecutive completed ticket after the first, capped at 45. An expired ticket breaks the streak. Spills affect scoring through time and cargo loss.
- **Grade:** divide earned score by the maximum for the current seed and group size. Start with S at 90%, A at 75%, B at 55%, and C below 55%. Display the number of delivered orders alongside the grade.
- **Finish condition:** the cart crosses the finish or reaches the run time cap, provisionally 12 minutes. A bad run still reaches results. Use a shared score and highlight each player's contributions without a leaderboard within the crew.
- **Scaling:** start with 4, 5, and 6 tickets for 2, 3, and 4 players respectively. Two-player seeds limit simultaneous demand and ensure a single courier can collect everything required. Larger seeds add split supply choices and overlapping tickets while keeping chef station throughput feasible.

## 7. Communication, UI, and accessibility

- **Shared HUD:** next named junction, persistent rendezvous marker, relevant teammate arrows, and the pinned ticket with available and missing ingredients.
- **Chef HUD:** pantry grouped by ingredient, station state, plate rack, and a route timeline of visible customer stops. Make ready and burning states visually distinct with icons and sound.
- **Courier HUD:** cargo slots, next pickups, chef-relative direction, and a proximity or speed-match cue during handoffs.
- **Pings:** “Need ingredient,” “Taking this lane,” “Meet here,” and “Missed it.” Pings show the sender and relevant target. Rate-limit repeated signals. Every required action can be coordinated with the ping system. External voice is optional.
- **Teaching:** a short first-run briefing, contextual prompts during the protected opening stretch, and an optional starter route replay. Teach one decision at a time and let players learn from the results feed.
- **Accessibility:** scalable HUD and text, high-contrast symbols paired with colors, captions for meaningful audio, adjustable camera shake and field of view, reduced motion effects, and remappable inputs. An easier lobby preset widens timing windows and reduces hazards. Grade against that preset's own targets.

## 8. Art and audio

Use stylized 3D, chunky vehicle silhouettes, fabric awnings, bright painted road signs, and high-contrast ingredient shapes. Keep the cart's kitchen readable from its chase camera. Distinguish routes with landmarks visible above the track so teammates can call them by name. Use original riders, roadside businesses, and festivals. Focus comedy on reactions, wobble, and cargo physics while keeping collisions and interactive objects visually reliable.

Layer light percussion and acoustic instruments under the descent. Use separate sound signatures for route warnings, successful handoffs, ready food, and service. Let the cooking cue cut through the mix without burying nearby road hazards. Give music, effects, and UI sound separate volume controls.

## 9. Multiplayer and implementation

- **Architecture:** private invite or room-code lobbies with a listen host for the prototype. The host owns pickups, inventories, station timers, tickets, spills, and the score. Clients predict their own steering and interpolate remote riders. Use a fixed simulation step, starting at 30 Hz for authoritative state.
- **Transactions:** validate route segment, proximity, speed difference, item ownership, and capacity on the host. Give interaction requests unique IDs so retries cannot duplicate ingredients or deliveries. Send reliable state changes for transfers and serving. Vehicle snapshots can be lossy.
- **Run state:** lobby → countdown → descent → finish → results. Disable mid-run joins for the first release scope. Give a disconnected courier a short reclaim window. After it expires, future tickets use the smaller crew's schedule where feasible. If the chef disconnects and cannot reclaim the role after a short grace period, end the run and show the results accumulated so far. Host departure returns the group to its lobby flow. Host migration is deferred.
- **Data:** define recipes, ingredient types, routes, pickup sockets, merge and service zones, ticket schedules, and tuning presets as editable assets. Log seed, role counts, ticket outcomes, and failed handoff reasons in development builds without recording chat.
- **Engine selection:** open. Compare engine candidates against multiplayer iteration, vehicle prediction, spline authoring, input remapping, and PC build workflow. Prove the transfer online before committing to a production content pipeline.
- **Performance target:** 60 fps at 1080p on a representative midrange PC, with a stable simulation through four-player spills and transfers. Define specific reference hardware during the prototype.

## 10. Progression and replayability

The complete core menu is available from the start. Route medals can unlock later routes and cosmetic vehicle paint, aprons, and rider accessories. Upgrades should avoid increasing speed, inventory, or customer patience. Crew presets support a gentler service or denser traffic and tighter timing. Results offer replay of the same seed for score chasing and a new seed for variety.

## 11. Scope and milestones

| Milestone | Deliverable | Exit criteria |
| --- | --- | --- |
| Movement and exchange prototype | Graybox fork, two vehicles, one pickup, one merge, two online players | Riders can read the split, converge, and transfer an ingredient repeatedly under simulated network delay |
| Vertical slice | Festival Pass, three recipes, both roles, 2–4 players, tutorial prompts, one complete results loop | A new crew can finish a descent and understand any missed order |
| First-release candidate | Three distinct routes, six recipes, private room flow, score grades, accessibility settings, disconnect handling, performance pass | Every route passes online validation at each supported party size |
| After release, subject to playtests | More routes, cosmetics, rotating seeds, alternate stations | Each addition earns its place through playtest demand and production capacity |

Public matchmaking, dedicated servers, split-screen, solo bots, built-in voice, and a route editor are outside the initial release scope. Voice-free coordination and two-player viability are in scope from the vertical slice.

## 12. Playtest and acceptance plan

| Check | Prototype target |
| --- | --- |
| First delivery | At least 80% of first-time crews complete one order on their first descent without live coaching |
| Transfer readability | At least 80% of deliberate transfers succeed when the riders reach the chosen merge together. Record the dominant causes of failures. |
| Role engagement | A two-player team can complete every ticket on the authored baseline seed. In four-player tests, each courier has useful route decisions and transfers. |
| Recovery | Force a spill and a missed merge early. Verify the crew can return to a common route and complete a later order. |
| Voice-free coordination | Complete a descent using only pings and verify that the crew can communicate the missing ingredient and the correct meeting point |
| Network correctness | Test at 100–150 ms round-trip delay and 1–2% packet loss. Verify retries, full inventories, simultaneous transfers, and repeated serve inputs preserve item and score counts. |
| Session behavior | Exercise lobby start, replay, role change, client disconnect, and host exit. Check that every remaining player has a clear next action. |

Prioritize fixing missed information before increasing difficulty. Compare chef workload against courier downtime using observation, interaction timing, and a short role preference survey after each run. If parallel tickets overload the chef, simplify station input or expand the spacing between stops. If couriers spend too long pacing the cart, add meaningful supply fork choices before adding pickup volume.

## 13. Open decisions

- Does the cart need stronger steering assistance while a station prompt is active?
- How soon can a new crew read and act on a second simultaneous ticket?
- Which merge tolerances feel intentional under latency without requiring precise steering?
- Should a disconnected chef be replaceable by a courier at a designated pullout in a later version?
- Which visual theme, engine, and release scope fit the available team and budget?

Treat player counts, tuning values, platform, business model, and production scope as working assumptions. Revise them against the vertical-slice playtests.
