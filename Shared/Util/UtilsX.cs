using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TciPM.Blazor.Shared.Util
{
    public class UtilsX
    {
        public static readonly string[] ACCEPTABLE_FILE_EXTENTIONS_TO_UPLOAD = new string[]
        {
            "png", "jpg", "jpeg", "gif", "dwg",
            "doc", "docx", "pdf", "ppt", "pptx", "xls", "xlsx", "txt", "vso", "accdb"
        };

        public static bool IsFileUploadAcceptable(string mimeType, string fileName)
        {
            string fileExtention = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();
            return ACCEPTABLE_FILE_EXTENTIONS_TO_UPLOAD.Contains(fileExtention);
        }

        public static double CalculateStandardDeviation(params double[] items)
        {
            if (items.Length == 0)
                return 0;
            double avg = items.Average();
            return Math.Sqrt(items.Select(i => Math.Pow(i - avg, 2)).Average());
        }

        public static float CalculateStandardDeviation(params float[] items)
        {
            if (items.Length == 0)
                return 0;
            float avg = items.Average();
            return (float)Math.Sqrt(items.Select(i => Math.Pow(i - avg, 2)).Average());
        }

        public static string DisplayName<T>(Expression<Func<T, object>> p)
        {
            string memberName;
            if (p.Body is MemberExpression)
                memberName = ((MemberExpression)p.Body).Member.Name;
            else if (p.Body is UnaryExpression)
                memberName = ((p.Body as UnaryExpression).Operand as MemberExpression).Member.Name;
            else
                throw new NotImplementedException();
            return DisplayName(typeof(T), memberName);
        }

        public static string DisplayName(Type classType, string memberName)
        {
            MemberInfo[] members = classType.GetMember(memberName);
            if (members == null || members.Length == 0)
                return memberName;
            return DisplayName(members[0]);
        }

        public static string DisplayName(MemberInfo member)
        {
            if (member == null)
                return null;
            var attr = member.GetCustomAttribute<DisplayAttribute>();
            if (attr != null)
                return attr.Name;
            var attr2 = member.GetCustomAttribute<DisplayNameAttribute>();
            if (attr2 != null)
                return attr2.DisplayName;
            return member.Name;
        }

        public static string DisplayName<T>(T value)
        {
            if (value == null)
                return null;
            var fieldInfo = value.GetType().GetField(value.ToString());
            return DisplayName(fieldInfo);
        }
    }
}