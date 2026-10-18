# Agent Instructions: Creating Short-Form Game Marketing Videos

**Purpose:** Produce vertical short videos (YouTube Shorts, TikTok, Instagram Reels) that build wishlists for an indie game.
**Source:** Distilled from an indie dev's talk on marketing their game with shorts (6M+ views, 44K wishlists at launch).

---

## 0. Inputs the agent needs before starting

Collect these from the human. Ask if any are missing; do not guess.

- [ ] Game name and genre ("X-like, but ___")
- [ ] **Unique selling point (USP):** what makes it different from similar games
- [ ] **Player fantasy:** what the player gets to be or do
- [ ] **Signature visual mechanic:** the most eye-catching thing on screen
- [ ] Core gameplay loop in one sentence
- [ ] A list of real dev problems hit during development (bugs, design issues, tough decisions), plus how each was solved or whether it is still open
- [ ] Gameplay footage library (see Step 1)
- [ ] Royalty-free or licensed music
- [ ] Store / wishlist link

---

## 1. Prepare the footage library (one-time, reused forever)

1. Record a large amount of gameplay footage in advance, so editing never requires reopening the game.
2. Cut it into short clips and **sort the clips into folders by action**, for example:
   `walking/`, `attacking/`, `group_attacking/`, `posing/`, `menus/`, `bosses/`, `funny_moments/`, `signature_mechanic/`
3. Name each clip descriptively (e.g. `attacking_fire_aoe_03.mp4`) so the right one can be pulled quickly.
4. Format: vertical 9:16, 1080×1920.

---

## 2. Choose a format (rotate through all three)

### Format A: Gameplay montage + caption (low effort, ~1 hour)
Best for volume and testing what resonates.
1. Pick 3–8 clips that show the **USP**.
2. Cut them to the music beat.
3. Add one on-screen text caption. Choose one of:
   - The genre pitch ("Vampire Survivors but 3D and holy")
   - The player fantasy ("POV: you lead a co-op dungeon raid")
   - The gameplay loop
4. Add music. No voiceover needed.
5. Make many variations (different clips, different captions).

### Format B: Mechanic-as-hook (medium effort)
1. Open on the **signature visual mechanic within the first 3 seconds**. This is the hook that stops the scroll.
2. Show something viewers haven't seen before, as early as possible (striking mechanic, distinctive character design, or art).
3. Follow with supporting gameplay that explains or expands on the mechanic.
4. Add a caption or voiceover and music.

### Format C: "The Game Dev's Problem" story (highest performing)
A story arc of intro, then conflict, then resolution, built from a real development problem.

**Script template 1 (intro first):**
1. "In my indie game, [core loop / player fantasy in one quick sentence]."
2. "But there was a problem."
3. Describe the problem in detail, with footage showing it.
4. Show the resolution, with footage showing the fix.
   - *If it's unresolved:* ask viewers how they would solve it, which drives comments.

**Script template 2 (problem first, stronger hook):**
1. One sentence saying it's your game and that it has a problem: "My indie game has a massive problem."
2. Then explain the player fantasy and gameplay.
3. Detail the problem, then resolve it (or ask the audience).

Draw on the human's list of real dev problems. Each problem can become its own short.

---

## 3. Rules that apply to every short

| Rule | Detail |
|---|---|
| **Hook in 0–3 s** | The opening frame must show something unique or intriguing. |
| **Say "my indie game" in sentence one** (voiced shorts) | It filters out uninterested viewers early, so they don't swipe away midway, and it earns indie-dev goodwill. |
| **Lead with the USP** | Always show what makes this game different. |
| **Always add music** | Shorts feel off without it, especially ones with no voiceover. |
| **Keep it short** | Cut anything that doesn't serve the hook, story, or USP. |
| **Call to action is optional, so A/B test it** | "Wishlist on Steam" at the end can raise conversions but lowers retention and virality. Test with and without. Lean toward a CTA when views are low, since every wishlist counts. |

---

## 4. Publishing checklist

1. **Cross-post every short** to YouTube Shorts, TikTok, and Instagram Reels.
2. **TikTok specifics:**
   - New account? **Warm it up for about one week first.** Only browse, like, and comment on other game-dev videos. Posting immediately gets flagged as a bot and gets about 0 views.
   - **Don't schedule TikTok posts.** Post manually, ideally **from a phone**.
   - Avoid anything that looks automated.
3. Post frequently. When launching a store page, aim for about 10 shorts in the first 2 weeks.

> ⚠️ **For the AI agent:** do not auto-post to TikTok. Prepare the file, caption, and hashtags, then hand off to the human to post manually from a phone.

---

## 5. Iterate

1. Track for each short: views, average view duration, and wishlists gained.
2. **Double down** on formats, hooks, and captions that work.
3. Borrow structures from successful game-dev creators, but vary them to fit your game's personality. Don't copy them outright.

---

## Deliverable per short (what the agent outputs)

```
title:            <internal name>
format:           A | B | C
hook (0-3s):      <what's on screen + text/voice line>
script/caption:   <full text>
clip list:        <ordered list of clips from the library, with in/out times>
music:            <track + mood>
cta:              yes | no  (note A/B variant)
platform captions:<YouTube / TikTok / Instagram text + hashtags>
posting notes:    <TikTok = manual phone post by human>
```
