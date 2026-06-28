using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUANLYNET
{
    /// <summary>
    /// Lớp static để lưu thông tin user đang login
    /// </summary>
    public static class UserSession
    {
        public static string UserId { get; set; }
        public static string FullName { get; set; }
        public static decimal Balance { get; set; }
        public static string CurrentSessionId { get; set; }

        public static void Clear()
        {
            UserId = null;
            FullName = null;
            Balance = 0;
            CurrentSessionId = null;
        }

        public static bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(UserId);
        }
    }
}
