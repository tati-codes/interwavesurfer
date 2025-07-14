-> intro_quiz
=== player_stats
/*NOTE for assigning stats in the quiz i'm imagining some kind of RPG scaling that looks like this?? we can tweak this altho like. it doesn't matter for gameplay
1k+ = [crescent moon + up]
999-650 = S
649-450 = A
449-300 = B
299-200 = C
199-100 = D
99-50 = E
49-1 = F
- stephen */
VAR pNav = 50 //navigation
VAR pSwth = 50 //seaworthy
VAR pRdwk = 50 //rudderwork
VAR pDol = 50 //doldrums
VAR pPort = 100 //portside--exclusive w/starboard
VAR pStar = 100 //starboard--exclusive w/portside
VAR pSalt = 50 //salt
//potential "special" stats, 1 of the below
VAR pAlb = 0  //albatross
VAR pHonr = 0 //honor
VAR pImp = 0 //impish
VAR pMag = 0 //magnetism
VAR pTar = 0 //tar
VAR pJov = 0 //jovial
VAR pNorm = 0 //normal
VAR pMojo = 0 //mojo
VAR pFbug = 0 //firebug
VAR pStrn = 0 //saturnine
- -> DONE
=== intro_quiz
LIST quizCategories = (nav), (swth), (rdwk), (dol), (salt)
TODO write an actual introduction
But never mind that for now.
Before I can let you depart, I need to ask you a few questions.
I'd like you to answer earnestly.
Ready? Let's begin.
- (opts)
//this is just for debugging / balancing questions -- stephen
<- stat_panel
~ temp next_category = pop_random(quizCategories)
~ temp category_questions = -> answered_q
{ next_category:
- nav: ~ category_questions = -> navigation_qs
- swth: ~ category_questions = -> seaworthy_qs
- rdwk: ~ category_questions = -> rudderwork_qs
- dol: ~ category_questions = -> doldrums_qs
- salt: ~ category_questions = -> salt_qs
}
-> category_questions ->
{ answered_q < 6: -> opts }
- (handedness_q)
<- stat_panel
Are you left- or right-handed?
+ [sail_left: LEFT]
~ alter(pPort, 150)
+ [sail_right: RIGHT]
~ alter(pStar, 150)
-
TODO quiz outro, assign the right personality type. now that i think abt it finding the highest + lowest stats by hand in ink is rlly annoying so maybe need to talk to programmers abt that
I see...in that case...
-> quiz_results ->
That will do well enough for now.
- -> DONE
= answered_q //empty so we can count how many qs we get
- ->->
= navigation_qs
- (lie_1)
Do you often lie  when there's no good reason to?
+ [\(sail_left\): CONSTANTLY]
~ alter(pNav, 50)
~ alter(pSwth, 50)
+ [\(sail_straight\): ONLY SOMETIMES]
~ alter(pNav, 100)
+ [\(sail_right\): I'M LYING RIGHT NOW]
~ alter(pSwth, 100)
- -> answered_q ->
{ answered_q < 6: -> lie_2 }
->->
- (lie_2) //followup to lie_1
Your father suspects you're lying and threatens to punish you! What do you do?
+ [\(sail_left\): CONVINCE HIM IT WAS A MISTAKE]
~ alter(pNav, 100)
+ [\(sail_straight\): CONFESS BEFORE IT GETS WORSE]
~ alter(pNav, 50)
~ alter(pStar, 50)
+ [\(sail_right\): BLAME A SERVANT!]
~ alter(pSalt, 100)
- -> answered_q ->
->->
TODO 2 more navigation qs
= seaworthy_qs
{ RANDOM(1, 3):
- 1: -> sick
- 2: -> omens
- 3: -> eye
}
- (sick)
Have you ever been sick for so long you thought you might never recover?
+ [\(sail_left\): ONLY ONCE]
~ alter(pSwth, 50)
+ [\(sail_straight\): MANY TIMES]
~ alter(pDol, 100)
+ [\(sail_right\): NEVER]
~ alter(pSwth, 100)
- -> answered_q ->
->->
- (omens)
Do you believe in signs and lucky omens?
+ [\(sail_left\): NO]
~ alter(pNav, 50)
~ alter(pSalt, 50)
+ [sail_straight: YES]
~ alter(pSwth, 100)
+ [\(sail_right\): ONLY WHEN THEY'RE SCARY!]
~ alter(pDol, 50)
- -> answered_q ->
->->
- (eye)
When you have to read something close to your face, which eye do you close?
+ [\(sail_left\): LEFT]
~ alter(pStar, 50)
+ [\(sail_straight\): RIGHT]
~ alter(pPort, 50)
+ [\(sail_right\): NEITHER...?]
~ alter(pSwth, 50)
- -> answered_q ->
->->
TODO 1 more seaworthy q
= rudderwork_qs
{ RANDOM(1, 2):
- 1: -> accomplishment
- 2: -> orders
}
- (accomplishment)
Can you generally accomplish anything you decide to do?
+ [\(sail_left\): YES]
~ alter(pNav, 50)
~ alter(pRdwk, 50)
+ [\(sail_straight\): NO]
~ alter(pDol, 100)
+ [\(sail_right\): IF IT'S EASY]
~ alter(pRdwk, 50)
- -> answered_q ->
->->
- (orders)
Do you rudely order your father's servants and chancellors about?
+ [\(sail_left\): IF I NEED SOMETHING]
~ alter(pRdwk, 50)
+ [\(sail_straight\): JUST FOR FUN]
~ alter(pSalt, 100)
+ [\(sail_right\): I WOULDN'T SAY RUDELY...]
~ alter(pRdwk, 50)
~ alter(pNav, 50)
- -> answered_q ->
->->
TODO 2 more rudderwork qs
= doldrums_qs
{ RANDOM(1, 2):
- 1: -> hard_on_self
- 2: -> doorway
}
- (hard_on_self)
Do you tend to be quite hard on yourself, as if you can't do anything right?
+ [\(sail_left\): YES]
~ alter(pDol, 100)
+ [\(sail_right\): NO]
~ alter(pSalt, 50)
~ alter(pSwth, 50)
- -> answered_q ->
->->
- (doorway)
When passing someone in a crowded doorway, do you turn toward them or away from them?
+ [\(sail_left\): AWAY]
~ alter(pDol, 100)
~ alter(pPort, 50)
+ [\(sail_straight\): TOWARD]
~ alter(pRdwk, 50)
+ [\(sail_right\): WAIT FOR THEM TO GO FIRST]
~ alter(pDol, 50)
~ alter(pNav, 50)
- -> answered_q ->
->->
TODO 2 more doldrums qs
= salt_qs
- (servants)
You overhear the servants laughing at you behind your back! What do you do?
+ [\(sail_left\): CONFRONT THEM]
~ alter(pSalt, 100)
+ [\(sail_straight\): INFORM YOUR FATHER]
~ alter(pRdwk, 50)
+ [\(sail_right\): PLAY A PRANK ON THEM]
~ alter(pNav, 50)
~ alter(pSalt, 50)
- -> answered_q ->
->->
TODO 3 more salt qs. 1 of these should be smth like "what is ur favorite food" (BIG OMINOUS LETTERS) PEANUT BUTTER as discussed w gary. i feel like salt is the appropriate category for that
=== quiz_results
{ pPort >= pStar: //first set handedness
- true: ~ pStar = 0
- false: ~ pPort = 0
}
<- stat_panel
~ temp highest_stat = ()
~ temp lowest_stat = ()
TODO probably have to debug this / see if the brave + magnanimous tati can just numerically rank stats in godot
{
- pNav >= pSwth && pNav >= pRdwk && pNav >= pDol && pNav >= pSalt:
~ highest_stat = nav
- pSwth >= pNav && pSwth >= pRdwk && pSwth >= pDol && pNav >= pSalt:
~ highest_stat = swth
- pDol >= pNav && pDol >= pSwth && pDol >= pRdwk && pDol >= pSalt:
~ highest_stat = dol
- pSalt >= pNav && pSalt >= pSwth && pSalt >= pRdwk && pSalt >= pDol:
~ highest_stat = salt
- pRdwk >= pNav && pRdwk >= pSwth && pRdwk >= pDol && pRdwk >= pSalt:
~ highest_stat = rdwk
}
{
- pNav < pSwth && pNav < pRdwk && pNav < pDol && pNav < pSalt:
~ lowest_stat = nav
- pSwth < pNav && pSwth < pRdwk && pSwth < pDol && pNav < pSalt:
~ lowest_stat = swth
- pDol < pNav && pDol < pSwth && pDol < pRdwk && pDol < pSalt:
~ lowest_stat = dol
- pSalt < pNav && pSalt < pSwth && pSalt < pRdwk && pSalt < pDol:
~ lowest_stat = salt
- else:
~ lowest_stat = rdwk
}
~ temp resulting_stat = -> honor
{
- (highest_stat ? nav && lowest_stat ? swth) || (highest_stat ? rdwk && lowest_stat ? dol):
~ resulting_stat = -> albatross
- (highest_stat ? nav && lowest_stat ? rdwk) || (highest_stat ? swth && lowest_stat ? salt):
~ resulting_stat = -> honor
- (highest_stat ? nav && lowest_stat ? dol) || (highest_stat ? salt && lowest_stat ? rdwk):
~ resulting_stat = -> impish
- (highest_stat ? nav && lowest_stat ? salt) || (highest_stat ? dol && lowest_stat ? swth):
~ resulting_stat = -> magnetism
- (highest_stat ? swth && lowest_stat ? nav) || (highest_stat ? rdwk && lowest_stat ? swth):
~ resulting_stat = -> tar
- (highest_stat ? swth && lowest_stat ? rdwk) || (highest_stat ? salt && lowest_stat ? dol):
~ resulting_stat = -> jovial
- (highest_stat ? swth && lowest_stat ? dol) || (highest_stat ? rdwk && lowest_stat ? salt):
~ resulting_stat = -> normal
- (highest_stat ? rdwk && lowest_stat ? nav) || (highest_stat ? dol && lowest_stat ? salt):
~ resulting_stat = -> mojo
- (highest_stat ? dol && lowest_stat ? nav) || (highest_stat ? salt && lowest_stat ? swth):
~ resulting_stat = -> firebug
- (highest_stat ? dol && lowest_stat ? rdwk) || (highest_stat ? salt && lowest_stat ? nav):
~ resulting_stat = -> saturnine
}
-> resulting_stat ->
TODO might need to write more fleshed out descriptions?
->->
= albatross //+nav -seaworthy || +rudder -doldrums
You're a bit of an ALBATROSS, it seems.
~ pAlb = 1000
->->
= honor //+nav -rudder || +seaworthy -salt
You seem to possess a strong sense of HONOR.
~ pHonr = 1000
->->
= impish //+nav -doldrums || +salt -rudder
You seem a rather IMPISH character.
~ pImp = 1000
->->
= magnetism //+nav -salt || +doldrums -seaworthy
You show a certain personal MAGNETISM.
~ pMag = 1000
->->
= tar //+seaworth -nav || +rudder -seaworth
{ pSwth >= pRdwk:
- true: You've a fair amount of TAR to you already.
~ pTar = 1000
- else: You'd do well to pick up a bit more TAR on this voyage.
~ pTar = 100
}
->->
= jovial //+seaworth -rudder || +salt -doldrum
You seem a fairly JOVIAL character.
~ pJov = 1000
->->
= normal //+seaworth -doldrum || +rudder -salt
Your disposition seems NORMAL enough.
~ pNorm = 450
->->
= mojo //+rudder -nav || +doldrum -salt
{ pDol >= pRdwk:
- true: ~ pMojo = 100
Perhaps this voyage will provide you with a bit
- else: ~ pMojo = 1000
You possess a great deal
}
<> of...I believe it's termed "MOJO".
->->
= firebug //+doldrum -nav || +salt -seaworthy
You may not look it, but you're a bit of a FIREBUG, aren't you?
~ pFbug = 1000
->->
= saturnine //+doldrum -rudder || +salt -nav
You have a rather SATURNINE disposition, don't you?
~ pStrn = 1000
->->
=== stat_panel
//exposing the numerical values is just for debugging! will comment that out later - stephen
NAVIGATION: {printRank(pNav)} (\= {pNav})
SEAWORTHY: {printRank(pSwth)} (\= {pSwth})
RUDDERWORK: {printRank(pRdwk)} (\= {pRdwk})
DOLDRUMS: {printRank(pDol)} (\= {pDol})
//in the quiz u start w/a bit of both but you only get 1 by the end of the quiz
{pPort > 0:PORTSIDE\: {printRank(pPort)} (\= {pPort})|}
{pStar > 0:STARBOARD\: {printRank(pStar)} (\= {pStar})|}
SALT {printRank(pSalt)} (\= {pSalt})
//only 1 of these potential special stats
{pAlb > 0:ALBATROSS\: {printRank(pAlb)}|}
{pHonr > 0:HONOR\: {printRank(pHonr)}|}
{pImp > 0:IMPISH\: {printRank(pImp)}|}
{pMag > 0:MAGNETISM\: {printRank(pMag)}|}
{pTar >0:TAR\: {printRank(pTar)}|}
{pJov > 0:JOVIAL\: {printRank(pJov)}|}
{pNorm > 0:NORMAL\: {printRank(pNorm)}|}
{pMojo > 0:MOJO\: {printRank(pMojo)}|}
{pFbug > 0:FIREBUG\: {printRank(pFbug)}|}
{pStrn >0:SATURNINE\: {printRank(pStrn)}|}
- -> DONE
=== thread_tunnel(-> link, -> backto)
~ temp entryTurnCount = TURNS()
-> link ->
{ entryTurnCount != TURNS():
-> backto
}
- -> DONE
=== function alter(ref k, x)
~ k = k + x
=== function printRank(stat)
{
- stat >= 1000:
🌙
- 999 >= stat && stat >= 650:
S
- 649 >= stat && stat >= 450:
A
- 449 >= stat && stat >= 300:
B
- 299 >= stat && stat >= 200:
C
- 199 >= stat && stat >= 100:
D
- 99 >= stat && stat >= 50:
E
- 49 >= stat && stat >= 1:
F
}
=== function pop_random(ref _list) 
    ~ temp el = LIST_RANDOM(_list) 
    ~ _list -= el
    ~ return el 