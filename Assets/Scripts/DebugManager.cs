using UnityEngine;
using UnityEngine.UI;

namespace SandVich.Utility
{
	public class DebugManager : MonoBehaviour
	{
		public bool working = false;
		public GameObject debugGO;
		public GameObject player;
		public Text playerLocText;

		void Update()
		{
			if (working)
			{
				debugGO.SetActive(true);
			} else
			{
				debugGO.SetActive(false);
			}
			playerLocText.text = "X: " + player.transform.position.x + " " + "Y: " + player.transform.position.y + " "  + "Z: " + player.transform.position.z;
		}
	}
}
