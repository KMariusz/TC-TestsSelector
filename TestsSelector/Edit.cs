using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace TestsSelector
{
  public partial class Edit : Form
  {
    public TreeNode _node;
    public string _newValue;
    public string _checkLevel;
    public string _comment;
    public string _name;
    private bool _booleanMode = false;
    public bool _modeMDS = false;
    public bool _editTable = false;
    public ProjectVariable _item = null;
    public bool _gridLoaded = false;
    public DataTable table = null;
    private DataGridViewCell _lastSelected = null;
    public bool _modified = false;
    private int _res_W = 0;
    

    public Edit()
    {
      InitializeComponent();
    }

    private void Edit_Load(object sender, EventArgs e)
    {
      InitialState();
    }

    private void InitialState()
    {
      CheckLVL_TextBox.Visible = false;
      if (_editTable)
      {
        label1.Visible = false;
        label2.Visible = false;
        label3.Text = "Columns:";
        label8.Text = "Rows:";
        label4.Visible = false;
        label5.Visible = false;
        label6.Visible = false;
        label7.Visible = false;
        Value_CheckBox.Visible = false;
        Value_TextBox.Visible = false;
        Value_UpDown.Visible = false;
        this.Size = new Size(this.Size.Width * 2, this.Size.Height * 2);
        this.MinimumSize = this.Size;
        this.FormBorderStyle = FormBorderStyle.Sizable;
        Delete_Button.Location = new Point(Insert_Button.Location.X + 80, Insert_Button.Location.Y);
        Columns_UpDown.Value = _item.Table_Columns.Count;
        Rows_UpDown.Value = _item.Table_Rows.Count;
        Table_GridView.AutoGenerateColumns = true;
        Table_GridView.AllowUserToAddRows = false;
        Table_GridView.AllowUserToDeleteRows = false;
        Table_GridView.AllowUserToOrderColumns = false;
        Table_GridView.AllowUserToResizeRows = false;
        Table_GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        Prepare_Table();
      }
      else
      {
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        label8.Visible = false;
        Columns_UpDown.Visible = false;
        Rows_UpDown.Visible = false;
        Delete_Button.Visible = false;
        Insert_Button.Visible = false;
        Table_GridView.Visible = false;
        Cancel_Button.Visible = false;

        if (_modeMDS)
        {
          _newValue = _node.ToolTipText;
          _name = _node.Name;

          //Init value
          label1.Text = _node.Name;
          label4.Text = "Iterations:";
          label2.Text = _node.ToolTipText;

          //Disable some elements
          label6.Visible = false;
          label7.Visible = false;
          Value_CheckBox.Visible = false;
          Value_TextBox.Visible = false;

          //Numeric UpDown
          Value_UpDown.Value = Decimal.Parse(_newValue);
          Value_UpDown.Minimum = 1;
          Value_UpDown.Maximum = 2147483647;
        }
        else
        {
          //GlobalConfig
          var _xmlTag = (XmlNode)_node.Tag;
          _name = _xmlTag.Name;
          _checkLevel = _xmlTag.Attributes["CheckLevel"] == null ? null : _xmlTag.Attributes["CheckLevel"].Value;
          _newValue = _xmlTag.Attributes["Value"].Value;
          _comment = _xmlTag.Attributes["Comment"].Value;

          //View
          if (_comment.Length > 30) {
            _res_W = (_comment.Length - 30) * 5;
          }
          this.Size = new Size(this.Size.Width + _res_W, this.Size.Height + 50);
          label1.Text = _name;
          label2.Visible = false;
          label4.Visible = false;
          label7.Location = label2.Location;
          label6.Location = label4.Location;
          label5.Text = "Value:";
          Value_UpDown.Visible = false;
          label7.Text = _comment;
          label5.Location = new Point(label5.Location.X, label5.Location.Y - 20);
          Value_TextBox.Location = new Point(Value_TextBox.Location.X, Value_TextBox.Location.Y - 20);
          Value_CheckBox.Location = new Point(Value_TextBox.Location.X, Value_TextBox.Location.Y);          

          //Support for previous GlobalConfig file without CheckLevel
          if (_checkLevel == null) {
            CheckLVL_TextBox.Visible = false;
            label8.Visible = false;
            this.Size = new Size(this.Size.Width + _res_W, this.Size.Height - 50);
          } else {
            label8.Visible = true;
            label8.Text = "CheckLevel:";
            label8.Location = new Point(label5.Location.X, label5.Location.Y + 50);
            CheckLVL_TextBox.Visible = true;
            CheckLVL_TextBox.Size = Value_TextBox.Size;
            CheckLVL_TextBox.Location = new Point(Value_CheckBox.Location.X, Value_CheckBox.Location.Y + 50);
            CheckLVL_TextBox.Text = _checkLevel;
          }

          //TextBox/CheckBox init value
          _booleanMode = _newValue == "true" || _newValue == "false";
          var test = _comment.Length;
          Value_TextBox.Text = _newValue;
          if (_booleanMode) {
            Value_CheckBox.Checked = _node.Name == "true";
          }
          Value_CheckBox.Visible = _booleanMode;
          Value_TextBox.Visible = !_booleanMode;
        }
      }
    }

    private void Value_TextBox_TextChanged(object sender, EventArgs e)
    {
      _newValue = Value_TextBox.Text;
      _modified = true;
    }

    private void Value_CheckBox_CheckedChanged(object sender, EventArgs e)
    {
      _newValue = Value_CheckBox.Checked ? "true" : "false";
      _modified = true;
    }

    private void CheckLVL_TextBox_TextChanged(object sender, EventArgs e)
    {
      _checkLevel = CheckLVL_TextBox.Text;
      _modified = true;
    }

    private void Edit_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Return)
      {
        Close_WithOK();
      }
      else if (e.KeyCode == Keys.Escape)
      {
        Close_WithCancel();
      }
    }

    private void Prepare_Table()
    {
      table = new DataTable();
      var _columnCount = _item.Table_Columns.Count;
      var _rowsCount = _item.Table_Rows.Count;

      for (var x = 0; x < _columnCount; x++)
      {
        table.Columns.Add(_item.Table_Columns[x] + " [" + x.ToString() + "]");
      }

      for (var x = 0; x < _rowsCount; x++)
      {
        var _row = table.Rows.Add(_item.Table_Rows[x].ToArray());
      }

      //table.Rows.Add()

      Table_GridView.DataSource = table;
    }

    private void Close_WithOK()
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void Close_WithCancel()
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void OK_Button_Click(object sender, EventArgs e)
    {
      if (_editTable)
      {
        if (Columns_UpDown.Value == 0 && Rows_UpDown.Value != 0)
        {
          MessageBox.Show("The table contains no columns. Please add at leas one column or cancel editing the variable.");
        }
        else if (Columns_UpDown.Value != 0 && Rows_UpDown.Value == 0)
        {
          MessageBox.Show("The table contains no rows. Please add at leas one row or cancel editing the variable.");
        }
        else
        {
          Close_WithOK();
        }
      }
      else
      {
        Close_WithOK();
      }
    }

    private void Value_UpDown_ValueChanged(object sender, EventArgs e)
    {
      _newValue = Decimal.ToInt32(Value_UpDown.Value).ToString();
      _node.ToolTipText = _newValue;
      _modified = true;
    }

    private void Edit_Resize(object sender, EventArgs e)
    {
      OK_Button.Location = new Point(this.Size.Width - 140, this.Size.Height - 70);
      Cancel_Button.Location = new Point(this.Size.Width - 98, this.Size.Height - 70);
      Table_GridView.Size = new Size(this.Size.Width - 50, this.Size.Height - 110);
    }

    private void Columns_UpDown_ValueChanged(object sender, EventArgs e)
    {
      if (_gridLoaded)
      {
        if (Columns_UpDown.Value > table.Columns.Count)
        {
          table.Columns.Add("[" + table.Columns.Count.ToString() + "]");
        }
        if (Columns_UpDown.Value < table.Columns.Count)
        {
          table.Columns.RemoveAt(table.Columns.Count - 1);
        }
      }
      Columns_UpDown.Focus();
      _modified = true;
    }

    private void Rows_UpDown_ValueChanged(object sender, EventArgs e)
    {
      if (_gridLoaded)
      {
        if (Rows_UpDown.Value > table.Rows.Count)
        {
          table.Rows.Add();
        }
        if (Rows_UpDown.Value < table.Rows.Count)
        {
          table.Rows.RemoveAt(table.Rows.Count - 1);
        }
      }
      Rows_UpDown.Focus();
      _modified = true;
    }

    private void Insert_Button_Click(object sender, EventArgs e)
    {
      var _row = table.Rows.Add(); //create by add
      table.Rows[table.Rows.Count - 1].Delete(); //remove added
      if (_lastSelected != null && _lastSelected.RowIndex > -1)
      {
        table.Rows.InsertAt(_row, _lastSelected.RowIndex); //add empty row at selected index
      }
      else
      {
        table.Rows.InsertAt(_row, 0); //add empty row at selected index
      }
      Rows_UpDown.Value = table.Rows.Count;
      _modified = true;
    }

    private void Delete_Button_Click(object sender, EventArgs e)
    {
      if (_lastSelected != null && _lastSelected.RowIndex > -1)
      {
        table.Rows.RemoveAt(_lastSelected.RowIndex);
      }
      else if (Rows_UpDown.Value > 0)
      {
        table.Rows.RemoveAt(0);
      }
      Rows_UpDown.Value = table.Rows.Count;
      _modified = true;
    }

    private void Table_GridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
      FillRecordNo();
      _gridLoaded = true;
    }

    private void FillRecordNo()
    {
      for (int i = 0; i < this.Table_GridView.Rows.Count; i++)
      {
        this.Table_GridView.Rows[i].HeaderCell.Value = (i).ToString();
      }
    }

    private void Table_GridView_CurrentCellChanged(object sender, EventArgs e)
    {
      if (Table_GridView.CurrentCell != null)
      {
        _lastSelected = Table_GridView.CurrentCell;
      }
    }

    private void Cancel_Button_Click(object sender, EventArgs e)
    {
      Close_WithCancel();
    }
  }
}