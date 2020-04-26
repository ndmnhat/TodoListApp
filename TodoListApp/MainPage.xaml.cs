using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using System.Threading.Tasks;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TodoListApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            TodoList.LoadTodoList();
            TaskList.Source = TodoList.Todos;
            LoadCommand();
        }

        private void LoadCommand()
        {
            EditCommand.Command = new StandardUICommand(StandardUICommandKind.Open);
            EditCommand.ExecuteRequested += EditTodoBtn_Click;
            DeleteCommand.Command = new StandardUICommand(StandardUICommandKind.Delete);
            DeleteCommand.ExecuteRequested += DeleteTodoBtn_Click;
        }

        private void DeleteTodoBtn_Click(XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            TodoList.DeleteTodo((string)args.Parameter);
        }

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            await AddDialog.ShowAsync();
        }

        private void AddDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            TodoList.AddTodo(TxbTitle.Text, TxbDescription.Text);
            TxbTitle.Text = string.Empty;
            TxbDescription.Text = string.Empty;
        }
        private  async void EditTodoBtn_Click(XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            string id = args.Parameter as string;
            EditDialog.Tag = id;
            TxbEditTitle.Text = TodoList.GetTodo(id).Title;
            TxbEditDescription.Text = TodoList.GetTodo(id).Description;
            await EditDialog.ShowAsync();
        }

        private void EditDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            TodoList.ModifyTodo((string)EditDialog.Tag, TxbEditTitle.Text, TxbEditDescription.Text);
            TxbEditTitle.Text = string.Empty;
            TxbEditDescription.Text = string.Empty;
        }

        private void ChangeTheme_Click(object sender, RoutedEventArgs e)
        {
            if (page.ActualTheme == ElementTheme.Light)
                page.RequestedTheme = ElementTheme.Dark;
            else
                page.RequestedTheme = ElementTheme.Light;
        }
    }
}
