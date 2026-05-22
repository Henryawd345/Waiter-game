# Rude Customer – Development Roadmap
### by Henry and TK

This is our roadmap that is used to track out progress.

---

### ''' Important '''
In each scene if you want the game to work u need at least of the following gameObjects and its children.

- _System
- _Player

or else the other codes will get null references and error at last. Since I haven't made any exception handlers lol.

-TK

---

# Tasks

### Henry
| Status | Done%(Aprox) | Task |
|:-:|:-:|:-|
|[x]| 100% | Bad customer movement|
|[]| 0% | |
|[]| 0% | |
|[]| 0% | |
|[]| 0% | |
|[]| 0% | |
|[]| 0% | |
|[]| 0% | |
|[]| 0% | |
|[]| 0% | |
|[]| 0% | |
|[]| 0% | |

### TK
| Status | Done%(Aprox) | Task |
|:-:|:-:|:-|
|[]| 70% | Character Movements (Basic) |
|[x]| 100% | Good Customer System |
|[x]| 100% | Table System |
|[x]| 100% | Ordering System |
|[]| 60% | Game Manager |
|[]| 0% | Gameplay Loop (No Bad yet) |
|[]| 0% | |
|[]| 0% | |
|[]| 0% | |
|[]| 0% | |
|[]| 0% | |
|[]| 0% | |

---
---
---
---
---

## PHASE 0 — Foundation (Very Short)
**Goal:** Stable project, no tech friction.

**Build**
- [X] Git + Git LFS
- [X] Folder structure
- [X] One scene: Prototype_Arena
- [X] Floor, walls, counter, 2 tables

**Do NOT build**
- Upgrades
- Weapons
- Luck system

**Exit condition**
- Both devs can open the project and see the same room.

---

## PHASE 1 — Core Play Loop (Most Important)
**Goal:** Prove the game is fun.

**Core loop**
Customer appears → waits → player picks food → delivers → customer leaves

**Build**
- [ ] Player: move, pick up food, deliver
- [X] Customer: Waiting → Good → Leave
- [ ] UI: order text, timer bar, money counter

**Do NOT build**
- Combat depth
- Bad customers
- Upgrades

**Exit condition**
- Serving customers repeatedly works without bugs.

---

## PHASE 2 — Failure & Pressure
**Goal:** Make it a real game.

**Build**
- [ ] Customer BAD state
- [ ] Reputation (global health)
- [ ] Very simple combat (push / punch)
- [ ] Lose condition

**Do NOT build**
- Upgrades
- Shop
- Weapons

**Exit condition**
- Player can lose and understands why.

---

## PHASE 3 — Day / Wave Structure
**Goal:** Roguelite framework.

**Build**
- [ ] Day / wave timer
- [ ] Between-wave pause
- [ ] Difficulty scaling (faster timers, more customers)

**Do NOT build**
- Luck
- Chaos
- Weapon systems

**Exit condition**
- Each run feels different based on survival time.

---

## PHASE 4 — Upgrades (Core Roguelite Moment)
**Goal:** Replayability begins.

**Build (start small: 6–8 upgrades total)**

Service:
- [ ] Customers wait longer
- [ ] Carry speed increase
- [ ] More money from food

Combat:
- [ ] Knockback increase
- [ ] Faster attack
- [ ] Reduced stun duration

**Rules**
- Choose 1 of 3
- Run-only
- Limited stacking (max 2)

**Do NOT build**
- Chaos upgrades
- Meta progression

**Exit condition**
- Runs feel meaningfully different.

---

## PHASE 5 — Luck & Economy
**Goal:** Add depth, not noise.

**Build**
- [ ] Luck stat
- [ ] Income formula: Food sales + (Tips × Luck)
- [ ] Luck affects:
  - [ ] Crit chance
  - [ ] Rare upgrade chance
  - [ ] Tip bonus

**Do NOT build**
- Permanent upgrades
- Save system

**Exit condition**
- Service builds and combat builds are both viable.

---

## PHASE 6 — Weapons & Tray System
**Goal:** Merge service and combat.

**Build**
- [ ] Tray capacity upgrades
- [ ] Hover tray
- [ ] Weapon pickup
- [ ] Weapon durability & break

**Limit scope**
- 1–2 weapon types only

**Exit condition**
- Fighting while serving is manageable, not chaotic.

---

## PHASE 7 — Chaos Upgrades (Last)
**Goal:** Memorable runs.

**Build**
- [ ] Food-as-weapon
- [ ] Weapon break effects
- [ ] Risk–reward upgrades
- [ ] Random effects

**Rule**
- Add chaos only after balance is stable.

**Exit condition**
- Runs create fun stories worth sharing.

---

## What NOT to Build Yet
- Meta progression
- Skill trees
- Save system
- Multiplayer
- Too many enemy types
- Heavy polish

---

## Team Responsibility
**Person A**
- Player logic
- Customer logic
- Upgrade framework
- Combat math

**Person B**
- Scenes
- UI
- Prefabs
- Balance ideas
- Visual clarity

---

End of roadmap.
