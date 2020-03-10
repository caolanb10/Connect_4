﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManoMotion.TermsAndServices;
using ManoMotion.HowToUse;
using ManoMotion.RunTime;
using System;
using ManoMotion.UI.Buttons;
public class ApplicationManager : MonoBehaviour
{
	private static ApplicationManager _instance;
	public static ApplicationManager Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		if (!_instance)
		{
			_instance = this;
		}
		else
		{
			Debug.LogError("More than 1 ApplicationManagers in the scene");
			Destroy(this.gameObject);
		}

		InitializeComponents();
	}

	PrivacyPolicyDisclaimair privacyPolicy;
	public HowToInstructor howToInstructor;
	public RunTimeApplication runTimeApplication;



	private void Start()
	{
		privacyPolicy.InitializeUsageDisclaimer();
	}
	/// <summary>
	/// Handles the privacy policy accepted.
	/// </summary>
	void HandlePrivacyPolicyAccepted()
	{
		Debug.Log("Privacy Policy Accepted");
		runTimeApplication.HideApplicationComponents();
		howToInstructor.DisplayFirstTimeInstructions();
	}

	/// <summary>
	/// Forces the instructions to be seen even if seen in the past. Used from within the main menu.
	/// </summary>
	public void ForceInstructions()
	{
        runTimeApplication.SaveDefalutFeaturesToDisplay();
		runTimeApplication.SetMenuIconVisibility();
		howToInstructor.InitializeHowtoInstructor();
		runTimeApplication.HideApplicationComponents();
	}

	/// <summary>
	/// Handles the logic and what happens after the player has seen all of the instructions.
	/// </summary>
	void HandleHowToInstructionsFinished()
	{
        Debug.Log("Anton handleINstructionFinished");
		runTimeApplication.StartMainApplicationWithDefaultSettings();
	}

	/// <summary>
	/// Handles the logic and what happens after the player has skipped the instructions. For now it calls the same method as if seen them all.
	/// </summary>
	void HandleHowToInstructionsSkipped()
	{
        Debug.Log("Anton handleINstructionSkipped");
		HandleHowToInstructionsFinished();
	}

	/// <summary>
	/// Initializes the components needed in order to operate the application.
	/// </summary>
	void InitializeComponents()
	{
		#region RunTimeApplication
		try
		{
			runTimeApplication = this.GetComponent<RunTimeApplication>();
		}
		catch (Exception ex)
		{
			runTimeApplication = new RunTimeApplication();
		}
		runTimeApplication.InitializeRuntimeComponents();

		#endregion
	}
}
