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

        // 设置 Items
        cut.SetParametersAndRender(pb =>
        {
            pb.Add(a => a.Items, new List<TreeItem>()
            {
                new TreeItem() { Text = "Test1" }
            });
        });
        cut.Contains("li");
    }

    [Fact]
    public void OnClick_Ok()
    {
        var clicked = false;
        var expanded = false;
        var cut = Context.RenderComponent<Tree>(pb =>
        {
            pb.Add(a => a.IsAccordion, true);
            pb.Add(a => a.ClickToggleNode, true);
            pb.Add(a => a.OnTreeItemClick, item =>
            {
                clicked = true;
                return Task.CompletedTask;
            });
            pb.Add(a => a.OnExpandNode, item =>
            {
                expanded = true;
                return Task.CompletedTask;
            });
            pb.Add(a => a.Items, new List<TreeItem>()
            {
                new TreeItem()
                {
                    Text = "Test1",
                    Items = new List<TreeItem>()
                    {
                        new TreeItem()
                        {
                            Text = "Test11",
                        }
                    }
                },
                new TreeItem()
                {
                    Text = "Test2",
                    IsCollapsed = false,
                    Items = new List<TreeItem>()
                    {
                        new TreeItem()
                        {
                            Text = "Test21",
                        }
                    }
                }
            });
        });

        cut.InvokeAsync(() => cut.Find(".tree-node").Click());
        Assert.True(clicked);
        Assert.True(expanded);
    }

    [Fact]
    public void OnStateChanged_Ok()
    {
        List<TreeItem>? checkedLists = null;
        var cut = Context.RenderComponent<Tree>(pb =>
        {
            pb.Add(a => a.ShowCheckbox, true);
            pb.Add(a => a.OnTreeItemChecked, items =>
            {
                checkedLists = items;
                return Task.CompletedTask;
            });
            pb.Add(a => a.Items, new List<TreeItem>()
            {
                new TreeItem() { Text = "Test1", Icon = "fa fa-fa" }
            });
        });

        cut.InvokeAsync(() => cut.Find("[type=\"checkbox\"]").Click());
        Assert.Single(checkedLists);
        Assert.DoesNotContain("fa fa-fa", cut.Markup);

        cut.SetParametersAndRender(pb =>
        {
            pb.Add(a => a.ShowIcon, true);
        });
        Assert.Contains("fa fa-fa", cut.Markup);
    }

    [Fact]
    public void Template_Ok()
    {
        var cut = Context.RenderComponent<Tree>(pb =>
        {
            pb.Add(a => a.Items, new List<TreeItem>()
            {
                new TreeItem() { Text = "Test1", Template = builder => builder.AddContent(0, "Test-Template")}
            });
        });
        cut.Contains("Test-Template");
    }
}
