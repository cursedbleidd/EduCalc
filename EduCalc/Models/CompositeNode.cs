using System.Collections.ObjectModel;

namespace EduCalc.Models;

public class CompositeNode : TreeNode
{
    public ObservableCollection<TreeNode> Children { get; } = new();
    public double[] Coefficients { get; }

    public CompositeNode(string name, double[] coefficients) : base(name)
    {
        Coefficients = coefficients;
        _calculator = () => 
        { //fix
            //if (Children.Count != Coefficients.Length)
               
            try
            {
                return Children.Select((child, i) => child.CalculatedValue * Coefficients[i]).Sum();
            }
            catch
            {
                return Children.OrderByDescending(c => c.CalculatedValue).Take(Coefficients.Length).Average(child => child.CalculatedValue);
            }
        };
    }


}