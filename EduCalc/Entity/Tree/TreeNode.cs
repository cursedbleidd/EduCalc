using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;

namespace EduCalc.Entity.Tree;

public class TreeNode : INotifyPropertyChanged
{
    private double _value;
    private string _name;
    protected Func<double> _calculator;
    public string Description { get; set; }
    public string Id { get; set; }

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

    public double CalculatedValue
    {
        get
        {
            double value = _calculator?.Invoke() ?? Value;
            if (value < 0)
                return 0;
            if (value > 100)
                return 100;
            return value;
        }
    }

    public TreeNode(string name, string id = null, string desc = null, Func<double> calculator = null)
    {
        Name = name;
        Id = id;
        Description = desc;
        _calculator = calculator;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    protected List<Recommend> GetWeightsWithNames(List<InputComponentSetting> settings, List<Recommend> list = null)
    {
        List<Recommend> weights = new();

        if (this is not CompositeNode node)
        {
            bool? nodeIncluded = settings.Find(s => s.Id == Id)?.IsIncluded;
            if (CalculatedValue < 100.0 && (list is null || !list.Any(r => r.Name == Name)) && (!nodeIncluded.HasValue || (nodeIncluded.HasValue && nodeIncluded.Value)))
                weights.Add(new Recommend() { Name = Name, Id = Id, Description = Description, Coef = 1.0, Value = CalculatedValue });
            return weights;
        }

        var childs = node.Children.ToArray();
        if (childs.Length != node.Coefficients.Length)
        {
            childs = childs.OrderByDescending(c => c.CalculatedValue).ToArray();
        }

        for (int i = 0; i < node.Coefficients.Length; i++)
        {
            var vals = childs[i].GetWeightsWithNames(settings, list);
            foreach (var val in vals)
            {
                val.Coef *= node.Coefficients[i];
                weights.Add(val);
            }
        }

        return weights;
    }
    public List<Recommend> GetRecomendations(double targetScore, List<InputComponentSetting> settings)
    {
        double diffScore = targetScore - CalculatedValue;
        var list = GetWeightsWithNames(settings);
        double coef = diffScore / list.Sum(d => d.Coef * d.Coef);
        foreach (var kvp in list)
        {
            kvp.Inc = coef * kvp.Coef;
        }

        var above = list.Where(kv => kv.Inc + kv.Value > 100.0);

        List<Recommend> addToResult = new();
        while (above.Any())
        {
            var exclude = above.ToList();
            foreach (var kvp in exclude)
            {
                kvp.Inc = 100.0 - kvp.Value;
            }
            diffScore -= exclude.Sum(kvp => kvp.Inc * kvp.Coef);
            addToResult.AddRange(exclude);
            list = GetWeightsWithNames(settings, addToResult);
            coef = diffScore / list.Sum(d => d.Coef * d.Coef);
            foreach (var kvp in list)
            {
                kvp.Inc = coef * kvp.Coef;
            }
            above = list.Where(kv => kv.Inc + kv.Value > 100.0);
        }

        list.AddRange(addToResult);

        return list;
    }
}