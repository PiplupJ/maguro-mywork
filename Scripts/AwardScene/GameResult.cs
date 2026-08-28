using UnityEngine;

public enum PlayerSlot
{
    None = 0,
    Player1 = 1,
    Player2 = 2,
}

//ゲーム結果を保持するクラス。勝者の情報を保持する
public static class GameResult
{
    public static PlayerSlot Winner { get; private set; } = PlayerSlot.None;
    public static void SetWinner(PlayerSlot slot){
        Winner = slot;
    }

    public static void Clear()
    {
        Winner = PlayerSlot.None;
    }
}
