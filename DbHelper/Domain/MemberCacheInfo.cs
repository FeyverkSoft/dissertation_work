using System.Reflection;
using System.Runtime.Serialization;

namespace DbHelper.Domain
{
    internal class MemberCacheInfo
    {
        /// <summary>
        /// информация о члене
        /// </summary>
        public MemberInfo @MemberInfo { get; set; }
        /// <summary>
        /// аттрибут
        /// </summary>
        public DataMemberAttribute @DataMemberAttribute { get; set; }
    }
}
