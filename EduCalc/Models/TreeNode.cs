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

    protected List<Recommend> GetWeightsWithNames(List<Recommend> list = null)
    {
        List<Recommend> weights = new();

        if (this is not CompositeNode node)
        {
            if (this.CalculatedValue < 100.0 && (list is null || !list.Any(r => r.Name == Name)))
                weights.Add(new Recommend() { Name = this.Name, Coef = 1.0, Value = CalculatedValue });
            return weights;
        }

        var childs = node.Children.OrderByDescending(c => c.CalculatedValue).ToArray();
        for (int i = 0; i < node.Coefficients.Length; i++)
        {
            var vals = childs[i].GetWeightsWithNames(list);
            foreach (var val in vals)
            {
                val.Coef *= node.Coefficients[i];
                weights.Add(val);
            }
        }
        
        return weights;
    }
    public List<Recommend> GetRecomendations(double targetScore)
    {
        double diffScore = targetScore - CalculatedValue;
        var list = GetWeightsWithNames();
        double coef = diffScore / list.Sum(d => d.Coef * d.Coef);
        foreach (var kvp in list)
        {
            kvp.Inc = coef * kvp.Coef;
        }

        var above = list.Where(kv => kv.Inc + kv.Value > 100.0);

        List<Recommend> addToResult = new();
        while (above.Any())
        {
            var hundredDict = above.ToList();
            foreach (var kvp in hundredDict)
            {
                kvp.Inc = 100.0 - kvp.Value;
            }
            diffScore -= hundredDict.Sum(kvp => kvp.Inc * kvp.Coef);
            list = GetWeightsWithNames(hundredDict);
            coef = diffScore / list.Sum(d => d.Coef * d.Coef);
            foreach (var kvp in list)
            {
                kvp.Inc = coef * kvp.Coef;
            }
            addToResult.AddRange(hundredDict);
        }

        list.AddRange(addToResult);

        return list;
    }
}