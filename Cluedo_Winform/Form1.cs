using Cluedo_Winform_HelperApp.SuggestionInput;
using Cluedo_Winform_HelperApp.Entity;
using Cluedo_Winform_HelperApp.SuggestionInput;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Cluedo_Winform_HelperApp
{
    public partial class Form1 : Form
    {
        private SuggestionInputPanel suggestionPanel;
        private List<Player> players;
        private int currentPlayerIndex = 0;
        private int currentRespondingPlayerIndex = 0;
        private bool inResponseSequence = false;
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

            // Add the panel to the form
            Controls.Add(suggestionPanel);

            // Handle form resize to keep the panel at half width
            Resize += Form1_Resize;

            // Update title to show current player
            UpdateSuggestionPanelTitle();
        }

        private void UpdateSuggestionPanelTitle()
        {
            suggestionPanel?.UpdateTitle($"Player {players[currentPlayerIndex].PlayerNumber}'s Turn");
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // Keep the suggestion panel at exactly half the form width when resizing
            if (suggestionPanel != null)
            {
                suggestionPanel.Width = ClientSize.Width / 2;
            }
        }

        private void SuggestionPanel_SuggestionMade(object sender, SuggestionEventArgs e)
        {
            // Store the current suggestion
            currentSuspect = new Card(e.Who, CardType.Suspect);
            currentWeapon = new Card(e.What, CardType.Weapon);
            currentRoom = new Card(e.Where, CardType.Room);

            // Start the response sequence
            inResponseSequence = true;

            // Find the first player to respond (player to the right of current player)
            currentRespondingPlayerIndex = (currentPlayerIndex + 1) % players.Count;

            Debug.WriteLine($"Player {players[currentPlayerIndex].PlayerNumber} suggests: {e.Who}, {e.What}, {e.Where}");
            Debug.WriteLine($"Waiting for Player {players[currentRespondingPlayerIndex].PlayerNumber} to respond...");

            // Update the UI to show we're waiting for a response
            suggestionPanel.SetResponseMode(true);
        }

        private void SuggestionPanel_ResponseRecorded(object sender, ResponseEventArgs e)
        {
            Player respondingPlayer = players[currentRespondingPlayerIndex];

            if (e.HasResponse)
            {
                // Player has at least one of the suggested cards
                Debug.WriteLine($"Player {respondingPlayer.PlayerNumber} holds at least one of the cards: {e.Who}, {e.What}, {e.Where}");

                // Record the possible card set for this player
                respondingPlayer.AddPossibleCardSet(
                    new Card(e.Who, CardType.Suspect),
                    new Card(e.What, CardType.Weapon),
                    new Card(e.Where, CardType.Room)
                );

                // End the response sequence
                inResponseSequence = false;

                // Move to the next player's turn
                currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
                UpdateSuggestionPanelTitle();

                // Reset the suggestion panel for the next turn
                suggestionPanel.SetResponseMode(false);
                suggestionPanel.ClearSelections();
            }
            else
            {
                // Player does not have any of the suggested cards
                Debug.WriteLine($"Player {respondingPlayer.PlayerNumber} does NOT have the cards: {currentSuspect.Name}, {currentWeapon.Name}, {currentRoom.Name}");

                // Record that this player doesn't have these cards
                respondingPlayer.AddExcludedCard(currentSuspect);
                respondingPlayer.AddExcludedCard(currentWeapon);
                respondingPlayer.AddExcludedCard(currentRoom);

                // Move to the next responding player
                currentRespondingPlayerIndex = (currentRespondingPlayerIndex + 1) % players.Count;

                // Check if we've gone all the way around to the player who made the suggestion
                if (currentRespondingPlayerIndex == currentPlayerIndex)
                {
                    // End the response sequence if we've gone all the way around
                    Debug.WriteLine("No players have any of the suggested cards!");
                    inResponseSequence = false;

                    // Move to the next player's turn
                    currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
                    UpdateSuggestionPanelTitle();

                    // Reset the suggestion panel for the next turn
                    suggestionPanel.SetResponseMode(false);
                    suggestionPanel.ClearSelections();
                }
                else
                {
                    // Continue waiting for the next player to respond
                    Debug.WriteLine($"Waiting for Player {players[currentRespondingPlayerIndex].PlayerNumber} to respond...");
                }
            }
        }


        // Your existing InitializeComponent method will be here, typically auto-generated by the designer
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