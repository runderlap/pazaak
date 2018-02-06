using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pazaak
{
    class PazaakGame
    {
        GameState State => GetCurentPlayersGameState();
        PlayerStatus Player1;
        PlayerStatus Player2;
        PlayerStatus CurrentPlayer;
        PlayerStatus OtherPlayer => GetOtherPlayer();

        public void Run()
        {
            GameInit();
            var GameOn = true;
            while (GameOn)
            {
                var RoundOn = true;
                while (RoundOn)
                {
                    PrintPlayField();
                    RunTurn();
                    RoundOn = IsRoundOn();
                    if (!RoundOn)
                    {
                        HandleRoundOver();
                    }
                    SwitchPlayers();
                }
                GameOn = IsGameOn();
                if (!GameOn)
                {
                    HandleGameOver();
                }
            }
        }

        private bool IsGameOn()
        {
            var gameOn = true;
            if (Player1.RoundWins >= Rules.WINS_NEEDED)
            {
                gameOn = false;
                Player1.GameWins++;
                w("Game Over! You Win");
            }
            if (Player2.RoundWins >= Rules.WINS_NEEDED)
            {
                gameOn = false;
                Player2.GameWins++;
                w("Game Over! You Loose");
            }

            return gameOn;
        }

        private void RunTurn()
        {
            if (!CurrentPlayer.Stand)
            {
                HandleNewCard();
                if (Exactly20())
                {
                    CurrentPlayer.Stand = true;
                }
                else
                {
                    HandlePlayCard();
                    if (Exactly20())
                    {
                        CurrentPlayer.Stand = true;
                    }
                    else
                    {
                        CurrentPlayer.Stand = CurrentPlayer.Player.WillStand(State);
                    }
                }
            }
            PrintPlayField();
        }

        private bool Exactly20()
        {
            return CurrentPlayer.FieldValue == Rules.MAX_ALLOWED_FIELD_VALUE;
        }

        private void SwitchPlayers()
        {
            CurrentPlayer = OtherPlayer;
        }
        private PlayerStatus GetOtherPlayer()
        {
            return (CurrentPlayer == Player1) ? Player2 : Player1;
        }
        private GameState GetCurentPlayersGameState()
        {
            return new GameState
            {
                YourWins = CurrentPlayer.RoundWins,
                OpponentWins = OtherPlayer.RoundWins,
                YourFieldValue = CurrentPlayer.FieldCards.Sum(),
                OpponentFieldValue = OtherPlayer.FieldCards.Sum(),
                HandCards = CurrentPlayer.HandCards,
                NumberOfOpponentHandCards = OtherPlayer.HandCards.Count,
                OpponentStands = OtherPlayer.Stand
            };
        }
        
        private void HandlePlayCard()
        {
            if (CurrentPlayer.HandCards.Count > 0)
            {
                PrintPlayField();
                var cardChosen = CurrentPlayer.Player.CardToPlay(State);
                if (cardChosen!=null)
                {
                    var cardToPlay = CurrentPlayer.HandCards.Where(c => c.Equals(cardChosen)).FirstOrDefault();                
                    if (cardToPlay!=null)
                    {
                        PlayCardOnField(cardChosen);
                        CurrentPlayer.HandCards.Remove(cardToPlay);
                        PrintPlayField();
                    }
                    else
                    {
                        w($"You try to play {cardChosen}, but you have no such card.");
                    }
                }
            }
            else
            {
                w("player has no hand cards left");
            }
        }        
        
        private void HandleNewCard()
        {
            var card = Card.FieldCard();
            PlayCardOnField(card);
            PrintPlayField();
        }

        private void HandleRoundOver()
        {
            Player1.Player.RoundOver(State);
            Player2.Player.RoundOver(State);
        }
        private void HandleGameOver()
        {
            Player1.Player.GameOver(Player1.RoundWins, Player2.RoundWins);
            Player2.Player.GameOver(Player2.RoundWins, Player1.RoundWins);
            Player1.RoundWins = 0;
            Player1.HandCards = PopulateHandCards();
            Player2.RoundWins = 0;
            Player2.HandCards = PopulateHandCards();

        }

        private void PlayCardOnField(Card card)
        {
            CurrentPlayer.FieldCards.Add(card.FieldValue);
            PrintPlayField();
        }
        
        private void GameInit()
        {
            Player1 = Player1 ?? GetPlayer1();
            Player2 = Player2 ?? GetPlayer2();
            CurrentPlayer = Player1;
        }

        private PlayerStatus GetPlayer1()
        {
            return new PlayerStatus {
                Player = new PazaakAIPlayerSmarter(),
                FieldCards = new List<int>(),
                HandCards = PopulateHandCards(),
                Stand = false,
                RoundWins = 0,
                GameWins = 0
            };
        }

        private PlayerStatus GetPlayer2()
        {
            return new PlayerStatus
            {
                Player = new PazaakHumanPlayer(),
                FieldCards = new List<int>(),
                HandCards = PopulateHandCards(),
                Stand = false,
                RoundWins = 0,
                GameWins = 0
            };
        }

        public string GetFinalGameWinState()
        {
            return $"Player1: {Player1.GameWins} Player2: {Player2.GameWins}";
        }
        private bool IsRoundOn()
        {
            if (Player1.FieldValue > Rules.MAX_ALLOWED_FIELD_VALUE)
            {
                Player2.RoundWins++;
                PrintPlayField();
                w($"Player1 looses. He has {Player1.FieldValue} points. This exceeds the max allowed {Rules.MAX_ALLOWED_FIELD_VALUE}.");
                ResetRound();
                return false;
            }
            else if (Player2.FieldValue > Rules.MAX_ALLOWED_FIELD_VALUE)
            {
                Player1.RoundWins++;
                PrintPlayField();
                w($"Player2 looses. He has {Player2.FieldValue} points. This exceeds the max allowed {Rules.MAX_ALLOWED_FIELD_VALUE}.");
                ResetRound();
                return false;
            }
            else if (Player1.Stand && Player2.Stand)
            {
                if (Player1.FieldValue > Player2.FieldValue)
                {
                    Player1.RoundWins++;
                    PrintPlayField();
                    w($"Player1 has {Player1.FieldValue} points, which is more then Player 2's {Player2.FieldValue} points. Player1 wins");
                    ResetRound();
                    return false;
                }
                else if (Player1.FieldValue < Player2.FieldValue)
                {
                    Player2.RoundWins++;
                    PrintPlayField();
                    w($"Player2 has {Player2.FieldValue} points, which is more then Player 1's {Player1.FieldValue} points. Player2 wins");
                    ResetRound();
                    return false;
                }
                else
                {
                    PrintPlayField();
                    w($"Player1 has {Player1.FieldValue} points, and Player 2 has {Player2.FieldValue} points. Draw!");
                    ResetRound();
                    return false;
                }
            }
            return true;
        }

        private void ResetRound()
        {
            Player1.FieldCards = new List<int>();
            Player1.Stand = false;
            Player2.FieldCards = new List<int>();
            Player2.Stand = false;
        }
        
        private List<Card> PopulateHandCards()
        {
            var list = new List<Card>();
            for (var i = 0;i< Rules.NUMBER_OF_HAND_CARDS;i++)
            {
                list.Add(Card.HandCard());
            }
            return list;
        }
        private void PrintPlayField()
        {
            if (Player1.Player.IsHuman() || Player2.Player.IsHuman())
            {
                w(GamestatePrinter.PlayField(Player1, Player2));
            }
        }        
        private static void w(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
