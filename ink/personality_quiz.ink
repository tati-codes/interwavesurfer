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
- gary */
VAR progress = 0 
//stephen:
VAR navigation = 50
VAR seaworthy = 50
VAR rudderwork = 50
VAR doldrums = 50
VAR portside = 100
VAR starboard = 100
VAR salt = 50
//potential "special" stats, 1 of the below
VAR albatross = 0
VAR honor = 0
VAR impish = 0
VAR magnetism = 0
VAR tar = 0
VAR joviaL = 0
VAR normal = 0
VAR groove = 0
VAR iconoclast = 0
VAR saturnine = 0
- -> DONE
=== intro_quiz
LIST quizCategories = (navQ), (swthQ), (rdwkQ), (dolQ), (saltQ)
From a Land of Sunken Ash. #laur
Only a mailed fist as firm as Duke Montelroy's could elevate the Digal house.
From obscurity to infamy. Destitution to decadence. Despair to Digal.
A transformation already becoming legend.
Your father's legacy is unassailable.
+ [So he says.]
- But never mind that for now. #general
Before I can let you depart, I need to ask you a few questions. 
I'd like you to answer earnestly.
Ready? Let's begin.
- (quiz_opts)
//<- stat_panel //this is just for debugging / balancing stat increases from questions - stephen
~ temp category_questions = -> answered_q
{ LIST_COUNT(quizCategories) == 0 && answered_q <= 5: //if you didn't get a followup q to any of the first 5 questions, get us a 6th Q by duplicating a random category
~ quizCategories = LIST_ALL(quizCategories)
}
~ temp next_category = pop_random(quizCategories)
{ next_category:
- navQ: ~ category_questions = -> navigation_qs
- swthQ: ~ category_questions = -> seaworthy_qs
- rdwkQ: ~ category_questions = -> rudderwork_qs
- dolQ: ~ category_questions = -> doldrums_qs
- saltQ: ~ category_questions = -> salt_qs
- else: -> handedness_q //extra fallback
}
-> category_questions ->
{ answered_q < 6: -> quiz_opts }
- (handedness_q)
//<- stat_panel //just for debugging
Are you left- or right-handed?  #general
+ [sail_left: LEFT]
~ alter(portside, 150)
+ [sail_right: RIGHT]
~ alter(starboard, 150)
-
- (ending_quiz)
I see...in that case...
-> quiz_results ->
That will do well enough for now.
+ [This seems pointless.] 
- ~ navigation = 300
~ seaworthy = 200
~ rudderwork = 50
~ doldrums = 200
~ salt = 100
{ portside > 0:
- true: ~ portside = 450
- false: ~ starboard = 450
}
Oh yes. You. Not cast out, leaping of your own volition. #laur
Shorn of rotten ties. To seize the one bond that holds fast.
What legacy awaits the crest of your wave?
//<- stat_panel
- -> DONE
= answered_q //empty so we can count how many qs we get
- ->->
= laur_talk
~ alter(progress, 1)
//wrapping progress == 1 into the implicit/no conditions met dropdown!
{ progress == 2: -> laur2 }
{ progress == 3: -> laur3 }
{ progress == 4: -> laur4 }
{ progress == 5: -> laur5 }
{progress == 6: -> laur6 }
- (laur1)
The lies of a child are shallow squabbles. #laur
A lie is merely an untruth. Lady Esselie is an artisan.
She spins fabrications so intricate any truth pales in comparison. 
Her unshakeable faith is a bulwark against untangling what was ever true.
+ [What was ever true?] -> answered_q
- (laur2)
Wings to turn wind. Breath to end battle. Belly to demand rubs. #laur
Triscuit Digal never returned.
His mission so secret, even the lady truly knows it not.
It must be of great import to leave the land devoid of dragon.
+ [Still wake up lonely.] -> answered_q
- (laur3)
The land has its share of ministers. #laur
Miniscule men with petty affairs puppeted by the chancellor atop.
Some claim that the whole capital dances for his grand machinations.
Such claimants spend their last breath carelessly.
+ [Or carefully.] -> answered_q
- (laur4)
There once was the lodestar. Descended to us in pieces. Every part a treasure. #laur
To be of the stone itself is a sacred duty an unquestionable designation above reproach.
True good transcends kind words and comforting gestures.
It must be seized with stained hands.
+ [Don't seize, do.] -> answered_q
- (laur5)
Beware the nameless. #laur
It is said that to be named is to anchor yourself to a home.
The most fearsome of foes freely forsakes a name, without crime, without disgrace.
What does it leave them with?
+ [Unmatched skill.] -> answered_q
- (laur6)
Ahhh, but let us not forget... #laur
The instigator. [X]. Renounced. Disowned. Excommunicated. Exiled. Struck.
And yet, alive?
+ [Or so you hope.] -> answered_q
->->
= navigation_qs
{ RANDOM(1, 3):
- 1: -> lie_1
- 2: -> someone_else
- 3: -> lonely
}
- (lie_1)
Do you often lie when there's no good reason to? #general
+ [\(sail_left\): CONSTANTLY]
~ alter(navigation, 50)
~ alter(seaworthy, 50)
+ [\(sail_straight\): ONLY SOMETIMES]
~ alter(navigation, 100)
+ [\(sail_right\): I'M LYING RIGHT NOW]
~ alter(seaworthy, 100)
- -> laur_talk ->
{ answered_q < 6: -> lie_2 }
->->
- (lie_2) //followup to lie_1
Your father suspects you're lying and threatens to punish you! What do you do? #general
+ [\(sail_left\): CONVINCE HIM IT WAS A MISTAKE]
~ alter(navigation, 100)
+ [\(sail_straight\): CONFESS BEFORE IT GETS WORSE]
~ alter(navigation, 50)
~ alter(starboard, 50)
+ [\(sail_right\): BLAME A SERVANT!]
~ alter(rudderwork, 50)
~ alter(salt, 50)
- -> laur_talk ->
->->
- (someone_else)
Do you often imagine events in your life as if they were happening to someone else? #general
+ [\(sail_left\): YES]
~ alter(navigation, 50)
~ alter(doldrums, 50)
~ alter(portside, 50)
+ [\(sail_right\): NO]
~ alter(rudderwork, 50)
~ alter(starboard, 50)
- -> laur_talk ->
->->
- (lonely)
Can you spend all day on your own, without becoming bored or lonely? #general
+ [\(sail_left\): WHEN I'M HAPPY]
~ alter(navigation, 100)
+ [sail_straight: NOT AT ALL]
~ alter(rudderwork, 50)
~ alter(seaworthy, 50)
+ [\(sail_right\): WHEN I'M SAD]
~ alter(doldrums, 50)
- -> laur_talk ->
->->
= seaworthy_qs
{ RANDOM(1, 3):
- 1: -> sick
- 2: -> omens
- 3: -> eye
}
- (sick)
Have you ever been sick for so long you thought you might never recover? #general
+ [\(sail_left\): ONLY ONCE]
~ alter(seaworthy, 50)
+ [\(sail_straight\): MANY TIMES]
~ alter(doldrums, 100)
+ [\(sail_right\): NEVER]
~ alter(seaworthy, 100)
- -> laur_talk ->
->->
- (omens)
Do you believe in signs and lucky omens? #general
+ [\(sail_left\): NO]
~ alter(salt, 50)
+ [sail_straight: YES]
~ alter(seaworthy, 100)
+ [\(sail_right\): ONLY WHEN THEY'RE SCARY!]
~ alter(doldrums, 50)
~ alter(seaworthy, 50)
- -> laur_talk ->
->->
- (eye)
When you have to read something close to your face, which eye do you close? #general
+ [\(sail_left\): LEFT]
~ alter(starboard, 50)
+ [\(sail_straight\): RIGHT]
~ alter(portside, 50)
+ [\(sail_right\): NEITHER...?]
~ alter(seaworthy, 50)
- -> laur_talk ->
->->
- (help_fran2) //followup to (bluff) from help_fran1
You get started on Fran's problem and have no idea what you're doing. What's your plan? #general
+ [\(sail_left\): ASK TRISKIE]
~ alter(rudderwork, 50)
+ [\[sail_straight\): PRETEND TO FORGET]
~ alter(seaworthy, 50)
~ alter(doldrums, 50)
+ [\(sail_right\): MAYBE I SHOULD FESS UP...]
~ alter(seaworthy, 100)
- -> laur_talk ->
->->
= rudderwork_qs
{ RANDOM(1, 3):
- 1: -> accomplishment
- 2: -> orders
- 3: -> help_fran1
}
- (accomplishment)
Can you generally accomplish anything you decide to do?  #general
+ [\(sail_left\): YES]
~ alter(navigation, 50)
~ alter(rudderwork, 50)
+ [\(sail_straight\): NO]
~ alter(doldrums, 100)
+ [\(sail_right\): IF IT'S EASY]
~ alter(rudderwork, 50)
- -> laur_talk ->
->->
- (orders)
Do you rudely order your father's servants and chancellors about? #general
+ [\(sail_left\): IF I NEED SOMETHING]
~ alter(rudderwork, 50)
+ [\(sail_straight\): JUST FOR FUN]
~ alter(salt, 50)
+ [\(sail_right\): I WOULDN'T SAY RUDELY...]
~ alter(rudderwork, 100)
~ alter(navigation, 50)
- -> laur_talk ->
->->
- (help_fran1)
Fran is asking for your help when you suddenly realize you weren't paying attention! What do you say? #general
+ (bluff)[\(sail_left\): LEAVE IT TO ME!]
~ alter(seaworthy, 50)
+ [\[sail_straight\): YOU'VE GOT THIS!]
~ alter(rudderwork, 50)
+ [\(sail_right\): UHH...SAY AGAIN?]
~ alter(navigation, 50)
~ alter(salt, 50)
- -> laur_talk ->
{ answered_q < 6 && bluff: -> help_fran2 ->}
{ answered_q < 6: -> good_side -> }
->->
- (good_side) //followup to 2/3 of help_fran1
When a friend is angry with you, how do you get back on their good side? #general
+ [\(sail_left\): WRITE AN APOLOGY]
~ alter(rudderwork, 50)
+ [\[(sail_straight\): GIVE THEM A PRESENT]
~ alter(rudderwork, 100)
+ [(\sail_right\): THIS FRIEND IS FRAN HUH]
~ alter(doldrums, 50)
~ alter(navigation, 50)
- -> laur_talk ->
->->
= doldrums_qs
{ RANDOM(1, 4):
- 1: -> hard_on_self
- 2: -> doorway
- 3: -> monologue
- 4: -> accomplishments
}
- (hard_on_self)
Do you tend to be quite hard on yourself, as if you can't do anything right? #general
+ [\(sail_left\): YES]
~ alter(doldrums, 100)
+ [\(sail_right\): NO]
~ alter(salt, 50)
~ alter(seaworthy, 50)
- -> laur_talk ->
->->
- (doorway)
When passing someone in a crowded doorway, do you turn toward them or away from them? #general
+ [\(sail_left\): AWAY]
~ alter(doldrums, 100)
~ alter(portside, 50)
+ [\(sail_straight\): TOWARD]
~ alter(rudderwork, 50)
+ [\(sail_right\): WAIT FOR THEM TO GO FIRST]
~ alter(doldrums, 50)
~ alter(navigation, 50)
- -> laur_talk ->
->->
- (monologue)
When you talk to yourself, are you more likely to call yourself "I", "you", or "we"? #general
+ ["I"]
~ alter(salt, 50)
+ ["YOU"]
~ alter(doldrums, 100)
+ ["WE"]
~ alter(doldrums, 50)
~ alter(navigation, 50)
- -> laur_talk ->
->->
- (accomplishments)
Do you have a difficult time hearing about the accomplishments of someone else? Whose? #general
+ [\(sail_left\): THOSE I LIKE]
~ alter(doldrums, 100)
+ [sail_straight: THOSE I DISLIKE]
~ alter(rudderwork, 50)
+ [\(sail_right\): THERE'S ONE PERSON...]
~ alter(doldrums, 50)
~ alter(salt, 50)
- -> laur_talk ->
->->
= salt_qs
{ RANDOM(1, 4):
- 1: -> servants
- 2: -> scolding
- 3: -> lodestone
- 4: -> favorite_food
}
- (servants)
You overhear the servants laughing at you behind your back! What do you do? #general
+ [\(sail_left\): CONFRONT THEM]
~ alter(salt, 50)
+ [\(sail_straight\): INFORM YOUR FATHER]
~ alter(rudderwork, 50)
+ [\(sail_right\): PLAY A PRANK ON THEM]
~ alter(navigation, 100)
~ alter(salt, 50)
- -> laur_talk ->
->->
- (scolding)
What do you think about when someone more important scolds you? #general
+ [\(sail_left\): CORRECTING MYSELF]
~ alter(navigation, 50)
+ [\(sail_straight\): LOOKING SORRY ENOUGH]
~ alter(salt, 50)
~ alter(rudderwork, 50)
+ [\(sail_right\): GETTING EVEN LATER]
~ alter(salt, 100)
- -> laur_talk ->
->->
- (lodestone)
Have you ever lost your lodestone? #general
+ [\(sail_left\): OF COURSE NOT!]
~ alter(navigation, 50)
+ [\(sail_right\): WHAT, AM I IN TROUBLE?]
~ alter(salt, 50)
~ alter(doldrums, 50)
- -> laur_talk ->
->->
- (favorite_food)
What's your favorite food? #general
+ [\(sail_left\): ORANGES]
~ alter(seaworthy, 50)
+ [\(sail_straight\): PEANUT BUTTER]
~ alter(salt, 50)
+ [\(sail_right\): BRISKET NOODLE SOUP]
~ alter(rudderwork, 50)
~ alter(salt, 50)
- -> laur_talk ->
->->
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
~ resulting_stat = -> groove_result
- (highest_stat == "doldrums" && lowest_stat == "navigation") || (highest_stat == "salt" && lowest_stat == "seaworthy"):
~ resulting_stat = -> iconoclast_result
- (highest_stat == "doldrums" && lowest_stat == "rudderwork") || (highest_stat == "salt" && lowest_stat == "navigation"):
~ resulting_stat = -> saturnine_result
}
-> resulting_stat ->
TODO might need to write more fleshed out descriptions?
->->
= albatross_result //+nav -seaworthy || +rudder -doldrums
You're a bit of an ALBATROSS, it seems.
One wind blows you this way, another blows you that way.
You weren't meant for your old life--when you're not moving, you feel low.
~ albatross = 1000
->->
= honor_result //+nav -rudder || +seaworthy -salt
You seem to possess a strong sense of HONOR.
Not self-serving compliance, like the others, but real conviction.
Dangerous, isn't it?
~ honor = 1000
->->
= impish_result //+nav -doldrums || +salt -rudder
I imagine you're quite an IMPISH character!
You like to laugh, and you like to shock others. No wonder you weren't the favorite.
You have a powerful weapon, so try not to cut yourself.
~ impish = 1000
->->
= magnetism_result //+nav -salt || +doldrums -seaworthy
You show a certain personal MAGNETISM.
Without liking it, without being to able to describe it, you have a strong personality.
Have we met before? You seem so familiar.
~ magnetism = 1000
->->
= tar_result //+seaworth -nav || +rudder -seaworth
{ seaworthy >= rudderwork:
- true: You've a fair amount of TAR on you already.
~ tar = 1000
- else: You'd do well to build up some TAR on this voyage.
~ tar = 100
}
A bit of toughness to help you along when few others will.
Are you ready? Are you certain?
->->
= jovial_result //+seaworth -rudder || +salt -doldrum
You seem a fairly JOVIAL character.
They're not sure where you get it from.
Maybe you haven't suffered, but you're not as naive as those around you raised you to be.
~ joviaL = 1000
->->
= normal_result //+seaworth -doldrum || +rudder -salt
Your disposition seems NORMAL.
~ normal = 450
->->
= groove_result //+rudder -nav || +doldrum -salt
{ doldrums >= rudderwork:
- true: ~ groove = 100
Perhaps you need to search harder for
- false: ~ groove = 450
Perhaps you're about to find
}
<>...let's call it a "GROOVE".
Momentum. Top form. The current your ship was made to sail.
Of course, it may not take you where you need to go.
->->
= iconoclast_result //+doldrum -nav || +salt -seaworthy
You may not look it, but you're a bit of an ICONOCLAST, aren't you?
When someone stacks blocks, you knock them over--whether or not you meant to.
You don't mind destroying yourself to find the truth.
~ iconoclast = 1000
->->
= saturnine_result //+doldrum -rudder || +salt -nav
You have a rather SATURNINE disposition, don't you?
As did those who came before. It's a wonder you weren't favored in your own House.
But you're not concerned with the same things they are, are you?
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
{groove > 0:GROOVE\: {printRank(groove)}|}
{iconoclast > 0:ICONOCLAST\: {printRank(iconoclast)}|}
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