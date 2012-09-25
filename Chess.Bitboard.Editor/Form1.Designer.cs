namespace Chess.Bitboard.Editor
{
	partial class Form1
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
			this.labelBitsUpper = new System.Windows.Forms.Label();
			this.labelBitsLower = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textBoxHex = new System.Windows.Forms.TextBox();
			this.textBoxDec = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.buttonClear = new System.Windows.Forms.Button();
			this.buttonInvert = new System.Windows.Forms.Button();
			this.buttonPrev = new System.Windows.Forms.Button();
			this.buttonNext = new System.Windows.Forms.Button();
			this.textBoxPos = new System.Windows.Forms.TextBox();
			this.buttonShiftUp = new System.Windows.Forms.Button();
			this.buttonShiftDown = new System.Windows.Forms.Button();
			this.buttonShiftRight = new System.Windows.Forms.Button();
			this.buttonShiftLeft = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.textBoxList = new System.Windows.Forms.TextBox();
			this.boardControl1 = new Chess.Bitboard.Editor.BoardControl();
			this.SuspendLayout();
			// 
			// labelBitsUpper
			// 
			this.labelBitsUpper.AutoSize = true;
			this.labelBitsUpper.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBitsUpper.Location = new System.Drawing.Point(446, 44);
			this.labelBitsUpper.Name = "labelBitsUpper";
			this.labelBitsUpper.Size = new System.Drawing.Size(252, 14);
			this.labelBitsUpper.TabIndex = 1;
			this.labelBitsUpper.Text = "00000000.00000000.00000000.00000000";
			// 
			// labelBitsLower
			// 
			this.labelBitsLower.AutoSize = true;
			this.labelBitsLower.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBitsLower.Location = new System.Drawing.Point(446, 84);
			this.labelBitsLower.Name = "labelBitsLower";
			this.labelBitsLower.Size = new System.Drawing.Size(252, 14);
			this.labelBitsLower.TabIndex = 2;
			this.labelBitsLower.Text = "00000000.00000000.00000000.00000000";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(446, 70);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(252, 14);
			this.label4.TabIndex = 4;
			this.label4.Text = "      24       16        8        0";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(446, 30);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(252, 14);
			this.label3.TabIndex = 5;
			this.label3.Text = "      56       48       40       32";
			// 
			// textBoxHex
			// 
			this.textBoxHex.Location = new System.Drawing.Point(449, 146);
			this.textBoxHex.Name = "textBoxHex";
			this.textBoxHex.Size = new System.Drawing.Size(249, 20);
			this.textBoxHex.TabIndex = 6;
			this.textBoxHex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textBoxHex.TextChanged += new System.EventHandler(this.textBoxHex_TextChanged);
			// 
			// textBoxDec
			// 
			this.textBoxDec.Location = new System.Drawing.Point(449, 196);
			this.textBoxDec.Name = "textBoxDec";
			this.textBoxDec.Size = new System.Drawing.Size(249, 20);
			this.textBoxDec.TabIndex = 7;
			this.textBoxDec.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textBoxDec.TextChanged += new System.EventHandler(this.textBoxDec_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(630, 130);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(68, 13);
			this.label1.TabIndex = 8;
			this.label1.Text = "Hexadecimal";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(653, 180);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(45, 13);
			this.label2.TabIndex = 9;
			this.label2.Text = "Decimal";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// buttonClear
			// 
			this.buttonClear.Location = new System.Drawing.Point(449, 297);
			this.buttonClear.Name = "buttonClear";
			this.buttonClear.Size = new System.Drawing.Size(52, 23);
			this.buttonClear.TabIndex = 10;
			this.buttonClear.Text = "Clear";
			this.buttonClear.UseVisualStyleBackColor = true;
			this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
			// 
			// buttonInvert
			// 
			this.buttonInvert.Location = new System.Drawing.Point(507, 297);
			this.buttonInvert.Name = "buttonInvert";
			this.buttonInvert.Size = new System.Drawing.Size(52, 23);
			this.buttonInvert.TabIndex = 11;
			this.buttonInvert.Text = "Invert";
			this.buttonInvert.UseVisualStyleBackColor = true;
			this.buttonInvert.Click += new System.EventHandler(this.buttonInvert_Click);
			// 
			// buttonPrev
			// 
			this.buttonPrev.Location = new System.Drawing.Point(572, 297);
			this.buttonPrev.Name = "buttonPrev";
			this.buttonPrev.Size = new System.Drawing.Size(36, 23);
			this.buttonPrev.TabIndex = 12;
			this.buttonPrev.Text = "<";
			this.buttonPrev.UseVisualStyleBackColor = true;
			this.buttonPrev.Click += new System.EventHandler(this.buttonPrev_Click);
			// 
			// buttonNext
			// 
			this.buttonNext.Location = new System.Drawing.Point(662, 297);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.Size = new System.Drawing.Size(36, 23);
			this.buttonNext.TabIndex = 13;
			this.buttonNext.Text = ">";
			this.buttonNext.UseVisualStyleBackColor = true;
			this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
			// 
			// textBoxPos
			// 
			this.textBoxPos.Location = new System.Drawing.Point(614, 299);
			this.textBoxPos.Name = "textBoxPos";
			this.textBoxPos.Size = new System.Drawing.Size(42, 20);
			this.textBoxPos.TabIndex = 14;
			this.textBoxPos.Text = "0";
			this.textBoxPos.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// buttonShiftUp
			// 
			this.buttonShiftUp.Location = new System.Drawing.Point(487, 342);
			this.buttonShiftUp.Name = "buttonShiftUp";
			this.buttonShiftUp.Size = new System.Drawing.Size(34, 27);
			this.buttonShiftUp.TabIndex = 15;
			this.buttonShiftUp.Text = "˄";
			this.buttonShiftUp.UseVisualStyleBackColor = true;
			this.buttonShiftUp.Click += new System.EventHandler(this.buttonShiftUp_Click);
			// 
			// buttonShiftDown
			// 
			this.buttonShiftDown.Location = new System.Drawing.Point(487, 375);
			this.buttonShiftDown.Name = "buttonShiftDown";
			this.buttonShiftDown.Size = new System.Drawing.Size(34, 27);
			this.buttonShiftDown.TabIndex = 16;
			this.buttonShiftDown.Text = "˅";
			this.buttonShiftDown.UseVisualStyleBackColor = true;
			this.buttonShiftDown.Click += new System.EventHandler(this.buttonShiftDown_Click);
			// 
			// buttonShiftRight
			// 
			this.buttonShiftRight.Location = new System.Drawing.Point(525, 358);
			this.buttonShiftRight.Name = "buttonShiftRight";
			this.buttonShiftRight.Size = new System.Drawing.Size(34, 27);
			this.buttonShiftRight.TabIndex = 17;
			this.buttonShiftRight.Text = "˃";
			this.buttonShiftRight.UseVisualStyleBackColor = true;
			this.buttonShiftRight.Click += new System.EventHandler(this.buttonShiftRight_Click);
			// 
			// buttonShiftLeft
			// 
			this.buttonShiftLeft.Location = new System.Drawing.Point(449, 358);
			this.buttonShiftLeft.Name = "buttonShiftLeft";
			this.buttonShiftLeft.Size = new System.Drawing.Size(34, 27);
			this.buttonShiftLeft.TabIndex = 18;
			this.buttonShiftLeft.Text = "˂";
			this.buttonShiftLeft.UseVisualStyleBackColor = true;
			this.buttonShiftLeft.Click += new System.EventHandler(this.buttonShiftLeft_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(675, 230);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(23, 13);
			this.label5.TabIndex = 20;
			this.label5.Text = "List";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textBoxList
			// 
			this.textBoxList.Location = new System.Drawing.Point(449, 246);
			this.textBoxList.Name = "textBoxList";
			this.textBoxList.Size = new System.Drawing.Size(249, 20);
			this.textBoxList.TabIndex = 19;
			this.textBoxList.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// boardControl1
			// 
			this.boardControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.boardControl1.Location = new System.Drawing.Point(12, 12);
			this.boardControl1.Name = "boardControl1";
			this.boardControl1.Size = new System.Drawing.Size(402, 402);
			this.boardControl1.State = ((ulong)(0ul));
			this.boardControl1.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(713, 428);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textBoxList);
			this.Controls.Add(this.buttonShiftLeft);
			this.Controls.Add(this.buttonShiftRight);
			this.Controls.Add(this.buttonShiftDown);
			this.Controls.Add(this.buttonShiftUp);
			this.Controls.Add(this.textBoxPos);
			this.Controls.Add(this.buttonNext);
			this.Controls.Add(this.buttonPrev);
			this.Controls.Add(this.buttonInvert);
			this.Controls.Add(this.buttonClear);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBoxDec);
			this.Controls.Add(this.textBoxHex);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.labelBitsLower);
			this.Controls.Add(this.labelBitsUpper);
			this.Controls.Add(this.boardControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "Form1";
			this.Text = "Bitboard Editor";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private BoardControl boardControl1;
		private System.Windows.Forms.Label labelBitsUpper;
		private System.Windows.Forms.Label labelBitsLower;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxHex;
		private System.Windows.Forms.TextBox textBoxDec;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonClear;
		private System.Windows.Forms.Button buttonInvert;
		private System.Windows.Forms.Button buttonPrev;
		private System.Windows.Forms.Button buttonNext;
		private System.Windows.Forms.TextBox textBoxPos;
		private System.Windows.Forms.Button buttonShiftUp;
		private System.Windows.Forms.Button buttonShiftDown;
		private System.Windows.Forms.Button buttonShiftRight;
		private System.Windows.Forms.Button buttonShiftLeft;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBoxList;

	}
}

