-> intro_quiz
=== intro_quiz
VAR pNavg = 0
VAR pSwth = 0
VAR pRdwk = 0
VAR pDldm = 0
VAR pPort = 0
VAR pStar = 0
VAR pSalt = 0
VAR pSpec = 0
VAR pSpecName = ""
LIST quizCategories = (navg), (swth), (rdwk), (dldm), (salt)
TODO write an actual introduction
(The quiz has some kind of introduction)
- (opts)
~ temp next_category = pop_random(quizCategories)
~ temp category_questions = -> answered_q
{ next_category:
- navg: ~ category_questions = -> navigation_qs
- swth: ~ category_questions = -> seaworthy_qs
- rdwk: ~ category_questions = -> rudderwork_qs
- dldm: ~ category_questions = -> doldrums_qs
- salt: ~ category_questions = -> salt_qs
}
-> category_questions ->
{ answered_q < 6: -> opts }
- (handedness_q)
Are you left- or right-handed?
+ [sail_left: LEFT]
~ alter(pPort, 3)
+ [sail_right: RIGHT]
~ alter(pStar, 3)
- 
TODO quiz outro, assign the right personality type. now that i think abt it finding the highest + lowest stats by hand in ink is rlly annoying so maybe need to talk to programmers abt that
(The quiz has some kind of outro)
- -> DONE
= answered_q //empty so we can count how many qs we get
- ->->
= navigation_qs
- (lie_1)
Do you often lie  when there's no good reason to?
+ [\(sail_left\): CONSTANTLY]
~ alter(pNavg, 1)
~ alter(pSwth, 1)
+ [\(sail_straight\): ONLY SOMETIMES]
~ alter(pNavg, 2)
+ [\(sail_right\): I'M LYING RIGHT NOW]
~ alter(pSwth, 2)
- -> answered_q ->
{ answered_q < 6: -> lie_2 }
->->
- (lie_2) //followup to lie_1
Your father suspects you're lying and threatens to punish you! What do you do?
+ [\(sail_left\): CONVINCE HIM IT WAS A MISTAKE]
~ alter(pNavg, 2)
+ [\(sail_straight\): CONFESS BEFORE IT GETS WORSE]
~ alter(pNavg, 1)
~ alter(pStar, 1)
+ [\(sail_right\): BLAME A SERVANT!]
~ alter(pSalt, 2)
- -> answered_q ->
->->
TODO 2 more navigation qs
= seaworthy_qs
- (sick)
Have you ever been sick for so long you thought you might never recover?
+ [\(sail_left\): ONLY ONCE]
~ alter(pSwth, 1)
+ [\(sail_straight\): MANY TIMES]
~ alter(pDldm, 2)
+ [\(sail_right\): NEVER]
~ alter(pSwth, 2)
- -> answered_q ->
->->
- (omens)
Do you believe in signs and lucky omens?
+ [\(sail_left\): NO]
~ alter(pNavg, 1)
~ alter(pSalt, 1)
+ [sail_straight: YES]
~ alter(pSwth, 2)
+ [\(sail_right\): ONLY WHEN THEY'RE SCARY!]
~ alter(pDldm, 1)
- -> answered_q ->
->->
- (eye)
When you have to read something close to your face, which eye do you close?
+ [\(sail_left\): LEFT]
~ alter(pStar, 1)
+ [\(sail_straight\): RIGHT]
~ alter(pPort, 1)
+ [\(sail_right\): NEITHER...?]
~ alter(pSwth, 1)
- -> answered_q ->
->->
TODO 1 more seaworthy q

= rudderwork_qs
- (accomplishment)
Can you generally accomplish anything you decide to do?
+ [\(sail_left\): YES]
~ alter(pNavg, 2)
~ alter(pRdwk, 2)
+ [\(sail_straight\): NO]
~ alter(pDldm, 2)
+ [\(sail_right\): IF IT'S EASY]
~ alter(pRdwk, 1)
- -> answered_q ->
->->
- (orders)
Do you rudely order your father's servants and chancellors about?
+ [\(sail_left\): IF I NEED SOMETHING]
~ alter(pRdwk, 1)
+ [\(sail_straight\): JUST FOR FUN]
~ alter(pSalt, 2)
+ [\(sail_right\): I WOULDN'T SAY RUDELY...]
~ alter(pRdwk, 1)
~ alter(pNavg, 1)
- -> answered_q ->
->->
TODO 2 more rudderwork qs
= doldrums_qs
- (hard_on_self)
Do you tend to be quite hard on yourself, as if you can't do anything right?
+ [\(sail_left\): YES]
~ alter(pDldm, 2)
+ [\(sail_right\): NO]
~ alter(pSalt, 1)
~ alter(pSwth, 1)
- -> answered_q ->
->->
- (doorway)
When passing someone in a crowded doorway, do you turn toward them or away from them?
+ [\(sail_left\): AWAY]
~ alter(pDldm, 2)
~ alter(pPort, 1)
+ [\(sail_straight\): TOWARD]
~ alter(pRdwk, 1)
+ [\(sail_right\): WAIT FOR THEM TO GO FIRST]
~ alter(pDldm, 1)
~ alter(pNavg, 1)
- -> answered_q ->
->->
TODO 2 more doldrums qs
= salt_qs
- (servants)
You overhear the servants laughing at you behind your back! What do you do?
+ [\(sail_left\): CONFRONT THEM]
~ alter(pRdwk, 1)
+ [\(sail_straight\): INFORM YOUR FATHER]
~ alter(pSalt, 2)
+ [\(sail_right\): PLAY A PRANK ON THEM]
~ alter(pNavg, 1)
~ alter(pSalt, 1)
- -> answered_q ->
->->
TODO 3 more salt qs. 1 of these should be smth like "what is ur favorite food" (BIG OMINOUS LETTERS) PEANUT BUTTER as discussed w gary. i feel like salt is the appropriate category for that
=== function alter(ref k, x)
~ k = k + x
=== function pop_random(ref _list) 
    ~ temp el = LIST_RANDOM(_list) 
    ~ _list -= el
    ~ return el 