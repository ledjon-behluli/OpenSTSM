using SimulinkModelGenerator.Modeler.GrammarRules;
using System;
using System.Collections.Generic;

namespace SimulinkModelGenerator.Modeler.Builders
{
    public sealed class ModelBuilder : IModelBuilder
    {
        private readonly ModelInformation modelInformation;
        private readonly ModelInformationBuilder modelInformationBuilder;

        private string modelName;

        public ModelBuilder(ModelInformationBuilder modelInformationBuilder, ModelInformation modelInformation)
        {
            this.modelInformation = modelInformation;
            this.modelInformationBuilder = modelInformationBuilder;
        }

        public IModelBuilder WithName(string name)
        {
            modelName = name;
            return this;
        }
      
        internal IFinalizeModelBuilder Build()
        {
            this.modelInformation.Model = new Model()
            {
                Name = modelName,
                P = new List<P>()
                {
                    new P(){ Name = "", Text = "" }

                    // TODO other elements
                }
            };

            return modelInformationBuilder;
        }
    }
}
