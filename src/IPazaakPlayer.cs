namespace Pazaak
{
    public interface IPazaakPlayer
    {
        Card CardToPlay(GameState gameState);
        bool WillStand(GameState gameState);
        void RoundOver(GameState gameState);
        void GameOver(int yourWins, int opponentWins);
        bool IsHuman();
        string Name();
    }
}