namespace Cluedo_Winform_HelperApp.SheetNote
{
    public partial class SheetNotePanel : Panel
    {
        private Label titleLabel;
        private TableLayoutPanel playerCardsTable;
        private TableLayoutPanel whoDidItTable;
        private TableLayoutPanel withWhatTable;
        private TableLayoutPanel andWhereTable;

        private List<Label> playerCardLabels = [];
        private Dictionary<string, List<Label>> whoDidItRows = [];
        private Dictionary<string, List<Label>> withWhatRows = [];
        private Dictionary<string, List<Label>> andWhereRows = [];

        public SheetNotePanel()
        {
            // Set panel properties
            Dock = DockStyle.Right;
            Width = 600;
            BorderStyle = BorderStyle.FixedSingle;
            Padding = new Padding(10);

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Create title label
            titleLabel = new Label
            {
                Text = "📋 Auto-Updating Notesheet Panel",
                Font = new Font("Arial", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 30
            };
            Controls.Add(titleLabel);

            // Create player cards section
            CreatePlayerCardsSection();

            // Create "Who Did It?" section
            CreateCategorySection("WHO DID IT?", ["Peacock", "Mustard", "Scarlett", "Green", "Plum", "White"], whoDidItRows, ref whoDidItTable);

            // Create "With What?" section
            CreateCategorySection("WITH WHAT?", ["Harpoon G.", "Ice Axe", "Microscope", "Oxygen T.", "Shovel", "Welding T."], withWhatRows, ref withWhatTable);

            // Create "And Where?" section
            CreateCategorySection("AND WHERE?", ["C. Quarters", "Helipad", "Hydroponics L.", "Medical Bay", "Mess Hall", "Radio Room", "Research L.", "Sample Vault", "Submarine D."], andWhereRows, ref andWhereTable);
        }

        private void CreatePlayerCardsSection()
        {
            Label sectionLabel = new()
            {
                Text = "PLAYER CARDS",
                Font = new Font("Arial", 11, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 20
            };
            Controls.Add(sectionLabel);

            playerCardsTable = new TableLayoutPanel
            {
                ColumnCount = 6,
                RowCount = 1,
                Dock = DockStyle.Top,
                Height = 30,
                AutoSize = true
            };

            for (int i = 1; i <= 6; i++)
            {
                Label playerLabel = new()
                {
                    Text = $"[✓✓✓]",
                    TextAlign = ContentAlignment.MiddleCenter,
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = SystemColors.ControlLight,
                    AutoSize = true
                };
                playerCardsTable.Controls.Add(playerLabel);
                playerCardLabels.Add(playerLabel);
            }

            Controls.Add(playerCardsTable);
        }

        private void CreateCategorySection(string title, string[] items, Dictionary<string, List<Label>> rows, ref TableLayoutPanel table)
        {
            Label sectionLabel = new()
            {
                Text = title,
                Font = new Font("Arial", 11, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 20
            };
            Controls.Add(sectionLabel);

            table = new TableLayoutPanel
            {
                ColumnCount = 7, // 1 for item name, 6 for players
                RowCount = items.Length,
                Dock = DockStyle.Top,
                AutoSize = true
            };

            foreach (var item in items)
            {
                Label itemLabel = new()
                {
                    Text = item,
                    TextAlign = ContentAlignment.MiddleLeft,
                    BorderStyle = BorderStyle.FixedSingle,
                    AutoSize = true
                };
                table.Controls.Add(itemLabel);

                List<Label> playerLabels = [];
                for (int i = 0; i < 6; i++)
                {
                    Label playerLabel = new()
                    {
                        Text = "⬜",
                        TextAlign = ContentAlignment.MiddleCenter,
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = SystemColors.ControlLight,
                        AutoSize = true
                    };
                    table.Controls.Add(playerLabel);
                    playerLabels.Add(playerLabel);
                }

                rows[item] = playerLabels;
            }

            Controls.Add(table);
        }

        public void UpdatePlayerCards(int playerIndex, int cardCount)
        {
            if (playerIndex < 0 || playerIndex >= playerCardLabels.Count) return;

            string checkmarks = new('✓', cardCount);
            playerCardLabels[playerIndex].Text = $"[{checkmarks}]";
        }

        public void UpdateCategory(string category, string item, int playerIndex, string status)
        {
            Dictionary<string, List<Label>> rows = category switch
            {
                "WHO" => whoDidItRows,
                "WHAT" => withWhatRows,
                "WHERE" => andWhereRows,
                _ => null
            };

            if (rows == null || playerIndex < 0 || playerIndex >= 6) return;

            if (rows.TryGetValue(item, out var playerLabels))
            {
                Label playerLabel = playerLabels[playerIndex];
                playerLabel.Text = status switch
                {
                    "✅" => "✅",
                    "❌" => "❌",
                    _ => "⬜"
                };
                playerLabel.BackColor = status == "✅" ? Color.LightGreen : SystemColors.ControlLight;
            }
        }
    }
}
