﻿using System;
using System.Linq;
using Godot;
using Tutorial;

/// <summary>
///   GUI control that contains the microbe stage tutorial.
///   Should be placed over any game state GUI so that things drawn by this are on top. Visibility of things is
///   Controlled by the TutorialState object
/// </summary>
public partial class MicrobeTutorialGUI : Control, ITutorialGUI
{
    [Export]
    public NodePath? MicrobeWelcomeMessagePath;

    [Export]
    public NodePath MicrobeMovementKeyPromptsPath = null!;

    [Export]
    public NodePath MicrobeMovementPopupPath = null!;

    [Export]
    public NodePath MicrobeMovementKeyForwardPath = null!;

    [Export]
    public NodePath MicrobeMovementKeyLeftPath = null!;

    [Export]
    public NodePath MicrobeMovementKeyRightPath = null!;

    [Export]
    public NodePath MicrobeMovementKeyBackwardsPath = null!;

    [Export]
    public NodePath GlucoseTutorialPath = null!;

    [Export]
    public NodePath StayingAlivePath = null!;

    [Export]
    public NodePath ReproductionTutorialPath = null!;

    [Export]
    public NodePath EditorButtonTutorialPath = null!;

    [Export]
    public NodePath UnbindTutorialPath = null!;

    [Export]
    public NodePath LeaveColonyTutorialPath = null!;

    [Export]
    public NodePath EarlyMulticellularWelcomePath = null!;

    [Export]
    public NodePath DayNightTutorialPath = null!;

    [Export]
    public NodePath OrganelleDivisionTutorialPath = null!;

    [Export]
    public NodePath BecomeMulticellularTutorialPath = null!;

    [Export]
    public NodePath CheckTheHelpMenuPath = null!;

    [Export]
    public NodePath EngulfmentExplanationPath = null!;

    [Export]
    public NodePath EngulfedExplanationPath = null!;

    [Export]
    public NodePath EngulfmentFullCapacityPath = null!;

    [Export]
    public NodePath EditorButtonHighlightPath = null!;

#pragma warning disable CA2213
    private TutorialDialog microbeWelcomeMessage = null!;

    [Export]
    private Control tutorialDisabledExplanation = null!;

    [Export]
    private ScrollContainer welcomeTutorialScrollContainer = null!;

    private Control microbeMovementKeyPrompts = null!;
    private Control microbeMovementKeyForward = null!;
    private Control microbeMovementKeyLeft = null!;
    private Control microbeMovementKeyRight = null!;
    private Control microbeMovementKeyBackwards = null!;
    private CustomWindow microbeMovementPopup = null!;
    private CustomWindow glucoseTutorial = null!;
    private CustomWindow stayingAlive = null!;
    private CustomWindow reproductionTutorial = null!;
    private CustomWindow editorButtonTutorial = null!;
    private CustomWindow unbindTutorial = null!;
    private CustomWindow checkTheHelpMenu = null!;
    private CustomWindow engulfmentExplanation = null!;
    private CustomWindow engulfedExplanation = null!;
    private CustomWindow engulfmentFullCapacity = null!;
    private CustomWindow leaveColonyTutorial = null!;
    private CustomWindow multicellularWelcome = null!;
    private CustomWindow dayNightTutorial = null!;
    private CustomWindow becomeMulticellularTutorial = null!;
    private CustomWindow organelleDivisionTutorial = null!;

    [Export]
    private CustomWindow openProcessPanelTutorial = null!;

    [Export]
    private CustomWindow processPanelTutorial = null!;

    [Export]
    private CustomWindow resourceSplitTutorial = null!;

    [Export]
    private CustomWindow pausingTutorial = null!;

    [Export]
    private CustomWindow speciesMemberDied = null!;

#pragma warning restore CA2213

    [Signal]
    public delegate void OnHelpMenuOpenRequestedEventHandler();

    public ITutorialInput? EventReceiver { get; set; }

    public MainGameState AssociatedGameState => MainGameState.MicrobeStage;

    public bool AllTutorialsDesiredState { get; private set; } = true;

    public Node GUINode => this;

    public ControlHighlight? PressEditorButtonHighlight { get; private set; }

    [Export]
    public ControlHighlight? ProcessPanelButtonHighlight { get; private set; }

    public bool IsClosingAutomatically { get; set; }

    public bool MicrobeWelcomeVisible
    {
        get => microbeWelcomeMessage.Visible;
        set
        {
            if (value == microbeWelcomeMessage.Visible)
                return;

            if (value)
            {
                microbeWelcomeMessage.PopupCenteredShrink();
            }
            else
            {
                microbeWelcomeMessage.Hide();
            }
        }
    }

    public bool MicrobeMovementPromptsVisible
    {
        get => microbeMovementKeyPrompts.Visible;
        set
        {
            if (value == microbeMovementKeyPrompts.Visible)
                return;

            microbeMovementKeyPrompts.Visible = value;

            // Apply visible to children to make the key prompts visible. This saves a lot of processing time overall
            foreach (var child in microbeMovementKeyPrompts.GetChildren().OfType<Control>())
            {
                child.Visible = value;
            }
        }
    }

    public bool MicrobeMovementPopupVisible
    {
        get => microbeMovementPopup.Visible;
        set
        {
            if (value == microbeMovementPopup.Visible)
                return;

            if (value)
            {
                microbeMovementPopup.Show();
            }
            else
            {
                microbeMovementPopup.Hide();
            }
        }
    }

    public bool GlucoseTutorialVisible
    {
        get => glucoseTutorial.Visible;
        set
        {
            if (value == glucoseTutorial.Visible)
                return;

            if (value)
            {
                glucoseTutorial.Show();
            }
            else
            {
                glucoseTutorial.Hide();
            }
        }
    }

    public bool StayingAliveVisible
    {
        get => stayingAlive.Visible;
        set
        {
            if (value == stayingAlive.Visible)
                return;

            if (value)
            {
                stayingAlive.Show();
            }
            else
            {
                stayingAlive.Hide();
            }
        }
    }

    public bool ReproductionTutorialVisible
    {
        get => reproductionTutorial.Visible;
        set
        {
            if (value == reproductionTutorial.Visible)
                return;

            if (value)
            {
                reproductionTutorial.Show();
            }
            else
            {
                reproductionTutorial.Hide();
            }
        }
    }

    public bool EditorButtonTutorialVisible
    {
        get => editorButtonTutorial.Visible;
        set
        {
            if (value == editorButtonTutorial.Visible)
                return;

            if (value)
            {
                editorButtonTutorial.Show();
            }
            else
            {
                editorButtonTutorial.Hide();
            }
        }
    }

    public bool UnbindTutorialVisible
    {
        get => unbindTutorial.Visible;
        set
        {
            if (value == unbindTutorial.Visible)
                return;

            if (value)
            {
                unbindTutorial.Show();
            }
            else
            {
                unbindTutorial.Hide();
            }
        }
    }

    public bool LeaveColonyTutorialVisible
    {
        get => leaveColonyTutorial.Visible;
        set
        {
            if (value == leaveColonyTutorial.Visible)
                return;

            if (value)
            {
                leaveColonyTutorial.Show();
            }
            else
            {
                leaveColonyTutorial.Hide();
            }
        }
    }

    public bool MulticellularWelcomeVisible
    {
        get => multicellularWelcome.Visible;
        set
        {
            if (value == multicellularWelcome.Visible)
                return;

            if (value)
            {
                multicellularWelcome.PopupCenteredShrink();
            }
            else
            {
                multicellularWelcome.Hide();
            }
        }
    }

    public float MicrobeMovementRotation
    {
        get => microbeMovementKeyPrompts.Rotation;
        set
        {
            if (Math.Abs(value - microbeMovementKeyPrompts.Rotation) < 0.001f)
                return;

            microbeMovementKeyPrompts.Rotation = value;
        }
    }

    public bool MicrobeMovementPromptForwardVisible
    {
        get => microbeMovementKeyForward.Visible;
        set => microbeMovementKeyForward.Visible = value;
    }

    public bool MicrobeMovementPromptLeftVisible
    {
        get => microbeMovementKeyLeft.Visible;
        set => microbeMovementKeyLeft.Visible = value;
    }

    public bool MicrobeMovementPromptRightVisible
    {
        get => microbeMovementKeyRight.Visible;
        set => microbeMovementKeyRight.Visible = value;
    }

    public bool MicrobeMovementPromptBackwardsVisible
    {
        get => microbeMovementKeyBackwards.Visible;
        set => microbeMovementKeyBackwards.Visible = value;
    }

    public bool CheckTheHelpMenuVisible
    {
        get => checkTheHelpMenu.Visible;
        set
        {
            if (value == checkTheHelpMenu.Visible)
                return;

            if (value)
            {
                checkTheHelpMenu.Show();
            }
            else
            {
                checkTheHelpMenu.Hide();
            }
        }
    }

    public bool EngulfmentExplanationVisible
    {
        get => engulfmentExplanation.Visible;
        set
        {
            if (value == engulfmentExplanation.Visible)
                return;

            engulfmentExplanation.Visible = value;
        }
    }

    public bool EngulfedExplanationVisible
    {
        get => engulfedExplanation.Visible;
        set
        {
            if (value == engulfedExplanation.Visible)
                return;

            engulfedExplanation.Visible = value;
        }
    }

    public bool EngulfmentFullCapacityVisible
    {
        get => engulfmentFullCapacity.Visible;
        set
        {
            if (value == engulfmentFullCapacity.Visible)
                return;

            engulfmentFullCapacity.Visible = value;
        }
    }

    public bool DayNightTutorialVisible
    {
        get => dayNightTutorial.Visible;
        set
        {
            if (value == dayNightTutorial.Visible)
                return;

            dayNightTutorial.Visible = value;
        }
    }

    public bool OrganelleDivisionTutorialVisible
    {
        get => organelleDivisionTutorial.Visible;
        set
        {
            if (value == organelleDivisionTutorial.Visible)
                return;

            organelleDivisionTutorial.Visible = value;
        }
    }

    public bool BecomeMulticellularTutorialVisible
    {
        get => becomeMulticellularTutorial.Visible;
        set
        {
            if (value == becomeMulticellularTutorial.Visible)
                return;

            becomeMulticellularTutorial.Visible = value;
        }
    }

    public bool OpenProcessPanelTutorialVisible
    {
        get => openProcessPanelTutorial.Visible;
        set
        {
            if (value == openProcessPanelTutorial.Visible)
                return;

            if (value)
            {
                openProcessPanelTutorial.Show();
            }
            else
            {
                openProcessPanelTutorial.Hide();
            }
        }
    }

    public bool ProcessPanelTutorialVisible
    {
        get => processPanelTutorial.Visible;
        set
        {
            if (value == processPanelTutorial.Visible)
                return;

            if (value)
            {
                processPanelTutorial.Show();
            }
            else
            {
                processPanelTutorial.Hide();
            }
        }
    }

    public bool ResourceSplitTutorialVisible
    {
        get => resourceSplitTutorial.Visible;
        set
        {
            if (value == resourceSplitTutorial.Visible)
                return;

            if (value)
            {
                resourceSplitTutorial.Show();
            }
            else
            {
                resourceSplitTutorial.Hide();
            }
        }
    }

    public bool PausingTutorialVisible
    {
        get => pausingTutorial.Visible;
        set
        {
            if (value == pausingTutorial.Visible)
                return;

            if (value)
            {
                pausingTutorial.Show();
            }
            else
            {
                pausingTutorial.Hide();
            }
        }
    }

    public bool SpeciesMemberDiedVisible
    {
        get => speciesMemberDied.Visible;
        set
        {
            if (value == speciesMemberDied.Visible)
                return;

            if (value)
            {
                speciesMemberDied.Show();
            }
            else
            {
                speciesMemberDied.Hide();
            }
        }
    }

    public override void _Ready()
    {
        microbeWelcomeMessage = GetNode<TutorialDialog>(MicrobeWelcomeMessagePath);
        microbeMovementKeyPrompts = GetNode<Control>(MicrobeMovementKeyPromptsPath);
        microbeMovementPopup = GetNode<CustomWindow>(MicrobeMovementPopupPath);
        microbeMovementKeyForward = GetNode<Control>(MicrobeMovementKeyForwardPath);
        microbeMovementKeyLeft = GetNode<Control>(MicrobeMovementKeyLeftPath);
        microbeMovementKeyRight = GetNode<Control>(MicrobeMovementKeyRightPath);
        microbeMovementKeyBackwards = GetNode<Control>(MicrobeMovementKeyBackwardsPath);
        glucoseTutorial = GetNode<CustomWindow>(GlucoseTutorialPath);
        stayingAlive = GetNode<CustomWindow>(StayingAlivePath);
        reproductionTutorial = GetNode<CustomWindow>(ReproductionTutorialPath);
        editorButtonTutorial = GetNode<CustomWindow>(EditorButtonTutorialPath);
        unbindTutorial = GetNode<CustomWindow>(UnbindTutorialPath);
        checkTheHelpMenu = GetNode<CustomWindow>(CheckTheHelpMenuPath);
        engulfmentExplanation = GetNode<CustomWindow>(EngulfmentExplanationPath);
        engulfedExplanation = GetNode<CustomWindow>(EngulfedExplanationPath);
        engulfmentFullCapacity = GetNode<CustomWindow>(EngulfmentFullCapacityPath);
        leaveColonyTutorial = GetNode<CustomWindow>(LeaveColonyTutorialPath);
        multicellularWelcome = GetNode<CustomWindow>(EarlyMulticellularWelcomePath);
        dayNightTutorial = GetNode<CustomWindow>(DayNightTutorialPath);
        becomeMulticellularTutorial = GetNode<CustomWindow>(BecomeMulticellularTutorialPath);
        organelleDivisionTutorial = GetNode<CustomWindow>(OrganelleDivisionTutorialPath);

        PressEditorButtonHighlight = GetNode<ControlHighlight>(EditorButtonHighlightPath);

        ProcessMode = ProcessModeEnum.Always;
    }

    public override void _Process(double delta)
    {
        TutorialHelper.ProcessTutorialGUI(this, (float)delta);
    }

    public void OnClickedCloseAll()
    {
        TutorialHelper.HandleCloseAllForGUI(this);
    }

    public void OnSpecificCloseClicked(string closedThing)
    {
        TutorialHelper.HandleCloseSpecificForGUI(this, closedThing);
    }

    public void OnTutorialEnabledValueChanged(bool value)
    {
        AllTutorialsDesiredState = value;

        if (tutorialDisabledExplanation.Visible != !value)
        {
            tutorialDisabledExplanation.Visible = !value;

            if (tutorialDisabledExplanation.Visible)
            {
                // Scroll down to ensure the new text is visible
                // Need to invoke to make sure this happens after the new text appears
                Invoke.Instance.QueueForObject(() => welcomeTutorialScrollContainer.ScrollVertical = 500,
                    welcomeTutorialScrollContainer);
            }
        }
    }

    public void SetWelcomeTextForLifeOrigin(WorldGenerationSettings.LifeOrigin gameLifeOrigin)
    {
        microbeWelcomeMessage.Description = gameLifeOrigin switch
        {
            WorldGenerationSettings.LifeOrigin.Vent => "MICROBE_STAGE_INITIAL",
            WorldGenerationSettings.LifeOrigin.Pond => "MICROBE_STAGE_INITIAL_POND",
            WorldGenerationSettings.LifeOrigin.Panspermia => "MICROBE_STAGE_INITIAL_PANSPERMIA",
            _ => throw new ArgumentOutOfRangeException(nameof(gameLifeOrigin), gameLifeOrigin,
                "Unhandled life origin for tutorial message"),
        };
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (MicrobeWelcomeMessagePath != null)
            {
                MicrobeWelcomeMessagePath.Dispose();
                MicrobeMovementKeyPromptsPath.Dispose();
                MicrobeMovementPopupPath.Dispose();
                MicrobeMovementKeyForwardPath.Dispose();
                MicrobeMovementKeyLeftPath.Dispose();
                MicrobeMovementKeyRightPath.Dispose();
                MicrobeMovementKeyBackwardsPath.Dispose();
                GlucoseTutorialPath.Dispose();
                StayingAlivePath.Dispose();
                ReproductionTutorialPath.Dispose();
                EditorButtonTutorialPath.Dispose();
                UnbindTutorialPath.Dispose();
                LeaveColonyTutorialPath.Dispose();
                EarlyMulticellularWelcomePath.Dispose();
                DayNightTutorialPath.Dispose();
                OrganelleDivisionTutorialPath.Dispose();
                BecomeMulticellularTutorialPath.Dispose();
                CheckTheHelpMenuPath.Dispose();
                EngulfmentExplanationPath.Dispose();
                EngulfedExplanationPath.Dispose();
                EngulfmentFullCapacityPath.Dispose();
                EditorButtonHighlightPath.Dispose();
            }
        }

        base.Dispose(disposing);
    }

    private void CheckHelpMenuPressed()
    {
        TutorialHelper.HandleCloseSpecificForGUI(this, CheckTheHelpMenu.TUTORIAL_NAME);

        // Note that this opening while the tutorial box is still visible is a bit problematic due to:
        // https://github.com/Revolutionary-Games/Thrive/issues/2326
        EmitSignal(SignalName.OnHelpMenuOpenRequested);
    }

    private void DummyKeepInitialTextTranslations()
    {
        Localization.Translate("MICROBE_STAGE_INITIAL_POND");
        Localization.Translate("MICROBE_STAGE_INITIAL_PANSPERMIA");
    }
}
