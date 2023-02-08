using DataBaseEditor.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DataBaseEditor.Classes
{
    public class Data
    {
        #region ConString

        public static string conString
        {
            get { return read_Setting(); }
        }

        SqlConnection con = new SqlConnection(conString);
        #endregion     
        public int m1 { get; set; } // Метка для загрузки или группы материало или материала

        #region BindingMatBase
        public DataTable LoadDataBase(int id)
        {
            var dt = new DataTable();
            try
            {
                var ds = new DataSet();
                ds.Tables.Add(dt);
                SqlDataAdapter da = default(SqlDataAdapter);

                switch (m1)
                {
                    case 0:
                        // Загрузка группы материала
                        da = new SqlDataAdapter("select * FROM MaterialsProp where GroupID='" + id + "'order by MaterialsName", con);
                        break;
                    case 1:
                        // Загрузка материала по ID
                        da = new SqlDataAdapter("select * FROM MaterialsProp where LevelID='" + id + "'", con);
                        break;
                }

                da.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "; " + ex.StackTrace);
            }

            return dt;
        }
        public class MatBaseFields
        {
            public string levelid { get; set; }
            public bool Thickness { get; set; }
            public string MaterialsName { get; set; }
            public string MaterialsNameEng { get; set; }
            public string ERP { get; set; }
            public string Density { get; set; }
            public string SWProperty { get; set; }
            public string Description { get; set; }
            public string CodeMaterial { get; set; }
            public string DescriptionCode { get; set; }
            public string xhatch { get; set; }
            public string angle { get; set; }
            public string scale { get; set; }
            public string pwshader2 { get; set; }
            public string path { get; set; }
            public string RGB { get; set; }
        }
        public List<MatBaseFields> MatBaseList(int id)
        {
            var listString = default(List<MatBaseFields>);
            try
            {
                listString = (from DataRow datarow in LoadDataBase(id).Rows
                              select new MatBaseFields
                              {
                                  levelid = datarow["levelid"].ToString(),
                                  Thickness = Convert.ToBoolean(datarow["Thickness"].ToString()),
                                  MaterialsName = datarow["MaterialsName"].ToString(),
                                  MaterialsNameEng = datarow["MaterialsNameEng"].ToString(),
                                  ERP = datarow["ERP"].ToString(),
                                  Density = datarow["Density"].ToString(),
                                  SWProperty = datarow["SWProperty"].ToString(),
                                  CodeMaterial = datarow["CodeMaterial"].ToString(),
                                  DescriptionCode = datarow["DescriptionCode"].ToString(),
                                  xhatch = datarow["xhatch"].ToString(),
                                  angle = datarow["angle"].ToString(),
                                  scale = datarow["scale"].ToString(),
                                  pwshader2 = datarow["pwshader2"].ToString(),
                                  path = datarow["path"].ToString(),
                                  RGB = datarow["RGB"].ToString()
                              }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "; " + ex.StackTrace);
            }
            return listString;
        }
        #endregion

        #region BindingMatGroup
        public void CboDataSet(ComboBox cboMatGroups)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from MaterialsGroups order by GroupName";

                var objDs = new DataSet();
                var dAdapter = new SqlDataAdapter();
                dAdapter.SelectCommand = cmd;
                con.Open();
                dAdapter.Fill(objDs, "MaterialsProp");

                cboMatGroups.SelectedValuePath = "LevelID";
                cboMatGroups.DisplayMemberPath = "GroupName";
                cboMatGroups.ItemsSource = objDs.Tables[0].DefaultView;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "; " + ex.StackTrace);
            }
        }
        #endregion

        private static string read_Setting()
        {
            string sResult = null;

            if (Settings.Default.Properties["conString"] != null)
            {
                sResult = Settings.Default.Properties["conString"].DefaultValue.ToString();
            }
            return sResult;

        }

    }
}