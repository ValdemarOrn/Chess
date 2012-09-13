namespace Chess.UI
{
	partial class GameView
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.buttonNewGame = new System.Windows.Forms.Button();
			this.labelSelectedTile = new System.Windows.Forms.Label();
			this.labelChecked = new System.Windows.Forms.Label();
			this.buttonSetState = new System.Windows.Forms.Button();
			this.textBoxFEN = new System.Windows.Forms.TextBox();
			this.labelWhiteScore = new System.Windows.Forms.Label();
			this.labelBlackScore = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.labelScore = new System.Windows.Forms.Label();
			this.boardControl = new Chess.UI.BoardControl();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// buttonNewGame
			// 
			this.buttonNewGame.Location = new System.Drawing.Point(499, 95);
			this.buttonNewGame.Name = "buttonNewGame";
			this.buttonNewGame.Size = new System.Drawing.Size(75, 23);
			this.buttonNewGame.TabIndex = 1;
			this.buttonNewGame.Text = "New Game";
			this.buttonNewGame.UseVisualStyleBackColor = true;
			this.buttonNewGame.Click += new System.EventHandler(this.button1_Click);
			// 
			// labelSelectedTile
			// 
			this.labelSelectedTile.AutoSize = true;
			this.labelSelectedTile.Location = new System.Drawing.Point(496, 157);
			this.labelSelectedTile.Name = "labelSelectedTile";
			this.labelSelectedTile.Size = new System.Drawing.Size(72, 13);
			this.labelSelectedTile.TabIndex = 2;
			this.labelSelectedTile.Text = "Selected Tile:";
			// 
			// labelChecked
			// 
			this.labelChecked.AutoSize = true;
			this.labelChecked.Location = new System.Drawing.Point(496, 188);
			this.labelChecked.Name = "labelChecked";
			this.labelChecked.Size = new System.Drawing.Size(72, 13);
			this.labelChecked.TabIndex = 3;
			this.labelChecked.Text = "labelChecked";
			// 
			// buttonSetState
			// 
			this.buttonSetState.Location = new System.Drawing.Point(499, 66);
			this.buttonSetState.Name = "buttonSetState";
			this.buttonSetState.Size = new System.Drawing.Size(75, 23);
			this.buttonSetState.TabIndex = 4;
			this.buttonSetState.Text = "Set State";
			this.buttonSetState.UseVisualStyleBackColor = true;
			this.buttonSetState.Click += new System.EventHandler(this.buttonSetState_Click);
			// 
			// textBoxFEN
			// 
			this.textBoxFEN.Location = new System.Drawing.Point(499, 35);
			this.textBoxFEN.Name = "textBoxFEN";
			this.textBoxFEN.Size = new System.Drawing.Size(242, 20);
			this.textBoxFEN.TabIndex = 5;
			// 
			// labelWhiteScore
			// 
			this.labelWhiteScore.AutoSize = true;
			this.labelWhiteScore.Location = new System.Drawing.Point(496, 254);
			this.labelWhiteScore.Name = "labelWhiteScore";
			this.labelWhiteScore.Size = new System.Drawing.Size(38, 13);
			this.labelWhiteScore.TabIndex = 6;
			this.labelWhiteScore.Text = "Score:";
			// 
			// labelBlackScore
			// 
			this.labelBlackScore.AutoSize = true;
			this.labelBlackScore.Location = new System.Drawing.Point(738, 254);
			this.labelBlackScore.Name = "labelBlackScore";
			this.labelBlackScore.Size = new System.Drawing.Size(38, 13);
			this.labelBlackScore.TabIndex = 7;
			this.labelBlackScore.Text = "Score:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(738, 229);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(65, 13);
			this.label1.TabIndex = 9;
			this.label1.Text = "Black Score";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(496, 229);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(66, 13);
			this.label2.TabIndex = 8;
			this.label2.Text = "White Score";
			// 
			// labelScore
			// 
			this.labelScore.AutoSize = true;
			this.labelScore.Location = new System.Drawing.Point(647, 498);
			this.labelScore.Name = "labelScore";
			this.labelScore.Size = new System.Drawing.Size(57, 13);
			this.labelScore.TabIndex = 10;
			this.labelScore.Text = "labelScore";
			// 
			// boardControl
			// 
			this.boardControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.boardControl.Location = new System.Drawing.Point(60, 35);
			this.boardControl.Name = "boardControl";
			this.boardControl.Size = new System.Drawing.Size(400, 400);
			this.boardControl.TabIndex = 0;
			this.boardControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.boardUI1_MouseClick);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(650, 95);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 11;
			this.button1.Text = "Evaluate";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click_1);
			// 
			// GameView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(940, 532);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.labelScore);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.labelBlackScore);
			this.Controls.Add(this.labelWhiteScore);
			this.Controls.Add(this.textBoxFEN);
			this.Controls.Add(this.buttonSetState);
			this.Controls.Add(this.labelChecked);
			this.Controls.Add(this.labelSelectedTile);
			this.Controls.Add(this.buttonNewGame);
			this.Controls.Add(this.boardControl);
			this.Name = "GameView";
			this.Text = "Game View";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public BoardControl boardControl;
		private System.Windows.Forms.Button buttonNewGame;
		private System.Windows.Forms.Label labelSelectedTile;
		public System.Windows.Forms.Label labelChecked;
		private System.Windows.Forms.Button buttonSetState;
		private System.Windows.Forms.TextBox textBoxFEN;
		public System.Windows.Forms.Label labelWhiteScore;
		public System.Windows.Forms.Label labelBlackScore;
		public System.Windows.Forms.Label label1;
		public System.Windows.Forms.Label label2;
		public System.Windows.Forms.Label labelScore;
		private System.Windows.Forms.Button button1;
	}
}

