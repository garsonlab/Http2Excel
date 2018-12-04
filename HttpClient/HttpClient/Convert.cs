using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TinyJSON;

namespace HttpClient
{
    public partial class Convert : Form
    {
        private Node roleInfo;
        public Convert()
        {
            InitializeComponent();
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            textBox1.ReadOnly = true;
        }

        public void FillData(string name, Node node)
        {
            roleInfo = node;
            textBox1.Text = name;

            DataTable dt = new DataTable();//建立个数据表
            dt.Columns.Add(new DataColumn("字段名", typeof(string)));
            dt.Columns.Add(new DataColumn("值", typeof(string)));

            Dictionary<string, Node> dics = (Dictionary<string, Node>) node;
            foreach (var dic in dics)
            {
                DataRow dr = dt.NewRow();
                dr["字段名"] = dic.Key.ToString();
                dr["值"] = dic.Value.ToString();
                dt.Rows.Add(dr);
            }
            gridView.DataSource = dt;
        }


        private void btnFill_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Excel文件|*.xlsx|Excel文件|*.xls";
            dialog.Multiselect = false;
            dialog.Title = "选择导出信息模版";
            dialog.InitialDirectory = Directory.GetCurrentDirectory() + "../";

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string path = dialog.FileName;

            Workbook book = new Workbook(path);
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = book;

            Dictionary<string, Node> dics = (Dictionary<string, Node>)roleInfo;
            foreach (var node in dics)
            {
                string key = node.Key;
                string value = node.Value.ToString();
                designer.SetDataSource(key, value);
            }
            designer.Process();

            string fileName = Path.GetFileNameWithoutExtension(path);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel文件|*.xlsx|Excel文件|*.xls";
            saveFileDialog.FileName = fileName + "--" + textBox1.Text + ".xlsx";
            saveFileDialog.Title = "选择保存位置";
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                book.Save(saveFileDialog.FileName);

                if (MessageBox.Show("保存成功, 是否自动打开？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(saveFileDialog.FileName);
                }
            }
        }

        private void gridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                gridView.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                gridView.RowHeadersDefaultCellStyle.Font,
                rectangle,
                gridView.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
    }
}
