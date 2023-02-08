using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DataBaseEditor.Classes;

namespace DataBaseEditor
{
    /// <summary>
    /// Interaction logic for AddMatGroupeFrm.xaml
    /// </summary>
    public partial class AddMatGroupeFrm : Window
    {
        public AddMatGroupeFrm()
        {
            InitializeComponent();
        }
        private void btnAddGroupe_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Добавить группу?", "Новая группа.", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                addGroupeMat();
            }
            Close();
        }
        void addGroupeMat()
        {
            if (txtNameGroupe.Text != "")
            {
                using (var con = new SqlConnection(Data.conString))
                {
                    con.Open();
                    var cmd = new SqlCommand("INSERT INTO MaterialsGroups (GroupName, GroupID) VALUES('" + txtNameGroupe.Text + "', '1')", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
