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
VAR navigation = 50 //navigation
VAR seaworthy = 50 //seaworthy
VAR rudderwork = 50 //rudderwork
VAR doldrums = 50 //doldrums
VAR portside = 100 //portside--exclusive w/starboard
VAR starboard = 100 //starboard--exclusive w/portside
VAR salt = 50 //salt
//potential "special" stats, 1 of the below
VAR albatross = 0  //albatross
VAR honor = 0 //honor
VAR impish = 0 //impish
VAR magnetism = 0 //magnetism
VAR tar = 0 //tar
VAR joviaL = 0 //jovial
VAR normal = 0 //normal
VAR mojo = 0 //mojo
VAR firebug = 0 //firebug
VAR saturnine = 0 //saturnine
- -> DONE
=== intro_quiz
LIST quizCategories = (navQ), (swthQ), (rdwkQ), (dolQ), (saltQ)
TODO write an actual introduction
But never mind that for now.
Before I can let you depart, I need to ask you a few questions.
I'd like you to answer earnestly.
Ready? Let's begin.
- (opts)
//<- stat_panel //this is just for debugging / balancing stat increases from questions - stephen
~ temp next_category = pop_random(quizCategories)
~ temp category_questions = -> answered_q
{ next_category:
- navQ: ~ category_questions = -> navigation_qs
- swthQ: ~ category_questions = -> seaworthy_qs
- rdwkQ: ~ category_questions = -> rudderwork_qs
- dolQ: ~ category_questions = -> doldrums_qs
- saltQ: ~ category_questions = -> salt_qs
}
-> category_questions ->
{ answered_q < 6: -> opts }
- (handedness_q)
//<- stat_panel //just for debugging
Are you left- or right-handed?
+ [sail_left: LEFT]
~ alter(portside, 150)
+ [sail_right: RIGHT]
~ alter(starboard, 150)
- I see...in that case...
-> quiz_results ->
That will do well enough for now.
~ navigation = 300
~ seaworthy = 200
~ rudderwork = 50
~ doldrums = 200
~ salt = 100
{ portside > 0:
- true: ~ portside = 450
- false: ~ starboard = 450
}
<- stat_panel
- -> DONE
= answered_q //empty so we can count how many qs we get
- ->->
= navigation_qs
- (lie_1)
Do you often lie  when there's no good reason to?
+ [\(sail_left\): CONSTANTLY]
~ alter(navigation, 50)
~ alter(seaworthy, 50)
+ [\(sail_straight\): ONLY SOMETIMES]
~ alter(navigation, 100)
+ [\(sail_right\): I'M LYING RIGHT NOW]
~ alter(seaworthy, 100)
- -> answered_q ->
{ answered_q < 6: -> lie_2 }
->->
- (lie_2) //followup to lie_1
Your father suspects you're lying and threatens to punish you! What do you do?
+ [\(sail_left\): CONVINCE HIM IT WAS A MISTAKE]
~ alter(navigation, 100)
+ [\(sail_straight\): CONFESS BEFORE IT GETS WORSE]
~ alter(navigation, 50)
~ alter(starboard, 50)
+ [\(sail_right\): BLAME A SERVANT!]
~ alter(salt, 100)
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
~ alter(seaworthy, 50)
+ [\(sail_straight\): MANY TIMES]
~ alter(doldrums, 100)
+ [\(sail_right\): NEVER]
~ alter(seaworthy, 100)
- -> answered_q ->
->->
- (omens)
Do you believe in signs and lucky omens?
+ [\(sail_left\): NO]
~ alter(navigation, 50)
~ alter(salt, 50)
+ [sail_straight: YES]
~ alter(seaworthy, 100)
+ [\(sail_right\): ONLY WHEN THEY'RE SCARY!]
~ alter(doldrums, 50)
- -> answered_q ->
->->
- (eye)
When you have to read something close to your face, which eye do you close?
+ [\(sail_left\): LEFT]
~ alter(starboard, 50)
+ [\(sail_straight\): RIGHT]
~ alter(portside, 50)
+ [\(sail_right\): NEITHER...?]
~ alter(seaworthy, 50)
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
~ alter(navigation, 50)
~ alter(rudderwork, 50)
+ [\(sail_straight\): NO]
~ alter(doldrums, 100)
+ [\(sail_right\): IF IT'S EASY]
~ alter(rudderwork, 50)
- -> answered_q ->
->->
- (orders)
Do you rudely order your father's servants and chancellors about?
+ [\(sail_left\): IF I NEED SOMETHING]
~ alter(rudderwork, 50)
+ [\(sail_straight\): JUST FOR FUN]
~ alter(salt, 100)
+ [\(sail_right\): I WOULDN'T SAY RUDELY...]
~ alter(rudderwork, 50)
~ alter(navigation, 50)
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
~ alter(doldrums, 100)
+ [\(sail_right\): NO]
~ alter(salt, 50)
~ alter(seaworthy, 50)
- -> answered_q ->
->->
- (doorway)
When passing someone in a crowded doorway, do you turn toward them or away from them?
+ [\(sail_left\): AWAY]
~ alter(doldrums, 100)
~ alter(portside, 50)
+ [\(sail_straight\): TOWARD]
~ alter(rudderwork, 50)
+ [\(sail_right\): WAIT FOR THEM TO GO FIRST]
~ alter(doldrums, 50)
~ alter(navigation, 50)
- -> answered_q ->
->->
TODO 2 more doldrums qs
= salt_qs
- (servants)
You overhear the servants laughing at you behind your back! What do you do?
+ [\(sail_left\): CONFRONT THEM]
~ alter(salt, 100)
+ [\(sail_straight\): INFORM YOUR FATHER]
~ alter(rudderwork, 50)
+ [\(sail_right\): PLAY A PRANK ON THEM]
~ alter(navigation, 50)
~ alter(salt, 50)
- -> answered_q ->
->->
TODO 3 more salt qs. 1 of these should be smth like "what is ur favorite food" (BIG OMINOUS LETTERS) PEANUT BUTTER as discussed w gary. i feel like salt is the appropriate category for that
=== quiz_results
{ portside >= starboard: //first set handedness
- true: ~ starboard = 0
- false: ~ portside = 0
}
//<- stat_panel //just for debugging
VAR highest_stat = ""
VAR lowest_stat = ""
>>> CUTSCENE: out of the var navigation, seaworthy, rudderwork, doldrums, salt, find the one with the HIGHEST numerical value (breaking ties randomly) + set the var highest_stat to a string [name of the lowest stat]
>>> CUTSCENE: out of the var navigation, seaworthy, rudderwork, doldrums, salt, find the one with the LOWEST numerical value (breaking ties randomly) + set the var lowest_stat to a string [name of the lowest stat]
~ temp resulting_stat = -> honor_result
{
- (highest_stat == "navigation" && lowest_stat == "seaworthy") || (highest_stat == "rudderwork" && lowest_stat == "doldrums"):
~ resulting_stat = -> albatross_result
- (highest_stat == "navigation" && lowest_stat == "rudderwork") || (highest_stat == "seaworthy" && lowest_stat == "salt"):
~ resulting_stat = -> honor_result
- (highest_stat == "navigation" && lowest_stat == "doldrums") || (highest_stat == "salt" && lowest_stat == "rudderwork"):
~ resulting_stat = -> impish_result
- (highest_stat == "navigation" && lowest_stat == "salt") || (highest_stat == "doldrums" && lowest_stat == "seaworthy"):
~ resulting_stat = -> magnetism_result
- (highest_stat == "seaworthy" && lowest_stat == "navigation") || (highest_stat == "rudderwork" && lowest_stat == "seaworthy"):
~ resulting_stat = -> tar_result
- (highest_stat == "seaworthy" && lowest_stat == "rudderwork") || (highest_stat == "salt" && lowest_stat == "doldrums"):
~ resulting_stat = -> jovial_result
- (highest_stat == "seaworthy" && lowest_stat == "doldrums") || (highest_stat == "rudderwork" && lowest_stat == "salt"):
~ resulting_stat = -> normal_result
- (highest_stat == "rudderwork" && lowest_stat == "navigation") || (highest_stat == "doldrums" && lowest_stat == "salt"):
~ resulting_stat = -> mojo_result
- (highest_stat == "doldrums" && lowest_stat == "navigation") || (highest_stat == "salt" && lowest_stat == "seaworthy"):
~ resulting_stat = -> firebug_result
- (highest_stat == "doldrums" && lowest_stat == "rudderwork") || (highest_stat == "salt" && lowest_stat == "navigation"):
~ resulting_stat = -> saturnine_result
}
-> resulting_stat ->
TODO might need to write more fleshed out descriptions?
->->
= albatross_result //+nav -seaworthy || +rudder -doldrums
You're a bit of an ALBATROSS, it seems.
~ albatross = 1000
->->
= honor_result //+nav -rudder || +seaworthy -salt
You seem to possess a strong sense of HONOR.
~ honor = 1000
->->
= impish_result //+nav -doldrums || +salt -rudder
You seem a rather IMPISH character.
~ impish = 1000
->->
= magnetism_result //+nav -salt || +doldrums -seaworthy
You show a certain personal MAGNETISM.
~ magnetism = 1000
->->
= tar_result //+seaworth -nav || +rudder -seaworth
{ seaworthy >= rudderwork:
- true: You've a fair amount of TAR to you already.
~ tar = 1000
- else: You'd do well to pick up a bit more TAR on this voyage.
~ tar = 100
}
->->
= jovial_result //+seaworth -rudder || +salt -doldrum
You seem a fairly JOVIAL character.
~ joviaL = 1000
->->
= normal_result //+seaworth -doldrum || +rudder -salt
Your disposition seems NORMAL enough.
~ normal = 450
->->
= mojo_result //+rudder -nav || +doldrum -salt
{ doldrums >= rudderwork:
- true: ~ mojo = 100
Perhaps this voyage will provide you with a bit
- else: ~ mojo = 1000
You possess a great deal
}
<> of...I believe it's termed "MOJO".
->->
= firebug_result //+doldrum -nav || +salt -seaworthy
You may not look it, but you're a bit of a FIREBUG, aren't you?
~ firebug = 1000
->->
= saturnine_result //+doldrum -rudder || +salt -nav
You have a rather SATURNINE disposition, don't you?
~ saturnine = 1000
->->
=== stat_panel
//exposing the numerical values is just for debugging! will comment that out later - stephen
NAVIGATION: {printRank(navigation)} //(\= {navigation})
SEAWORTHY: {printRank(seaworthy)} //(\= {seaworthy})
RUDDERWORK: {printRank(rudderwork)} //(\= {rudderwork})
DOLDRUMS: {printRank(doldrums)} //(\= {doldrums})
//in the quiz u start w/a bit of both but you only get 1 by the end of the quiz
{portside > 0:PORTSIDE\: {printRank(portside)}|} //{portside > 0:PORTSIDE\: {printRank(portside)} (\= {portside})|}
{starboard > 0:STARBOARD\: {printRank(starboard)}|} //{starboard > 0:STARBOARD\: {printRank(starboard)} (\= {starboard})|}
SALT {printRank(salt)} //(\= {salt})
//only 1 of these potential special stats
{albatross > 0:ALBATROSS\: {printRank(albatross)}|}
{honor > 0:HONOR\: {printRank(honor)}|}
{impish > 0:IMPISH\: {printRank(impish)}|}
{magnetism > 0:MAGNETISM\: {printRank(magnetism)}|}
{tar >0:TAR\: {printRank(tar)}|}
{joviaL > 0:JOVIAL\: {printRank(joviaL)}|}
{normal > 0:NORMAL\: {printRank(normal)}|}
{mojo > 0:MOJO\: {printRank(mojo)}|}
{firebug > 0:FIREBUG\: {printRank(firebug)}|}
{saturnine >0:SATURNINE\: {printRank(saturnine)}|}
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