using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager{

	//public delegate void DelSendHP (Transform identity, int amount);
	public delegate void DelReceiveHP (Transform identity, float amount);
	public delegate void DelUpdatePoints (float amount);
	public delegate void DelReducePart ();
	public delegate void DelEndGame ();
	public delegate void DelIncreaseDiff ();
	public delegate void DelCanRun (bool run);

	//delegate bool DelIsGameReady();
	//delegate int DelTutorialPhase();
	public delegate void DelModifyPhase (int phase);
	//delegate float DelGetBest ();
	public delegate void DelScoreBest (float score);

	//public static DelSendHP SendHPCallback;
	public static event DelReceiveHP ReceiveHPCallback;
	public static event DelUpdatePoints UpdatePointsCallback;
	public static event DelReducePart ReducePartCallback;
	public static event DelEndGame EndGameCallback;
	public static event DelIncreaseDiff IncreaaseDiffCallback;
	public static event DelCanRun CanRunCallback;

	//public static event DelIsGameReady IsGameReadyCallback;
	//public static event DelTutorialPhase TutorialPhaseCallback;
	public static event DelModifyPhase ModifyPhaseCallback;
	//public static event DelGetBest GetBestCallback;
	public static event DelScoreBest ScoreBestCallback;

	public static void SendHPCallback (Transform identity, float amount)
	{
		if (ReceiveHPCallback != null)
			ReceiveHPCallback (identity, amount);

		if (UpdatePointsCallback != null && amount > 0)
			UpdatePointsCallback (amount);
	}

	public static void ReducePart ()
	{
		if (ReducePartCallback != null)
			ReducePartCallback ();
	}

	public static void TriggerEndGame ()
	{
		if (EndGameCallback != null)
			EndGameCallback ();	
		CanRun (false);
	}

	public static void IncreaseDiff()
	{
		if (IncreaaseDiffCallback != null)
			IncreaaseDiffCallback ();	
	}

	public static void CanRun(bool run)
	{
		if (CanRunCallback != null)
			CanRunCallback (run);
	}
}
