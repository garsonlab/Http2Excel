using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Aspose.Cells;
using TinyJSON;

namespace HttpClient
{
    public partial class HttpTool : Form
    {
        private Convert convert;
        private ParseType parser;
        private string configPath = "../Excel/所需信息类型.xlsx";
        public HttpTool()
        {
            InitializeComponent();
            parser = new ParseType();
            convert = new Convert();
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void HttpTool_Load(object sender, EventArgs e)
        {
            RefreshSheets();
        }

        private void RefreshSheets()
        {
            if (!File.Exists(configPath))
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Excel文件|*.xlsx|Excel文件|*.xls";
                dialog.Multiselect = false;
                dialog.Title = "选择所需信息类型配置";
                dialog.InitialDirectory = Directory.GetCurrentDirectory();

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    configPath = dialog.FileName;
                }
            }

            if (!File.Exists(configPath))
            {
                MessageBox.Show("无配置文件");
                Refresh();
                return;
            }
            parser.Load(configPath);
        }

        private void UploadConfigs()
        {
            List<string> successList = new List<string>();
            List<string> failedList = new List<string>();

            Node clear = Node.NewTable();
            clear["Clear"] = Node.NewString("All");
            HttpHelper.Clear(clear);

            foreach (var sheetData in parser.Sheets)
            {
                string name = sheetData.Key;
                var sheet = sheetData.Value;

                Node node = Node.NewTable();
                bool useable = parser.ParseSheet(sheet, ref node);
                if (useable)
                {
                    successList.Add(name);

                    Node root = Node.NewTable();
                    root["Name"] = Node.NewString(name);
                    root["Rule"] = node;
                    HttpHelper.SendRules(root);
                }
                else
                {
                    failedList.Add(name);
                }
            }
            
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"成功{successList.Count}个：");
            foreach (var s in successList)
            {
                builder.AppendLine($"\t{s}");
            }

            builder.AppendLine();
            builder.AppendLine($"失败{failedList.Count}个:");
            foreach (var s in failedList)
            {
                builder.AppendLine($"\t{s}");
            }

            MessageBox.Show(builder.ToString(), "上传结果", MessageBoxButtons.OK);
        }

        private void GetAllInfos()
        {
            Node array = HttpHelper.GetAllInfos();
            gridView.DataSource = null;
            //gridView.Rows.Clear();

            if(!array.IsArray())
                return;

            DataTable dt = new DataTable();//建立个数据表
            dt.Columns.Add(new DataColumn("身份证号", typeof(string)));
            
            
            int num = array.Count;
            for (int i = 0; i < num; i++)
            {
                DataRow dr = dt.NewRow();
                dr["身份证号"] = array[i].ToString();
                dt.Rows.Add(dr);
            }

            gridView.DataSource = dt;
        }

        private void GetInfo(string name)
        {
            Node info = HttpHelper.GetInfo(name);
            if (info == null)
            {
                MessageBox.Show($"找不到 {name} 的文件", "错误", MessageBoxButtons.OK);
                return;
            }
            convert.FillData(name, info);
            convert.ShowDialog();
        }

        private void 配置相关ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           RefreshSheets();
        }

        private void 上传配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!parser.Loaded)
            {
                RefreshSheets();
                if (!parser.Loaded)
                {
                    MessageBox.Show("先选择配置类型", "错误", MessageBoxButtons.OK);
                    return;
                }
            }

            try
            {
                UploadConfigs();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "错误", MessageBoxButtons.OK);
            }
        }

        private void 查看列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetAllInfos();
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

        private void gridView_MouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataTable table = (DataTable)gridView.DataSource;//数据源
                string name = table.Rows[e.RowIndex]["身份证号"].ToString();
                //测试
                GetInfo(name);
            }
        }
    }


    public class ParseType
    {
        private Workbook workbook;
        private bool loaded;
        private Dictionary<string, Worksheet> sheets = new Dictionary<string, Worksheet>();
        public void Load(string path)
        {
            sheets.Clear();
            workbook = new Workbook(path);
            foreach (Worksheet sheet in workbook.Worksheets)
            {
                sheets.Add(sheet.Name, sheet);
            }

            loaded = true;
        }

        public Dictionary<string, Worksheet> Sheets => sheets;

        public bool Loaded => loaded;

        public bool ParseSheet(Worksheet sheet, ref Node node)
        {
            bool useable = false;
            var cells = sheet.Cells;
            for (int i = 1; i < cells.Rows.Count; i++)
            {
                Row row = cells.Rows[i];

                string des, name;
                if (GetCell(row, 0, out des) && GetCell(row, 1, out name))
                {
                    useable = true;

                    Node data = Node.NewTable();
                    data["Des"] = des;
                    data["Required"] = GetCell(row, 2);
                    node[name] = data;
                }
            }

            return useable;
        }

        private bool GetCell(Row row, int idx, out string value)
        {
            Cell cell = row[idx];
            if (cell != null)
            {
                value = cell.DisplayStringValue.Trim();
                if (string.IsNullOrEmpty(value))
                    return false;
                return true;
            }
            else
            {
                value = "";
                return false;
            }
        }

        private bool GetCell(Row row, int idx)
        {
            Cell cell = row[idx];
            if (cell == null)
                return false;

            int v;
            if (int.TryParse(cell.DisplayStringValue, out v))
            {
                return v == 1;
            }

            return false;
        } 
    }
}
