using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using EduCalc.Wpf.Commands;

namespace EduCalc.ViewModels
{
    public class RecommendationsViewModel : INotifyPropertyChanged
    {
        private readonly Window _window;
        private List<string> _recommendations = new();

        public List<string> Recommendations
        {
            get => _recommendations;
            set
            {
                _recommendations = value;
                OnPropertyChanged();
            }
        }

        public ICommand CloseCommand { get; }

        public RecommendationsViewModel(Window window, List<string> recommendations)
        {
            _window = window;
            _recommendations = recommendations;
            CloseCommand = new RelayCommand(() => _window.Close());
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Recommendations)));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}