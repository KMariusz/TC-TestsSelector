namespace TestsSelector
{
  partial class Edit
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Edit));
      this.Value_TextBox = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.Value_CheckBox = new System.Windows.Forms.CheckBox();
      this.OK_Button = new System.Windows.Forms.Button();
      this.Value_UpDown = new System.Windows.Forms.NumericUpDown();
      this.Columns_UpDown = new System.Windows.Forms.NumericUpDown();
      this.label8 = new System.Windows.Forms.Label();
      this.Rows_UpDown = new System.Windows.Forms.NumericUpDown();
      this.Insert_Button = new System.Windows.Forms.Button();
      this.Delete_Button = new System.Windows.Forms.Button();
      this.Table_GridView = new System.Windows.Forms.DataGridView();
      this.Cancel_Button = new System.Windows.Forms.Button();
      this.CheckLVL_TextBox = new System.Windows.Forms.TextBox();
      ((System.ComponentModel.ISupportInitialize)(this.Value_UpDown)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.Columns_UpDown)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.Rows_UpDown)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.Table_GridView)).BeginInit();
      this.SuspendLayout();
      // 
      // Value_TextBox
      // 
      this.Value_TextBox.Location = new System.Drawing.Point(13, 111);
      this.Value_TextBox.Name = "Value_TextBox";
      this.Value_TextBox.Size = new System.Drawing.Size(260, 20);
      this.Value_TextBox.TabIndex = 0;
      this.Value_TextBox.TextChanged += new System.EventHandler(this.Value_TextBox_TextChanged);
      this.Value_TextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Edit_KeyUp);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(89, 13);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(35, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "label1";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(89, 36);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(35, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "label2";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(13, 13);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(41, 13);
      this.label3.TabIndex = 3;
      this.label3.Text = "Name: ";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(13, 36);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(37, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "Value:";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(13, 95);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(61, 13);
      this.label5.TabIndex = 5;
      this.label5.Text = "New value:";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(13, 59);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(54, 13);
      this.label6.TabIndex = 6;
      this.label6.Text = "Comment:";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(89, 59);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(35, 13);
      this.label7.TabIndex = 7;
      this.label7.Text = "label7";
      // 
      // Value_CheckBox
      // 
      this.Value_CheckBox.AutoSize = true;
      this.Value_CheckBox.Location = new System.Drawing.Point(13, 112);
      this.Value_CheckBox.Name = "Value_CheckBox";
      this.Value_CheckBox.Size = new System.Drawing.Size(84, 17);
      this.Value_CheckBox.TabIndex = 8;
      this.Value_CheckBox.Text = "True / False";
      this.Value_CheckBox.UseVisualStyleBackColor = true;
      this.Value_CheckBox.CheckedChanged += new System.EventHandler(this.Value_CheckBox_CheckedChanged);
      this.Value_CheckBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Edit_KeyUp);
      // 
      // OK_Button
      // 
      this.OK_Button.Location = new System.Drawing.Point(234, 138);
      this.OK_Button.Name = "OK_Button";
      this.OK_Button.Size = new System.Drawing.Size(37, 23);
      this.OK_Button.TabIndex = 9;
      this.OK_Button.Text = "OK";
      this.OK_Button.UseVisualStyleBackColor = true;
      this.OK_Button.Click += new System.EventHandler(this.OK_Button_Click);
      this.OK_Button.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Edit_KeyUp);
      // 
      // Value_UpDown
      // 
      this.Value_UpDown.Location = new System.Drawing.Point(16, 111);
      this.Value_UpDown.Name = "Value_UpDown";
      this.Value_UpDown.Size = new System.Drawing.Size(120, 20);
      this.Value_UpDown.TabIndex = 10;
      this.Value_UpDown.ValueChanged += new System.EventHandler(this.Value_UpDown_ValueChanged);
      this.Value_UpDown.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Edit_KeyUp);
      // 
      // Columns_UpDown
      // 
      this.Columns_UpDown.Location = new System.Drawing.Point(63, 10);
      this.Columns_UpDown.Name = "Columns_UpDown";
      this.Columns_UpDown.Size = new System.Drawing.Size(44, 20);
      this.Columns_UpDown.TabIndex = 11;
      this.Columns_UpDown.ValueChanged += new System.EventHandler(this.Columns_UpDown_ValueChanged);
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(110, 13);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(37, 13);
      this.label8.TabIndex = 12;
      this.label8.Text = "Rows:";
      // 
      // Rows_UpDown
      // 
      this.Rows_UpDown.Location = new System.Drawing.Point(151, 10);
      this.Rows_UpDown.Name = "Rows_UpDown";
      this.Rows_UpDown.Size = new System.Drawing.Size(42, 20);
      this.Rows_UpDown.TabIndex = 13;
      this.Rows_UpDown.ValueChanged += new System.EventHandler(this.Rows_UpDown_ValueChanged);
      // 
      // Insert_Button
      // 
      this.Insert_Button.Location = new System.Drawing.Point(199, 8);
      this.Insert_Button.Name = "Insert_Button";
      this.Insert_Button.Size = new System.Drawing.Size(75, 23);
      this.Insert_Button.TabIndex = 14;
      this.Insert_Button.Text = "Insert Row";
      this.Insert_Button.UseVisualStyleBackColor = true;
      this.Insert_Button.Click += new System.EventHandler(this.Insert_Button_Click);
      // 
      // Delete_Button
      // 
      this.Delete_Button.Location = new System.Drawing.Point(199, 38);
      this.Delete_Button.Name = "Delete_Button";
      this.Delete_Button.Size = new System.Drawing.Size(75, 23);
      this.Delete_Button.TabIndex = 15;
      this.Delete_Button.Text = "Delete Row";
      this.Delete_Button.UseVisualStyleBackColor = true;
      this.Delete_Button.Click += new System.EventHandler(this.Delete_Button_Click);
      // 
      // Table_GridView
      // 
      this.Table_GridView.BackgroundColor = System.Drawing.SystemColors.Control;
      this.Table_GridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.Table_GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.Table_GridView.Location = new System.Drawing.Point(16, 36);
      this.Table_GridView.Name = "Table_GridView";
      this.Table_GridView.Size = new System.Drawing.Size(67, 43);
      this.Table_GridView.TabIndex = 16;
      this.Table_GridView.CurrentCellChanged += new System.EventHandler(this.Table_GridView_CurrentCellChanged);
      this.Table_GridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.Table_GridView_DataBindingComplete);
      // 
      // Cancel_Button
      // 
      this.Cancel_Button.Location = new System.Drawing.Point(151, 138);
      this.Cancel_Button.Name = "Cancel_Button";
      this.Cancel_Button.Size = new System.Drawing.Size(75, 23);
      this.Cancel_Button.TabIndex = 17;
      this.Cancel_Button.Text = "Cancel";
      this.Cancel_Button.UseVisualStyleBackColor = true;
      this.Cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
      this.Cancel_Button.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Edit_KeyUp);
      // 
      // CheckLVL_TextBox
      // 
      this.CheckLVL_TextBox.Location = new System.Drawing.Point(13, 141);
      this.CheckLVL_TextBox.Name = "CheckLVL_TextBox";
      this.CheckLVL_TextBox.Size = new System.Drawing.Size(100, 20);
      this.CheckLVL_TextBox.TabIndex = 18;
      this.CheckLVL_TextBox.TextChanged += new System.EventHandler(this.CheckLVL_TextBox_TextChanged);
      // 
      // Edit
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(284, 173);
      this.Controls.Add(this.CheckLVL_TextBox);
      this.Controls.Add(this.Cancel_Button);
      this.Controls.Add(this.Table_GridView);
      this.Controls.Add(this.Delete_Button);
      this.Controls.Add(this.Insert_Button);
      this.Controls.Add(this.Rows_UpDown);
      this.Controls.Add(this.label8);
      this.Controls.Add(this.Columns_UpDown);
      this.Controls.Add(this.OK_Button);
      this.Controls.Add(this.label7);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.Value_CheckBox);
      this.Controls.Add(this.Value_UpDown);
      this.Controls.Add(this.Value_TextBox);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "Edit";
      this.Text = "Edit";
      this.Load += new System.EventHandler(this.Edit_Load);
      this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Edit_KeyUp);
      this.Resize += new System.EventHandler(this.Edit_Resize);
      ((System.ComponentModel.ISupportInitialize)(this.Value_UpDown)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.Columns_UpDown)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.Rows_UpDown)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.Table_GridView)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox Value_TextBox;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.CheckBox Value_CheckBox;
    private System.Windows.Forms.Button OK_Button;
    private System.Windows.Forms.NumericUpDown Value_UpDown;
    private System.Windows.Forms.NumericUpDown Columns_UpDown;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.NumericUpDown Rows_UpDown;
    private System.Windows.Forms.Button Insert_Button;
    private System.Windows.Forms.Button Delete_Button;
    private System.Windows.Forms.DataGridView Table_GridView;
    private System.Windows.Forms.Button Cancel_Button;
    private System.Windows.Forms.TextBox CheckLVL_TextBox;
  }
}