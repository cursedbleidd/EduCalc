using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
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

    protected Dictionary<string, Recommend> GetWeightsWithNames(Dictionary<string, Recommend> dict = null)
    {
        Dictionary<string, Recommend> weights = new();

        if (this is not CompositeNode node)
        {
            if (this.CalculatedValue < 100.0 && (dict is null || !dict.ContainsKey(Name)))
                weights[Name] = new Recommend() { Coef = 1.0, Value = CalculatedValue };
            return weights;
        }

        var childs = node.Children.OrderByDescending(c => c.CalculatedValue).ToArray();
        for (int i = 0; i < node.Coefficients.Length; i++)
        {
            var vals = childs[i].GetWeightsWithNames(dict);
            foreach (var val in vals)
            {
                val.Value.Coef *= node.Coefficients[i];
                weights[val.Key] = val.Value;
            }
        }
        
        return weights;
    }
    public Dictionary<string, Recommend> GetRecomendations(double targetScore)
    {
        double diffScore = targetScore - CalculatedValue;
        var dict = GetWeightsWithNames();
        double coef = diffScore / dict.Values.Sum(d => d.Coef * d.Coef);
        foreach (var kvp in dict)
        {
            kvp.Value.Inc = coef * kvp.Value.Coef;
        }

        var above = dict.Where(kv => kv.Value.Inc + kv.Value.Value > 100.0);

        Dictionary<string, Recommend> addToResult = new();
        while (above.Any())
        {
            var hundredDict = above.ToDictionary();
            foreach (var kvp in hundredDict)
            {
                kvp.Value.Inc = 100.0 - kvp.Value.Value;
            }
            diffScore -= hundredDict.Sum(kvp => kvp.Value.Inc * kvp.Value.Coef);
            dict = GetWeightsWithNames(hundredDict);
            coef = diffScore / dict.Values.Sum(d => d.Coef * d.Coef);
            foreach (var kvp in dict)
            {
                kvp.Value.Inc = coef * kvp.Value.Coef;
            }
            foreach (var item in hundredDict)
            {
                addToResult[item.Key] = item.Value;
            }
        }

        foreach (var item in addToResult)
        {
            dict[item.Key] = item.Value;
        }

        return dict;
    }
}