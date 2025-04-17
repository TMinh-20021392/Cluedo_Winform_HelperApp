using System.Diagnostics;

namespace Cluedo_Winform_HelperApp.SuggestionInput
{
    public partial class SuggestionInputPanel : Panel
    {
        // Selected items
        private Button? selectedWho = null;
        private Button? selectedWhat = null;
        private Button? selectedWhere = null;

        // Lists to track buttons by category
        private List<Button> whoButtons = [];
        private List<Button> whatButtons = [];
        private List<Button> whereButtons = [];

        // Action buttons
        private Button suggestButton;
        private Button noResponseButton;
        private Button yesResponseButton;

        // Initial
        private ComboBox playerComboBox;
        private Button initialButton;
        private bool initialMode = false;

        // Title label
        private Label titleLabel;
        private bool inResponseMode = false;

        // Events
        public event EventHandler<SuggestionEventArgs> SuggestionMade;
        public event EventHandler<ResponseEventArgs> ResponseRecorded;
        public event EventHandler<InitialCardEventArgs> InitialCardAssigned;

        // Constructor
        public SuggestionInputPanel()
        {
            // Set panel properties
            Size = new Size(600, 550);
            BorderStyle = BorderStyle.FixedSingle;
            Padding = new Padding(10);

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Create title label
            titleLabel = new Label
            {
                Text = "Suggestion Input Panel",
                Font = new Font("Arial", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 30
            };
            Controls.Add(titleLabel);

            // Create sections
            int yPos = 40;

            // WHO section
            yPos = CreateSection("🕵️ WHO (Suspect)", yPos,
                ["Peacock", "Mustard", "Scarlett", "Green", "Plum", "White"],
                2, whoButtons, ButtonType.Who);

            // WHAT section
            yPos = CreateSection("🔧 WHAT (Weapon)", yPos,
                ["Harpoon G.", "Ice Axe", "Microscope", "Oxygen T.", "Shovel", "Welding T."],
                2, whatButtons, ButtonType.What);

            // WHERE section
            yPos = CreateSection("🏠 WHERE (Room)", yPos,
                ["C. Quarters", "Helipad", "Hydroponics L.", "Medical Bay", "Mess Hall", "Radio Room", "Research L.", "Sample Vault", "Submarine D."],
                3, whereButtons, ButtonType.Where);

            // Action buttons section
            yPos = CreateActionButtonsSection(yPos);

            // Initial section
            yPos = CreateInitialSection(yPos);
        }

        public void UpdateTitle(string title)
        {
            titleLabel.Text = title;
        }

        private int CreateSection(string title, int yPosition, string[] items, int rows, List<Button> buttonList, ButtonType type)
        {
            // Add section title
            Label sectionLabel = new()
            {
                Text = title,
                Font = new Font("Arial", 11, FontStyle.Bold),
                Location = new Point(20, yPosition),
                AutoSize = true
            };
            Controls.Add(sectionLabel);
            yPosition += 30;

            // Create group box for buttons
            GroupBox groupBox = new()
            {
                Location = new Point(20, yPosition),
                Size = new Size(560, rows * 40 + 10),
                Text = ""
            };
            Controls.Add(groupBox);

            // Calculate items per row
            int itemsPerRow = (int)Math.Ceiling((double)items.Length / rows);
            int buttonWidth = 160;
            int buttonHeight = 30;
            int padding = 10;

            // Create buttons for items
            for (int i = 0; i < items.Length; i++)
            {
                int row = i / itemsPerRow;
                int col = i % itemsPerRow;

                Button button = new()
                {
                    Text = items[i],
                    Size = new Size(buttonWidth, buttonHeight),
                    Location = new Point(10 + col * (buttonWidth + padding), 20 + row * (buttonHeight + padding)),
                    Tag = type,
                    BackColor = SystemColors.Control,
                    FlatStyle = FlatStyle.Flat
                };
                button.Click += ItemButton_Click;
                groupBox.Controls.Add(button);
                buttonList.Add(button);
            }

            return yPosition + groupBox.Height + 20;
        }

        private int CreateInitialSection(int yPosition)
        {
            // Add section title
            Label sectionLabel = new()
            {
                Text = "🎲 Initial Cards",
                Font = new Font("Arial", 11, FontStyle.Bold),
                Location = new Point(20, yPosition),
                AutoSize = true
            };
            Controls.Add(sectionLabel);
            yPosition += 30;

            // Create group box for controls
            GroupBox groupBox = new()
            {
                Location = new Point(20, yPosition),
                Size = new Size(560, 50),
                Text = ""
            };
            Controls.Add(groupBox);

            // Create player selection combo box
            Label playerLabel = new()
            {
                Text = "Player:",
                Location = new Point(10, 20),
                AutoSize = true
            };
            groupBox.Controls.Add(playerLabel);

            playerComboBox = new ComboBox
            {
                Location = new Point(60, 17),
                Size = new Size(80, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Add player numbers to combo box
            for (int i = 1; i <= 6; i++)
            {
                playerComboBox.Items.Add(i);
            }

            // Set default selection
            if (playerComboBox.Items.Count > 0)
            {
                playerComboBox.SelectedIndex = 0;
            }

            groupBox.Controls.Add(playerComboBox);

            // Create initial button
            initialButton = new Button
            {
                Text = "Initial Mode",
                Size = new Size(160, 30),
                Location = new Point(180, 15),
                FlatStyle = FlatStyle.Flat
            };
            initialButton.Click += InitialButton_Click;
            groupBox.Controls.Add(initialButton);

            return yPosition + groupBox.Height + 20;
        }

        private int CreateActionButtonsSection(int yPosition)
        {
            // Add section title
            Label sectionLabel = new()
            {
                Text = "🎮 Action Buttons",
                Font = new Font("Arial", 11, FontStyle.Bold),
                Location = new Point(20, yPosition),
                AutoSize = true
            };
            Controls.Add(sectionLabel);
            yPosition += 30;

            // Create group box for buttons
            GroupBox groupBox = new()
            {
                Location = new Point(20, yPosition),
                Size = new Size(560, 50),
                Text = ""
            };
            Controls.Add(groupBox);

            // Create action buttons
            suggestButton = new Button
            {
                Text = "Suggest",
                Size = new Size(160, 30),
                Location = new Point(10, 15),
                Enabled = false
            };
            suggestButton.Click += SuggestButton_Click;
            groupBox.Controls.Add(suggestButton);

            noResponseButton = new Button
            {
                Text = "No Response",
                Size = new Size(160, 30),
                Location = new Point(180, 15),
                Enabled = false
            };
            noResponseButton.Click += NoResponseButton_Click;
            groupBox.Controls.Add(noResponseButton);

            yesResponseButton = new Button
            {
                Text = "Yes Response",
                Size = new Size(160, 30),
                Location = new Point(350, 15),
                Enabled = false
            };
            yesResponseButton.Click += YesResponseButton_Click;
            groupBox.Controls.Add(yesResponseButton);

            return yPosition + groupBox.Height + 20;
        }

        private void ItemButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ButtonType type = (ButtonType)button.Tag;

            if (initialMode)
            {
                // In Initial mode, clicking a card marks it as belonging to the selected player
                if (playerComboBox.SelectedItem != null)
                {
                    int playerNumber = (int)playerComboBox.SelectedItem;
                    string cardName = button.Text;
                    string cardType = type.ToString();

                    Debug.WriteLine($"Player {playerNumber} certainly has {cardName} ({cardType})");

                    // Disable initialMode immediately
                    initialMode = false;
                    initialButton.BackColor = SystemColors.Control;
                    ClearSelections();

                }
            }
            else
            {
                // Normal suggestion mode behavior
                // Reset selection in the same category
                switch (type)
                {
                    case ButtonType.Who:
                        ResetButtonsInCategory(whoButtons);
                        selectedWho = button;
                        break;
                    case ButtonType.What:
                        ResetButtonsInCategory(whatButtons);
                        selectedWhat = button;
                        break;
                    case ButtonType.Where:
                        ResetButtonsInCategory(whereButtons);
                        selectedWhere = button;
                        break;
                }

                // Highlight the selected button
                button.BackColor = Color.LightGreen;

                // Enable the suggest button if all required selections are made
                suggestButton.Enabled = (selectedWho != null && selectedWhat != null && selectedWhere != null);
            }
            ButtonEnablingByMode(inResponseMode);
        }

        private void ResetButtonsInCategory(List<Button> buttons)
        {
            foreach (var button in buttons)
            {
                button.BackColor = SystemColors.Control;
            }
        }

        private void SuggestButton_Click(object sender, EventArgs e)
        {
            if (selectedWho != null && selectedWhat != null && selectedWhere != null)
            {
                // Raise suggestion event
                SuggestionEventArgs args = new(
                    selectedWho.Text,
                    selectedWhat.Text,
                    selectedWhere.Text
                );
                SuggestionMade?.Invoke(this, args);
            }
            ButtonEnablingByMode(true);
        }

        private void NoResponseButton_Click(object sender, EventArgs e)
        {
            if (selectedWho != null && selectedWhat != null && selectedWhere != null)
            {
                // Raise response event
                ResponseEventArgs args = new(
                    selectedWho.Text,
                    selectedWhat.Text,
                    selectedWhere.Text,
                    false
                );
                ResponseRecorded?.Invoke(this, args);
            }
        }

        private void YesResponseButton_Click(object sender, EventArgs e)
        {
            if (selectedWho != null && selectedWhat != null && selectedWhere != null)
            {
                // Raise response event
                ResponseEventArgs args = new(
                    selectedWho.Text,
                    selectedWhat.Text,
                    selectedWhere.Text,
                    true
                );
                ResponseRecorded?.Invoke(this, args);
            }
            ButtonEnablingByMode(false);
        }

        private void InitialButton_Click(object sender, EventArgs e)
        {
            initialMode = !initialMode;

            if (initialMode)
            {
                ClearSelections();
                initialButton.BackColor = Color.Red;
            }
            else
            {
                initialButton.BackColor = SystemColors.Control;
            }
            ButtonEnablingByMode(inResponseMode);
        }

        // Enable/disable appropriate buttons for modes
        public void ButtonEnablingByMode(bool inResponseMode)
        {
            this.inResponseMode = inResponseMode;
            if (initialMode)
            {
                // In Initial mode: only Initial button is enabled
                initialButton.Enabled = true;
                suggestButton.Enabled = false;
                yesResponseButton.Enabled = false;
                noResponseButton.Enabled = false;
            }
            else if (inResponseMode)
            {
                // In Response mode: only Yes/No buttons are enabled
                initialButton.Enabled = false;
                suggestButton.Enabled = false;
                yesResponseButton.Enabled = true;
                noResponseButton.Enabled = true;
            }
            else
            {
                // Normal mode: Initial and Suggest are enabled (if selections complete)
                initialButton.Enabled = true;
                suggestButton.Enabled = (selectedWho != null && selectedWhat != null && selectedWhere != null);
                yesResponseButton.Enabled = false;
                noResponseButton.Enabled = false;
            }
        }

        // Clear selections after each suggestion/response sequence
        public void ClearSelections()
        {
            // Reset all card buttons
            ResetAllCardButtons();

            // Clear selections
            selectedWho = null;
            selectedWhat = null;
            selectedWhere = null;

            // Reset button states
            suggestButton.Enabled = false;
            noResponseButton.Enabled = false;
            yesResponseButton.Enabled = false;

        }

        // Reset all card buttons
        public void ResetAllCardButtons()
        {
            ResetButtonsInCategory(whatButtons);
            ResetButtonsInCategory(whoButtons);
            ResetButtonsInCategory(whereButtons);
        }
    }
}
