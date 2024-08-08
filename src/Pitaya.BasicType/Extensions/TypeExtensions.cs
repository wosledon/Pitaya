using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pitaya.BasicType.BitTypes;

namespace Pitaya.BasicType.Extensions;

/// <summary>
/// 提供对类型的扩展方法。
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// 判断对象是否为数字类型。
    /// </summary>
    /// <param name="value">要判断的对象。</param>
    /// <returns>如果对象是数字类型，则为 true；否则为 false。</returns>
    public static bool IsNumber(this object value)
    {
        return value is byte or sbyte or short or ushort
            or int or uint
            or long or ulong
            or float or double
            or decimal or bool

            or Bit or Bit2;
    }
}
