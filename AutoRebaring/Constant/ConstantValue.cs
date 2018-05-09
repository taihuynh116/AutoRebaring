using Autodesk.Revit.DB;
using AutoRebaring.Single;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRebaring.Constant
{
    public static class ConstantValue
    {
        public const int LoopCount = 20;
        public const double RebarStandardOffsetControl = 1.3;
        public const double feet2Meter = 0.3048;
        public const double meter2Feet = 1 / feet2Meter;
        public const double feet2MiliMeter = feet2Meter * 1000;
        public const double milimeter2Feet = 1 / feet2MiliMeter;

        public const string B1Param_Column = "b";
        public const string B2Param_Column = "h";
        public const string B1Param_Wall = "Length";
        public const string B2Param_Wall = "Width";

        public const string StartLevelWall = "Base Constraint";
        public const string EndLevelWall = "Top Constraint";
        public const string StartLevelColumn = "Base Level";
        public const string EndLevelColumn = "Top Level";

        public const string InvalidLogin = "Yêu cầu nhập username, password";
        public const string WrongPassword = "Tên đăng nhập, mật khẩu không đúng!";
        public const string NonAuthorized = "Tài khoản của bạn không được phân quyền vào dự án này!";
        public const string AuthorizationNotExisted = "Tài khoản bạn không tồn tại, liên hệ bộ phận kĩ thuật để xử lý vấn đề.";
        public const string AuthorizationNotActive = "Tài khoản bạn chưa được kích hoạt, liên hệ bộ phận kĩ thuật để xử lý vấn đề.";

        public const string User = "User";
        public const string Admin = "Admin";

        public const string Column = "Column";
        public const string Wall = "Wall";

        public const string Standard = "Standard";
        public const string Stirrup = "Stirrup";

        public const string FitL = "FitL";
        public const string FitL2 = "FitL2";
        public const string FitL3 = "FitL3";
        public const string ImplantL = "ImplantL";
        public const string ImplantL2 = "ImplantL2";

        public const string CoverStirrup = "CoverStirrup";
        public const string EdgeStandard = "EdgeStandard";
        public const string MiddleStandard = "MiddleStandard";
        public const string EdgeCoverStirrup = "EdgeCoverStirrup";
        public const string CStirrup = "CStirrup";

        public const string Name = "Name";

        public const string Edge = "Edge";
        public const string Middle = "Middle";

        public const string Starter = "Starter";
        public const string Lockhead = "Lockhead";

        public const string B1 = "B1";
        public const string B2 = "B2";
        public const string NE11 = "NE11";
        public const string NE12 = "NE12";
        public const string CE12 = "CE12";
        public const string NE2 = "NE2";
        public const string DE2 = "DE2";
        public const string NM = "NM";

        public const string TB1 = "TB1";
        public const string TB2 = "TB2";
        public const string M1 = "M1";
        public const string M2 = "M2";

        public const string KieuThepDoc = "Kiểu thép dọc:";
        public const string Vungbien = "Vùng biên:";

        public static List<string> CoverStirrupParameters = new List<string> { "B", "D", "C", "E" };
        public static List<string> CStirrupParameters = new List<string> { "B" };

        public static List<Type> CoverStirrupTypes = new List<Type> { typeof(double), typeof(double), typeof(double), typeof(double) };
        public static List<Type> CStirrupTypes = new List<Type> { typeof(double) };
    }
}
