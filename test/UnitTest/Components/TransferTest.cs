// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor;

namespace UnitTest.Components;

public class TransferTest : BootstrapBlazorTestBase
{

    [NotNull]
    private IEnumerable<SelectedItem>? Items { get; set; } = Enumerable.Range(1, 15).Select(i => new SelectedItem()
    {
        Text = $"DataLeft {i:d2}",
        Value = i.ToString()
    });

    [NotNull]
    private IEnumerable<SelectedItem>? ItemsRight { get; set; } = Enumerable.Range(1, 15).Select(i => new SelectedItem()
    {
        Text = $"DataRight {i:d2}",
        Value = i.ToString()
    });

    private static string? SetItemClass(SelectedItem item) => item.Value switch
    {
        "2" => "bg-success text-white",
        "4" => "bg-info text-white",
        "6" => "bg-primary text-white",
        "8" => "bg-warning text-white",
        _ => null
    };

    [Fact]
    public void TransferPanel_Ok()
    {
        var select = false;
        var cut = Context.RenderComponent<TransferPanel>(builder =>
        {
            builder.Add(a => a.Items, Items.ToList());
            builder.Add(a => a.OnSelectedItemsChanged, () =>
            {
                select = true;
                return Task.CompletedTask;
            });
        });
        cut.SetParametersAndRender(builder => builder.Add(a => a.Text, "Test"));
        Assert.Contains("Test", cut.Markup);

        cut.SetParametersAndRender(builder => builder.Add(a => a.OnSetItemClass, SetItemClass));
        var items = cut.FindAll(".transfer-panel-list div");
        Assert.Contains("bg-success text-white", items[1].ClassName);

        Assert.DoesNotContain("transfer-panel-filter", cut.Markup);
        cut.SetParametersAndRender(builder => builder.Add(s => s.ShowSearch, true));

        cut.SetParametersAndRender(builder => builder.Add(s => s.SearchPlaceHolderString, "SearchPlaceHolderStringOK"));
        Assert.Contains("SearchPlaceHolderStringOK", cut.Markup);

        //左侧头部复选框
        var btns = cut.Find(".transfer-panel-header input");
        btns.Click();
        Assert.True(select);

        cut.SetParametersAndRender(builder => builder.Add(s => s.IsDisabled, true));
        Assert.Contains(" disabled", cut.Markup);

    }

    [Fact]
    public void Items_Ok()
    {
        var cut = Context.RenderComponent<Transfer<string>>(builder =>
        {
            builder.Add(a => a.Items, Items);
        });
        var items = cut.FindAll(".transfer-panel-list div");

        Assert.True(items.Count == Items.Count());
    }
    
    [Fact]
    public void TextAndIsDisabled_Ok()
    {
        var cut = Context.RenderComponent<Transfer<string>>(builder =>
        {
            builder.Add(a => a.Items, Items);
            builder.Add(a => a.LeftPanelText, "LeftPanelTextOK");
        });
        Assert.Contains("LeftPanelTextOK", cut.Markup);

        cut.SetParametersAndRender(builder => builder.Add(s => s.RightPanelText, "RightPanelTextOK"));
        Assert.Contains("RightPanelTextOK", cut.Markup);

        cut.SetParametersAndRender(builder => builder.Add(s => s.LeftButtonText, "LeftButtonTextOK"));
        Assert.Contains("LeftButtonTextOK", cut.Markup);

        cut.SetParametersAndRender(builder => builder.Add(s => s.RightButtonText, "RightButtonTextOK"));
        Assert.Contains("RightButtonTextOK", cut.Markup);

        Assert.DoesNotContain("transfer-panel-filter", cut.Markup);
        cut.SetParametersAndRender(builder => builder.Add(s => s.ShowSearch,  true));

        cut.SetParametersAndRender(builder => builder.Add(s => s.LeftPannelSearchPlaceHolderString, "LeftPannelSearchPlaceHolderStringOK"));
        Assert.Contains("LeftPannelSearchPlaceHolderStringOK", cut.Markup);

        cut.SetParametersAndRender(builder => builder.Add(s => s.RightPannelSearchPlaceHolderString, "RightPannelSearchPlaceHolderStringOK"));
        Assert.Contains("RightPannelSearchPlaceHolderStringOK", cut.Markup);

        cut.SetParametersAndRender(builder => builder.Add(s => s.IsDisabled, true));
        Assert.Contains("transfer-panel-filter", cut.Markup);
    }
        
    [Fact]
    public void OnSetItemClass_Ok()
    {
        var cut = Context.RenderComponent<Transfer<string>>(builder =>
        {
            builder.Add(a => a.Items, Items);
            builder.Add(a => a.OnSetItemClass, SetItemClass);
        });
        var items = cut.FindAll(".transfer-panel-list div");

        Assert.Contains("bg-success text-white", items[1].ClassName );
    }


    [Fact]
    public void SelectedItemsChanged_Ok()
    {
        IEnumerable<SelectedItem> selecteditems=new List<SelectedItem> ();
        var cut = Context.RenderComponent<Transfer<string>>(builder =>
        {
            builder.Add(a => a.Items, Items);
            builder.Add(a => a.OnSelectedItemsChanged, (v1) =>
            {
                selecteditems = v1;
                return Task.CompletedTask;
            });
        });

        Assert.DoesNotContain("form-check is-checked", cut.Markup);
        var btns = cut.FindAll(".transfer-buttons button");

        // 选中事件
        var item1 = cut.FindAll(".transfer-panel-list input");
        item1[0].Click();
        Assert.Contains("form-check is-checked", cut.Markup);
        //DataLeft 03
        item1[2].Click();
        //走两个到右边
        btns[1].Click();
        Assert.True(selecteditems.Any() && selecteditems.Count() == 2);
        Assert.True(selecteditems.ToList()[0].Text == "DataLeft 01");
        Assert.True(selecteditems.ToList()[1].Text == "DataLeft 03");

        //回来一个到左边
        var item2 = cut.FindAll(".transfer-panel-list input");
        item2.Last().Click();
        btns[0].Click();
        Assert.True(selecteditems.Any() && selecteditems.Count() == 1);
        Assert.True(selecteditems.ToList()[0].Text == "DataLeft 01");

        // [全部]选中事件
        var item = cut.Find(".form-check-input");
        item.Click();
        Assert.Contains("form-check is-checked", cut.Markup);

        btns[1].Click();
        Assert.True(selecteditems.Any() && selecteditems.Count()== Items.Count());

     }

    //[Fact]
    //public void OnTransferEnd_Ok()
    //{
    //    var TransferEnd = false;
    //    var cut = Context.RenderComponent<Transfer<string>>(builder =>
    //    {
    //        builder.Add(a => a.OnTransferEnd, () =>
    //        {
    //            TransferEnd = true;
    //            return Task.FromResult(true);
    //        });
    //    });

    //    cut.InvokeAsync(() => cut.Instance.TransferEndAsync());
    //    Assert.True(TransferEnd);
    //}
}
