﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant {
	// public const string DAMAGE = "Official/red_ball";

	// public const string HEAL = "Official/green_ball";
	public const string DAMAGE = "new/red_ball";

	public const string HEAL = "new/green_ball";

	public const string PROTECT = "Official/protection";

	public const string DESTROY = "Official/destroy";

	public const string PADLEFT = "new/pad_L";

	public const string PADRIGHT = "new/pad_R";

	public const float HOUR_PER_SECONDS = 3600f;

	public static bool RECALCULATE = true;

	public const string TAG_WHEEL = "Wheel";

	public const string TAG_OBSTACLE = "Obstacle";

	public const string TAG_PLAYER = "Player";

	public const string TAG_SPAWNPOINT = "SpawnPoint";

	public static Color RED = new Color (0.933f,0.106f,0.141f);

	public static Color GREEN = new Color (0.137f,0.694f,0.302f);

	public const float CAMERA_HALF_HEIGHT = 6;
	public static float CAMERA_HALF_WIDTH = 0;
	public static float CAMERA_UP_BOUND = 0;
	public static float CAMERA_DOWN_BOUND = 0;
	public static float CAMERA_LEFT_BOUND = 0;
	public static float CAMERA_RIGHT_BOUND = 0;

	//Game tutorial phase
	public const int TUTORIAL_PHASE_0 = 0;
	public const int TUTORIAL_PHASE_1 = 1;
	public const int TUTORIAL_PHASE_2 = 2;

	//save game
	public const string SAVE_GAME = "/GameInfo.dat";
	public const string SCENE_LOADING = "Loading";

	public const string SCENE_MAIN = "Main";

	public const string SCENE_END = "EndScene";

}
