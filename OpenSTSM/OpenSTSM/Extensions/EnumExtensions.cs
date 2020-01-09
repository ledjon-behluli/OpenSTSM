using OpenSTSM.Attributes;
using OpenSTSM.Models.MainWindow.SimulinkElements;
using System;
using System.ComponentModel;

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

            SimulinkElementProperties sep = new SimulinkElementProperties();

            var member = simulinkType.ToString();
            var enumValueMemberInfo = type.GetField(member);
            var valueAttributes = enumValueMemberInfo.GetCustomAttributes(typeof(SimulinkElementPropertiesAttribute), false);
           
            sep.Name = ((SimulinkElementPropertiesAttribute)valueAttributes[0]).Name;
            sep.NumberOfInputs = ((SimulinkElementPropertiesAttribute)valueAttributes[0]).NumberOfInputs;
            sep.NumberOfOutputs = ((SimulinkElementPropertiesAttribute)valueAttributes[0]).NumberOfOutputs;

            return sep;
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
