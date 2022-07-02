using Aplicatie_Licenta.Commands;
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
    public class SavedPostsViewModel : ViewModelBase
    {
        // nagigation store
        private readonly NavigationStore _navigationStore;
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        // saved post
        private readonly ObservableCollection<PostCardViewModel> _viewablePosts;
        public IEnumerable<PostCardViewModel> ViewablePosts => _viewablePosts;

        // loading bool for loading circle
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

        private bool _newestCheck = true;
        public bool NewestCheck
        {
            get => _newestCheck;
            set => _newestCheck = value;
        }

        public bool _popularCheck = false;
        public bool PopularCheck
        {
            get => _popularCheck;
            set => _popularCheck = value;
        }

        public bool _priceLHCheck = false;
        public bool PriceLHCheck
        {
            get => _priceLHCheck;
            set => _priceLHCheck = value;
        }

        public bool _priceHLCheck = false;
        public bool PriceHLCheck
        {
            get => _priceHLCheck;
            set => _priceHLCheck = value;
        }


        public ICommand SelectColorCmd { get; }
        public ICommand ClearColorCmd { get; }


        public SavedPostsViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _viewablePosts = new ObservableCollection<PostCardViewModel>();

            // filter // 
            SelectColorCmd = new ExecuteCommand(() =>
            {
                var picker = SingleOpenHelper.CreateControl<ColorPicker>();
                picker.Margin = new Thickness(5);
                var window = new PopupWindow
                {
                    PopupElement = picker,
                    Title = "Pick a color",
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    ShowInTaskbar = true,
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
        }

        private void OnColorChange(object? sender, FunctionEventArgs<Color> e)
        {
            Color = e.Info.ToString();
        }

        public static ViewModelBase LoadSavedPostsViewModel(NavigationStore navigationStore)
        {
            var savedPostViewModel = new SavedPostsViewModel(navigationStore);
            savedPostViewModel.LoadSavedPosts(navigationStore);

            return savedPostViewModel;
        }

        public async void LoadSavedPosts(NavigationStore navigationStore)
        {
            IsLoading = true;
            await PostService.GetAllSavedPost().ContinueWith(
                (task) =>
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        foreach (var post in task.Result)
                        {
                            _viewablePosts.Add(new PostCardViewModel(navigationStore, post, this));
                        }
                        IsLoading = false;
                    });
                });
        }

    }
}
