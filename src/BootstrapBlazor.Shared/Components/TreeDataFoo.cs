﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;

namespace BootstrapBlazor.Shared.Components;

class TreeDataFoo
{
    public string? Code { get; set; }

    public string? ParentCode { get; set; }

    public string? Text { get; set; }

    public string Icon { get; set; } = "fa fa-fa";

    public bool IsActive { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static List<TreeItem> GetTreeItems()
    {
        var items = new List<TreeItem>
        {
            new TreeItem() { Text = "导航一", Id = "1010" },
            new TreeItem() { Text = "导航二", Id = "1020" },
            new TreeItem() { Text = "导航三", Id = "1030" },

            new TreeItem() { Text = "子菜单一", Id = "1040", ParentId = "1020" },
            new TreeItem() { Text = "子菜单二", Id = "1050", ParentId = "1020" },
            new TreeItem() { Text = "子菜单三", Id = "1060", ParentId = "1020" },

            new TreeItem() { Text = "孙菜单一", Id = "1070", ParentId = "1050" },
            new TreeItem() { Text = "孙菜单二", Id = "1080", ParentId = "1050" },
            new TreeItem() { Text = "孙菜单三", Id = "1090", ParentId = "1050" },

            new TreeItem() { Text = "曾孙菜单一", Id = "1100", ParentId = "1080" },
            new TreeItem() { Text = "曾孙菜单二", Id = "1110", ParentId = "1080" },
            new TreeItem() { Text = "曾孙菜单三", Id = "1120", ParentId = "1080" },

            new TreeItem() { Text = "曾曾孙菜单一", Id = "1130", ParentId = "1100" },
            new TreeItem() { Text = "曾曾孙菜单二", Id = "1140", ParentId = "1100" },
            new TreeItem() { Text = "曾曾孙菜单三", Id = "1150", ParentId = "1100" }
        };

        // 算法获取属性结构数据
        return items.CascadingTree().ToList();
    }
}
