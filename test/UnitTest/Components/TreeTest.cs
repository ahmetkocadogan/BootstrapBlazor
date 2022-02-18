// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

namespace UnitTest.Components;

public class TreeTest : BootstrapBlazorTestBase
{
    [Fact]
    public void Items_Ok()
    {
        var cut = Context.RenderComponent<Tree>();
        cut.Contains("tree-root");

        // 由于 Items 为空不生成 TreeItem
        cut.DoesNotContain("li");

        cut.SetParametersAndRender(pb =>
        {
            pb.Add(a => a.Items, new List<TreeItem>()
            {
                new TreeItem() { Text = "Test1", id },
                new TreeItem() { Text = "Test2", IsActive = true }
            });
        });
    }
}
