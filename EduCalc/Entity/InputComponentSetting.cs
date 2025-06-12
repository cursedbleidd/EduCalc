using EduCalc.Entity.Tree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EduCalc.Entity
{
    public class InputComponentSetting : INotifyPropertyChanged
    {
        public string Id { get; set; }
        public string Description { get; set; }
        private bool _isIncluded;
        public bool IsIncluded
        {
            get => _isIncluded;
            set
            {
                if (_isIncluded != value)
                {
                    _isIncluded = value;
                    OnPropertyChanged();
                }
            }
        }
        public InputComponentSetting(string id, string desc, bool isIncluded)
        {
            Id = id;
            Description = desc;
            IsIncluded = isIncluded;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public static implicit operator InputComponentSetting(TreeNode node) => new InputComponentSetting(node.Id, node.Description, true);
    }
}
