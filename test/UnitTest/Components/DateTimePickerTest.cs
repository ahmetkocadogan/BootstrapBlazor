// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Components;

public class DateTimePickerTest : BootstrapBlazorTestBase
{
    #region DateTimePicker
    [Fact]
    public void ShowSiderBar_Ok()
    {
        var cut = Context.RenderComponent<DateTimePicker<DateTime>>(builder => builder.Add(a => a.ShowSidebar, true));

        var ele = cut.Find(".picker-panel-sidebar");

        Assert.NotNull(ele);
    }

    [Fact]
    public void Placement_Ok()
    {
        var cut = Context.RenderComponent<DateTimePicker<DateTime>>(builder => builder.Add(a => a.Placement, Placement.Top));

        Assert.Contains("data-bs-placement=\"top\"", cut.Markup);
    }

    [Fact]
    public void Format_OK()
    {
        var cut = Context.RenderComponent<DateTimePicker<DateTime>>(builder =>
        {
            builder.Add(a => a.Value, DateTime.Now);
            builder.Add(a => a.Format, "yyyy/MM/dd");
        });

        var value = cut.Find(".datetime-picker-bar").Children.First().GetAttribute("value");

        Assert.Equal(value, DateTime.Now.ToString("yyyy/MM/dd"));
    }

    [Fact]
    public void MaxValue_Ok()
    {
        var cut = Context.RenderComponent<DateTimePicker<DateTime>>(builder => builder.Add(a => a.MaxValue, DateTime.Today.AddDays(1)));
    }

    [Fact]
    public void MinValue_Ok()
    {
        var cut = Context.RenderComponent<DateTimePicker<DateTime>>(builder => builder.Add(a => a.MinValue, DateTime.Today.AddDays(-1)));
    }

    [Fact]
    public void OnDateTimeChanged_Ok()
    {
        var res = false;
        var cut = Context.RenderComponent<DateTimePicker<DateTime>>(builder =>
        {
            builder.Add(a => a.Value, DateTime.Now);
            builder.Add(a => a.OnDateTimeChanged, new Func<DateTime, Task>(d =>
            {
                res = true;
                return Task.CompletedTask;
            }));
        });

        cut.Find(".picker-panel-footer").Children.Last().Click();

        Assert.True(res);
    }

    [Fact]
    public void ViewModel_Ok()
    {
        var cut = Context.RenderComponent<DateTimePicker<DateTime>>(builder =>
        {
            builder.Add(a => a.Value, DateTime.Now);
            builder.Add(a => a.ViewModel, DatePickerViewModel.DateTime);
        });

        var value = cut.Find(".datetime-picker-bar").Children.First().GetAttribute("value");

        Assert.Equal(value, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    [Fact]
    public void NullValue_Ok()
    {

        var res = false;
        var cut = Context.RenderComponent<DateTimePicker<DateTime?>>(builder =>
        {
            builder.Add(a => a.Value, DateTime.Now);
            builder.Add(a => a.ViewModel, DatePickerViewModel.DateTime);
            builder.Add(a => a.OnDateTimeChanged, new Func<DateTime?, Task>(d =>
            {
                res = true;
                return Task.CompletedTask;
            }));
        });

        var value = cut.Find(".picker-panel-footer .flex-fill").HasChildNodes;

        Assert.True(value);

        cut.Find(".picker-panel-footer .flex-fill").Children.First().Click();

        Assert.True(res);
    }
    #endregion

    #region DatePicker
    [Fact]
    public void DateFormat_Ok()
    {
        var res = "";
        var cut = Context.RenderComponent<DatePickerBody>(builder =>
        {
            builder.Add(a => a.Value, DateTime.Now);
            builder.Add(a => a.DateFormat, "yyyy/MM/dd");
            builder.Add(a => a.ValueChanged, EventCallback.Factory.Create<DateTime>(this, d =>
            {
                res = d.ToString();
            }));
        });

        cut.Find(".current").Children.First().Children.First().Click();

        //Assert.Equal(DateTime.Now.ToString("yyyy/MM/dd"), res);
    }

    [Fact]
    public void DatePickerViewModel_Ok()
    {
        var cut = Context.RenderComponent<DatePickerBody>(builder =>
        {
            builder.Add(a => a.Value, DateTime.Now);
            builder.Add(a => a.ViewModel, DatePickerViewModel.Year);
        });

        var ele = cut.Find(".d-none");

        Assert.NotNull(ele);
    }

    [Fact]
    public void ShowSidebar_Ok()
    {
        var cut = Context.RenderComponent<DatePickerBody>(builder =>
        {
            builder.Add(a => a.Value, DateTime.Now);
            builder.Add(a => a.ShowSidebar, true);
        });

        var ele = cut.Find(".picker-panel-sidebar");

        Assert.NotNull(ele);
    }

    [Fact]
    public void ShowLeftButtons_Ok()
    {
        var cut = Context.RenderComponent<DatePickerBody>(builder =>
        {
            builder.Add(a => a.Value, DateTime.Now);
            builder.Add(a => a.ShowLeftButtons, true);
        });

        var ele = cut.Find(".fa-angle-double-left");

        Assert.NotNull(ele);
    }

    [Fact]
    public void ShowRightButtons_Ok()
    {
        var cut = Context.RenderComponent<DatePickerBody>(builder =>
        {
            builder.Add(a => a.Value, DateTime.Now);
            builder.Add(a => a.ShowRightButtons, true);
        });

        var ele = cut.Find(".fa-angle-right");

        Assert.NotNull(ele);
    }

    [Fact]
    public void IsShown_Ok()
    {
        var cut = Context.RenderComponent<DatePickerBody>(builder =>
        {
            builder.Add(a => a.Value, DateTime.Now);
            builder.Add(a => a.IsShown, true);
        });

        var value = cut.Find(".picker-panel").ClassList.Contains("d-none");

        Assert.False(value);
    }

    [Fact]
    public void ShowFooter_Ok()
    {
        var cut = Context.RenderComponent<DatePickerBody>(builder =>
        {
            builder.Add(a => a.Value, DateTime.Now);
            builder.Add(a => a.ShowFooter, true);
        });

        var ele = cut.Find(".picker-panel-footer");

        Assert.NotNull(ele);
    }
    #endregion

    #region TimePicker
    [Fact]
    public void OnClose_Ok()
    {
        var res = false;
        var cut = Context.RenderComponent<TimePickerBody>(builder =>
        {
            builder.Add(a => a.Value, TimeSpan.FromDays(1));
            builder.Add(a => a.OnClose, new Action(() =>
            {
                res = true;
            }));
        });

        cut.Find(".time-panel-footer .cancel").Click();

        Assert.True(res);
    }

    [Fact]
    public void OnConfirm_Ok()
    {
        var res = false;
        var res1 = false;
        var cut = Context.RenderComponent<TimePickerBody>(builder =>
        {
            builder.Add(a => a.Value, TimeSpan.FromDays(1));
            builder.Add(a => a.ValueChanged, EventCallback.Factory.Create<TimeSpan>(this, t =>
            {
                res1 = true;
            }));
            builder.Add(a => a.OnConfirm, new Action(() =>
            {
                res = true;
            }));
        });

        cut.Find(".time-panel-footer .confirm").Click();

        Assert.True(res);
        Assert.True(res1);
    }
    #endregion
}
