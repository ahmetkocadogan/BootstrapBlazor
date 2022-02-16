﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Console = BootstrapBlazor.Components.Console;

namespace UnitTest.Components;

public class ConsoleTest : BootstrapBlazorTestBase
{
    [Fact]
    public void Height_OK()
    {
        var cut = Context.RenderComponent<Console>(builder => builder.Add(a => a.Height, 100));

        Assert.Contains("style=\"height: 100px;\"", cut.Markup);
    }

    [Fact]
    public void HeaderText_OK()
    {
        var cut = Context.RenderComponent<Console>(builder => builder.Add(a => a.HeaderText, "HeaderText"));

        Assert.Contains("HeaderText", cut.Markup);
    }

    [Fact]
    public void Items_OK()
    {
        var cut = Context.RenderComponent<Console>(builder =>
        {
            builder.Add(a => a.Items,
                new List<ConsoleMessageItem>()
                {
                    new ConsoleMessageItem() {Message = "Test1 "}, new ConsoleMessageItem() {Message = "Test2"}
                });
        });

        var res = cut.Find(".console-window").HasChildNodes;
        Assert.True(res);
    }

    [Fact]
    public void OnClear_OK()
    {
        var res = false;
        var cut = Context.RenderComponent<Console>(builder =>
        {
            builder.Add(a => a.Items,
                new List<ConsoleMessageItem>()
                {
                    new ConsoleMessageItem() {Message = "Test1 "}, new ConsoleMessageItem() {Message = "Test2"}
                });
            builder.Add(a => a.OnClear, new Action(() =>
            {
                res = true;
            }));
        });

        cut.Find(".btn-secondary").Click();
        Assert.True(res);
    }

    [Fact]
    public void ClearButtonText_OK()
    {
        var cut = Context.RenderComponent<Console>(builder =>
        {
            builder.Add(a => a.Items,
                new List<ConsoleMessageItem>()
                {
                    new ConsoleMessageItem() {Message = "Test1 "}, new ConsoleMessageItem() {Message = "Test2"}
                });
            builder.Add(a => a.ClearButtonText, "Console Clear");
        });

        Assert.Contains("Console Clear", cut.Markup);
    }

    [Fact]
    public void OnClearButtonText_OK()
    {
        var cut = Context.RenderComponent<Console>(builder =>
        {
            builder.Add(a => a.Items,
                new List<ConsoleMessageItem>()
                {
                    new ConsoleMessageItem() {Message = "Test1 "}, new ConsoleMessageItem() {Message = "Test2"}
                });
            builder.Add(a => a.ClearButtonIcon, "fa fa-times");
        });

        Assert.Contains("fa fa-times", cut.Markup);
    }


    [Fact]
    public void ClearButtonColor_OK()
    {
        var cut = Context.RenderComponent<Console>(builder =>
        {
            builder.Add(a => a.Items,
                new List<ConsoleMessageItem>()
                {
                    new ConsoleMessageItem() {Message = "Test1 "}, new ConsoleMessageItem() {Message = "Test2"}
                });
            builder.Add(a => a.ClearButtonColor, Color.Primary);
        });

        Assert.Contains(".btn-primary", cut.Markup);
    }

    [Fact]
    public void ShowAutoScroll_OK()
    {
        var cut = Context.RenderComponent<Console>(builder =>
        {
            builder.Add(a => a.Items,
                new List<ConsoleMessageItem>()
                {
                    new ConsoleMessageItem() {Message = "Test1 "}, new ConsoleMessageItem() {Message = "Test2"}
                });
            builder.Add(a => a.ShowAutoScroll, true);
        });

        Assert.Contains("style=\"cursor: pointer;\"", cut.Markup);
    }

    [Fact]
    public void IsAutoScroll_OK()
    {
        var cut = Context.RenderComponent<Console>(builder =>
        {
            builder.Add(a => a.Items,
                new List<ConsoleMessageItem>()
                {
                    new ConsoleMessageItem() {Message = "Test1 "}, new ConsoleMessageItem() {Message = "Test2"}
                });
            builder.Add(a => a.IsAutoScroll, true);
        });

        var res = cut.Find(".console-body").GetAttribute("data-scroll");
        Assert.Equal("on", res);
    }

    [Fact]
    public void AutoScrollText_OK()
    {
        var cut = Context.RenderComponent<Console>(builder =>
        {
            builder.Add(a => a.Items,
                new List<ConsoleMessageItem>()
                {
                    new ConsoleMessageItem() {Message = "Test1 "}, new ConsoleMessageItem() {Message = "Test2"}
                });
            builder.Add(a => a.AutoScrollText, "AutoScrollText");
        });

        Assert.Contains("AutoScrollText", cut.Markup);
    }

    [Fact]
    public void LightTitle_OK()
    {
        var cut = Context.RenderComponent<Console>(builder =>
        {
            builder.Add(a => a.Items,
                new List<ConsoleMessageItem>()
                {
                    new ConsoleMessageItem() {Message = "Test1 "}, new ConsoleMessageItem() {Message = "Test2"}
                });
            builder.Add(a => a.LightTitle, "LightTitle");
        });

        Assert.Contains("LightTitle", cut.Markup);
    }
}
