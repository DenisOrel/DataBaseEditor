using Microsoft.Win32;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DataBaseEditor.Classes;

namespace DataBaseEditor
{
    public partial class HomeForm : Window
    {
        public HomeForm()
        {
            InitializeComponent();
        }
        public int m1; // Метка для загрузки или группы материало или материала
        public int m2; // Метка добавления или редактирования материала
        Data dta;
        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            m1 = 0; // Загрузка основной базы по id группы
            var cMenu = new ContMenu() { hmFrm = this };
            cMenu.GenerateContextMenu(dgMatBase);
            BindingCboGroups();
            cboAngle.ItemsSource = Angel();
            cboScale.ItemsSource = Scale();
        }
        void BindingCboGroups()
        {
            //m1 = 0;
            dta = new Data() { m1 = this.m1 };
            dta.CboDataSet(CboMatGroups);
            //CboMatGroups.SelectedIndex = 0;
        }
        // ID группы материалов
        int groupeId()
        {
            var groupeId = (int)CboMatGroups.SelectedValue;
            return groupeId;
        }
        public int GroupeID { get { return groupeId(); } set { value = groupeId(); }}
        void BindingDataGrid(int id)
        {
            dta = new Data() { m1 = 0 };
            dgMatBase.ItemsSource = dta.MatBaseList(id);
        }
        void CboMatGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BindingDataGrid(GroupeID);
        }
        private void dgMatBase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var itemId = (Data.MatBaseFields)dgMatBase.SelectedItem;
            if (itemId != null)
            {
                loadMatPropForEdit(int.Parse(itemId.levelid));
            }
        }
        //TODO: BtnAddGroupe_Click
        private void BtnAddGroupe_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var addGroupeFrm = new AddMatGroupeFrm();
                addGroupeFrm.ShowDialog();
                BindingCboGroups();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ";\n" + ex.StackTrace);
            }
        }
        private void BtnDelGroupe_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Вы действительно хотите удалить группу материалов?", "Удаление группы материалов", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (var con = new SqlConnection(Data.conString))
                    {
                        con.Open();
                        var cmd = new SqlCommand("DELETE FROM MaterialsGroups WHERE GroupName ='" + CboMatGroups.Text + "'", con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    //CboMatGroups.ItemsSource = null;
                    BindingCboGroups();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // ID материала
        int ItemId()
        {
            var item = dgMatBase.SelectedItem as Data.MatBaseFields;
            var x = Convert.ToInt32(item.levelid);  
            return x;
        }
        public int MatID { get { return ItemId(); } set { value = ItemId(); } }
        SqlCommand cmd;
        public void DelMat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Вы действительно хотите удалить материал?", "Удаление материала", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var con = new SqlConnection(Classes.Data.conString);
                    con.Open();
                    cmd = new SqlCommand();
                    
                    var q = "DELETE FROM MaterialsProp WHERE LevelID ='" + MatID + "'";

                    cmd = new SqlCommand(q, con);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    BindingDataGrid(GroupeID);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "; " + ex.StackTrace);
            }
        }
        #region Array
        Array Angel()
        {
            var x = Enumerable.Range(0, 100).ToArray();
            return x;
        }
        Array Scale()
        {
            var x = Enumerable.Range(0, 100).ToArray();
            return x;
        }
        #endregion
        public void loadMatPropForEdit(int getId)
        {
            try
            {
                dta = new Data() { m1 = 1 }; // Загрузка материала по id
                foreach (var item in dta.MatBaseList(getId))
                {
                    chkThickness.IsChecked = item.Thickness;
                    txtMatName.Text = item.MaterialsName;
                    txtMatNameEng.Text = item.MaterialsNameEng;
                    txtCode1c.Text = item.ERP;
                    txtDensity.Text = item.Density;
                    txtSwProp.Text = item.SWProperty;
                    txtMatCode.Text = item.CodeMaterial;
                    txtHatch.Text = item.xhatch;
                    cboAngle.Text = item.angle;
                    cboScale.Text = item.scale;
                    txtShader.Text = item.pwshader2;
                    txtPath.Text = item.path;
                    txtRgb.Text = item.RGB;
                }
                txtBlockHelp.Text = txtSwProp.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "; " + ex.StackTrace);
            }
        }
        void UpdateMat()
        {
            try
            {
                var con = new SqlConnection(Classes.Data.conString);
                con.Open();

                var q = "UPDATE MaterialsProp SET MaterialsName='" + txtMatName.Text +
                                                    "', MaterialsNameEng='" + txtMatNameEng.Text +
                                                    "', ERP='" + txtCode1c.Text +
                                                    "', Density='" + txtDensity.Text +
                                                    "', SWProperty='" + txtSwProp.Text +
                                                    "', CodeMaterial='" + txtMatCode.Text +
                                                    "', xhatch='" + txtHatch.Text +
                                                    "', angle='" + cboAngle.Text +
                                                    "', scale='" + cboScale.Text +
                                                    "', pwshader2='" + txtShader.Text +
                                                    "', path='" + txtPath.Text +
                                                    "', RGB='" + txtRgb.Text +
                                                    "', Thickness='" + Convert.ToInt32(chkThickness.IsChecked) +
                                                    "' WHERE LevelID='" + MatID + "'";

                cmd = new SqlCommand(q, con);

                cmd.ExecuteNonQuery();
                con.Close();
                BindingDataGrid(GroupeID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "; " + ex.StackTrace);
            }
        }
        void AddMat()
        {
            try
            {
                var ok = "";
                dta = new Data() { m1 = 0 }; // Загрузка по id группы
                foreach (DataRow item in dta.LoadDataBase(GroupeID).Rows)
                {
                
                    var existMatName = item["MaterialsName"].ToString();
                    if (txtMatName.Text == existMatName)
                    {
                        ok = MessageBox.Show("Материал с таким именем уже существует, введите новое имя!", "Добавление материала.", MessageBoxButton.OK).ToString();
                        if (ok.ToString() == "OK")
                        {
                            return;
                        }
                    }
                }
                if (ok == "")
                {
                    using (var con = new SqlConnection(Data.conString)) { 
                        con.Open();
                        object column1 = Convert.ToInt32(chkThickness.IsChecked); // Толщина листового металла - Thickness
                        object column2 = txtMatName.Text; // Имя материала - MaterialsName
                        object column3 = txtMatNameEng.Text; // Имя материала ENG - MaterialsNameEng
                        object column4 = txtCode1c.Text; // Код 1C - ERP
                        object column5 = txtDensity.Text; // Плотность - Density
                        object column6 = txtSwProp.Text; // Свойства SolidWorks
                        object column7 = txtMatName.Text; // Description
                        object column8 = txtMatCode.Text; // Код материала
                        object column9 = ""; // DescriptionCode
                        object column10 = txtHatch.Text; // xhatch
                        object column11 = cboAngle.Text; // angle
                        object column12 = cboScale.Text; // scale
                        object column13 = txtShader.Text; // pwshader2
                        object column14 = txtPath.Text; // path
                        object column15 = txtRgb.Text; // RGB
                        cmd = new SqlCommand("INSERT INTO MaterialsProp (Thickness, MaterialsName, MaterialsNameEng, ERP, Density, SWProperty, description, CodeMaterial, DescriptionCode, xhatch, angle, scale, pwshader2, path, RGB, GroupID) VALUES('" +
                                                        column1 + "','" +
                                                        column2 + "','" +
                                                        column3 + "','" +
                                                        column4 + "','" +
                                                        column5 + "','" +
                                                        column6 + "','" +
                                                        column7 + "','" +
                                                        column8 + "','" +
                                                        column9 + "','" +
                                                        column10 + "','" +
                                                        column11 + "','" +
                                                        column12 + "','" +
                                                        column13 + "','" +
                                                        column14 + "','" +
                                                        column15 + "','" +
                                                        groupeId() + "')", con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        BindingDataGrid(GroupeID);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "; " + ex.StackTrace);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e) {
            var result = MessageBox.Show("Добавить материал?", "Новый материал.", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes) {
                if (txtMatName.Text != "") {
                    AddMat();
                }
                else {
                    MessageBox.Show("Имя материала отсутствует!");
                }
            }
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Сохранить изменение?", "Редактирование материала.", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes){
                UpdateMat();
            }
        }
        private void BtnSearchPath_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:\Program Files\SolidWorks Corp\SolidWorks\data\graphics\Materials";
            openFileDialog.Filter = "Text files (*.p2m)|*.p2m|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {

                txtPath.Text = openFileDialog.FileName;
            }
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}