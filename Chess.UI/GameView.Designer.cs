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
			this.button1 = new System.Windows.Forms.Button();
			this.labelSelectedTile = new System.Windows.Forms.Label();
			this.gameControl = new Chess.UI.BoardControl();
			this.labelChecked = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(499, 35);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "New Game";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// labelSelectedTile
			// 
			this.labelSelectedTile.AutoSize = true;
			this.labelSelectedTile.Location = new System.Drawing.Point(496, 97);
			this.labelSelectedTile.Name = "labelSelectedTile";
			this.labelSelectedTile.Size = new System.Drawing.Size(72, 13);
			this.labelSelectedTile.TabIndex = 2;
			this.labelSelectedTile.Text = "Selected Tile:";
			// 
			// boardUI1
			// 
			this.gameControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.gameControl.Location = new System.Drawing.Point(60, 35);
			this.gameControl.Name = "boardUI1";
			this.gameControl.Size = new System.Drawing.Size(400, 400);
			this.gameControl.TabIndex = 0;
			this.gameControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.boardUI1_MouseClick);
			// 
			// labelChecked
			// 
			this.labelChecked.AutoSize = true;
			this.labelChecked.Location = new System.Drawing.Point(496, 128);
			this.labelChecked.Name = "labelChecked";
			this.labelChecked.Size = new System.Drawing.Size(72, 13);
			this.labelChecked.TabIndex = 3;
			this.labelChecked.Text = "labelChecked";
			// 
			// GameView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(753, 532);
			this.Controls.Add(this.labelChecked);
			this.Controls.Add(this.labelSelectedTile);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.gameControl);
			this.Name = "GameView";
			this.Text = "Game View";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public BoardControl gameControl;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label labelSelectedTile;
		public System.Windows.Forms.Label labelChecked;
	}
}

