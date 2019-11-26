using OpenSTSM.Attributes;
using OpenSTSM.Models.MainWindow.SimulinkElements;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace OpenSTSM.Extensions
{
    public static class EnumExtensions
    {
        public static SimulinkElementProperties GetSimulinkElementPropertyValues<TSimulinkType>(this TSimulinkType simulinkType) where TSimulinkType : Enum
        {
            var type = simulinkType.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException($"{nameof(simulinkType)} must be of Enum type", nameof(simulinkType));
            }

            SimulinkElementProperties cep = new SimulinkElementProperties();

            var member = simulinkType.ToString();
            var enumValueMemberInfo = type.GetField(member);
            var valueAttributes = enumValueMemberInfo.GetCustomAttributes(typeof(SimulinkElementPropertiesAttribute), false);
           
            cep.Name = ((SimulinkElementPropertiesAttribute)valueAttributes[0]).Name;
            cep.NumberOfInputs = ((SimulinkElementPropertiesAttribute)valueAttributes[0]).NumberOfInputs;
            cep.NumberOfOutputs = ((SimulinkElementPropertiesAttribute)valueAttributes[0]).NumberOfOutputs;
            cep.GraphElementType = ((SimulinkElementPropertiesAttribute)valueAttributes[0]).GraphElementType;

            return cep;
        }

        public static string GetDescription<T>(this T enumerationValue) where T : struct
        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException($"{nameof(enumerationValue)} must be of Enum type", nameof(enumerationValue));
            }
            var memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return enumerationValue.ToString();
        }
    }
}
