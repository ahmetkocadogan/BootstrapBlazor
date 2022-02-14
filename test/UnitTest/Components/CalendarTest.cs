// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace UnitTest.Components;

public class CalendarTest : BootstrapBlazorTestBase
{

    [Fact]
    public void ViewModelWeek_Ok()
    {
        var cutWeek = Context.RenderComponent<Calendar>(builder => builder.Add(s => s.ViewModel, CalendarViewModel.Week));
        Assert.Contains("table-week", cutWeek.Markup);
    }

    [Fact]
    public void Normal_Ok()
    {
        var cut = Context.RenderComponent<Calendar>();
        Assert.Contains(DateTime.Now.Year.ToString(), cut.Markup);
    }

    [Fact]
    public void Value_Ok()
    {
        var cut = Context.RenderComponent<Calendar>(builder => builder.Add(s => s.Value, new DateTime(1999,1,1)));
        Assert.Contains("1999", cut.Markup);
    }


    [Fact]
    public void ChildContent_Ok()
    {

        var cut = Context.RenderComponent<Calendar>(builder =>
        {
            builder.Add(s => s.ViewModel, CalendarViewModel.Week);
            builder.Add(a => a.ChildContent, CreateComponent());
        });
        Assert.Contains("test", cut.Markup);

        static RenderFragment CreateComponent() => builder =>
        {
            builder.OpenElement(1, "div");
            builder.AddContent(2, "test");
            builder.CloseComponent();
        };
    }

    [Fact]
    public void ButtonClick_Ok()
    {
        var cut = Context.RenderComponent<Calendar>();
        var buttons = cut.FindAll("button");

        //btn上一年
        buttons.FirstOrDefault(s => s.TextContent == "上一年").Click();
        Assert.Contains((DateTime.Now.Year-1).ToString(), cut.Markup);

        //btn下一年,测了上一个按钮,这里结果将会是今年
        buttons.FirstOrDefault(s => s.TextContent == "下一年").Click();
        Assert.Contains((DateTime.Now.Year).ToString(), cut.Markup);

        //btn上一月
        buttons.FirstOrDefault(s => s.TextContent == "上一月").Click();
        Assert.Contains((DateTime.Now.Month - 1).ToString(), cut.Markup);

        //btn下一月,同理
        buttons.FirstOrDefault(s => s.TextContent == "下一月").Click();
        Assert.Contains((DateTime.Now.Month).ToString(), cut.Markup);

    }


    [Fact]
    public void ValueChanged_Ok()
    {
        DateTime? value = null;

        var cut = Context.RenderComponent<Calendar>(builder =>
        {
            builder.Add(a => a.ValueChanged, (v) =>
            value = v
            );
        });

        cut.Find(".is-selected").Click();
        Assert.True(value!.Value==DateTime.Now.Date);

        cut.Find(".current").Click();
        Assert.True(value!.Value.Day == 1);
    }


}
