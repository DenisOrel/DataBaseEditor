using System;
using System.Windows;
using System.Windows.Controls;

namespace DataBaseEditor.Classes
{
    public class ContMenu
    {
        public HomeForm hmFrm { get; set; }
        public void GenerateContextMenu(DataGrid dg)
        {
            try
            {
                dg.ContextMenu = BuildContextMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private MenuItem _miaDelMat;
        private ContextMenu BuildContextMenu()
        {
            var theMenu = new ContextMenu();

            _miaDelMat = new MenuItem { Header = "Удалить материал" };
            _miaDelMat.Click += hmFrm.DelMat_Click;

            theMenu.Items.Add(_miaDelMat);

            return theMenu;
        }
    }
}