using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EduCalc.Models;

public class TreeNode : INotifyPropertyChanged
{
    private double _value;
    private string _name;
    protected Func<double> _calculator;

    public string Name 
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    public double Value
    {
        get => _value;
        set
        {
            _value = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CalculatedValue));
        }
    }

    public double CalculatedValue => _calculator?.Invoke() ?? Value;

    public TreeNode(string name, Func<double> calculator = null)
    {
        Name = name;
        _calculator = calculator;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    internal Dictionary<string, double> GetWeightsWithNames()
    {
        Dictionary<string, double> weights = new();
        if (this is not CompositeNode node)
        {
            if (this.CalculatedValue < 100.0)
                weights[Name] = 1.0;
            return weights;
        }

        for (int i = 0; i < node.Coefficients.Length; i++)
        {
            var vals = node.Children[i].GetWeightsWithNames();
            foreach (var val in vals)
            {
                weights[val.Key] = val.Value * node.Coefficients[i];
            }
        }
        
        return weights;
    }
}