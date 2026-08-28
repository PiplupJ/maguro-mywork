using Unity.VisualScripting;
using UnityEngine;

public class PlayerPlacement : MonoBehaviour
{
    [SerializeField] private GameObject playeroneRoom;
    [SerializeField] private GameObject playertwoRoom;
    [SerializeField] private GameObject playeroneStartPos;
    [SerializeField] private GameObject playertwoStartPos;

    public void PlayerOneToRoom(GameObject player)
    {
        player.transform.position = playeroneRoom.transform.position;
    }
    public void PlayerTwoToRoom(GameObject player)
    {
        player.transform.position = playertwoRoom.transform.position;
    }
    public void PlayerOneToStart(GameObject player)
    {
        player.transform.position = playeroneStartPos.transform.position;
    }
    public void PlayerTwoToStart(GameObject player)
    {
        player.transform.position = playertwoStartPos.transform.position;
    }
}
