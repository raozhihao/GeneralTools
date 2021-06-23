using System;
using System.Windows;
using System.Windows.Controls;

namespace GeneralTool.General.WPFHelper.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataGridScrollHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty DataGridScrollProperty = DependencyProperty.RegisterAttached("DataGridScroll", typeof(bool), typeof(DataGridScrollHelper), new PropertyMetadata(false, AlwaysScrollToEndChanged));



        private static void AlwaysScrollToEndChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DataGrid grid)
            {
                if (e.NewValue != null && (bool)e.NewValue)
                {
                    if (grid.Items.Count > 0)
                        grid.ScrollIntoView(grid.Items[grid.Items.Count - 1]);
                    grid.LoadingRow += Grid_LoadingRow;
                }
                else
                {
                    grid.LoadingRow -= Grid_LoadingRow;
                }

                return;
            }

            throw new InvalidOperationException("附加的Always Scroll To End属性只能应用于Scroll Viewer实例.");
        }



        private static void Grid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            if (grid.IsMouseOver)
                return;


            if (grid.IsMouseDirectlyOver)
            {

            }
            int lastIndex = grid.Items.Count - 1;

            grid.ScrollIntoView(grid.Items[lastIndex]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scroll"></param>
        /// <returns></returns>
        public static bool GetDataGridScroll(DataGrid scroll)
        {
            if (scroll == null)
            {
                throw new ArgumentNullException("scroll");
            }

            return (bool)scroll.GetValue(DataGridScrollProperty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scroll"></param>
        /// <param name="alwaysScrollToEnd"></param>
        public static void SetDataGridScroll(DataGrid scroll, bool alwaysScrollToEnd)
        {
            if (scroll == null)
            {
                throw new ArgumentNullException("scroll");
            }

            scroll.SetValue(DataGridScrollProperty, alwaysScrollToEnd);
        }

    }
}
