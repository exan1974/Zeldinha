using UnityEngine;

namespace Player.States
{

public class Attack : State
{
    private PlayerController controller;

    public int stage = 1;
    private float stateTime;
    private bool firstFixedUpdate;

    public Attack(PlayerController controller) : base("Attack")
    {
        this.controller = controller;
    }
    public override void Enter()
    {
        base.Enter();
        
        // ERROR: invalid stage
        if (stage <= 0 || stage > controller.attackStages)
        {
            controller.stateMachine.ChangeState(controller.idleState);
            return;
        }

        //Reset variables
        stateTime = 0;
        firstFixedUpdate = true;

        // Set animator trigger
        controller.thisAnimator.SetTrigger("tAttack" + stage);

        // Toggle hitbox
        controller.swordHitbox.SetActive(true);

    }
    public override void Exit()
    {
        base.Exit();

        // Toggle hitbox
        controller.swordHitbox.SetActive(false);
    }
    public override void Update()
    {
        base.Update();

        // Switch to attack (again)
        if (controller.AttemptToAttack())
        {
            return;
        }

        // Update stateTime
        stateTime += Time.deltaTime;

        // Get attack variables
        var isLastState = stage == controller.attackStages;
        var stageDuration = controller.attackStageDurations[stage -1];
        var stageMaxInterval = isLastState ? 0 : controller.attackStageMaxIntervals[stage - 1];

        // Exit after time
        if(IsStageExpired())
        {
            controller.stateMachine.ChangeState(controller.idleState);
            return;
        }
    }
    public override void LateUpdate()
    {
        base.LateUpdate();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if(firstFixedUpdate)
        {
            firstFixedUpdate = false;

            // Look to input
            controller.RotateBodyToFaceInput(1);

            // Impulse
            var impulseValue = controller.attackStageImpulses[stage -1];
            var impulseVector = controller.rb.rotation * Vector3.forward;
            impulseVector *= impulseValue;
            controller.rb.AddForce(impulseVector, ForceMode.Impulse);
        }
    }

    public bool CanSwitchStages()
    {
        // Get attack variables
        var isLastState = stage == controller.attackStages;
        var stageDuration = controller.attackStageDurations[stage -1];
        var stageMaxInterval = isLastState ? 0 : controller.attackStageMaxIntervals[stage - 1];
        var maxstageDuration = stageDuration + stageMaxInterval;

        return !isLastState && stateTime >= stageDuration && stateTime <= maxstageDuration;
    }

        public bool IsStageExpired()
    {
        // Get attack variables
        var isLastState = stage == controller.attackStages;
        var stageDuration = controller.attackStageDurations[stage -1];
        var stageMaxInterval = isLastState ? 0 : controller.attackStageMaxIntervals[stage - 1];
        var maxstageDuration = stageDuration + stageMaxInterval;

        return stateTime > maxstageDuration;
    }
}
}