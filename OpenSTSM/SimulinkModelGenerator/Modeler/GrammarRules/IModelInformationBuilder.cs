using SimulinkModelGenerator.Modeler.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimulinkModelGenerator.Modeler.GrammarRules
{
    public interface IModelInformationBuilder
    {
        IModelInformationBuilder WithVersion(string version);
        IFinalizeModelBuilder AddModel(Action<ModelBuilder> action = null);       
    }


    public interface IFinalizeModelBuilder
    {
        void Build();
        void Save(string path);
    }
}
