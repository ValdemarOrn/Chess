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
			this.boardControl = new Chess.UI.BoardControl();
			this.labelChecked = new System.Windows.Forms.Label();
			this.buttonSetState = new System.Windows.Forms.Button();
			this.textBoxFEN = new System.Windows.Forms.TextBox();
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
			// boardControl
			// 
			this.boardControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.boardControl.Location = new System.Drawing.Point(60, 35);
			this.boardControl.Name = "boardControl";
			this.boardControl.Size = new System.Drawing.Size(400, 400);
			this.boardControl.TabIndex = 0;
			this.boardControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.boardUI1_MouseClick);
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
			// GameView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(753, 532);
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
	}
}

