// Based on Game Dev's Cheat Console (https://www.youtube.com/watch?v=VzOEM-4A2OM) with some changes.
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugController : MonoBehaviour
{
	bool showConsole;
	bool showHelp;

	string input;

	public static DebugCommand HELP;
	public static DebugCommand<bool> NOCLIP;
	public static DebugCommand<bool> SHOW_FPS;
	public static DebugCommand<bool> DEBUGMODE;
	public static DebugCommand<bool> CHEATMODE;
	public static DebugCommand RESTARTSCENE;
	public static DebugCommand<string> LOADSCENE;

	public List<object> commandList;

	[Header("General")]
	public bool allowConsole;
	public bool allowCheats;
	public GameObject fps;
	public PlayerMovement player;
	[Header("Cheats")]
	public bool debugMode;
	public DebugManager debugManager;
	public bool noclip;
	
	private string errorCheatMessage = "You cannot use cheat commands unless cheat mode is enabled";

	private void Update()
	{
		if (PlayerPrefs.HasKey("Can Open Console"))
		{
			if (Convert.ToBoolean(PlayerPrefs.GetInt("Can Open Console", 1)))
			{
				allowConsole = true;
			} else
			{
				allowConsole = false;
			}
		}
		if (PlayerPrefs.HasKey("Can Cheat"))
		{
			if (Convert.ToBoolean(PlayerPrefs.GetInt("Can Cheat", 1)))
			{
				allowCheats = true;
			} else
			{
				allowCheats = false;
			}
		}
		if (Input.GetButtonDown("Toggle Console") && allowConsole)
		{
			showConsole = !showConsole;
			if (!showConsole)
			{
				input = "";
				return;
			}
		}
		if (Input.GetKeyDown(KeyCode.Return))
		{
			if (showConsole)
			{
				HandleInput();
				input = "";
			}
		}
		player.noclip = noclip;
		debugManager.working = debugMode;
	}

	private void Awake()
	{
		HELP = new DebugCommand("help", "Shows a list of commands", "help", () => { showHelp = !showHelp; });

		NOCLIP = new DebugCommand<bool>("noclip", "Turning player into noclip", "noclip [boolean]", (x) =>
		{
			if (!allowCheats)
			{
				Debug.Log(errorCheatMessage);
				return;
			}
			noclip = x;
		});

		SHOW_FPS = new DebugCommand<bool>("show_fps", "Shows a FPS", "show_fps [boolean]", (x) =>
		{
			fps.SetActive(x);
		});

		DEBUGMODE = new DebugCommand<bool>("debugmode", "Toggle debug features", "debugmode true,false", (x) =>
		{
			if (!allowCheats)
			{
				Debug.Log(errorCheatMessage);
				return;
			}
			debugMode = x;
			Debug.Log("Debug mode has " + (x ? "enabled" : "disabled"));
		});
		
		CHEATMODE = new DebugCommand<bool>("cheatmode", "Toggle cheat system", "cheatmode true,false", (x) =>
		{
			PlayerPrefs.SetInt("Can Cheat", x ? 0 : 1);
			Debug.Log("Cheat mode has " + (x ? "enabled" : "disabled"));
		});

		RESTARTSCENE = new DebugCommand("restart", "Restarts a scene", "restart", () => { SceneManager.LoadScene(SceneManager.GetActiveScene().name); });

		LOADSCENE = new DebugCommand<string>("load_scene", "Loads a scene", "load_scene <scene>", (scene_name) => { SceneManager.LoadScene(scene_name); });

		commandList = new List<object>
		{
			HELP,
			DEBUGMODE,
			CHEATMODE,
			NOCLIP,
			SHOW_FPS,
			RESTARTSCENE,
			LOADSCENE
		};
	}

	Vector2 scroll;

	private void OnGUI()
	{
		if (!showConsole) { return; }

		float y = 0f;

		if (showHelp)
		{
			GUI.Box(new Rect(0, y, Screen.width, 100), "");
			Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);
			scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);
			for (int i = 0; i < commandList.Count; i++)
			{
				DebugCommandBase command = commandList[i] as DebugCommandBase;
				string label = $"{command.commandFormat} - {command.commandDescription}";
				Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
				GUI.Label(labelRect, label);
			}
			GUI.EndScrollView();
			y += 100;
		}

		GUI.Box(new Rect(0, y, Screen.width, 30), "");
		GUI.backgroundColor = new Color(0, 0, 0, 0);
		input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
	}

	private void HandleInput()
	{
		string[] properties = input.Split(' ');
		for (int i = 0; i < commandList.Count; i++)
		{
			DebugCommandBase commandBase = commandList[i] as DebugCommandBase;
			if (input.Contains(commandBase.commandId))
			{
				if (commandList[i] as DebugCommand != null)
				{
					(commandList[i] as DebugCommand).Invoke();
				}
				else if (commandList[i] as DebugCommand<int> != null)
				{
					(commandList[i] as DebugCommand<int>).Invoke(int.Parse(properties[1]));
				}
				else if (commandList[i] as DebugCommand<string> != null)
				{
					(commandList[i] as DebugCommand<string>).Invoke(properties[1]);
				}
				else if (commandList[i] as DebugCommand<bool> != null)
				{
					(commandList[i] as DebugCommand<bool>).Invoke(bool.Parse(properties[1]));
				}
			}
		}
	}
}
