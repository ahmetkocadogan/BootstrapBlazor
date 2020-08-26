﻿using System.ComponentModel;

namespace BootstrapBlazor.Components
{
    /// <summary>
    /// 按钮类型枚举
    /// </summary>
    public enum ButtonType
    {
        /// <summary>
        /// 正常按钮
        /// </summary>
        [Description("button")]
        Button,

        /// <summary>
        /// 提交按钮
        /// </summary>
        [Description("submit")]
        Submit,

        /// <summary>
        /// 重置按钮
        /// </summary>
        [Description("reset")]
        Reset
    }
}
