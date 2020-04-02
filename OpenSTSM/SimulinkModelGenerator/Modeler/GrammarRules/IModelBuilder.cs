using System;
using System.Collections.Generic;
using System.Text;

namespace SimulinkModelGenerator.Modeler.GrammarRules
{
    public interface IModelBuilder
    {
        IModelBuilder WithName(string name);
    }
}
