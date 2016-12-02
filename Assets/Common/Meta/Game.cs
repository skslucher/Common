using UnityEngine;
using System.Collections;

public enum GameState
{
    Play,
    Paused
}

public class Game : Singleton<Game> {
    
    const string playerTag = "Player";

    static private Transform player;
    static public Transform Player
    {
        get
        {
            if (player == null || player.tag != playerTag)
            {
                GameObject playerObj = GameObject.FindWithTag(playerTag);
                if (playerObj != null)
                {
                    player = playerObj.transform;
                }
            }
            return player;
        }
    }

    public static GameState gameState = GameState.Play;


    static public void Pause()
    {
        gameState = GameState.Paused;
        MyTime.Pause();
        AudioListener.pause = true;
    }

    static public void Unpause()
    {
        gameState = GameState.Play;
        MyTime.Unpause();
        AudioListener.pause = false;
    }


}
