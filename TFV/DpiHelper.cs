using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;

namespace TFV
{
    class DpiHelper
    {
        private static Type s_DpiHelper;
        private static PropertyInfo s_IsScalingRequired;
        private static MethodInfo s_CreateResizedBitmap;
        private static MethodInfo s_LogicalToDeviceUnits;
        private static MethodInfo s_LogicalToDeviceUnitsX;
        private static MethodInfo s_LogicalToDeviceUnitsY;
        private static MethodInfo s_ScaleBitmapLogicalToDevice;
        static DpiHelper()
        {
            s_DpiHelper = typeof(System.Windows.Forms.Form).Assembly.GetType("System.Windows.Forms.DpiHelper");
            if (s_DpiHelper != null)
            {
                s_IsScalingRequired = s_DpiHelper.GetProperty("IsScalingRequired", BindingFlags.Static | BindingFlags.Public, null, typeof(bool), Type.EmptyTypes, null);
                s_CreateResizedBitmap = s_DpiHelper.GetMethod("CreateResizedBitmap", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(Bitmap), typeof(Size) }, null);
                s_LogicalToDeviceUnits = s_DpiHelper.GetMethod("LogicalToDeviceUnits", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(Size) }, null);
                s_LogicalToDeviceUnitsX = s_DpiHelper.GetMethod("LogicalToDeviceUnitsX", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(int) }, null);
                s_LogicalToDeviceUnitsY = s_DpiHelper.GetMethod("LogicalToDeviceUnitsY", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(int) }, null);
                s_ScaleBitmapLogicalToDevice = s_DpiHelper.GetMethod("ScaleBitmapLogicalToDevice", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(Bitmap).MakeByRefType() }, null);
            }
        }
        public static bool IsScalingRequired
        {
            get
            {
                if (s_IsScalingRequired != null)
                    return (bool)s_IsScalingRequired.GetValue(null);
                else
                    return false;
            }
        }

        public static Bitmap CreateResizedBitmap(Bitmap logicalImage, Size targetImageSize)
        {
            if (s_CreateResizedBitmap != null)
                return s_CreateResizedBitmap.Invoke(null, new object[] { logicalImage, targetImageSize }) as Bitmap;
            else
                return logicalImage.Clone() as Bitmap;
        }

        public static Size LogicalToDeviceUnits(Size logicalSize)
        {
            if (s_LogicalToDeviceUnits != null)
                return (Size)s_LogicalToDeviceUnits.Invoke(null, new object[] { logicalSize });
            else
                return logicalSize;
        }

        public static int LogicalToDeviceUnitsX(int value)
        {
            if (s_LogicalToDeviceUnitsX != null)
                return (int)s_LogicalToDeviceUnitsX.Invoke(null, new object[] { value });
            else
                return value;
        }

        public static int LogicalToDeviceUnitsY(int value)
        {
            if (s_LogicalToDeviceUnitsY != null)
                return (int)s_LogicalToDeviceUnitsY.Invoke(null, new object[] { value });
            else
                return value;
        }

        public static void ScaleBitmapLogicalToDevice(ref Bitmap logicalBitmap)
        {
            if (s_ScaleBitmapLogicalToDevice != null)
            {
                object[] args = new object[] { logicalBitmap };
                s_ScaleBitmapLogicalToDevice.Invoke(null, args);
                logicalBitmap = args[0] as Bitmap;
            }
        }

        public static Bitmap ScaleBitmapLogicalToDevice(Bitmap logicalBitmap)
        {
            ScaleBitmapLogicalToDevice(ref logicalBitmap);
            return logicalBitmap;
        }
    }
}
