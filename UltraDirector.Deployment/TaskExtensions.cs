using System.Runtime.CompilerServices;

namespace UltraDirector.Deployment;

public static class TaskExtensions
{
    private static ConditionalWeakTable<ICakeTaskInfo, TypeDictionary> data = [];
    extension(CakeTaskBuilder ctb)
    {
        public CakeTaskBuilder With<TObj>(Func<TObj> factory)
        {
            if (data.TryGetValue(ctb.Task, out var dict))
            {
                dict.Add(factory());
            }
            return ctb;
        }
    }
}

public sealed class TypeDictionary
{
    private Dictionary<Type, object> dictionary;
    
    public TObj Get<TObj>() where TObj : notnull => (TObj)dictionary[typeof(TObj)];

    public void Add<TObj>(TObj obj) where TObj : notnull => dictionary.Add(typeof(TObj), obj);
}