namespace Chess.Lib.BitboardEditor
{
	partial class BoardControlPanel
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxFEN = new System.Windows.Forms.TextBox();
			this.buttonLoadFEN = new System.Windows.Forms.Button();
			this.buttonClear = new System.Windows.Forms.Button();
			this.buttonInit = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.radioButtonBlackPlayer = new System.Windows.Forms.RadioButton();
			this.radioButtonWhitePlayer = new System.Windows.Forms.RadioButton();
			this.checkBoxCastlingWK = new System.Windows.Forms.CheckBox();
			this.checkBoxCastlingBK = new System.Windows.Forms.CheckBox();
			this.checkBoxCastlingWQ = new System.Windows.Forms.CheckBox();
			this.checkBoxCastlingBQ = new System.Windows.Forms.CheckBox();
			this.textBoxBits0 = new System.Windows.Forms.TextBox();
			this.textBoxBits1 = new System.Windows.Forms.TextBox();
			this.textBoxBits3 = new System.Windows.Forms.TextBox();
			this.textBoxBits2 = new System.Windows.Forms.TextBox();
			this.textBoxBits7 = new System.Windows.Forms.TextBox();
			this.textBoxBits6 = new System.Windows.Forms.TextBox();
			this.textBoxBits5 = new System.Windows.Forms.TextBox();
			this.textBoxBits4 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.boardControl1 = new Chess.Lib.BitboardEditor.BoardControl();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(640, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(58, 13);
			this.label1.TabIndex = 10;
			this.label1.Text = "FEN String";
			// 
			// textBoxFEN
			// 
			this.textBoxFEN.Location = new System.Drawing.Point(449, 34);
			this.textBoxFEN.Name = "textBoxFEN";
			this.textBoxFEN.Size = new System.Drawing.Size(249, 20);
			this.textBoxFEN.TabIndex = 9;
			this.textBoxFEN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// buttonLoadFEN
			// 
			this.buttonLoadFEN.Location = new System.Drawing.Point(623, 60);
			this.buttonLoadFEN.Name = "buttonLoadFEN";
			this.buttonLoadFEN.Size = new System.Drawing.Size(75, 23);
			this.buttonLoadFEN.TabIndex = 11;
			this.buttonLoadFEN.Text = "Load";
			this.buttonLoadFEN.UseVisualStyleBackColor = true;
			this.buttonLoadFEN.Click += new System.EventHandler(this.buttonLoadFEN_Click);
			// 
			// buttonClear
			// 
			this.buttonClear.Location = new System.Drawing.Point(449, 60);
			this.buttonClear.Name = "buttonClear";
			this.buttonClear.Size = new System.Drawing.Size(75, 23);
			this.buttonClear.TabIndex = 12;
			this.buttonClear.Text = "Clear";
			this.buttonClear.UseVisualStyleBackColor = true;
			this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
			// 
			// buttonInit
			// 
			this.buttonInit.Location = new System.Drawing.Point(530, 60);
			this.buttonInit.Name = "buttonInit";
			this.buttonInit.Size = new System.Drawing.Size(75, 23);
			this.buttonInit.TabIndex = 13;
			this.buttonInit.Text = "Initialize";
			this.buttonInit.UseVisualStyleBackColor = true;
			this.buttonInit.Click += new System.EventHandler(this.buttonInit_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(446, 94);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(61, 13);
			this.label4.TabIndex = 18;
			this.label4.Text = "Player Turn";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(571, 94);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(44, 13);
			this.label7.TabIndex = 22;
			this.label7.Text = "Castling";
			// 
			// radioButtonBlackPlayer
			// 
			this.radioButtonBlackPlayer.AutoSize = true;
			this.radioButtonBlackPlayer.Location = new System.Drawing.Point(449, 138);
			this.radioButtonBlackPlayer.Name = "radioButtonBlackPlayer";
			this.radioButtonBlackPlayer.Size = new System.Drawing.Size(52, 17);
			this.radioButtonBlackPlayer.TabIndex = 25;
			this.radioButtonBlackPlayer.Text = "Black";
			this.radioButtonBlackPlayer.UseVisualStyleBackColor = true;
			this.radioButtonBlackPlayer.CheckedChanged += new System.EventHandler(this.StateChanged);
			// 
			// radioButtonWhitePlayer
			// 
			this.radioButtonWhitePlayer.AutoSize = true;
			this.radioButtonWhitePlayer.Checked = true;
			this.radioButtonWhitePlayer.Location = new System.Drawing.Point(449, 115);
			this.radioButtonWhitePlayer.Name = "radioButtonWhitePlayer";
			this.radioButtonWhitePlayer.Size = new System.Drawing.Size(53, 17);
			this.radioButtonWhitePlayer.TabIndex = 24;
			this.radioButtonWhitePlayer.TabStop = true;
			this.radioButtonWhitePlayer.Text = "White";
			this.radioButtonWhitePlayer.UseVisualStyleBackColor = true;
			this.radioButtonWhitePlayer.CheckedChanged += new System.EventHandler(this.StateChanged);
			// 
			// checkBoxCastlingWK
			// 
			this.checkBoxCastlingWK.AutoSize = true;
			this.checkBoxCastlingWK.Location = new System.Drawing.Point(574, 185);
			this.checkBoxCastlingWK.Name = "checkBoxCastlingWK";
			this.checkBoxCastlingWK.Size = new System.Drawing.Size(97, 17);
			this.checkBoxCastlingWK.TabIndex = 29;
			this.checkBoxCastlingWK.Text = "White Kingside";
			this.checkBoxCastlingWK.UseVisualStyleBackColor = true;
			this.checkBoxCastlingWK.CheckedChanged += new System.EventHandler(this.StateChanged);
			// 
			// checkBoxCastlingBK
			// 
			this.checkBoxCastlingBK.AutoSize = true;
			this.checkBoxCastlingBK.Location = new System.Drawing.Point(574, 139);
			this.checkBoxCastlingBK.Name = "checkBoxCastlingBK";
			this.checkBoxCastlingBK.Size = new System.Drawing.Size(96, 17);
			this.checkBoxCastlingBK.TabIndex = 28;
			this.checkBoxCastlingBK.Text = "Black Kingside";
			this.checkBoxCastlingBK.UseVisualStyleBackColor = true;
			this.checkBoxCastlingBK.CheckedChanged += new System.EventHandler(this.StateChanged);
			// 
			// checkBoxCastlingWQ
			// 
			this.checkBoxCastlingWQ.AutoSize = true;
			this.checkBoxCastlingWQ.Location = new System.Drawing.Point(574, 162);
			this.checkBoxCastlingWQ.Name = "checkBoxCastlingWQ";
			this.checkBoxCastlingWQ.Size = new System.Drawing.Size(108, 17);
			this.checkBoxCastlingWQ.TabIndex = 27;
			this.checkBoxCastlingWQ.Text = "White Queenside";
			this.checkBoxCastlingWQ.UseVisualStyleBackColor = true;
			this.checkBoxCastlingWQ.CheckedChanged += new System.EventHandler(this.StateChanged);
			// 
			// checkBoxCastlingBQ
			// 
			this.checkBoxCastlingBQ.AutoSize = true;
			this.checkBoxCastlingBQ.Location = new System.Drawing.Point(574, 116);
			this.checkBoxCastlingBQ.Name = "checkBoxCastlingBQ";
			this.checkBoxCastlingBQ.Size = new System.Drawing.Size(107, 17);
			this.checkBoxCastlingBQ.TabIndex = 26;
			this.checkBoxCastlingBQ.Text = "Black Queenside";
			this.checkBoxCastlingBQ.UseVisualStyleBackColor = true;
			this.checkBoxCastlingBQ.CheckedChanged += new System.EventHandler(this.StateChanged);
			// 
			// textBoxBits0
			// 
			this.textBoxBits0.Location = new System.Drawing.Point(470, 212);
			this.textBoxBits0.Name = "textBoxBits0";
			this.textBoxBits0.ReadOnly = true;
			this.textBoxBits0.Size = new System.Drawing.Size(228, 20);
			this.textBoxBits0.TabIndex = 30;
			this.textBoxBits0.Text = "0";
			this.textBoxBits0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textBoxBits1
			// 
			this.textBoxBits1.Location = new System.Drawing.Point(470, 238);
			this.textBoxBits1.Name = "textBoxBits1";
			this.textBoxBits1.ReadOnly = true;
			this.textBoxBits1.Size = new System.Drawing.Size(228, 20);
			this.textBoxBits1.TabIndex = 31;
			this.textBoxBits1.Text = "0";
			this.textBoxBits1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textBoxBits3
			// 
			this.textBoxBits3.Location = new System.Drawing.Point(470, 290);
			this.textBoxBits3.Name = "textBoxBits3";
			this.textBoxBits3.ReadOnly = true;
			this.textBoxBits3.Size = new System.Drawing.Size(228, 20);
			this.textBoxBits3.TabIndex = 33;
			this.textBoxBits3.Text = "0";
			this.textBoxBits3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textBoxBits2
			// 
			this.textBoxBits2.Location = new System.Drawing.Point(470, 264);
			this.textBoxBits2.Name = "textBoxBits2";
			this.textBoxBits2.ReadOnly = true;
			this.textBoxBits2.Size = new System.Drawing.Size(228, 20);
			this.textBoxBits2.TabIndex = 32;
			this.textBoxBits2.Text = "0";
			this.textBoxBits2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textBoxBits7
			// 
			this.textBoxBits7.Location = new System.Drawing.Point(470, 394);
			this.textBoxBits7.Name = "textBoxBits7";
			this.textBoxBits7.ReadOnly = true;
			this.textBoxBits7.Size = new System.Drawing.Size(228, 20);
			this.textBoxBits7.TabIndex = 37;
			this.textBoxBits7.Text = "0";
			this.textBoxBits7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textBoxBits6
			// 
			this.textBoxBits6.Location = new System.Drawing.Point(470, 368);
			this.textBoxBits6.Name = "textBoxBits6";
			this.textBoxBits6.ReadOnly = true;
			this.textBoxBits6.Size = new System.Drawing.Size(228, 20);
			this.textBoxBits6.TabIndex = 36;
			this.textBoxBits6.Text = "0";
			this.textBoxBits6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textBoxBits5
			// 
			this.textBoxBits5.Location = new System.Drawing.Point(470, 342);
			this.textBoxBits5.Name = "textBoxBits5";
			this.textBoxBits5.ReadOnly = true;
			this.textBoxBits5.Size = new System.Drawing.Size(228, 20);
			this.textBoxBits5.TabIndex = 35;
			this.textBoxBits5.Text = "0";
			this.textBoxBits5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textBoxBits4
			// 
			this.textBoxBits4.Location = new System.Drawing.Point(470, 316);
			this.textBoxBits4.Name = "textBoxBits4";
			this.textBoxBits4.ReadOnly = true;
			this.textBoxBits4.Size = new System.Drawing.Size(228, 20);
			this.textBoxBits4.TabIndex = 34;
			this.textBoxBits4.Text = "0";
			this.textBoxBits4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(446, 215);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(18, 13);
			this.label2.TabIndex = 38;
			this.label2.Text = "W";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(446, 241);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(14, 13);
			this.label3.TabIndex = 39;
			this.label3.Text = "B";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(446, 267);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(14, 13);
			this.label5.TabIndex = 40;
			this.label5.Text = "P";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(446, 293);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(15, 13);
			this.label6.TabIndex = 41;
			this.label6.Text = "N";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(446, 319);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(14, 13);
			this.label8.TabIndex = 42;
			this.label8.Text = "B";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(446, 345);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(15, 13);
			this.label9.TabIndex = 43;
			this.label9.Text = "R";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(446, 371);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(15, 13);
			this.label10.TabIndex = 44;
			this.label10.Text = "Q";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(446, 397);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(14, 13);
			this.label11.TabIndex = 45;
			this.label11.Text = "K";
			// 
			// boardControl1
			// 
			this.boardControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.boardControl1.LabeledTiles = false;
			this.boardControl1.Location = new System.Drawing.Point(12, 12);
			this.boardControl1.Name = "boardControl1";
			this.boardControl1.SelectedTile = 0;
			this.boardControl1.Size = new System.Drawing.Size(402, 402);
			this.boardControl1.State = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
			this.boardControl1.TabIndex = 0;
			this.boardControl1.Click += new System.EventHandler(this.boardControl1_Click);
			// 
			// BoardControlPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBoxBits7);
			this.Controls.Add(this.textBoxBits6);
			this.Controls.Add(this.textBoxBits5);
			this.Controls.Add(this.textBoxBits4);
			this.Controls.Add(this.textBoxBits3);
			this.Controls.Add(this.textBoxBits2);
			this.Controls.Add(this.textBoxBits1);
			this.Controls.Add(this.textBoxBits0);
			this.Controls.Add(this.checkBoxCastlingWK);
			this.Controls.Add(this.checkBoxCastlingBK);
			this.Controls.Add(this.checkBoxCastlingWQ);
			this.Controls.Add(this.checkBoxCastlingBQ);
			this.Controls.Add(this.radioButtonBlackPlayer);
			this.Controls.Add(this.radioButtonWhitePlayer);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.buttonInit);
			this.Controls.Add(this.buttonClear);
			this.Controls.Add(this.buttonLoadFEN);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBoxFEN);
			this.Controls.Add(this.boardControl1);
			this.Name = "BoardControlPanel";
			this.Size = new System.Drawing.Size(713, 428);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxFEN;
		private System.Windows.Forms.Button buttonLoadFEN;
		private System.Windows.Forms.Button buttonClear;
		private System.Windows.Forms.Button buttonInit;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.RadioButton radioButtonBlackPlayer;
		private System.Windows.Forms.RadioButton radioButtonWhitePlayer;
		private System.Windows.Forms.CheckBox checkBoxCastlingWK;
		private System.Windows.Forms.CheckBox checkBoxCastlingBK;
		private System.Windows.Forms.CheckBox checkBoxCastlingWQ;
		private System.Windows.Forms.CheckBox checkBoxCastlingBQ;
		private System.Windows.Forms.TextBox textBoxBits0;
		private System.Windows.Forms.TextBox textBoxBits1;
		private System.Windows.Forms.TextBox textBoxBits3;
		private System.Windows.Forms.TextBox textBoxBits2;
		private System.Windows.Forms.TextBox textBoxBits7;
		private System.Windows.Forms.TextBox textBoxBits6;
		private System.Windows.Forms.TextBox textBoxBits5;
		private System.Windows.Forms.TextBox textBoxBits4;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		public BoardControl boardControl1;
	}
}
