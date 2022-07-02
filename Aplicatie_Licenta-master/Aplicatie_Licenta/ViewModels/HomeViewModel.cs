using Aplicatie_Licenta.Commands;
using Aplicatie_Licenta.Commands.Async;
using Aplicatie_Licenta.Service;
using Aplicatie_Licenta.Stores;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Aplicatie_Licenta.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel CurentViewModel => this;

        private readonly ObservableCollection<PostCardViewModel> _viewablePosts;
        public IEnumerable<PostCardViewModel> ViewablePosts => _viewablePosts;
        private readonly NavigationStore _navigationStore;


        public bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        // Filters for search //
        private string _color = "";
        public string Color
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        private bool[] _sortCheck = new bool[4] {true, false, false, false};
        public bool NewestCheck 
        { 
            get => _sortCheck[0];
            set => _sortCheck[0] = value;
        }
        public bool PopularCheck
        {
            get => _sortCheck[1];
            set => _sortCheck[1] = value;
        }
        public bool PriceLHCheck 
        { 
            get => _sortCheck[2];
            set => _sortCheck[2] = value;
        }
        public bool PriceHLCheck
        {
            get => _sortCheck[3];
            set => _sortCheck[3] = value;
        }



        public ICommand SelectColorCmd { get;}
        public ICommand ClearColorCmd { get;}

        public HomeViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _viewablePosts = new ObservableCollection<PostCardViewModel>();

            // filter // 
            SelectColorCmd = new ExecuteCommand(() =>
            {
                var picker = SingleOpenHelper.CreateControl<ColorPicker>();
                var window = new PopupWindow
                {
                    PopupElement = picker,
                    Title = "Pick a color",
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    ShowInTaskbar = true,
                    AllowsTransparency = true,
                    WindowStyle = WindowStyle.None
                };
                picker.SelectedColorChanged += OnColorChange;
                picker.Canceled += delegate { window.Close(); };
                picker.Confirmed += delegate { window.Close(); };
                window.Show();
            });

            ClearColorCmd = new ExecuteCommand(() =>
            {
                Color = "";
            });

            LoadPosts();
        }

        private void OnColorChange(object? sender, FunctionEventArgs<Color> e)
        {
            Color = e.Info.ToString();
        }
        
        public async void LoadPosts()
        {
            IsLoading = true;
            _viewablePosts.Clear();

            var color = Color;
            var sortId = Array.IndexOf(_sortCheck, true);

            await PostService.GetFiterPosts(sortId, color).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    HandyControl.Controls.MessageBox.Show("Internal server error");
                    return;
                }
                // move to UI thread
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    foreach (var post in t.Result)
                    {
                        _viewablePosts.Add(new PostCardViewModel(_navigationStore, post, this));
                    }

                    IsLoading = false;
                });
            });
        }
    }
}
