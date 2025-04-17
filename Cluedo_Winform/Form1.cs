using Cluedo_Winform_HelperApp.SuggestionInput;
using Cluedo_Winform_HelperApp.Entity;
using System.Diagnostics;
using Cluedo_Winform_HelperApp.SheetNote;

namespace Cluedo_Winform_HelperApp
{
    public partial class Form1 : Form
    {
        private SuggestionInputPanel suggestionPanel;
        private SheetNotePanel sheetNotePanel;
        private List<Player> players;
        private int currentPlayerIndex = 0;
        private int currentRespondingPlayerIndex = 0;
        private Card? currentSuspect = null;
        private Card? currentWeapon = null;
        private Card? currentRoom = null;

        public Form1()
        {
            InitializeComponent();
            CustomInitializeComponent();
            Size = new Size(1200, 700);
            Text = "Cluedo Helper App";

            InitializePlayers();
            InitializeSuggestionPanel();
            InitializeSheetNotePanel();
        }

        private void InitializePlayers()
        {
            // Initialize players - you can adjust the number of players as needed
            players = [];
            for (int i = 1; i <= 6; i++)
            {
                players.Add(new Player(i));
            }
        }

        private void InitializeSuggestionPanel()
        {
            // Create the suggestion panel
            suggestionPanel = new SuggestionInputPanel
            {
                // Position and size the panel to take exactly the left half of the form
                Dock = DockStyle.Left,
                Width = ClientSize.Width / 2
            };

            // Subscribe to the events
            suggestionPanel.SuggestionMade += SuggestionPanel_SuggestionMade;
            suggestionPanel.ResponseRecorded += SuggestionPanel_ResponseRecorded;
            suggestionPanel.InitialCardAssigned += SuggestionPanel_InitialCardAssigned;

            // Add the panel to the form
            Controls.Add(suggestionPanel);

            // Handle form resize to keep the panel at half width
            Resize += Form1_Resize;

            // Update title to show current player
            UpdateSuggestionPanelTitle();
        }

        private void InitializeSheetNotePanel()
        {
            // Create the sheet note panel
            sheetNotePanel = new SheetNotePanel
            {
                Dock = DockStyle.Right,
                Width = ClientSize.Width / 2
            };

            // Add the panel to the form
            Controls.Add(sheetNotePanel);

            // Handle form resize to keep the panel at half width
            Resize += Form1_Resize;
        }

        private void UpdateSuggestionPanelTitle()
        {
            suggestionPanel?.UpdateTitle($"Player {players[currentPlayerIndex].PlayerNumber}'s Turn");
        }

        private void ProcessInitialCardAssignment(int playerNumber, Card card)
        {
            if (playerNumber < 1 || playerNumber > players.Count)
            {
                Debug.WriteLine($"Invalid player number: {playerNumber}");
                return;
            }

            Player player = players[playerNumber - 1];

            // Check if the player already has this card
            if (player.HasCard(card))
            {
                player.RemoveCard(card);
                Debug.WriteLine($"Removed card {card.Name} from Player {playerNumber}");
            }
            else
            {
                player.AddCard(card);
                Debug.WriteLine($"Added card {card.Name} to Player {playerNumber}");

                // Remove this card from other players
                foreach (var otherPlayer in players)
                {
                    if (otherPlayer.PlayerNumber != playerNumber && otherPlayer.HasCard(card))
                    {
                        otherPlayer.RemoveCard(card);
                        Debug.WriteLine($"Removed card {card.Name} from Player {otherPlayer.PlayerNumber}");
                    }
                }
            }

            // Update the SheetNotePanel with the player's card count
            sheetNotePanel.UpdatePlayerCards(playerNumber - 1, player.Cards.Count);
        }

        // Add this event handler to Form1
        private void SuggestionPanel_InitialCardAssigned(object sender, InitialCardEventArgs e)
        {
            Card card = new(e.CardName, GetCardTypeFromString(e.CardType));
            ProcessInitialCardAssignment(e.PlayerNumber, card);
        }

        // Helper method to convert string to CardType
        private static CardType GetCardTypeFromString(string typeString)
        {
            return typeString.ToLower() switch
            {
                "who" => CardType.Suspect,
                "what" => CardType.Weapon,
                "where" => CardType.Room,
                _ => throw new ArgumentException($"Unknown card type: {typeString}"),
            };
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (suggestionPanel != null)
            {
                suggestionPanel.Width = ClientSize.Width / 2;
            }
            if (sheetNotePanel != null)
            {
                sheetNotePanel.Width = ClientSize.Width / 2;
            }
        }

        private void SuggestionPanel_SuggestionMade(object sender, SuggestionEventArgs e)
        {
            currentSuspect = new Card(e.Who, CardType.Suspect);
            currentWeapon = new Card(e.What, CardType.Weapon);
            currentRoom = new Card(e.Where, CardType.Room);

            currentRespondingPlayerIndex = (currentPlayerIndex + 1) % players.Count;

            Debug.WriteLine($"Player {players[currentPlayerIndex].PlayerNumber} suggests: {e.Who}, {e.What}, {e.Where}");
            Debug.WriteLine($"Waiting for Player {players[currentRespondingPlayerIndex].PlayerNumber} to respond...");

            // Update the SheetNotePanel to highlight the suggestion
            sheetNotePanel.UpdateCategory("WHO", e.Who, currentPlayerIndex, "⬜");
            sheetNotePanel.UpdateCategory("WHAT", e.What, currentPlayerIndex, "⬜");
            sheetNotePanel.UpdateCategory("WHERE", e.Where, currentPlayerIndex, "⬜");
        }

        private void SuggestionPanel_ResponseRecorded(object sender, ResponseEventArgs e)
        {
            Player respondingPlayer = players[currentRespondingPlayerIndex];

            if (e.HasResponse)
            {
                Debug.WriteLine($"Player {respondingPlayer.PlayerNumber} holds at least one of the cards: {e.Who}, {e.What}, {e.Where}");

                respondingPlayer.AddPossibleCardSet(
                    new Card(e.Who, CardType.Suspect),
                    new Card(e.What, CardType.Weapon),
                    new Card(e.Where, CardType.Room)
                );

                // Update the SheetNotePanel to mark the response
                sheetNotePanel.UpdateCategory("WHO", e.Who, currentRespondingPlayerIndex, "✅");
                sheetNotePanel.UpdateCategory("WHAT", e.What, currentRespondingPlayerIndex, "✅");
                sheetNotePanel.UpdateCategory("WHERE", e.Where, currentRespondingPlayerIndex, "✅");

                currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
                UpdateSuggestionPanelTitle();
                suggestionPanel.ClearSelections();
            }
            else
            {
                Debug.WriteLine($"Player {respondingPlayer.PlayerNumber} does NOT have the cards: {currentSuspect.Name}, {currentWeapon.Name}, {currentRoom.Name}");

                respondingPlayer.AddExcludedCard(currentSuspect);
                respondingPlayer.AddExcludedCard(currentWeapon);
                respondingPlayer.AddExcludedCard(currentRoom);

                // Update the SheetNotePanel to mark the exclusion
                sheetNotePanel.UpdateCategory("WHO", currentSuspect.Name, currentRespondingPlayerIndex, "❌");
                sheetNotePanel.UpdateCategory("WHAT", currentWeapon.Name, currentRespondingPlayerIndex, "❌");
                sheetNotePanel.UpdateCategory("WHERE", currentRoom.Name, currentRespondingPlayerIndex, "❌");

                currentRespondingPlayerIndex = (currentRespondingPlayerIndex + 1) % players.Count;

                if (currentRespondingPlayerIndex == currentPlayerIndex)
                {
                    Debug.WriteLine("No players have any of the suggested cards!");

                    currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
                    UpdateSuggestionPanelTitle();
                    suggestionPanel.ButtonEnablingByMode(false);
                    suggestionPanel.ClearSelections();
                }
                else
                {
                    Debug.WriteLine($"Waiting for Player {players[currentRespondingPlayerIndex].PlayerNumber} to respond...");
                }
            }
        }


        // Custom InitializeComponent method
        private void CustomInitializeComponent()
        {
            SuspendLayout();
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1200, 700);
            Name = "Form1";
            Text = "Cluedo Helper App";
            ResumeLayout(false);
        }
    }
}