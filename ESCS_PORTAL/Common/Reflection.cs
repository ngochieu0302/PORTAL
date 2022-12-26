using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using ESCS_PORTAL.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace ESCS_PORTAL.Common
{
    public static class ReflectionExtensions
    {
        public static TValue GetAttributeValue<TAttribute, TValue>(this MemberInfo type, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        {
            var att = type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
            if (att != null)
            {
                return valueSelector(att);
            }
            return default(TValue);
        }
    }
    public class Reflection
    {
        public List<Type> GetControllers(string namespaces)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            IEnumerable<Type> types = assembly.GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type) && type.Namespace.Contains(namespaces)).OrderBy(x => x.Name);
            return types.ToList();
        }

        public List<ActionInfo> GetActions(Type controller)
        {
            List<ActionInfo> ListAction = new List<ActionInfo>();
            IEnumerable<MemberInfo> memberInfo = controller.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public).Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any()).OrderBy(x => x.Name);
            foreach (MemberInfo method in memberInfo)
            {
                string actionName = method.GetAttributeValue<ESCSDescriptionAttribute, string>(des => des.Description);
                string actionMethod = method.GetAttributeValue<ESCSDescriptionAttribute, string>(des => des.Method);
                if (method.ReflectedType.IsPublic && !method.IsDefined(typeof(NonActionAttribute)) && !ListAction.Select(n => n.ma).Any(n => n == method.Name.ToString().Trim()))
                {
                    ListAction.Add(new ActionInfo() { ma = method.Name.ToString().Trim(), ten = actionName, phuong_thuc = actionMethod });
                }
            }
            return ListAction;
        }
    }
    public class AppInfo
    {
        public string ma_doi_tac_nsd { get; set; }
        public string ma_chi_nhanh_nsd { get; set; }
        public string nsd { get; set; }
        public string pas { get; set; }

        public List<AreaInfo> area { get; set; }
        public List<ControllerInfo> controller { get; set; }
        public List<ActionInfo> action { get; set; }
        public AppInfo()
        {
            area = new List<AreaInfo>();
            controller = new List<ControllerInfo>();
            action = new List<ActionInfo>();
        }
    }
    public class AreaInfo
    {
        public string ma { get; set; }
        public string ten { get; set; }
        public string duong_dan { get; set; }
    }
    public class ControllerInfo
    {
        public string ma { get; set; }
        public string ten { get; set; }
        public string vung_qt { get; set; }
        public string duong_dan { get; set; }
    }
    public class ActionInfo
    {
        public string ma { get; set; }
        public string ten { get; set; }
        public string phuong_thuc { get; set; }
        public string nhom { get; set; }
        public string vung_qt { get; set; }
    }
}