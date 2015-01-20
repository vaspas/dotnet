using System;
using System.Linq;
using System.Reflection;
using Sigflow.Dataflow;
using System.Collections;
using System.Collections.Generic;

namespace Sigflow.Module
{
    /// <summary>
    /// Класс для замены свойств модулей. Модули должны иметь идентичный интерфейс подключения к входному и выходному сигналу.
    /// </summary>
    static class ChangeModulePropertiesHelper
    {
        public static void Change(IModule from, IModule to)
        {
            ChangeObjects<ISignalReader>(from, to);
            ChangeObjects<ISignalWriter>(from, to);

            ChangeList<ISignalReader>(from, to);
            ChangeList<ISignalWriter>(from, to);
        }

        private static void ChangeObjects<T>(IModule from, IModule to)
        {
            var toProperties =
                to.GetType().GetProperties().ToList();
            var fromProperties =
                from.GetType().GetProperties().ToList();

            fromProperties
                .FindAll(p => p.PropertyType == typeof (T)
                              || p.PropertyType.GetInterfaces().Any(i => i == typeof (T)))
                .ForEach(p =>
                         toProperties
                             .First(n => n.Name == p.Name)
                             .SetValue(to, p.GetValue(from, null), null));
        }

        private static void ChangeList<T>(IModule from, IModule to)
        {
            var predicate = new Predicate<PropertyInfo>(
                p =>
                    {
                        var isGenericIList = p.PropertyType.IsGenericType &&
                                             p.PropertyType.GetGenericTypeDefinition() == typeof (IList<>);
                        var isIList = p.PropertyType.GetInterfaces().Any(i => i == typeof (IList));

                        if (!isGenericIList && !isIList)
                            return false;
                        
                        if(p.PropertyType.GetGenericArguments().All(
                            gt => gt == typeof (T) || gt.GetInterfaces().Any(i => i == typeof (T))))
                            return true;

                        var elType = p.PropertyType.GetElementType();
                        
                        if (elType != null &&
                         (elType == typeof(T) || elType.GetInterfaces().Any(i => i == typeof(T))))
                            return true;

                        return false;
                    });

            var toProperties =
                to.GetType().GetProperties().ToList().FindAll(predicate);
            var fromProperties =
                from.GetType().GetProperties().ToList().FindAll(predicate);

            fromProperties
                .ForEach(p =>
                             {
                                 var newPi = toProperties
                                     .FirstOrDefault(n => n.Name == p.Name);
                                 if (newPi == null)
                                     return;

                                 var newList = (IList)newPi.GetValue(to, null);
                                 var oldList = (IList)p.GetValue(from, null);


                                 if (!newList.IsFixedSize)
                                 {
                                     newList.Clear();

                                     foreach (var v in oldList)
                                         newList.Add(v);
                                 }
                                 else
                                 {
                                     for (var i = 0; i < oldList.Count; i++)
                                         newList[i] = oldList[i];
                                 }
                             });
        }
    }
}
