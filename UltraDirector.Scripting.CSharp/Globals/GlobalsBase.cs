using System.Linq;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;
using UltraDirector.Scripting.CSharp.ReplManagement;
using UnityEngine;
using Logger = plog.Logger;

namespace UltraDirector.Scripting.CSharp.Globals;
[PublicAPI]
public abstract class GlobalsBase(Logger logger)
{
    [Description("The Logger instance for this script. Use it to print messages to the log.")]
    public Logger Logger { get; } = logger;
    [Description("The player's transform. Use it to get the player's position, rotation, etc.")]
    public Transform PlayerTransform => NewMovement.Instance!.transform;

    [Description("Prints a message to the log.")]
    public void Print(string msg) => Logger.Info(msg);

    [Description("Lists all accessible members of this Globals type.")]
    public void Help()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Accessible Globals members:"); // TODO: maybe come up with a better line
        var members = GetType().GetMembers();
        foreach (var member in members)
        {
            var info = member switch
            {
                FieldInfo field => GetFieldInfoStr(field),
                PropertyInfo property => GetPropertyInfoStr(property),
                MethodInfo method => GetMethodInfoStr(method),
                ConstructorInfo constructor => GetConstructorInfoStr(constructor),
                EventInfo eventInfo => GetEventInfoStr(eventInfo),
                _ => $"unknown member: {member.Name}, type {member.MemberType}"
            };
            sb.AppendLine($"- {info}");
            var description = member.GetCustomAttribute<DescriptionAttribute>()?.Description;
            if (!string.IsNullOrWhiteSpace(description)) sb.AppendLine($"  Description: {description}");
        }
        Logger.Info(sb.ToString());
        return;

        static string GetPropertyInfoStr(PropertyInfo property)
        {
            var getter = property.CanRead ? "get; " : "";
            var setter = property.CanWrite ? "set; " : "";
            return $"Property {property.PropertyType} {property.Name} {{ " + getter + setter + "}";
        }
        static string GetMethodInfoStr(MethodInfo method)
        {
            var methodParams = method.GetParameters().Select(static p => p.ParameterType.ToString());
            return $"Method: {method.Name}({string.Join(", ", methodParams)}) -> {method.ReturnType}";
        }
        static string GetFieldInfoStr(FieldInfo field)
        {
            var modifiers = field.IsInitOnly ? "(readonly) "
                            : field.IsLiteral ? "(const) "
                                                : null;
            return $"Field {modifiers}{field.FieldType} {field.Name}";
        }
        static string GetEventInfoStr(EventInfo eventInfo)
        {
            var eventDelegateInfo = eventInfo.EventHandlerType.GetMethod("Invoke")!;
            var eventParams = eventDelegateInfo
                             .GetParameters()
                             .Select(static p => p.ParameterType.ToString());
            return $"Event: {eventInfo.Name} ({string.Join(", ", eventParams)}) -> {eventDelegateInfo.ReturnType}";
        }
        static string GetConstructorInfoStr(ConstructorInfo constructor)
        {
            var ctorParams = constructor.GetParameters().Select(p => p.ParameterType.ToString());
            return $"Constructor: {constructor.Name}({string.Join(", ", ctorParams)}) -> {constructor.DeclaringType}";
        } // i dont think this is necessary
    }
}