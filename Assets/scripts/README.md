# Paradigm Manual

This manual focuses on explaining all the states (with their corresponding actions, decisions, and transitions) for paradigms from 800 to 1100. 800 and 900 are described in details, while add-ons in 1000 and 1100 are explained.

## P0800_LinearTrack

This paradigm rewards the animal when she is licking at the correct rewarding location. It has 20 states, and serves as the template also for paradigm 1100.

### P0800_LinearTrack

- Actions
    - P0800_TrialInitLinearTrack
        - Initialize the environment
        - Initialize the portentaOutputSHMInterface
        - Construct a dictionary "pillarCylinderMeshes" wich could be easier for other scripts to refer the pillarIdx to its mesh (not frequently used)
- Transitions
    - To [S0819_WithinInterTrialInterval](#S0819_WithinInterTrialInterval)
        - Always true

### S0801_TrialStart

- Actions
    - P0800_TrialStartLinearTrack
        - Reset most indicators: timers, successIndicator, currentRewardNum, rewardSuckedIndicator, etc. firstRewardNum records the number of rewards the animal received in the first rewarding location (only used when "DR"/double reward is switched on)
        - Manipulate the forward gain if "GF" is in trialVariablesDict
        - Randomly choose which texture to display on cue zone, or affected if "NP" is in trialVariablesDict
        - Flip the cue and reward matching if "RF" is in trialVariablesDict
        - Configure the texture in the cue zone
- Transitions
    - To [S0802_StartZone](#S0802_StartZone): 
        - Always true

### S0802_StartZone

- Actions
    - P0800_FadeOutScreen
        - Find the FadeScreen object in the scene (under PreRendering), and use Coroutine to slightly transform the screen from black to visible. Transition time is hard coded as 0.5f. Change if needed.
- Transitions
    - To [S0803_Cue1Visible](#S0803_Cue1Visible)
        - Based on the Decision P0800_RatPassCertainPillar, switch when the rat passes pillarIdx 5.

### S0803_Cue1Visible

- Actions
    - P0800_FadeInCue1
        - Based on Action P0800_FadeInCue, start to fade in the cue texture in the first cue zone. Parameters are explained:
            - Cue Name: the first or second cue to fade in. Here should be 1.
            - Cue Should Fade In: whether this cue will be faded in. Here in default is No.
            - Fade Distance: from the position of the cue pillar in the excel sheet, how much distance away the cue should start appearing. For example, the distance between pillar 5 (start entering the first cue zone) and 6 (the center of the first cue) is 100. Therefore Fade Distance as 100 means, the cue will start appearing exactly when the rat enters the first cue zone. Modify if needed.
        - Pay attention to the Fade Distance variable comparing to the distance between pillar 5 and 6 in the excel sheet. Weird things could happen if they are not reasonably matched, i.e. Fade Distance larger than the pillar distance. 
        - Also by hard coding, you can modify the fade speed.
- Transitions
    - To [S0804_Cue1](#S0804_Cue1)
        - Based on the Decision P0800_RatPassCertainPillar, switch when the rat passes pillarIdx 6.

### S0804_Cue1

- Transitions
    - To [S0805_Cue1Passed](#S0805_Cue1Passed)
        - Based on the Decision P0800_RatPassCertainPillar, switch when the rat passes pillarIdx 7.

### S0805_Cue1Passed

- Transitions
    - To [S0806_BetweenCues](#S0806_BetweenCues)
        - Based on the Decision P0800_RatPassCertainPillar, switch when the rat passes pillarIdx 8.

### S0806_BetweenCues

- Transitions
    - To [S0807_Cue2Visible](#S0807_Cue2Visible)
        - Based on the Decision P0800_RatPassCertainPillar, switch when the rat passes pillarIdx 9.

### S0807_Cue2Visible

- Actions
    - P0800_FadeInCue2
        - Refer to [P0800_FadeInCue1](#P0800_FadeInCue1)
- Transitions
    - To [S0808_Cue2](#S0808_Cue2)
        - Based on the Decision P0800_RatPassCertainPillar, switch when the rat passes pillarIdx 10.

### S0808_Cue2

- Transitions
    - To [S0809_Cue2Passed](#S0809_Cue2Passed)
        - Based on the Decision P0800_RatPassCertainPillar, switch when the rat passes pillarIdx 11.

### S0809_Cue2Passed

- Transitions
    - To [S0810_BeforeReward1](#S0810_BeforeReward1)
        - Based on the Decision P0800_RatPassCertainPillar, switch when the rat passes pillarIdx 12.

### S0810_BeforeReward1

- Transitions
    - To [S0811_Reward1](#S0811_Reward1)
        - Based on the Decision P0800_RatPassCertainPillar, switch when the rat passes pillarIdx 13.

### S0811_Reward1

- Actions
    - P0800_ResetReward
        - This state could be visited after the previous reward, therefore it is important to reset the environment (turning to white) and the timer for registering the rewards.
    - P0800_ClearSHMInput
        - In this paradigm, we detect animal licks in the reward locations for success. Therefore we should keep trashing all the events stacked in the SHM to clear it before the animal truly enters the reward location.
    - P0800_RegisterRewardZone
        - Register the current reward stateID the animal is staying, before it enters to the next state. Could be useful for some functions.

- Transitions
    - To [S0812_BeforeReward2](#S0812_BeforeReward2)
        - Based on the Decision P0800_RatPassCertainPillar, switch when the rat passes pillarIdx 14.
    - To [S0814_RewardEntry](#S0814_RewardEntry)
        - Based on the Decision P0800_RatUnderCorrectPillar, switch when the rat is at the correct reward zone.

### S0812_BeforeReward2

- Transitions
    - To [S0813_Reward2](#S0813_Reward2)
        - Based on the Decision P0800_RatPassCertainPillar, switch when the rat passes pillarIdx 15.

### S0813_Reward2

- Actions
    - P0800_ResetReward
        - Refer to [S0811_Reward1](#S0811_Reward1)
    - P0800_ClearSHMInput
        - Refer to [S0811_Reward1](#S0811_Reward1)
    - P0800_RegisterRewardZone
        - Refer to [S0811_Reward1](#S0811_Reward1)
- Transitions
    - To [S0814_RewardEntry](#S0814_RewardEntry)
        - Based on the Decision P0800_RatUnderCorrectPillar, switch when the rat is at the correct reward zone.
    - To [S0817_PostReward](#S0817_PostReward)
        - Based on the Decision P0800_RatPassCertainPillar, switch when the rat passes pillarIdx 16.

### S0814_RewardEntry

- Transitions
    - To [S0812_BeforeReward2](#S0812_BeforeReward2)
        - Based on the Decision P0800_RatLeaveRewardPillar, Switch to state S0812_BeforeReward2 if the rat hasn't reached the reward condition before leaving the region. Parameters are explained:
            - Pillar Identifier: the identifier for the corresponding rewarded pillar. For example, if the current rewarded place is the first one and the rat leaves the place, then the animal is acutally leaving the pillarIdx 3, which we notes here.
            - Last Reward State: check the current reward state (see [S0811_Reward1](#S0811_Reward1)), so that if the animal is coming from the first reward zone (811) to rewardEntry (814), next state should be 812 (since if the animal fails the reward and is leaving from the 811, she can only go to 812 next), and if the animal is coming from the second reward zone (813) to rewardEntry (814), next state should be 817 (since if the animal fails the reward and is leaving from the 813, she can only go to 817 next). Modify if the reward stateIDs change.
    - To [S0817_PostReward](#S0817_PostReward)   
        - Similar logic as the previous transition, switch to state S0817_PostReward if the rat hasn't reached the reward condition before leaving the region.
    - To [S0815_RewardStay](#S0815_RewardStay)
        - Based on the Decision P0800_RewardConditionReached, decide Whether to enter the reward state. This is also the rewarding function for paradigm 1100. It has 3 rewarding criterias where you can select or combine: Lick-based, Stay-based, and Stop-based. These are used only in P1100 paradigms, so here we will explain everything for both paradigms:
            - Lick-based (Private function LickReward): We check the SHM constantly for lick events. If lick is detected, rewardPresent variable in sessionManager will be set to false (meaning the animal has consumed the reward), which prevents the SuckReward function (if there is this function in certain states). For rewardPresent, also refer to function SuccessUnderPillar where the variable is set to true when we deliver the reward.
            - Stay-based (Private function StayTimeReached): We check if the timer has reached to the required threshold "ST" as set in the trialVariablesDict.
            - Stop-based (Private function StopMovement): In P1100_movementInitiation we initialize an empty queue, and in P1100_MovementInQueue we store the past ball sensor data within certain duration (or length). We convert this sensor data to real-world centimeter scale. In the private function StopMovement in P0800_RewardConditionReached, we compare the 3-dimensional average with the "ST" as set in the trialVariablesDict, or "ST_2" if we explicitly differentiate the stop threshold for the 2 rewarding locations. If "ST_2" is present, we need to utilize parameters including "reward1StateID" and "reward2StateID" to identify which threshold are we applying now. Modify these IDs when changing paradigms.
            - In the Decide function, if "LR" is present in the trialVariablesDict, we are in P0800 paradigm. If "LR" is set to 1, then it will be Lick-based, and if set to 0 it will be Stay-based. 
            - If no "LR" is present and "DR" is present, we are in P1100 paradigm. Before we give the first reward, it will be Stop-based no matter what. After the first reward, if "SR" is set to 1 in the trialVariablesDict, it will be switched to Stay-based, otherwise keeping the Stop-based criteria (default).

### S0815_RewardStay

- Actions
    - P0800_SuccessUnderPillar
        - Based on Action P0800_SuccessUnderPillar, give rewards and change the scene as long as the currentRewardNum is no more than the maxRewardNum as specified by "MRN" as in the trialVariablesDict.
        - After giving reward, the rewardPresent variable is also set to true, indicating the reward hasn't been consumed by the animal yet.
- Transitions
    - To [S0816_WithinSuccessSequence](#S0816_WithinSuccessSequence)
        - Always True

### S0816_WithinSuccessSequence

- Transitions
    - To [S0811_Reward1](#S0811_Reward1)
        - Based on Decision P0800_SuccessAndTeleportToBack, switched to the correct reward state (choosing between 811 or 813 based on the registered reward zone as in P0800_RegisterRewardZone). Parameter RewardStateID indicates the targetd reward state ID to compare with the registered one (here set as 811).
    - To [S0813_Reward2](#S0813_Reward2) 
        - Same as before, but with a RewardStateID as 813.

### S0817_PostReward

- Transitions
    - To [S0818_TrialEnd](#S0818_TrialEnd)
    - To [S0817_PostReward](#S0817_PostReward)
        - Based on the Decision P0800_ReachEnd, if the animal has passed certain coordinates then switch. Modify the hard coded coordinate information.

### S0818_TrialEnd

- Actions
    - P0800_TrialEndLinearTrack
        - Based on Action P0800_TrialEndLinearTrack
        - Collect all trialVariablesDict information and send it to logger. Notice that if the variable has decimal points, run Add_Decimal first on this variable.
        - Log the outcome. Notice that if rewardPresent is true (meaning there is still one reward unconsumed), the outcome will -1.

- Transitions
    - To [S0819_WithinInterTrialInterval](#S0819_WithinInterTrialInterval)
        - Always true

### S0819_WithinInterTrialInterval

- Actions
    - P0800_FadeInScreen
        - Similar to [S0802_StartZone](#S0802_StartZone), but this time fade into black screen.
- Transitions
    - To [S0801_TrialStart](#S0801_TrialStart)
        - Switch when InterTrialInterval has reached.
    - To [S0820_SessionEnded](#S0820_SessionEnded)
        - Switch when manually terminate the session.

### S0820_SessionEnded

- Actions
    - SessionEndedAction
        - Wrap up the entire session.

## P0900_MotorLickLearning

This paradigm rewards the animal when she is licking after producing a clean forward movement for certain time. It has 11 states, and serves as the template also for paradigm 1000. Notice most functions (actions and decision) are scriptableObjects from 500 paradigms, so in the following dicussion we will refer to their original functions.

### P0900_MotorLickLearning

- Actions
    - P0900_TrialInit
        - Based on Action P0500_TrialInitMotorLearning
        - Initialize and disable the environment
        - Initialize the portentaOutputSHMInterface
- Transitions
    - To [S0910_WithinInterTrialInterval](#S0910_WithinInterTrialInterval)
        - Always true

### S0901_TrialStart

- Actions
    - P0900_TrialStart
        - Based on Action P0500_TrialStartMotorLearning
        - Create an empty queue to store the ball sensor data later
        - Initialize the session parameters including currentRewardNum and rewardSucked
- Transitions
    - To [S0902_WithinTrial](#S0902_WithinTrial)
        - Always true

### S0902_WithinTrial

- Actions
    - P0900_MovementInQueue
        - Based on Action P0500_MovementInQueue, store the normalized ball sensor data in the queue. The length of the storage should be 60 (the frame rate) * "MT" ("move time" from the trialVariablesDict).
        - Also expose the temporary (current) normalized ball sensor data to other functions.
    - P0900_ClearSHMInput
        - In this paradigm, we detect animal licks after clean movement for success. Therefore we should keep trashing all the events stacked in the SHM to clear it before the animal is in the reward-decision state.

- Transitions
    - To [S0903_MovePhase](#S0903_MovePhase)
        - Based on Decision P0500_DirectedMovement, check if the animal is moving only in the desired direction.
        - We calculate the ratio between each dimensional movement and the sum of 3 dimensions. If the ratio of the desired dimension is greater the "MTH" ("movement threshold" from the trialVariablesDict), then return true.
        - The "desire dimenstion" is determined by "R" (raw), "Y" (yaw), and "P" (pitch) variables from the trialVariablesDict. Set any one of it to 1 will add the corresponding dimension to the decision.
        - Additional parameter "Return True" is used to decide whether to return True or False when the animal is moving in the desired direction. Could be useful to re-use this function elsewhere.
    - To [S0911_SessionEnded](#S0911_SessionEnded)
        - Switch when manually terminate the session.

### S0903_MovePhase

- Actions
    - P0900_MovementInQueue
        - Same as before
    - P0900_ClearSHMInput
        - Same as before
- Transitions
    - To [S0904_WaitLick](#S0904_WaitLick)
        - Based on Decision P0500_TimeReached. This decision will return true when the time in this state has exceed the threshold identified by the parameter "Time Variable Name" 
        - Here the parameter is set to "MT", which is the movement time. It means the animal has move in the desired direction for enough time, therefore we continue to detect its lick thereafter.
    - To [S0902_EarlyStop](#S0902_WithinTrial)   
        - Based on Decision P0500_Early stop. This decisin will return true if the summation of temporal 3 dimensional movements exceed the threshold defined by "STH" ("stop threshold" from the trialVariablesDict). It means the animal almost stops at the particular timestamp.
    - To [S0902_FailDirectedMove](#S0902_WithinTrial)  
        - Based on Decision P0500_FailDirectedAndReset. It acts very similar as P0500_DirectedMovement (See [S0902_WithinTrial](#S0902_WithinTrial)), but return true if the animal is not only moving in the desired direction. Also it will set the timer in "To S0904_WaitLick" as 0, so that it will re-count the time when re-enter this state.

### S0904_WaitLick

- Actions
    - P0900_MovementInQueue
        - Same as before
- Transitions
    - To [S0907_SuccessEntry](#S0907_SuccessEntry)
        - Based on Decision P0900_RewardConditionReached. This decision will return true if there is lick packages detected from the SHM, otherwise keep poping out the packages.
        - It will also change the rewardPresent variable to be false, indicating the reward has been consumed.
    - To [S0905_GracePeriod](#S0905_GracePeriod)
        - Based on Decision P0500_DirectedMovement. Similar as in [S0902_WithinTrial](#S0902_WithinTrial), but only return true if it fails to produce directed movement.
        - It means if the animal fails to move in the desired direction, we switch into grace period to still allow it to have another try.

### S0905_GracePeriod

- Actions
    - P0900_MovementInQueue
        - Same as before
- Transitions
    - To [S0907_SuccessEntry](#S0907_SuccessEntry)
        - Same as before
    - To [S0909_GraceTimeReached](#S0909_TrialEnd)
        - Based on Decision P0500_TimeReached. This decision will return true when the time in this state has exceed the threshold identified by the parameter "Time Variable Name" 
        - Here the parameter is set to "GPT", which is the grace period time. It means the animal has stayed in the grace period for too long without licking, so that we have to fail her.

### S0906_StayLick

- Actions
    - P0900_MovementInQueue
        - Same as before
    - P0900_ResetRewawrd
        - Based on Action P0500_ResetReward. Reset the scene back to white and reset the timer for success sequence.
- Transitions
    - To [S0907_SuccessEntry](#S0907_SuccessEntry)
        - Same as before
    - To [S0909_StayLickTimeReached](#S0909_TrialEnd)
        - Based on Decision P0500_TimeReached. This decision will return true when the time in this state has exceed the threshold identified by the parameter "Time Variable Name" 
        - Here the parameter is set to "SLT", which is the stay lick time. It means the animal has stayed in the lick detection state for too long, so that we have to end this trial.
        - This is only useful when we allow multiple rewards, which is not by default in this paradigm.
    - To [S0909_MaxRewardReached](#S0909_TrialEnd)
        - Based on Decision P0500_MaxRewardReached. This decision will return true if the animal has consumed rewards more than the maximum. Since by default the "MRN" (maximum reward number) is set to 1, it will just skip through.

### S0907_SuccessEntry

- Actions
    - P0900_SuccessUnderPillar
        - Based on Action P0500_SuccessUnderPillar, deliver the reward, setup the environment, and set the rewardPresent variable to be true (meaning the reward is present and not consumed yet).
- Transitions
    - To [S0908_WithinSuccessSequence](#S0908_WithinSuccessSequence)
        - Always true

### S0908_WithinSuccessSequence

- Actions
    - P0900_ClearSHMInput
        - Same as before
- Transitions
    - To [S0906_RewardFinished](#S0906_StayLick)
        - Switch when InterTrialInterval has reached.

### S0909_TrialEnd

- Actions
    - SuckReward
        - Based on Action SuckReward, suck out the unconsumed reward (if there is any) and set the rewardSucked parameter to be true for later logging.
   - P0900_TrialEnd
        - Based on Action P0900_TrialENdMotorLickLearning
        - Collect all trialVariablesDict information and send it to logger. Notice that if the variable has decimal points, run Add_Decimal first on this variable.
        - Log the outcome. Notice that if rewardSucked is true (meaning there is still one reward unconsumed), the outcome will -1.

- Transitions
    - To [S0910_WithinInterTrialInterval](#S0910_WithinInterTrialInterval)
        - Always true

### S0910_WithinInterTrialInterval

- Transitions
    - To [S0901_TrialStart](#S0901_TrialStart)
        - Switch when InterTrialInterval has reached.

### S0911_SessionEnded

- Actions
    - SessionEndedAction

## P1000_MotorLearningStop

This paradigm rewards the animal when she stops after producing a clean forward movement for certain time. It is mostly inherented from paradigm P0900, so here we will introduce only the differences. For unexplained Actions and Transitions, please refer to P0900.

### S1004_StopPhase
- Transitions
    - To [S1006_StayStop](#S1006_StayStop)
        - Based on Decision P0500_StopAttempt, return true if the summation of 3 dimensional movement (temporally at this frame) exceeds the threshold definend by "STH" ("stop threshold" from the trialVariablesDict)
        - Return true doesn't mean the animal will get the reward right after. It just mean the animal starts to slow down. It has to go through S1006_StayStop to make sure it really slows down but not just "stepping"

### S1005_GracePeriod

- Transitions
    - To [S1006_StayStop](#S1006_StayStop)
        - Same as before
    - To [S1009_FailStopInGracePeriod](#S1009_TrialEnd)   
        - Basd on Decision P0500_FailStopInGracePeriod, return true if after the timer exceeds "GPT" ("grace period time" from the trialVariablesDict), the summation of 3 dimensional movement (temporally at this frame) still exceeds the threshold definend by "STH" ("stop threshold" from the trialVariablesDict).
        - It means the animal failed to start slowing down for the entire grace period, therefore failed the trial. 

### S1006_StayStop

- Transitions
    - To [S1007_SuccessEntry](#S1007_SuccessEntry)
        - Based on Decision P1000_RewardConditionReached, reward true:
            - When before the first reward, the animal has to stop for at least "ST" ("stop time" from the trialVariablesDict). 
            - After the first reward, the animal has to stop AND lick the reward
    - To [S1009_FailStopInStayStop](#S1009_TrialEnd)    
        - Based on Decision P0500_FailStopInStayStop, return true if the animal doesn't steadily slow down after the first time the velocity drop down the "STH" as in [S1004_StopPhase](#S1004_StopPhase).    
        - Since the animal steps forward on the ball, we could witness momentary slowing down which shouldn't be viewed as "stopping", but actually pass the To S1006_StayStop From S1004_StopPhase. Therefore we need to keep confirming that the movement is constanly below certain value for real stopping.
        - Here we leverage another variable "SSTH" ("stay stop threshold"), which should be always slightly higher than "STH" (in default "STH" is 0.2, and "SSTH" is 0.3), acts as a softer condition to confirm the stopping.

## P1100_LinearTrackStop

This paradigm rewards the animal when she stops (and licks) at the correct rewarding location. It is mostly inherented from paradigm P1100, so here we will introduce only the differences. For unexplained Actions and Transitions, please refer to P1100.

### S1101_TrialStart

- Actions
    - P1100_MovementInitiation
        - Based on Action P1100_MovementInitiation, create an empty queue to store the ball sensor data later

### S1110_BeforeReward1

- Actions
    - P1100_MovementInQueue
        - Based on Action P1100_MovementInQueue, store the normalized ball sensor data in the queue. 
        - The length of the storage (hardcoded as "queueSize", in default is 1) could be modified to include averaging across past time window.  

### S1111_Reward1

- Transitions
    - To S1114_RewardEntry
        - Based on Decision P1100_RatUnderCorrectPillar
        - Similar to P0800_RatUnderCorrectPillar in [S0811_Reward1](#S0811_Reward1), return true when the rat is at the correct reward zone. However, if the "DR" ("double reward" from the trialVariablesDict) is set to 1, then return true when rat is at any one of the reward zone.

### S1112_BeforeReward2

- Actions
    - P1100_ResetCurentRewardInDR
        - Based on Action P1100_ResetCurentRewardInDR, this action register the number of rewards received in the first rewarding location to firstRewardNum, and reset the currentRewardNum in SessionManager for properly logging for the second rewarding location. It only happens when the double reward mode is on ("DR" from trialVariablesDict is 1).

### S1118_TrialEnd

- Actions
    - P1100_TrialEndLinearTrackStop
        - Based on Action P1100_TrialEndLinearTrackStop, similar to P0800_TrialEndLinearTrack in [S0818_TrialEnd](#S0818_TrialEnd) to log the trialVariablesDict and rewards. 
        - Since we could have 2 reward numbers for a trial (considering "DR"), then we use 2 digits to record the outcome. For example, if the animal received 5 rewards in the first location and 3 in the second, then the final outcome will be "53".

### S1119_WithinInterTrialInterval

- Transitions
    - To [S1101_TrialStart](#S1101_TrialStart)
        - Based on Decision P1100_InterTrialIntervalEnded, return ture after the inter trial interval time has reached. For success trials (outcome > 0), the time has set to interTrialIntervalLength. For failed trials (outcome <= 0), this time has to abortInterTrialIntervalLength (usually longer than interTrialIntervalLength for punishing the animal for failing). 

## FAQs

- If I want to create a new paradigm, can I just copy-paste the one we have?
    - Yes, but there are a few things needed to be changed:
        - Delete the redundant scripts in the new folder to avoid error/duplicates
        - Change the names for all the scriptableObjects to match the new paradigm ID
        - For actions/transitions, you have to link to the newly created one (in default the copy-pasted objects will have its components still refer to the objects in the old paradigm)
        - For some objects, some of its parameters should also change according to new paradigm ID (for example, P0800_RatLeaveRewardPillar). In general, I suggest go through the action and decision folders again to check if there are specific fields to modify (most of them are numbers so should be rather easy to identify)
        - Add the corresponding name of the paradigm excel to BaseStateMachine in the ExperimentCore in Unity Scene

- If I want to modify the excel sheet, what needs to pay attention?
    - Notice that we treat pillars in 2 ways: either they are used as a boundary (for example, separating the before_reward_1 to the reward_1), or as a landmark (like all the cues and rewards pillars). For the former one, we use its coordinates on the track and check if the animal's coordinate passes it. For the later one, we use the pillarRewardRadius field in EnvParmaters in the excel sheet to decide the detection radius for the animal to go under it. Pay attention if you want to change the ID of certain pillarIdx as you need change the same thing in Unity scriptableObjects
    - As mentioned before, basically the pillarRewardRadius is detached from the pillar position in the excel sheets. It means if you want to increase the reward radius, please not only change the pillarRewardRadius in the parameters, but change the position of the involved pillars.
    - For visibility of cues, cue_1_visible (for example) will be the starting point of the cue fading in, and cue_1 will be the point of cue completely visible (transparency 100%). Therefore, the parameter Fade Distance in P0800_FadeInCue (see [P0800_FadeInCue1](#P0800_FadeInCue1)) should be the distance between the pillar standing cue_1_visible and cue_1.

- If I want to add a trial variable
    - Add it in SessionParameters in the excel sheet. Modify 3 columns:
        - trialPackageVariables: add the short name for the variable (no space between variables)
        - trialPackageVariablesDefault: add the default value for the variable (no space between variables)
        - trialPackageVariablesFullNames: add the full name for the variable (no space between variables)
    - If you want to log this trial variable at the end of each trial, add it in the corresponding trialEnd function (for example, P1100_TrialEndLinearTrackStop). Notice that it has decimal points, you need to run Add_Decimal to the variable first.

- If I want to replace old functions
    - Pay attention to the public variables in this function and which functions are refering to this specific function. For example, if you want to create a new paradigm based on 1100 and a new trialStart function, it could be very tedious to complete since many functions/scriptableObjects in this paradigm need to refer to this specific function.
    - In that case, I would recommend only modify the script but not create a new one. You can use trial variables or other public variables to set the condition that it is only enabled in this paradigm, therefore not hurting the old paradigms.